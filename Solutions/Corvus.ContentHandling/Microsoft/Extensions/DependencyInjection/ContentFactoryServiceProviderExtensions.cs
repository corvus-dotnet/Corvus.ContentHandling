﻿// <copyright file="ContentFactoryServiceProviderExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Microsoft.Extensions.DependencyInjection
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using Corvus.ContentHandling;

    /// <summary>
    /// Generates instances for types identified by a content-type string.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This offers extensions to the <see cref="IServiceProvider"/> to provide
    /// instances of types identified by a "application/vnd.Corvus.some.space+suffix" content string.
    /// </para>
    /// <para>
    /// This is typically used for entities that need extensible, polymorphic serialization, or other
    /// similar content-specific services.
    /// </para>
    /// <para>
    /// Types that wish to participate must add a <c>const string</c> field called <c>RegisteredContentType</c>
    /// and a property <c>ContentType</c> which returns the same value. You do not need to implement any particular interface or
    /// derive from any base class.
    /// </para>
    /// </remarks>
    public static class ContentFactoryServiceProviderExtensions
    {
        /// <summary>
        /// Gets instances of all content registered with a particular suffix.
        /// </summary>
        /// <typeparam name="T">The type of the content.</typeparam>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="suffix">The suffix to query.</param>
        /// <returns>An enumerable of instances of the required content.</returns>
        public static IEnumerable<T> GetAllContent<T>(this IServiceProvider serviceProvider, string suffix)
            where T : class
        {
            ConcurrentDictionary<string, Type> typesForNamedContent = serviceProvider.GetRequiredService<ContentFactory>().Handlers;
            return typesForNamedContent.Keys.Where(k => k.EndsWith(suffix)).Select(k => GetRequiredContent<T>(serviceProvider, k));
        }

        /// <summary>
        /// Gets all content types registered with a particular suffix.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="suffix">The suffix to query.</param>
        /// <returns>An enumerable of strings representing the content type with the specified suffix.</returns>
        public static IEnumerable<string> GetAllContentTypes(this IServiceProvider serviceProvider, string suffix)
        {
            ConcurrentDictionary<string, Type> typesForNamedContent = serviceProvider.GetRequiredService<ContentFactory>().Handlers;

            return typesForNamedContent.Keys.Where(k => k.EndsWith(suffix));
        }

        /// <summary>
        /// Get the service for a particular content type.
        /// </summary>
        /// <typeparam name="T">The type of the service.</typeparam>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns>An instance of the required content, or null if no content is registered.</returns>
        /// <remarks>The type must provide a static/const string called. <c>RegisteredContentType</c> which defines its content type.</remarks>
        public static T GetContent<T>(this IServiceProvider serviceProvider)
            where T : class
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            string name = ContentFactory.GetContentType(typeof(T));
            return GetContent<T>(serviceProvider, name);
        }

        /// <summary>
        /// Get the service for a particular content type.
        /// </summary>
        /// <typeparam name="T">The type of the service.</typeparam>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="contentType">The content type.</param>
        /// <returns>An instance of the required content, or null if no content is registered.</returns>
        public static T GetContent<T>(this IServiceProvider serviceProvider, string contentType)
            where T : class
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            if (string.IsNullOrEmpty(contentType))
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (!TryGetTypeFor(serviceProvider, contentType, out Type serviceType))
            {
                return null;
            }

            return serviceProvider.GetService(serviceType) as T;
        }

        /// <summary>
        /// Get the service for a particular content type.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="contentType">The content type.</param>
        /// <returns>An instance of the required content, or null if no content is registered.</returns>
        public static object GetContent(this IServiceProvider serviceProvider, string contentType)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            if (string.IsNullOrEmpty(contentType))
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (!TryGetTypeFor(serviceProvider, contentType, out Type serviceType))
            {
                return null;
            }

            return serviceProvider.GetService(serviceType);
        }

        /// <summary>
        /// Get the service for a particular content type.
        /// </summary>
        /// <typeparam name="T">The type of the service.</typeparam>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns>An instance of the required content. It throws if no content is registered.</returns>
        /// <remarks>The type must provide a static/const string called. <c>RegisteredContentType</c> which defines its content type.</remarks>
        public static T GetRequiredContent<T>(this IServiceProvider serviceProvider)
            where T : class
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            string name = ContentFactory.GetContentType(typeof(T));
            return GetRequiredContent<T>(serviceProvider, name);
        }

        /// <summary>
        /// Get the service for a particular content type.
        /// </summary>
        /// <typeparam name="T">The type of the service.</typeparam>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="contentType">The required content type.</param>
        /// <returns>An instance of the required content. It throws if no content is registered.</returns>
        public static T GetRequiredContent<T>(this IServiceProvider serviceProvider, string contentType)
            where T : class
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            if (string.IsNullOrEmpty(contentType))
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (!TryGetTypeFor(serviceProvider, contentType, out Type serviceType))
            {
                throw new InvalidOperationException(string.Format(Resources.NoNamedServiceRegistered, contentType));
            }

            if (!(serviceProvider.GetRequiredService(serviceType) is T service))
            {
                throw new InvalidOperationException(string.Format(Resources.NamedServiceNotOfType, contentType, typeof(T)));
            }

            return service;
        }

        /// <summary>
        /// Get the service for a particular content type.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="contentType">The required content type.</param>
        /// <returns>An instance of the required content. It throws if no content is registered.</returns>
        public static object GetRequiredContent(this IServiceProvider serviceProvider, string contentType)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            if (string.IsNullOrEmpty(contentType))
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (!TryGetTypeFor(serviceProvider, contentType, out Type serviceType))
            {
                throw new InvalidOperationException(string.Format(Resources.NoNamedServiceRegistered, contentType));
            }

            return serviceProvider.GetRequiredService(serviceType);
        }

        /// <summary>
        /// Gets the registered type with the given name.
        /// </summary>
        /// <param name="serviceProvider">The service provider containing the services.</param>
        /// <param name="contentType">
        /// The content type to look up.
        /// </param>
        /// <param name="serviceType">
        /// The registered type for the given content type.
        /// </param>
        /// <returns>
        /// A boolean indicating whether or not a type was found.
        /// </returns>
        public static bool TryGetTypeFor(this IServiceProvider serviceProvider, string contentType, out Type serviceType)
        {
            ContentFactory typesForNamedContent = serviceProvider.GetRequiredService<ContentFactory>();
            if (!typesForNamedContent.Handlers.TryGetValue(contentType, out serviceType))
            {
                int indexOfPlusSuffix = contentType.LastIndexOf('+');
                string suffix = string.Empty;

                if (indexOfPlusSuffix >= 0)
                {
                    suffix = contentType.Substring(indexOfPlusSuffix);
                }

                int indexOfLastDot = contentType.LastIndexOf('.');
                if (indexOfLastDot > 0)
                {
                    return TryGetTypeFor(serviceProvider, contentType.Substring(0, indexOfLastDot) + suffix, out serviceType);
                }

                return false;
            }

            return true;
        }
    }
}
