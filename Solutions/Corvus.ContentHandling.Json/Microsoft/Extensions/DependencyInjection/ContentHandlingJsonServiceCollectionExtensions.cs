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
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// An installer for standard <see cref="JsonConverter"/>s.
    /// </summary>
    public static class ContentHandlingJsonServiceCollectionExtensions
    {
        /// <summary>
        /// Adds custom JsonConverters to the service collection to support content type-based serialization.
        /// </summary>
        /// <param name="services">The target service collection.</param>
        /// <param name="configure">Configure the content serialization.</param>
        /// <returns>The service collection.</returns>
        /// <remarks>
        /// <para>
        /// Also adds the Corvus.Extensions.Newtonsoft.Json <see cref="Corvus.Extensions.Json.IJsonSerializerSettingsProvider"/>
        /// and the four JsonConverters provided by that library to the collection (if not already present).
        /// </para>
        /// <para>
        /// Version 2 of Corvus.Extensions.Newtonsoft.Json provides separate methods to register each of the JsonConverters
        /// it provides. As such, this method is replaced with <see cref="AddContentTypeBasedSerializationSupport"/>; calling code
        /// should switch to that method and register any required JsonConverters separately.
        /// </para>
        /// </remarks>
        [Obsolete("Use AddContentTypeBasedSerializationSupport and register JsonConverters separately (see remarks).")]
        public static IServiceCollection AddContentSerialization(this IServiceCollection services, Action<ContentFactory> configure = null)
        {
            if (services.Any(s => s.ImplementationType == typeof(ContentTypeConverter)))
            {
                return services;
            }

            services.AddJsonNetSerializerSettingsProvider();
            services.AddJsonNetPropertyBag();
            services.AddJsonNetCultureInfoConverter();
            services.AddJsonNetDateTimeOffsetToIso8601AndUnixTimeConverter();
            services.AddSingleton<JsonConverter>(new StringEnumConverter(true));

            services.AddSingleton<JsonConverter, ContentTypeConverter>();
            services.AddSingleton<JsonConverter, ContentEnvelopeConverter>();
            services.AddContent(configure);
            return services;
        }

        /// <summary>
        /// Add the default JSON serialization settings.
        /// </summary>
        /// <param name="services">The target service collection.</param>
        /// <returns>The service collection.</returns>
        /// <remarks>
        /// <para>
        /// Adds custom JsonConverters to the service collection to support content type-based serialization. This
        /// relies on the Corvus.Extensions.Newtonsoft.Json JsonSerializationSettingsProvider to make these converters
        /// available to the Json.Net serializers, so calls <see cref="JsonSerializerSettingsProviderServiceCollectionExtensions.AddJsonNetSerializerSettingsProvider" />.
        /// </para>
        /// <para>
        /// When moving to this method from <see cref="AddContentSerialization"/>, you should also explicitly add
        /// any of the JsonConverters provided by Corvus.Extensions.Newtonsoft.Json that you require, as this method
        /// does not register them automatically. The Obsolete method did the equivalent of the following lines of code:
        /// </para>
        /// <code>
        /// <![CDATA[
        /// services.AddJsonNetPropertyBag();
        /// services.AddJsonNetCultureInfoConverter();
        /// services.AddJsonNetDateTimeOffsetToIso8601AndUnixTimeConverter();
        /// services.AddSingleton<JsonConverter>(new StringEnumConverter(true));
        /// ]]>
        /// </code>
        /// <para>
        /// Note also that this method no longer has a parameter allowing a callback to be provided to register content
        /// types with the content factory. Instead, you should call <c>services.AddContent()</c> directly to register
        /// your types.
        /// </para>
        /// </remarks>
        public static IServiceCollection AddContentTypeBasedSerializationSupport(this IServiceCollection services)
        {
            services.AddJsonNetSerializerSettingsProvider();

            if (!services.Any(s => s.ServiceType == typeof(ContentFactory)))
            {
                services.AddSingleton(new ContentFactory(services));
            }

            if (!services.Any(s => s.ServiceType == typeof(ContentTypeConverter)))
            {
                services.AddSingleton<JsonConverter, ContentTypeConverter>();
                services.AddSingleton<JsonConverter, ContentEnvelopeConverter>();
            }

            return services;
        }
    }
}