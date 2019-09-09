// <copyright file="ContentHandlingJsonServiceCollectionExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Microsoft.Extensions.DependencyInjection
{
    using System;
    using System.Linq;
    using Corvus.ContentHandling;
    using Corvus.ContentHandling.Json.Internal;
    using Newtonsoft.Json;

    /// <summary>
    /// An installer for standard <see cref="JsonConverter"/>s.
    /// </summary>
    public static class ContentHandlingJsonServiceCollectionExtensions
    {
        /// <summary>
        /// Add the default JSON serialization settings.
        /// </summary>
        /// <param name="services">The target service collection.</param>
        /// <param name="configure">Configure the content serialization.</param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection AddContentSerialization(this IServiceCollection services, Action<ContentFactory> configure = null)
        {
            if (services.Any(s => s.ImplementationType == typeof(ContentTypeConverter)))
            {
                return services;
            }

            services.AddJsonSerializerSettings();
            services.AddSingleton<JsonConverter, ContentTypeConverter>();
            services.AddSingleton<JsonConverter, ContentEnvelopeConverter>();
            services.AddContent(configure);
            return services;
        }
    }
}