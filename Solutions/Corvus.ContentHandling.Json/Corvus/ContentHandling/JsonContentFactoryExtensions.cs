// <copyright file="JsonContentFactoryExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling;

using System;
using System.Text.Json.Serialization;

using Corvus.ContentHandling.Json.Internal;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for <see cref="ContentFactory"/> enabling JSON-specific registration.
/// </summary>
public static class JsonContentFactoryExtensions
{
    /// <summary>
    /// Register a target type for a content type that does not depend on services, and which
    /// can be initialized directly through deserialization.
    /// </summary>
    /// <typeparam name="T">The type for which polymorphic deserialization is required.</typeparam>
    /// <param name="contentFactory">The content factory.</param>
    /// <returns>The content factory, enabling calls to be chained.</returns>
    public static ContentFactory RegisterPolymorphicContentTarget<T>(this ContentFactory contentFactory)
        where T : class
    {
        if (contentFactory == null)
        {
            throw new ArgumentNullException(nameof(contentFactory));
        }

        contentFactory.Services.AddSingleton<JsonConverter, PolymorphicTargetConverter<T>>();

        return contentFactory;
    }
}