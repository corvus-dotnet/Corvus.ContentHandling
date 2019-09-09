// <copyright file="ContentFactoryServiceCollectionExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Microsoft.Extensions.DependencyInjection
{
    using System;
    using System.Linq;
    using Corvus.ContentHandling;
    using Corvus.ContentHandling.Internal;

    /// <summary>
    /// Register a content factory with a service collection.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This offers extensions to the <see cref="IServiceCollection"/> to provide a content factory.
    /// </para>
    /// </remarks>
    public static class ContentFactoryServiceCollectionExtensions
    {
        /// <summary>
        /// Add content to the content factory.
        /// </summary>
        /// <param name="serviceCollection">The service collection with which to register content handlers.</param>
        /// <param name="configure">Configure the content factory.</param>
        /// <returns>An instance of the content factory for initialization.</returns>
        public static IServiceCollection AddContent(this IServiceCollection serviceCollection, Action<ContentFactory> configure)
        {
            ContentFactory contentFactory;
            ServiceDescriptor contentFactoryDescriptor = serviceCollection.Where(s => typeof(ContentFactory).IsAssignableFrom(s.ServiceType)).FirstOrDefault();
            if (!(contentFactoryDescriptor is null))
            {
                contentFactory = (ContentFactory)contentFactoryDescriptor.ImplementationInstance;
            }
            else
            {
                contentFactory = new ContentFactory(serviceCollection);
                serviceCollection.AddSingleton(contentFactory);

                // Register the generic factory for content handler dispatchers
                serviceCollection.Add(ServiceDescriptor.Singleton(typeof(IContentHandlerDispatcher<>), typeof(ContentHandlerDispatcher<>)));
            }

            configure(contentFactory);
            return serviceCollection;
        }
    }
}