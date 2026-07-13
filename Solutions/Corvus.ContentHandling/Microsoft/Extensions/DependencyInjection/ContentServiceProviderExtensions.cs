// <copyright file="ContentServiceProviderExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Microsoft.Extensions.DependencyInjection
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using Corvus.ContentHandling;

    /// <summary>
    /// Resolves instances of types identified by a content-type string.
    /// </summary>
    /// <remarks>
    /// <para>
    /// These extensions resolve content registered via <c>AddContentHandling</c>. Resolution
    /// applies hierarchical content-type fallback: if <c>application/vnd.corvus.a.b.c+suffix</c>
    /// is not registered, <c>application/vnd.corvus.a.b+suffix</c> is tried, and so on.
    /// </para>
    /// <para>
    /// Content registered with <see cref="ContentConstruction.FromServices"/> is resolved as a
    /// keyed service (keyed by its content type); content registered with
    /// <see cref="ContentConstruction.BySerializer"/> is constructed via its public
    /// parameterless constructor.
    /// </para>
    /// </remarks>
    public static class ContentServiceProviderExtensions
    {
        /// <summary>
        /// Gets an instance of the content registered for the content type derived from
        /// <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the content.</typeparam>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns>An instance of the content, or null if the content type is not registered.</returns>
        public static T? GetContent<T>(this IServiceProvider serviceProvider)
            where T : class
        {
            return serviceProvider.GetContent<T>(ContentTypes.GetContentType<T>());
        }

        /// <summary>
        /// Gets an instance of the content registered for a content type.
        /// </summary>
        /// <typeparam name="T">The type of the content.</typeparam>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="contentType">The content type.</param>
        /// <returns>An instance of the content, or null if the content type is not registered.</returns>
        public static T? GetContent<T>(this IServiceProvider serviceProvider, string contentType)
            where T : class
        {
            return serviceProvider.GetContent(contentType) as T;
        }

        /// <summary>
        /// Gets an instance of the content registered for a content type.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="contentType">The content type.</param>
        /// <returns>An instance of the content, or null if the content type is not registered.</returns>
        public static object? GetContent(this IServiceProvider serviceProvider, string contentType)
        {
            ArgumentNullException.ThrowIfNull(serviceProvider);
            ArgumentException.ThrowIfNullOrEmpty(contentType);

            IContentRegistry registry = serviceProvider.GetRequiredService<IContentRegistry>();
            if (!registry.TryResolve(contentType, out ContentRegistration? registration))
            {
                return null;
            }

            return serviceProvider.GetContent(registration);
        }

        /// <summary>
        /// Gets an instance of the content registered for the content type derived from
        /// <typeparamref name="T"/>, throwing if the content type is not registered.
        /// </summary>
        /// <typeparam name="T">The type of the content.</typeparam>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns>An instance of the content.</returns>
        public static T GetRequiredContent<T>(this IServiceProvider serviceProvider)
            where T : class
        {
            return serviceProvider.GetRequiredContent<T>(ContentTypes.GetContentType<T>());
        }

        /// <summary>
        /// Gets an instance of the content registered for a content type, throwing if the
        /// content type is not registered.
        /// </summary>
        /// <typeparam name="T">The type of the content.</typeparam>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="contentType">The content type.</param>
        /// <returns>An instance of the content.</returns>
        /// <exception cref="InvalidOperationException">
        /// The content type is not registered, or the registered content is not of type
        /// <typeparamref name="T"/>.
        /// </exception>
        public static T GetRequiredContent<T>(this IServiceProvider serviceProvider, string contentType)
            where T : class
        {
            object result = serviceProvider.GetRequiredContent(contentType);
            if (result is not T content)
            {
                throw new InvalidOperationException($"The content registered for content type '{contentType}' is not of type '{typeof(T)}'.");
            }

            return content;
        }

        /// <summary>
        /// Gets an instance of the content registered for a content type, throwing if the
        /// content type is not registered.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="contentType">The content type.</param>
        /// <returns>An instance of the content.</returns>
        /// <exception cref="InvalidOperationException">The content type is not registered.</exception>
        public static object GetRequiredContent(this IServiceProvider serviceProvider, string contentType)
        {
            ArgumentNullException.ThrowIfNull(serviceProvider);
            ArgumentException.ThrowIfNullOrEmpty(contentType);

            IContentRegistry registry = serviceProvider.GetRequiredService<IContentRegistry>();
            if (!registry.TryResolve(contentType, out ContentRegistration? registration))
            {
                throw new InvalidOperationException($"No content is registered for content type '{contentType}'.");
            }

            return serviceProvider.GetContent(registration)
                ?? throw new InvalidOperationException($"The content registered for content type '{contentType}' could not be resolved.");
        }

        /// <summary>
        /// Gets instances of all content registered with content types ending in a particular suffix.
        /// </summary>
        /// <typeparam name="T">The type of the content.</typeparam>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="suffix">The suffix to match.</param>
        /// <returns>An instance of each matching registered content type.</returns>
        public static IEnumerable<T> GetAllContent<T>(this IServiceProvider serviceProvider, string suffix)
            where T : class
        {
            ArgumentNullException.ThrowIfNull(serviceProvider);
            ArgumentNullException.ThrowIfNull(suffix);

            return serviceProvider
                .GetRequiredService<IContentRegistry>()
                .GetContentTypes(suffix)
                .Select(serviceProvider.GetRequiredContent<T>);
        }

        /// <summary>
        /// Gets all registered content types ending in a particular suffix.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="suffix">The suffix to match.</param>
        /// <returns>The matching content types.</returns>
        public static IEnumerable<string> GetAllContentTypes(this IServiceProvider serviceProvider, string suffix)
        {
            ArgumentNullException.ThrowIfNull(serviceProvider);
            ArgumentNullException.ThrowIfNull(suffix);

            return serviceProvider.GetRequiredService<IContentRegistry>().GetContentTypes(suffix);
        }

        /// <summary>
        /// Gets the implementing type registered for a content type, applying hierarchical fallback.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="contentType">The content type to look up.</param>
        /// <param name="implementingType">The implementing type registered for the content type.</param>
        /// <returns>True if a registration was found.</returns>
        public static bool TryGetTypeFor(this IServiceProvider serviceProvider, string contentType, [NotNullWhen(true)] out Type? implementingType)
        {
            return serviceProvider.TryGetTypeFor(contentType, out implementingType, out _);
        }

        /// <summary>
        /// Gets the implementing type registered for a content type, applying hierarchical fallback.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="contentType">The content type to look up.</param>
        /// <param name="implementingType">The implementing type registered for the content type.</param>
        /// <param name="construction">How instances of the content are constructed.</param>
        /// <returns>True if a registration was found.</returns>
        public static bool TryGetTypeFor(
            this IServiceProvider serviceProvider,
            string contentType,
            [NotNullWhen(true)] out Type? implementingType,
            out ContentConstruction construction)
        {
            ArgumentNullException.ThrowIfNull(serviceProvider);
            ArgumentException.ThrowIfNullOrEmpty(contentType);

            IContentRegistry registry = serviceProvider.GetRequiredService<IContentRegistry>();
            if (registry.TryResolve(contentType, out ContentRegistration? registration))
            {
                implementingType = registration.ImplementingType;
                construction = registration.Construction;
                return true;
            }

            implementingType = null;
            construction = default;
            return false;
        }

        private static object? GetContent(this IServiceProvider serviceProvider, ContentRegistration registration)
        {
            if (registration.Construction == ContentConstruction.FromServices)
            {
                return serviceProvider.GetKeyedService(registration.ImplementingType, registration.ContentType);
            }

            ConstructorInfo? constructor = registration.ImplementingType.GetConstructor(Type.EmptyTypes);
            if (constructor is null)
            {
                throw new InvalidOperationException($"The implementing type '{registration.ImplementingType}' for content type '{registration.ContentType}' does not have a public parameterless constructor.");
            }

            return constructor.Invoke([]);
        }
    }
}
