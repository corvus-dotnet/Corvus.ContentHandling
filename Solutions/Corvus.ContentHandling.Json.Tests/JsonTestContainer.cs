// <copyright file="JsonTestContainer.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json.Tests
{
    using System;
    using System.Text.Json;
    using Corvus.ContentHandling;
    using Corvus.Json.Serialization;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Builds the standard container used across the JSON tests, mirroring the registration
    /// pattern an application would use.
    /// </summary>
    internal static class JsonTestContainer
    {
        public static ServiceProvider Build(Action<IServiceCollection>? configureServices = null, Action<ContentHandlingBuilder>? configureContent = null)
        {
            var services = new ServiceCollection();

            services.AddContentTypeBasedJsonSerializationSupport();
            services
                .AddPolymorphicContentTarget<ISomeContentInterface>()
                .AddPolymorphicContentTarget<SomeContentAbstractBase>()
                .AddPolymorphicContentTarget<SomeContentBase>();

            services.AddContentHandling(content => content
                .AddSerializedContent<InterfaceContent>()
                .AddSerializedContent<InterfaceContentWithChild>()
                .AddSerializedContent<InterfaceContentWithPocChild>()
                .AddSerializedContent<InterfaceContentWithCtorInitializedPocChild>()
                .AddSerializedContent<AbstractBaseContent>()
                .AddSerializedContent<DerivedFromBaseContent>()
                .AddSerializedContent<ContentWithDictionary>()
                .AddSerializedContent<ContentWithEnum>()
                .AddTransientContent<DiInitializedContent>());

            configureServices?.Invoke(services);
            if (configureContent is not null)
            {
                services.AddContentHandling(configureContent);
            }

            return services.BuildServiceProvider();
        }

        public static JsonSerializerOptions GetOptions(this IServiceProvider provider)
        {
            return provider.GetRequiredService<IJsonSerializerOptionsProvider>().Instance;
        }
    }
}
