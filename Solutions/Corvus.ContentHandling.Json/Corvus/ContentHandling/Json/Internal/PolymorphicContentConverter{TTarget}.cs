// <copyright file="PolymorphicContentConverter{TTarget}.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json.Internal
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// A JSON converter enabling objects which conform to the content-type polymorphism pattern
    /// to be deserialized to a property of some interface or base type.
    /// </summary>
    /// <typeparam name="TTarget">
    /// The target for deserialization: an interface or base type of the registered content types.
    /// </typeparam>
    /// <remarks>
    /// <para>
    /// The concrete type to deserialize is determined by the <c>contentType</c> discriminator
    /// property in the source JSON (which may appear anywhere in the object), resolved through
    /// the content registry with hierarchical content-type fallback.
    /// </para>
    /// <para>
    /// The object is buffered into a pooled, read-only <see cref="JsonDocument"/> — not a
    /// mutable <see cref="System.Text.Json.Nodes.JsonNode"/> tree — since it is only inspected
    /// for the discriminator and then re-read; profiling showed the node tree dominating this
    /// path.
    /// </para>
    /// <para>
    /// Content registered <see cref="ContentConstruction.FromServices"/> is resolved from the
    /// container (as a keyed service, keyed by content type) and then populated from the JSON;
    /// content registered <see cref="ContentConstruction.BySerializer"/> is constructed by
    /// System.Text.Json directly, which preserves constructor-based deserialization. Because
    /// deserialization populates the resolved instance, <see cref="ContentConstruction.FromServices"/>
    /// content must be registered transient; singleton or scoped registrations are rejected
    /// with <see cref="NotSupportedException"/> (they would share one repeatedly overwritten
    /// instance across deserializations).
    /// </para>
    /// <para>
    /// The concrete type must differ from <typeparamref name="TTarget"/>: System.Text.Json
    /// caches one converter per type, so deserializing (or serializing) the target type itself
    /// through this converter would recurse infinitely. Use an interface or abstract base as
    /// the target type.
    /// </para>
    /// </remarks>
    internal sealed class PolymorphicContentConverter<TTarget> : JsonConverter<TTarget>
        where TTarget : class
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IContentRegistry registry;

        /// <summary>
        /// Initializes a new instance of the <see cref="PolymorphicContentConverter{TTarget}"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider from which content is resolved.</param>
        /// <param name="registry">The content registry.</param>
        public PolymorphicContentConverter(IServiceProvider serviceProvider, IContentRegistry registry)
        {
            this.serviceProvider = serviceProvider;
            this.registry = registry;
        }

        /// <inheritdoc/>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TTarget);
        }

        /// <inheritdoc/>
        public override TTarget? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException($"Expected object, was {reader.TokenType}");
            }

            using JsonDocument document = JsonDocument.ParseValue(ref reader);
            JsonElement root = document.RootElement;

            if (!root.TryGetProperty(ContentTypeJson.ContentTypePropertyName, out JsonElement contentTypeElement)
                || contentTypeElement.GetString() is not string contentTypeName)
            {
                throw new JsonException($"Object must have a '{ContentTypeJson.ContentTypePropertyName}' property.");
            }

            if (!this.registry.TryResolve(contentTypeName, out ContentRegistration? registration))
            {
                throw new InvalidOperationException($"The content type '{contentTypeName}' has not been registered.");
            }

            if (registration.ImplementingType == typeof(TTarget))
            {
                throw new NotSupportedException($"When using polymorphic contentType-driven deserialization, the concrete type must be different from the target type. (Here, both are {typeof(TTarget).Name}.) Consider using an interface or abstract base class as the target type.");
            }

            if (registration.Construction == ContentConstruction.FromServices)
            {
                // Deserialization populates the resolved instance, so the resolution must
                // produce a fresh instance per call: a singleton (or scoped — this converter
                // only has the root provider) registration would hand every deserialization
                // the same object, silently overwriting previously returned results.
                if (registration.Lifetime != ServiceLifetime.Transient)
                {
                    throw new NotSupportedException($"The content type '{registration.ContentType}' is registered {registration.Lifetime} and cannot be deserialized: deserializing DI-constructed content populates the resolved instance, so a shared instance would be overwritten by every subsequent payload. Register it with AddTransientContent, or use AddSerializedContent if it has no service dependencies.");
                }

                // The implementing type depends on services, so it must be constructed through
                // DI (resolved as a keyed service by its content type) and then populated from
                // the JSON, since System.Text.Json cannot construct it.
                object instance = this.serviceProvider.GetRequiredKeyedService(registration.ImplementingType, registration.ContentType);
                DiConstructedContentDeserializer.DeserializeInto(instance, registration.ImplementingType, root, options);
                return (TTarget)instance;
            }

            // The implementing type has no service dependencies, so let System.Text.Json do the
            // work. Deserializing as the concrete type (never as TTarget) is what prevents this
            // converter from being invoked recursively.
            return (TTarget?)root.Deserialize(registration.ImplementingType, options);
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, TTarget value, JsonSerializerOptions options)
        {
            if (value.GetType() == typeof(TTarget))
            {
                throw new NotSupportedException($"When using polymorphic contentType-driven serialization, the runtime type must be different from the target type. (Here, both are {typeof(TTarget).Name}.) Consider using an interface or abstract base class as the target type.");
            }

            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
