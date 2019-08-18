// <copyright file="ContentFactoryExtensions.cs" company="Endjin Limited">
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
    /// Types that wish to participate must add a <c>const string</c> field called <c>contentFactoryType</c>
    /// and a property <c>ContentType</c> which returns the same value. You do not need to implement any particular interface or
    /// derive from any base class.
    /// </para>
    /// </remarks>
    public static class ContentFactoryExtensions
    {
        /// <summary>
        /// Use the content factory pattern.
        /// </summary>
        /// <param name="serviceCollection">The service collection with which to register content handlers.</param>
        /// <returns>An instance of the content registry for this factory.</returns>
        public static ContentFactory UseContentFactory(IServiceCollection serviceCollection)
        {
            var contentFactory = new ContentFactory(serviceCollection);
            serviceCollection.AddSingleton(contentFactory);
            return contentFactory;
        }

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
        /// Registers a singleton instance for the specified type.
        /// </summary>
        /// <typeparam name="T">The type to register.</typeparam>
        /// <param name="contentFactory">The content registry for this factory.</param>
        /// <remarks>The type must provide a static/const string called. <c>contentFactoryType</c> which defines its content type.</remarks>
        public static void RegisterSingletonContent<T>(this ContentFactory contentFactory)
            where T : class
        {
            if (contentFactory == null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            string name = ContentFactory.GetContentType(typeof(T));

            RegisterSingletonContent<T>(contentFactory, name);
        }

        /// <summary>
        /// Register a single instance for the specified type and content type.
        /// </summary>
        /// <typeparam name="T">The type to register.</typeparam>
        /// <param name="contentFactory">The content registry for this factory.</param>
        /// <param name="contentType">The content type by which to register it.</param>
        public static void RegisterSingletonContent<T>(this ContentFactory contentFactory, string contentType)
            where T : class
        {
            if (contentFactory == null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (string.IsNullOrEmpty(contentType))
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (!contentFactory.Handlers.TryAdd(contentType, typeof(T)))
            {
                throw new InvalidOperationException(string.Format(Resources.NamedTypeAlreadyAdded, contentType));
            }

            if (!contentFactory.Services.Any(s => s.ServiceType == typeof(T)))
            {
                contentFactory.Services.AddSingleton<T>();
            }
        }

        /// <summary>
        /// Register a single instance for the specified type, using a factory function.
        /// </summary>
        /// <typeparam name="T">The type to register.</typeparam>
        /// <param name="contentFactory">The content registry for this factory.</param>
        /// <param name="implementationFactory">The factory function for the type.</param>
        /// <remarks>The type must provide a static/const string called. <c>contentFactoryType</c> which defines its content type.</remarks>
        public static void RegisterSingletonContent<T>(this ContentFactory contentFactory, Func<IServiceProvider, T> implementationFactory)
            where T : class
        {
            if (contentFactory == null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (implementationFactory == null)
            {
                throw new ArgumentNullException(nameof(implementationFactory));
            }

            string name = ContentFactory.GetContentType(typeof(T));

            RegisterSingletonContent(contentFactory, name, implementationFactory);
        }

        /// <summary>
        /// Register a single instance for the specified type, using a factory function.
        /// </summary>
        /// <typeparam name="T">The type to register.</typeparam>
        /// <param name="contentFactory">The content registry for this factory.</param>
        /// <param name="contentType">The content type by which to register it.</param>
        /// <param name="implementationFactory">The factory function for the type.</param>
        public static void RegisterSingletonContent<T>(this ContentFactory contentFactory, string contentType, Func<IServiceProvider, T> implementationFactory)
            where T : class
        {
            if (contentFactory == null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (string.IsNullOrEmpty(contentType))
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (implementationFactory == null)
            {
                throw new ArgumentNullException(nameof(implementationFactory));
            }

            if (!contentFactory.Handlers.TryAdd(contentType, typeof(T)))
            {
                throw new InvalidOperationException(string.Format(Resources.NamedTypeAlreadyAdded, contentType));
            }

            if (!contentFactory.Services.Any(s => s.ServiceType == typeof(T)))
            {
                contentFactory.Services.AddSingleton(implementationFactory);
            }
        }

        /// <summary>
        /// Register a single instance for the specified type.
        /// </summary>
        /// <typeparam name="T">The type to register.</typeparam>
        /// <param name="contentFactory">The content registry for this factory.</param>
        /// <param name="implementationInstance">The instance to use as the singleton.</param>
        /// <remarks>The type must provide a static/const string called. <c>contentFactoryType</c> which defines its content type.</remarks>
        public static void RegisterSingletonContent<T>(this ContentFactory contentFactory, T implementationInstance)
            where T : class
        {
            if (contentFactory == null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (implementationInstance == null)
            {
                throw new ArgumentNullException(nameof(implementationInstance));
            }

            string name = ContentFactory.GetContentType(typeof(T));

            RegisterSingletonContent(contentFactory, name, implementationInstance);
        }

        /// <summary>
        /// Register a single instance for the specified type.
        /// </summary>
        /// <typeparam name="T">The type to register.</typeparam>
        /// <param name="contentFactory">The content registry for this factory.</param>
        /// <param name="contentType">The content type by which to register it.</param>
        /// <param name="implementationInstance">The instance to use as the singleton.</param>
        public static void RegisterSingletonContent<T>(this ContentFactory contentFactory, string contentType, T implementationInstance)
            where T : class
        {
            if (contentFactory == null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (string.IsNullOrEmpty(contentType))
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (implementationInstance == null)
            {
                throw new ArgumentNullException(nameof(implementationInstance));
            }

            if (!contentFactory.Handlers.TryAdd(contentType, typeof(T)))
            {
                throw new InvalidOperationException(string.Format(Resources.NamedTypeAlreadyAdded, contentType));
            }

            if (!contentFactory.Services.Any(s => s.ServiceType == typeof(T)))
            {
                contentFactory.Services.AddSingleton(implementationInstance);
            }
        }

        /// <summary>
        /// Register a single instance for the specified type.
        /// </summary>
        /// <param name="contentFactory">The content registry for this factory.</param>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <remarks>The type must provide a static/const string called. <c>contentFactoryType</c> which defines its content type.</remarks>
        public static void RegisterSingletonContent(this ContentFactory contentFactory, Type serviceType)
        {
            if (contentFactory == null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            string name = ContentFactory.GetContentType(serviceType);

            RegisterSingletonContent(contentFactory, name, serviceType);
        }

        /// <summary>
        /// Register a single instance for the specified type.
        /// </summary>
        /// <param name="contentFactory">The content registry for this factory.</param>
        /// <param name="contentType">The content type by which to register it.</param>
        /// <param name="serviceType">The type of the service to register.</param>
        public static void RegisterSingletonContent(this ContentFactory contentFactory, string contentType, Type serviceType)
        {
            if (contentFactory == null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (string.IsNullOrEmpty(contentType))
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            if (!contentFactory.Handlers.TryAdd(contentType, serviceType))
            {
                throw new InvalidOperationException(string.Format(Resources.NamedTypeAlreadyAdded, contentType));
            }

            if (!contentFactory.Services.Any(s => s.ServiceType == serviceType))
            {
                contentFactory.Services.AddSingleton(serviceType);
            }
        }

        /// <summary>
        /// Register a single instance for the specified type, using a factory function.
        /// </summary>
        /// <param name="contentFactory">The content registry for this factory.</param>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="implementationFactory">The factory function for the type.</param>
        public static void RegisterSingletonContent(this ContentFactory contentFactory, Type serviceType, Func<IServiceProvider, object> implementationFactory)
        {
            if (contentFactory == null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            if (implementationFactory == null)
            {
                throw new ArgumentNullException(nameof(implementationFactory));
            }

            string name = ContentFactory.GetContentType(serviceType);

            if (!contentFactory.Services.Any(s => s.ServiceType == serviceType))
            {
                RegisterSingletonContent(contentFactory, name, serviceType);
            }
        }

        /// <summary>
        /// Register a single instance for the specified type, using a factory function.
        /// </summary>
        /// <param name="contentFactory">The content registry for this factory.</param>
        /// <param name="contentType">The content type to register.</param>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="implementationFactory">The factory function for the type.</param>
        public static void RegisterSingletonContent(this ContentFactory contentFactory, string contentType, Type serviceType, Func<IServiceProvider, object> implementationFactory)
        {
            if (contentFactory == null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (string.IsNullOrEmpty(contentType))
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            if (implementationFactory == null)
            {
                throw new ArgumentNullException(nameof(implementationFactory));
            }

            if (!contentFactory.Handlers.TryAdd(contentType, serviceType))
            {
                throw new InvalidOperationException(string.Format(Resources.NamedTypeAlreadyAdded, contentType));
            }

            if (!contentFactory.Services.Any(s => s.ServiceType == serviceType))
            {
                contentFactory.Services.AddSingleton(serviceType, implementationFactory);
            }
        }

        /// <summary>
        /// Register a single instance for the specified type.
        /// </summary>
        /// <param name="contentFactory">The content registry for this factory.</param>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="implementationInstance">The instance to register for the type.</param>
        public static void RegisterSingletonContent(this ContentFactory contentFactory, Type serviceType, object implementationInstance)
        {
            if (contentFactory == null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            if (implementationInstance == null)
            {
                throw new ArgumentNullException(nameof(implementationInstance));
            }

            string name = ContentFactory.GetContentType(implementationInstance.GetType());

            RegisterSingletonContent(contentFactory, name, serviceType, implementationInstance);
        }

        /// <summary>
        /// Register a single instance for the specified type, using a factory function.
        /// </summary>
        /// <param name="contentFactory">The content registry for this factory.</param>
        /// <param name="contentType">The content type to register.</param>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="implementationInstance">TThe instance to register for the type.</param>
        public static void RegisterSingletonContent(this ContentFactory contentFactory, string contentType, Type serviceType, object implementationInstance)
        {
            if (contentFactory == null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (string.IsNullOrEmpty(contentType))
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            if (implementationInstance == null)
            {
                throw new ArgumentNullException(nameof(implementationInstance));
            }

            if (!contentFactory.Handlers.TryAdd(contentType, serviceType))
            {
                throw new InvalidOperationException(string.Format(Resources.NamedTypeAlreadyAdded, contentType));
            }

            if (!contentFactory.Services.Any(s => s.ServiceType == serviceType))
            {
                contentFactory.Services.AddSingleton(serviceType, implementationInstance);
            }
        }

        /// <summary>
        /// Register a type as a transient service.
        /// </summary>
        /// <typeparam name="T">The type to register.</typeparam>
        /// <param name="contentFactory">The content registry for this factory.</param>
        /// <remarks>The type must provide a static/const string called. <c>contentFactoryType</c> which defines its content type.</remarks>
        public static void RegisterTransientContent<T>(this ContentFactory contentFactory)
            where T : class
        {
            if (contentFactory == null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            string name = ContentFactory.GetContentType(typeof(T));

            if (!contentFactory.Services.Any(s => s.ServiceType == typeof(T)))
            {
                RegisterTransientContent<T>(contentFactory, name);
            }
        }

        /// <summary>
        /// Register a type as a transient service.
        /// </summary>
        /// <typeparam name="T">The type to register.</typeparam>
        /// <param name="contentFactory">The content registry for this factory.</param>
        /// <param name="contentType">The content type against which to register the type.</param>
        public static void RegisterTransientContent<T>(this ContentFactory contentFactory, string contentType)
            where T : class
        {
            if (contentFactory == null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (string.IsNullOrEmpty(contentType))
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (!contentFactory.Handlers.TryAdd(contentType, typeof(T)))
            {
                throw new InvalidOperationException(string.Format(Resources.NamedTypeAlreadyAdded, contentType));
            }

            if (!contentFactory.Services.Any(s => s.ServiceType == typeof(T)))
            {
                contentFactory.Services.AddTransient<T>();
            }
        }

        /// <summary>
        /// Register a transient service for the specified type, using a factory function.
        /// </summary>
        /// <typeparam name="T">The type to register.</typeparam>
        /// <param name="contentFactory">The content registry for this factory.</param>
        /// <param name="implementationFactory">The factory function for the type.</param>
        /// <remarks>The type must provide a static/const string called. <c>contentFactoryType</c> which defines its content type.</remarks>
        public static void RegisterTransientContent<T>(this ContentFactory contentFactory, Func<IServiceProvider, T> implementationFactory)
            where T : class
        {
            if (contentFactory == null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (implementationFactory == null)
            {
                throw new ArgumentNullException(nameof(implementationFactory));
            }

            string name = ContentFactory.GetContentType(typeof(T));

            RegisterTransientContent(contentFactory, name, implementationFactory);
        }

        /// <summary>
        /// Register a transient service for the specified type, using a factory function and a specific content type.
        /// </summary>
        /// <typeparam name="T">The type to register.</typeparam>
        /// <param name="contentFactory">The content registry for this factory.</param>
        /// <param name="contentType">The content type against which to register the service.</param>
        /// <param name="implementationFactory">The factory function for the type.</param>
        public static void RegisterTransientContent<T>(this ContentFactory contentFactory, string contentType, Func<IServiceProvider, T> implementationFactory)
            where T : class
        {
            if (contentFactory == null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (string.IsNullOrEmpty(contentType))
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (implementationFactory == null)
            {
                throw new ArgumentNullException(nameof(implementationFactory));
            }

            if (!contentFactory.Handlers.TryAdd(contentType, typeof(T)))
            {
                throw new InvalidOperationException(string.Format(Resources.NamedTypeAlreadyAdded, contentType));
            }

            if (!contentFactory.Services.Any(s => s.ServiceType == typeof(T)))
            {
                contentFactory.Services.AddTransient(implementationFactory);
            }
        }

        /// <summary>
        /// Register a transient service for the specified type.
        /// </summary>
        /// <param name="contentFactory">The content registry for this factory.</param>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <remarks>The type must provide a static/const string called. <c>contentFactoryType</c> which defines its content type.</remarks>
        public static void RegisterTransientContent(this ContentFactory contentFactory, Type serviceType)
        {
            if (contentFactory == null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            string name = ContentFactory.GetContentType(serviceType);

            RegisterTransientContent(contentFactory, name, serviceType);
        }

        /// <summary>
        /// Register a transient service for the specified type.
        /// </summary>
        /// <param name="contentFactory">The content registry for this factory.</param>
        /// <param name="contentType">The content type against which to register the service.</param>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <remarks>The type must provide a static/const string called. <c>contentFactoryType</c> which defines its content type.</remarks>
        public static void RegisterTransientContent(this ContentFactory contentFactory, string contentType, Type serviceType)
        {
            if (contentFactory == null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (string.IsNullOrEmpty(contentType))
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (!contentFactory.Handlers.TryAdd(contentType, serviceType))
            {
                throw new InvalidOperationException(string.Format(Resources.NamedTypeAlreadyAdded, contentType));
            }

            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            if (!contentFactory.Services.Any(s => s.ServiceType == serviceType))
            {
                contentFactory.Services.AddTransient(serviceType);
            }
        }

        /// <summary>
        /// Register a transient service for the specified type, using a factory function.
        /// </summary>
        /// <param name="contentFactory">The content registry for this factory.</param>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="implementationFactory">The factory function for the type.</param>
        /// <remarks>The type must provide a static/const string called. <c>contentFactoryType</c> which defines its content type.</remarks>
        public static void RegisterTransientContent(this ContentFactory contentFactory, Type serviceType, Func<IServiceProvider, object> implementationFactory)
        {
            if (contentFactory == null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            if (implementationFactory == null)
            {
                throw new ArgumentNullException(nameof(implementationFactory));
            }

            string name = ContentFactory.GetContentType(serviceType);

            RegisterTransientContent(contentFactory, name, serviceType, implementationFactory);
        }

        /// <summary>
        /// Register a transient service for the specified type with a specific content type, using a factory function.
        /// </summary>
        /// <param name="contentFactory">The content registry for this factory.</param>
        /// <param name="contentType">The content type to register.</param>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="implementationFactory">The factory function for the type.</param>
        public static void RegisterTransientContent(this ContentFactory contentFactory, string contentType, Type serviceType, Func<IServiceProvider, object> implementationFactory)
        {
            if (contentFactory == null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (string.IsNullOrEmpty(contentType))
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            if (implementationFactory == null)
            {
                throw new ArgumentNullException(nameof(implementationFactory));
            }

            if (!contentFactory.Handlers.TryAdd(contentType, serviceType))
            {
                throw new InvalidOperationException(string.Format(Resources.NamedTypeAlreadyAdded, contentType));
            }

            if (!contentFactory.Services.Any(s => s.ServiceType == serviceType))
            {
                contentFactory.Services.AddTransient(serviceType, implementationFactory);
            }
        }

        /// <summary>
        /// Adds a scoped service of the required type to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="T">The type of the service to add.</typeparam>
        /// <param name="contentFactory">The content registry for this factory.</param>
        /// <remarks>The type must provide a static/const string called. <c>contentFactoryType</c> which defines its content type.</remarks>
        public static void RegisterScopedContent<T>(this ContentFactory contentFactory)
            where T : class
        {
            if (contentFactory == null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            string name = ContentFactory.GetContentType(typeof(T));

            RegisterScopedContent<T>(contentFactory, name);
        }

        /// <summary>
        /// Adds a scoped service of the required type to the specified <see cref="IServiceCollection"/>, with a specified content type.
        /// </summary>
        /// <typeparam name="T">The type of the service to add.</typeparam>
        /// <param name="contentFactory">The content registry for this factory.</param>
        /// <param name="contentType">The content type against which to register the service.</param>
        public static void RegisterScopedContent<T>(this ContentFactory contentFactory, string contentType)
            where T : class
        {
            if (contentFactory == null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (string.IsNullOrEmpty(contentType))
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (!contentFactory.Handlers.TryAdd(contentType, typeof(T)))
            {
                throw new InvalidOperationException(string.Format(Resources.NamedTypeAlreadyAdded, contentType));
            }

            if (!contentFactory.Services.Any(s => s.ServiceType == typeof(T)))
            {
                contentFactory.Services.AddScoped<T>();
            }
        }

        /// <summary>
        /// Register a scoped service for the specified type, using a factory function.
        /// </summary>
        /// <typeparam name="T">The type of the service to add.</typeparam>
        /// <param name="contentFactory">The content registry for this factory.</param>
        /// <param name="implementationFactory">The factory function for the type.</param>
        /// <remarks>The type must provide a static/const string called. <c>contentFactoryType</c> which defines its content type.</remarks>
        public static void RegisterScopedContent<T>(this ContentFactory contentFactory, Func<IServiceProvider, T> implementationFactory)
            where T : class
        {
            if (contentFactory == null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (implementationFactory == null)
            {
                throw new ArgumentNullException(nameof(implementationFactory));
            }

            string name = ContentFactory.GetContentType(typeof(T));

            RegisterScopedContent(contentFactory, name, implementationFactory);
        }

        /// <summary>
        /// Register a scoped service for the specified type, using a factory function and a particular content type.
        /// </summary>
        /// <typeparam name="T">The type of the service to add.</typeparam>
        /// <param name="contentFactory">The content registry for this factory.</param>
        /// <param name="contentType">The content type to register.</param>
        /// <param name="implementationFactory">The factory function for the type.</param>
        public static void RegisterScopedContent<T>(this ContentFactory contentFactory, string contentType, Func<IServiceProvider, T> implementationFactory)
            where T : class
        {
            if (contentFactory == null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (string.IsNullOrEmpty(contentType))
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (implementationFactory == null)
            {
                throw new ArgumentNullException(nameof(implementationFactory));
            }

            if (!contentFactory.Handlers.TryAdd(contentType, typeof(T)))
            {
                throw new InvalidOperationException(string.Format(Resources.NamedTypeAlreadyAdded, contentType));
            }

            if (!contentFactory.Services.Any(s => s.ServiceType == typeof(T)))
            {
                contentFactory.Services.AddScoped(implementationFactory);
            }
        }

        /// <summary>
        /// Register a scoped service for the specified type.
        /// </summary>
        /// <param name="contentFactory">The content registry for this factory.</param>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <remarks>The type must provide a static/const string called. <c>contentFactoryType</c> which defines its content type.</remarks>
        public static void RegisterScopedContent(this ContentFactory contentFactory, Type serviceType)
        {
            if (contentFactory == null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            string name = ContentFactory.GetContentType(serviceType);

            RegisterScopedContent(contentFactory, name, serviceType);
        }

        /// <summary>
        /// Register a scoped service for the specified type with a specific content type.
        /// </summary>
        /// <param name="contentFactory">The content registry for this factory.</param>
        /// <param name="contentType">The content type to register.</param>
        /// <param name="serviceType">The type of the service to register.</param>
        public static void RegisterScopedContent(this ContentFactory contentFactory, string contentType, Type serviceType)
        {
            if (contentFactory == null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (string.IsNullOrEmpty(contentType))
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (!contentFactory.Handlers.TryAdd(contentType, serviceType))
            {
                throw new InvalidOperationException(string.Format(Resources.NamedTypeAlreadyAdded, contentType));
            }

            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            if (!contentFactory.Services.Any(s => s.ServiceType == serviceType))
            {
                contentFactory.Services.AddScoped(serviceType);
            }
        }

        /// <summary>
        /// Register a scoped service for the specified type, using a factory function.
        /// </summary>
        /// <param name="contentFactory">The content registry for this factory.</param>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="implementationFactory">The factory function for the type.</param>
        /// <remarks>The type must provide a static/const string called. <c>contentFactoryType</c> which defines its content type.</remarks>
        public static void RegisterScopedContent(this ContentFactory contentFactory, Type serviceType, Func<IServiceProvider, object> implementationFactory)
        {
            if (contentFactory == null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            if (implementationFactory == null)
            {
                throw new ArgumentNullException(nameof(implementationFactory));
            }

            string name = ContentFactory.GetContentType(serviceType);

            RegisterScopedContent(contentFactory, name, serviceType, implementationFactory);
        }

        /// <summary>
        /// Register a scoped service for the specified type, using a factory function and a particular content type.
        /// </summary>
        /// <param name="contentFactory">The content registry for this factory.</param>
        /// <param name="contentType">The content type to register.</param>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="implementationFactory">The factory function for the type.</param>
        public static void RegisterScopedContent(this ContentFactory contentFactory, string contentType, Type serviceType, Func<IServiceProvider, object> implementationFactory)
        {
            if (contentFactory == null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (string.IsNullOrEmpty(contentType))
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            if (implementationFactory == null)
            {
                throw new ArgumentNullException(nameof(implementationFactory));
            }

            if (!contentFactory.Handlers.TryAdd(contentType, serviceType))
            {
                throw new InvalidOperationException(string.Format(Resources.NamedTypeAlreadyAdded, contentType));
            }

            if (!contentFactory.Services.Any(s => s.ServiceType == serviceType))
            {
                contentFactory.Services.AddScoped(serviceType, implementationFactory);
            }
        }

        /// <summary>
        /// Get the service for a particular content type.
        /// </summary>
        /// <typeparam name="T">The type of the service.</typeparam>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns>An instance of the required content, or null if no content is registered.</returns>
        /// <remarks>The type must provide a static/const string called. <c>contentFactoryType</c> which defines its content type.</remarks>
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
        /// <remarks>The type must provide a static/const string called. <c>contentFactoryType</c> which defines its content type.</remarks>
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
        public static bool TryGetTypeFor(IServiceProvider serviceProvider, string contentType, out Type serviceType)
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
