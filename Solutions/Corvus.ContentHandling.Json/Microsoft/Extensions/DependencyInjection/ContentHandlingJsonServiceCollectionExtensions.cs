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
        /// <remarks>
        /// <para>
        /// Adds custom JsonConverters to the service collection to support content type-based serialization. This
        /// relies on the Corvus.Extensions.Newtonsoft.Json JsonSerializationSettingsProvider to make these converters
        /// available to the Json.Net serializers, so calls <see cref="JsonSerializerSettingsProviderServiceCollectionExtensions.AddJsonNetSerializerSettingsProvider" />.
        /// </para>
        /// <para>
        /// Prior to v2, this also resulted in all of the JsonConverters provided by Corvus.Extensions.Newtonsoft.Json
        /// being registered. This is no longer the case, and any code that was relying on this method to make those
        /// converters available now needs to add them explicitly.
        /// </para>
        /// </remarks>
        public static IServiceCollection AddContentSerialization(this IServiceCollection services, Action<ContentFactory> configure = null)
        {
            if (services.Any(s => s.ImplementationType == typeof(ContentTypeConverter)))
            {
                return services;
            }

            services.AddJsonNetSerializerSettingsProvider();
            services.AddSingleton<JsonConverter, ContentTypeConverter>();
            services.AddSingleton<JsonConverter, ContentEnvelopeConverter>();
            services.AddContent(configure);
            return services;
        }
    }
}