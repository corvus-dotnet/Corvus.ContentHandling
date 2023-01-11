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
            // When the following types are serialization targets (either properties on objects
            // being deserialized, or the type specified when calling deserialization APIs),
            // the use of RegisterPolymorphicContentTarget causes a custom converter to kick in
            // which inspects the contentType property of the source JSON to work out which
            // concrete type to use.
            // (Note that because we support .NET 6.0, and due to the absence of general support
            // for polymorphic deserialization prior to .NET 7.0, there are limitations on what we
            // can make work. As a result, if you register a concrete type as a polymorphic
            // content target, you will not be able to deserialize instances of that concrete type.
            // You will only be able to deserialize to types that derive from that type.)
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

            // This tests that content types can be initialized with dependencies. This
            // works only on .NET 7.0 or later, because .NET 6.0 doesn't provide the necessary
            // API features.
            contentFactory.RegisterTransientContent<SomeContentRequiringDiInitialization>();
            return contentFactory;
        }
    }
}