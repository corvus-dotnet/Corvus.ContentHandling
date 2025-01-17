// <copyright file="SerializationSampleContentServiceCollectionExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json.Specs.Samples
{
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
            // When the following types are serialization targets (either properties on objects
            // being deserialized, or the type specified when calling deserialization APIs),
            // the use of RegisterPolymorphicContentTarget causes a custom converter to kick in
            // which inspects the contentType property of the source JSON to work out which
            // concrete type to use.
            contentFactory.RegisterPolymorphicContentTarget<ISomeContentInterface>();
            contentFactory.RegisterPolymorphicContentTarget<SomeContentAbstractBase>();
            contentFactory.RegisterPolymorphicContentTarget<SomeContentBase>();

            contentFactory.RegisterContent<SomeContentWithInterface>();
            contentFactory.RegisterContent<SomeContentWithInterfaceAndChild>();
            contentFactory.RegisterContent<SomeContentWithInterfaceAndPocChild>();
            contentFactory.RegisterContent<SomeContentWithAbstractBaseAndPocChildCtorInitialized>();

            contentFactory.RegisterContent<SomeContentWithAbstractBase>();
            contentFactory.RegisterContent<SomeContentWithAbstractBaseAndChild>();
            contentFactory.RegisterContent<SomeContentWithAbstractBaseAndPocChild>();

            contentFactory.RegisterContent<SomeContentWithBase>();
            contentFactory.RegisterContent<SomeContentWithBaseAndChild>();
            contentFactory.RegisterContent<SomeContentWithBaseAndPocChild>();

            contentFactory.RegisterTransientContent<SomeContentRequiringDiInitialization>();
            return contentFactory;
        }
    }
}