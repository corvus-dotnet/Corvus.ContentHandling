// <copyright file="ContentHandlingServiceCollectionExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Microsoft.Extensions.DependencyInjection
{
    using System;
    using System.Linq;
    using Corvus.ContentHandling;
    using Corvus.ContentHandling.Internal;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    /// <summary>
    /// Adds the content handling framework to a service collection.
    /// </summary>
    public static class ContentHandlingServiceCollectionExtensions
    {
        /// <summary>
        /// Adds content handling to the service collection, and optionally registers content.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configure">A callback registering content and content handlers.</param>
        /// <returns>The service collection, for chaining.</returns>
        /// <remarks>
        /// <para>
        /// This method can be called any number of times; all calls share a single content
        /// registry. Content is registered as keyed services (keyed by content type), so all
        /// registration must happen before the service provider is built.
        /// </para>
        /// </remarks>
        public static IServiceCollection AddContentHandling(this IServiceCollection services, Action<ContentHandlingBuilder>? configure = null)
        {
            ArgumentNullException.ThrowIfNull(services);

            ServiceDescriptor? existing = services.FirstOrDefault(
                s => !s.IsKeyedService && s.ServiceType == typeof(IContentRegistry));

            if (existing?.ImplementationInstance is not ContentRegistry registry)
            {
                registry = new ContentRegistry();
                services.AddSingleton<IContentRegistry>(registry);
                services.TryAdd(ServiceDescriptor.Transient(typeof(IContentDispatcher<>), typeof(ContentDispatcher<>)));
            }

            configure?.Invoke(new ContentHandlingBuilder(services, registry));
            return services;
        }
    }
}
