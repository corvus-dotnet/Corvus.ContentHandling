// <copyright file="SerializationSampleContentServiceCollectionExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling
{
    using Corvus.ContentHandling.Json.Specs.Samples;

    /// <summary>
    /// Installs sample content.
    /// </summary>
    public static class SerializationSampleContentServiceCollectionExtensions
    {
        /// <summary>
        /// Register sample content in the container.
        /// </summary>
        /// <param name="contentFactory">The content factory in which to install the content.</param>
        /// <returns>The service collection.</returns>
        public static ContentFactory AddSampleContent(this ContentFactory contentFactory)
        {
            contentFactory.RegisterTransientContent<SomeContentWithInterface>();
            contentFactory.RegisterTransientContent<SomeContentWithInterfaceAndChild>();
            contentFactory.RegisterTransientContent<SomeContentWithInterfaceAndPocChild>();
            contentFactory.RegisterContent<SomeContentWithAbstractBaseAndPocChildCtorInitialized>();

            contentFactory.RegisterTransientContent<SomeContentWithAbstractBase>();
            contentFactory.RegisterTransientContent<SomeContentWithAbstractBaseAndChild>();
            contentFactory.RegisterTransientContent<SomeContentWithAbstractBaseAndPocChild>();

            contentFactory.RegisterTransientContent<SomeContentWithBase>();
            contentFactory.RegisterTransientContent<SomeContentWithBaseAndChild>();
            contentFactory.RegisterTransientContent<SomeContentWithBaseAndPocChild>();
            return contentFactory;
        }
    }
}