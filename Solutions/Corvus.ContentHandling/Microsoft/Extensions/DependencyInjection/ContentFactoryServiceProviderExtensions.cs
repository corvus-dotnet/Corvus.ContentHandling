// <copyright file="ContentFactoryServiceProviderExtensions.cs" company="Endjin Limited">
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
            IEnumerable<string> registeredContentTypes = serviceProvider
                .GetRequiredService<ContentFactory>()
                .GetRegisteredContentTypes()
                .Select(kv => kv.Key);
            return registeredContentTypes.Where(k => k.EndsWith(suffix)).Select(k => GetRequiredContent<T>(serviceProvider, k));
        }

        /// <summary>
        /// Gets all content types registered with a particular suffix.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="suffix">The suffix to query.</param>
        /// <returns>An enumerable of strings representing the content type with the specified suffix.</returns>
        public static IEnumerable<string> GetAllContentTypes(this IServiceProvider serviceProvider, string suffix)
        {
            IEnumerable<string> registeredContentTypes = serviceProvider
                .GetRequiredService<ContentFactory>()
                .GetRegisteredContentTypes()
                .Select(kv => kv.Key);

            return registeredContentTypes.Where(k => k.EndsWith(suffix));
        }

        /// <summary>
        /// Get the service for a particular content type.
        /// </summary>
        /// <typeparam name="T">The type of the service.</typeparam>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns>An instance of the required content, or null if no content is registered.</returns>
        /// <remarks>The type must provide a static/const string called. <c>RegisteredContentType</c> which defines its content type.</remarks>
        public static T? GetContent<T>(this IServiceProvider serviceProvider)
            where T : class
        {
            ArgumentNullException.ThrowIfNull(serviceProvider);

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
        public static T? GetContent<T>(this IServiceProvider serviceProvider, string contentType)
            where T : class
        {
            ArgumentNullException.ThrowIfNull(serviceProvider);

            if (string.IsNullOrEmpty(contentType))
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (!TryGetTypeFor(serviceProvider, contentType, out Type? serviceType, out bool usesServices))
            {
                return null;
            }

            if (usesServices)
            {
                return serviceProvider.GetService(serviceType) as T;
            }
            else
            {
                ConstructorInfo? ctorInfo = serviceType.GetConstructor([]);
#pragma warning disable IDE0270 // Use coalesce expression - doesn't look any clearer to me
                if (ctorInfo == null)
                {
                    throw new InvalidOperationException(string.Format(Resources.ImplementingTypeNoDefaultCtor, serviceType, contentType));
                }
#pragma warning restore IDE0270 // Use coalesce expression

                return ctorInfo.Invoke([]) as T;
            }
        }

        /// <summary>
        /// Get the service for a particular content type.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="contentType">The content type.</param>
        /// <returns>An instance of the required content, or null if no content is registered.</returns>
        public static object? GetContent(this IServiceProvider serviceProvider, string contentType)
        {
            ArgumentNullException.ThrowIfNull(serviceProvider);

            if (string.IsNullOrEmpty(contentType))
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (!TryGetTypeFor(serviceProvider, contentType, out Type? serviceType, out bool usesServices))
            {
                return null;
            }

            if (usesServices)
            {
                return serviceProvider.GetService(serviceType);
            }
            else
            {
                ConstructorInfo? ctorInfo = serviceType.GetConstructor([]);
#pragma warning disable IDE0270 // Use coalesce expression - doesn't look any clearer to me
                if (ctorInfo is null)
                {
                    throw new InvalidOperationException(string.Format(Resources.ImplementingTypeNoDefaultCtor, serviceType, contentType));
                }
#pragma warning restore IDE0270

                return ctorInfo.Invoke([]);
            }
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
            ArgumentNullException.ThrowIfNull(serviceProvider);

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
            ArgumentNullException.ThrowIfNull(serviceProvider);

            if (string.IsNullOrEmpty(contentType))
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (!TryGetTypeFor(serviceProvider, contentType, out Type? serviceType, out bool usesServices))
            {
                throw new InvalidOperationException(string.Format(Resources.NoNamedServiceRegistered, contentType));
            }

            object? result;
            if (usesServices)
            {
                result = serviceProvider.GetRequiredService(serviceType);
            }
            else
            {
                ConstructorInfo? ctorInfo = serviceType.GetConstructor([]);
#pragma warning disable IDE0270 // Use coalesce expression - doesn't look any clearer to me
                if (ctorInfo == null)
                {
                    throw new InvalidOperationException(string.Format(Resources.ImplementingTypeNoDefaultCtor, serviceType, contentType));
                }
#pragma warning restore IDE0270

                result = ctorInfo.Invoke([]) as T;
            }

            if (result is not T service)
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
            ArgumentNullException.ThrowIfNull(serviceProvider);

            if (string.IsNullOrEmpty(contentType))
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (!TryGetTypeFor(serviceProvider, contentType, out Type? serviceType, out bool usesServices))
            {
                throw new InvalidOperationException(string.Format(Resources.NoNamedServiceRegistered, contentType));
            }

            if (usesServices)
            {
                return serviceProvider.GetRequiredService(serviceType);
            }
            else
            {
                ConstructorInfo? ctorInfo = serviceType.GetConstructor([]);
#pragma warning disable IDE0270 // Use coalesce expression - doesn't look any clearer to me
                if (ctorInfo == null)
                {
                    throw new InvalidOperationException(string.Format(Resources.ImplementingTypeNoDefaultCtor, serviceType, contentType));
                }
#pragma warning restore IDE0270

                return ctorInfo.Invoke([]);
            }
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
        public static bool TryGetTypeFor(this IServiceProvider serviceProvider, string contentType, [NotNullWhen(true)] out Type? serviceType)
            => TryGetTypeFor(serviceProvider, contentType, out serviceType, out _);

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
        /// <param name="usesServices">
        /// True if the implementation type depends on services, thus needing to be initialized
        /// through dependency injection, false if it does not, or if the content type was not
        /// registered.
        /// </param>
        /// <returns>
        /// A boolean indicating whether or not a type was found.
        /// </returns>
        public static bool TryGetTypeFor(
            this IServiceProvider serviceProvider,
            string contentType,
            [NotNullWhen(true)] out Type? serviceType,
            out bool usesServices)
        {
            ContentFactory typesForNamedContent = serviceProvider.GetRequiredService<ContentFactory>();
            if (!typesForNamedContent.TryGetContentType(contentType, out serviceType, out usesServices))
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
                    return TryGetTypeFor(serviceProvider, string.Concat(contentType.AsSpan(0, indexOfLastDot), suffix), out serviceType, out usesServices);
                }

                return false;
            }

            return true;
        }
    }
}