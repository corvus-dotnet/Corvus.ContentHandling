// <copyright file="ContentHandlingJsonServiceCollectionExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Microsoft.Extensions.DependencyInjection
{
    using System;
    using System.Linq;
    using System.Text.Json.Serialization;
    using Corvus.ContentHandling;
    using Corvus.ContentHandling.Json.Internal;

    /// <summary>
    /// Adds content-type-based JSON serialization support to a service collection.
    /// </summary>
    public static class ContentHandlingJsonServiceCollectionExtensions
    {
        /// <summary>
        /// Adds content-type-based JSON serialization support.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The service collection, for chaining.</returns>
        /// <remarks>
        /// <para>
        /// This registers the <see cref="Corvus.ContentHandling.Json.ContentEnvelope"/> converter
        /// and the Corvus.Json.Serialization <c>IJsonSerializerOptionsProvider</c>, which
        /// aggregates all <see cref="JsonConverter"/>s registered with the service collection
        /// into the application's <c>JsonSerializerOptions</c>.
        /// </para>
        /// <para>
        /// Register content via <c>services.AddContentHandling(...)</c>, and polymorphic
        /// serialization targets via <see cref="AddPolymorphicContentTarget{TTarget}(IServiceCollection)"/>.
        /// </para>
        /// </remarks>
        public static IServiceCollection AddContentTypeBasedJsonSerializationSupport(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.AddJsonSerializerOptionsProvider();
            services.AddContentHandling();

            if (!services.Any(s => !s.IsKeyedService && s.ImplementationType == typeof(ContentEnvelopeConverter)))
            {
                services.AddSingleton<JsonConverter, ContentEnvelopeConverter>();
            }

            return services;
        }

        /// <summary>
        /// Enables polymorphic contentType-driven serialization for properties of type
        /// <typeparamref name="TTarget"/>.
        /// </summary>
        /// <typeparam name="TTarget">
        /// The target type: an interface or base type of registered content types. The concrete
        /// registered types must differ from the target type.
        /// </typeparam>
        /// <param name="services">The service collection.</param>
        /// <returns>The service collection, for chaining.</returns>
        public static IServiceCollection AddPolymorphicContentTarget<TTarget>(this IServiceCollection services)
            where TTarget : class
        {
            ArgumentNullException.ThrowIfNull(services);

            if (!services.Any(s => !s.IsKeyedService && s.ImplementationType == typeof(PolymorphicContentConverter<TTarget>)))
            {
                services.AddSingleton<JsonConverter, PolymorphicContentConverter<TTarget>>();
            }

            return services;
        }

        /// <summary>
        /// Enables polymorphic contentType-driven serialization for properties of type
        /// <typeparamref name="TTarget"/>.
        /// </summary>
        /// <typeparam name="TTarget">
        /// The target type: an interface or base type of registered content types. The concrete
        /// registered types must differ from the target type.
        /// </typeparam>
        /// <param name="builder">The content handling builder.</param>
        /// <returns>The builder, for chaining.</returns>
        public static ContentHandlingBuilder AddPolymorphicContentTarget<TTarget>(this ContentHandlingBuilder builder)
            where TTarget : class
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.Services.AddPolymorphicContentTarget<TTarget>();
            return builder;
        }
    }
}
