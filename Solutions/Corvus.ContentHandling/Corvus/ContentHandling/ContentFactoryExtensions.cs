// <copyright file="ContentFactoryExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling
{
    using System;
    using System.Linq;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Registers types identified by a content-type string with a content factory.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This offers extensions to the <see cref="ContentFactory"/> to provide
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
    public static class ContentFactoryExtensions
    {
        /// <summary>
        /// Registers a singleton instance for the specified type.
        /// </summary>
        /// <typeparam name="T">The type to register.</typeparam>
        /// <param name="contentFactory">The content registry for this factory.</param>
        /// <remarks>The type must provide a static/const string called. <c>RegisteredContentType</c> which defines its content type.</remarks>
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
        /// Register an implementation type for a content type that does not depend on services, and which
        /// can be initialized directly through deserialization.
        /// </summary>
        /// <typeparam name="T">The type to register.</typeparam>
        /// <param name="contentFactory">The content registry for this factory.</param>
        /// <remarks>The type must provide a static/const string called. <c>RegisteredContentType</c> which defines its content type.</remarks>
        public static void RegisterContent<T>(this ContentFactory contentFactory)
            where T : class
        {
            if (contentFactory == null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            string contentType = ContentFactory.GetContentType(typeof(T));

            contentFactory.AddSimpleDeserializableType(contentType, typeof(T));
        }

        /// <summary>
        /// Register an implementation type for a content type that does not depend on services, and which
        /// can be initialized directly through deserialization.
        /// </summary>
        /// <typeparam name="T">The type to register.</typeparam>
        /// <param name="contentFactory">The content registry for this factory.</param>
        /// <param name="contentType">The content type by which to register it.</param>
        public static void RegisterContent<T>(this ContentFactory contentFactory, string contentType)
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

            contentFactory.AddSimpleDeserializableType(contentType, typeof(T));
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

            contentFactory.AddTypeRequiringServices(contentType, typeof(T));

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
        /// <remarks>The type must provide a static/const string called. <c>RegisteredContentType</c> which defines its content type.</remarks>
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

            contentFactory.AddTypeRequiringServices(contentType, typeof(T));

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
        /// <remarks>The type must provide a static/const string called. <c>RegisteredContentType</c> which defines its content type.</remarks>
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

            contentFactory.AddTypeRequiringServices(contentType, typeof(T));

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
        /// <remarks>The type must provide a static/const string called. <c>RegisteredContentType</c> which defines its content type.</remarks>
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

            contentFactory.AddTypeRequiringServices(contentType, serviceType);

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

            contentFactory.AddTypeRequiringServices(contentType, serviceType);

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

            contentFactory.AddTypeRequiringServices(contentType, serviceType);

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
        /// <remarks>The type must provide a static/const string called. <c>RegisteredContentType</c> which defines its content type.</remarks>
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

            contentFactory.AddTypeRequiringServices(contentType, typeof(T));

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
        /// <remarks>The type must provide a static/const string called. <c>RegisteredContentType</c> which defines its content type.</remarks>
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

            contentFactory.AddTypeRequiringServices(contentType, typeof(T));

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
        /// <remarks>The type must provide a static/const string called. <c>RegisteredContentType</c> which defines its content type.</remarks>
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
        /// <remarks>The type must provide a static/const string called. <c>RegisteredContentType</c> which defines its content type.</remarks>
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

            contentFactory.AddTypeRequiringServices(contentType, serviceType);

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
        /// <remarks>The type must provide a static/const string called. <c>RegisteredContentType</c> which defines its content type.</remarks>
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

            contentFactory.AddTypeRequiringServices(contentType, serviceType);

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
        /// <remarks>The type must provide a static/const string called. <c>RegisteredContentType</c> which defines its content type.</remarks>
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

            contentFactory.AddTypeRequiringServices(contentType, typeof(T));

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
        /// <remarks>The type must provide a static/const string called. <c>RegisteredContentType</c> which defines its content type.</remarks>
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

            contentFactory.AddTypeRequiringServices(contentType, typeof(T));

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
        /// <remarks>The type must provide a static/const string called. <c>RegisteredContentType</c> which defines its content type.</remarks>
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

            contentFactory.AddTypeRequiringServices(contentType, serviceType);

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
        /// <remarks>The type must provide a static/const string called. <c>RegisteredContentType</c> which defines its content type.</remarks>
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

            contentFactory.AddTypeRequiringServices(contentType, serviceType);

            if (!contentFactory.Services.Any(s => s.ServiceType == serviceType))
            {
                contentFactory.Services.AddScoped(serviceType, implementationFactory);
            }
        }
    }
}