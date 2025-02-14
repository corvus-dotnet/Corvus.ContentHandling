﻿// <copyright file="ContentHandlingContainerBindings.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json.Specs
{
    using Corvus.ContentHandling.Json.Specs.Samples;
    using Corvus.Testing.ReqnRoll;

    using Microsoft.Extensions.DependencyInjection;

    using Reqnroll;

    /// <summary>
    /// Provides Specflow bindings for Endjin Composition.
    /// </summary>
    [Binding]
    public static class ContentHandlingContainerBindings
    {
        /// <summary>
        /// Setup the endjin container for a feature.
        /// </summary>
        /// <remarks>We expect features run in parallel to be executing in separate app domains.</remarks>
        /// <param name="featureContext">The SpecFlow test context.</param>
        [BeforeFeature("@perFeatureContainer", Order = ContainerBeforeFeatureOrder.PopulateServiceCollection)]
        public static void SetupFeature(FeatureContext featureContext)
        {
            ContainerBindings.ConfigureServices(
                featureContext,
                serviceCollection =>
                {
                    serviceCollection.AddContentTypeBasedSerializationSupport();
                    serviceCollection.AddContent(contentFactory => contentFactory.AddSampleContent());

                    // Second call to test our guard against multiple registration calls each creating new instances
                    // of ContentFactory and losing previous content registrations.
                    serviceCollection.AddContentTypeBasedSerializationSupport();
                });
        }
    }
}