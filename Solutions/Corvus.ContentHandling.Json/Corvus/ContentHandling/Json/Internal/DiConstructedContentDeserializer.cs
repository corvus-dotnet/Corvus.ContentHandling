// <copyright file="DiConstructedContentDeserializer.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json.Internal
{
    using System;
    using System.Collections.Concurrent;
    using System.Runtime.CompilerServices;
    using System.Text.Json;
    using System.Text.Json.Serialization.Metadata;

    /// <summary>
    /// Deserializes JSON into an existing instance — used for content constructed through
    /// dependency injection, which System.Text.Json cannot instantiate itself.
    /// </summary>
    /// <remarks>
    /// <para>
    /// System.Text.Json has no direct equivalent of Newtonsoft.Json's <c>Populate</c>. Instead,
    /// we take a contract (<see cref="JsonTypeInfo"/>) for the concrete type whose
    /// <see cref="JsonTypeInfo.CreateObject"/> returns the instance being populated, so
    /// ordinary deserialization populates that instance.
    /// </para>
    /// <para>
    /// Building a contract is expensive (profiling showed it dominating this path), so
    /// contracts are cached per (options, type) and their <see cref="JsonTypeInfo.CreateObject"/>
    /// reads the instance from a thread-static slot. Deserialization from a
    /// <see cref="JsonElement"/> is fully synchronous, so the slot cannot leak across threads;
    /// it is saved and restored around each call so nested DI-constructed content (a
    /// DI-constructed property inside DI-constructed content) populates correctly.
    /// </para>
    /// </remarks>
    internal static class DiConstructedContentDeserializer
    {
        private static readonly DefaultJsonTypeInfoResolver TypeInfoResolver = new();
        private static readonly ConditionalWeakTable<JsonSerializerOptions, ConcurrentDictionary<Type, JsonTypeInfo>> ContractCache = new();

        [ThreadStatic]
        private static object? currentInstance;

        /// <summary>
        /// Deserializes JSON into an existing instance.
        /// </summary>
        /// <param name="instance">The instance to populate.</param>
        /// <param name="implementingType">The concrete type of the instance.</param>
        /// <param name="json">The JSON to deserialize from.</param>
        /// <param name="options">The serializer options.</param>
        public static void DeserializeInto(object instance, Type implementingType, JsonElement json, JsonSerializerOptions options)
        {
            ConcurrentDictionary<Type, JsonTypeInfo> contractsForOptions = ContractCache.GetValue(options, static _ => new ConcurrentDictionary<Type, JsonTypeInfo>());
            JsonTypeInfo jsonTypeInfo = contractsForOptions.GetOrAdd(implementingType, static (type, opts) =>
            {
                JsonTypeInfo typeInfo = TypeInfoResolver.GetTypeInfo(type, opts);
                typeInfo.CreateObject = static () => currentInstance ?? throw new InvalidOperationException("No instance is available to populate.");
                return typeInfo;
            },
            options);

            object? priorInstance = currentInstance;
            currentInstance = instance;
            try
            {
                _ = json.Deserialize(jsonTypeInfo);
            }
            finally
            {
                currentInstance = priorInstance;
            }
        }
    }
}
