// <copyright file="DefaultJsonSerializationSettingsServiceCollectionExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Microsoft.Extensions.DependencyInjection
{
    using Corvus.ContentHandling.Json;
    using Corvus.ContentHandling.Json.Internal;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// An installer for standard <see cref="JsonConverter"/>s.
    /// </summary>
    public static class DefaultJsonSerializationSettingsServiceCollectionExtensions
    {
        /// <summary>
        /// Add the default JSON serialization settings.
        /// </summary>
        /// <param name="services">The target service collection.</param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection AddDefaultJsonSerializationSettings(this IServiceCollection services)
        {
            services.AddSingleton<JsonConverter, ContentTypeConverter>();
            services.AddSingleton<JsonConverter, CultureInfoConverter>();
            services.AddSingleton<JsonConverter, DateTimeOffsetConverter>();
            services.AddSingleton<JsonConverter, PropertyBagConverter>();
            services.AddSingleton<JsonConverter>(new StringEnumConverter(new CamelCaseNamingStrategy()));
            services.AddSingleton<IDefaultJsonSerializerSettings, DefaultJsonSerializerSettings>();
            return services;
        }
    }
}