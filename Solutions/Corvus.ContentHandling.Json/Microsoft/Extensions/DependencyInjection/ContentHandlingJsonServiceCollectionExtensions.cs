// <copyright file="ContentHandlingJsonServiceCollectionExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Microsoft.Extensions.DependencyInjection
{
    using System.Linq;
    using System.Text.Json.Serialization;

    using Corvus.ContentHandling;
    using Corvus.ContentHandling.Json.Internal;

    /// <summary>
    /// An installer for standard <see cref="JsonConverter"/>s.
    /// </summary>
    public static class ContentHandlingJsonServiceCollectionExtensions
    {
        /// <summary>
        /// Add the default JSON serialization configuration.
        /// </summary>
        /// <param name="services">The target service collection.</param>
        /// <returns>The service collection.</returns>
        /// <remarks>
        /// <para>
        /// Adds custom JsonConverters to the service collection to support content type-based serialization. This
        /// relies on the Corvus.Json.Serialization IJsonSerializerOptionsProvider to make these converters
        /// available to the JSON serializers, so calls <see cref="CorvusJsonSerializationServiceCollectionExtensions.AddJsonSerializerOptionsProvider" />.
        /// </para>
        /// <para>
        /// You may also want to add any of the JsonConverters provided by Corvus that you require, as this method
        /// does not register them automatically. For example, if you want property bags and culture info handling,
        /// you would write this:
        /// </para>
        /// <code>
        /// <![CDATA[
        /// services
        ///     .AddJsonSerializerSettingsProvider()
        ///     .AddJsonPropertyBagFactory()
        ///     .AddJsonCultureInfoConverter()
        ///     .AddSingleton<JsonConverter>(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)));
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
            services.AddJsonSerializerOptionsProvider();

            if (!services.Any(s => s.ServiceType == typeof(ContentFactory)))
            {
                services.AddSingleton(new ContentFactory(services));
            }

            if (!services.Any(s => s.ServiceType == typeof(ContentEnvelopeConverter)))
            {
                services.AddSingleton<JsonConverter, ContentEnvelopeConverter>();
            }

            return services;
        }
    }
}