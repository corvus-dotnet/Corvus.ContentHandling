// <copyright file="ContentHandlingBuilder.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling
{
    using System;
    using Corvus.ContentHandling.Internal;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Registers content types with the content handling framework.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Content is registered as a keyed service whose service type is the concrete
    /// implementing type and whose key is the content-type string. All registration happens
    /// while the service collection is being configured; content cannot be registered after
    /// the service provider has been built.
    /// </para>
    /// <para>
    /// Obtain an instance via the <c>AddContentHandling</c> extension method on
    /// <see cref="IServiceCollection"/>.
    /// </para>
    /// </remarks>
    public sealed class ContentHandlingBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentHandlingBuilder"/> class.
        /// </summary>
        /// <param name="services">The service collection being configured.</param>
        /// <param name="registry">The content registry recording registrations.</param>
        internal ContentHandlingBuilder(IServiceCollection services, ContentRegistry registry)
        {
            this.Services = services;
            this.Registry = registry;
        }

        /// <summary>
        /// Gets the service collection being configured.
        /// </summary>
        public IServiceCollection Services { get; }

        /// <summary>
        /// Gets the content registry recording registrations.
        /// </summary>
        internal ContentRegistry Registry { get; }

        /// <summary>
        /// Registers a content type implemented by <typeparamref name="T"/>, resolved from the container.
        /// </summary>
        /// <typeparam name="T">The type implementing the content.</typeparam>
        /// <param name="contentType">The content type to register.</param>
        /// <param name="lifetime">The service lifetime for instances of the content.</param>
        /// <returns>The builder, for chaining.</returns>
        public ContentHandlingBuilder AddContent<T>(string contentType, ServiceLifetime lifetime)
            where T : class
        {
            ArgumentException.ThrowIfNullOrEmpty(contentType);

            this.Registry.Add(contentType, new ContentRegistration(contentType, typeof(T), ContentConstruction.FromServices, lifetime));
            this.Services.Add(ServiceDescriptor.DescribeKeyed(typeof(T), contentType, typeof(T), lifetime));
            return this;
        }

        /// <summary>
        /// Registers a content type implemented by <typeparamref name="T"/>, resolved from the
        /// container, deriving the content type from the type itself.
        /// </summary>
        /// <typeparam name="T">The type implementing the content.</typeparam>
        /// <param name="lifetime">The service lifetime for instances of the content.</param>
        /// <returns>The builder, for chaining.</returns>
        /// <remarks>
        /// The content type is discovered via <see cref="ContentTypes.GetContentType{T}"/> —
        /// a <see cref="ContentTypeAttribute"/> or a static string field named
        /// <c>RegisteredContentType</c>.
        /// </remarks>
        public ContentHandlingBuilder AddContent<T>(ServiceLifetime lifetime)
            where T : class
        {
            return this.AddContent<T>(ContentTypes.GetContentType<T>(), lifetime);
        }

        /// <summary>
        /// Registers a content type implemented by <typeparamref name="T"/>, created by a factory.
        /// </summary>
        /// <typeparam name="T">The type implementing the content.</typeparam>
        /// <param name="contentType">The content type to register.</param>
        /// <param name="implementationFactory">The factory creating instances of the content.</param>
        /// <param name="lifetime">The service lifetime for instances of the content.</param>
        /// <returns>The builder, for chaining.</returns>
        public ContentHandlingBuilder AddContent<T>(string contentType, Func<IServiceProvider, T> implementationFactory, ServiceLifetime lifetime)
            where T : class
        {
            ArgumentException.ThrowIfNullOrEmpty(contentType);
            ArgumentNullException.ThrowIfNull(implementationFactory);

            this.Registry.Add(contentType, new ContentRegistration(contentType, typeof(T), ContentConstruction.FromServices, lifetime));
            this.Services.Add(ServiceDescriptor.DescribeKeyed(typeof(T), contentType, (sp, _) => implementationFactory(sp), lifetime));
            return this;
        }

        /// <summary>
        /// Registers a content type implemented by a singleton instance.
        /// </summary>
        /// <typeparam name="T">The type implementing the content.</typeparam>
        /// <param name="contentType">The content type to register.</param>
        /// <param name="instance">The instance implementing the content.</param>
        /// <returns>The builder, for chaining.</returns>
        public ContentHandlingBuilder AddContentInstance<T>(string contentType, T instance)
            where T : class
        {
            ArgumentException.ThrowIfNullOrEmpty(contentType);
            ArgumentNullException.ThrowIfNull(instance);

            this.Registry.Add(contentType, new ContentRegistration(contentType, typeof(T), ContentConstruction.FromServices, ServiceLifetime.Singleton));
            this.Services.Add(ServiceDescriptor.KeyedSingleton(typeof(T), contentType, instance));
            return this;
        }

        /// <summary>
        /// Registers a content type implemented by a singleton instance, deriving the content
        /// type from the type itself.
        /// </summary>
        /// <typeparam name="T">The type implementing the content.</typeparam>
        /// <param name="instance">The instance implementing the content.</param>
        /// <returns>The builder, for chaining.</returns>
        public ContentHandlingBuilder AddContentInstance<T>(T instance)
            where T : class
        {
            return this.AddContentInstance(ContentTypes.GetContentType<T>(), instance);
        }

        /// <summary>
        /// Registers a content type whose instances are constructed by a serializer (or via the
        /// type's public parameterless constructor), not resolved from the container.
        /// </summary>
        /// <typeparam name="T">The type implementing the content.</typeparam>
        /// <param name="contentType">The content type to register.</param>
        /// <returns>The builder, for chaining.</returns>
        /// <remarks>
        /// Use this for plain content types with no service dependencies — including types
        /// designed for constructor-based deserialization. No keyed service is added for them.
        /// </remarks>
        public ContentHandlingBuilder AddSerializedContent<T>(string contentType)
            where T : class
        {
            ArgumentException.ThrowIfNullOrEmpty(contentType);

            this.Registry.Add(contentType, new ContentRegistration(contentType, typeof(T), ContentConstruction.BySerializer, null));
            return this;
        }

        /// <summary>
        /// Registers a content type whose instances are constructed by a serializer, deriving
        /// the content type from the type itself.
        /// </summary>
        /// <typeparam name="T">The type implementing the content.</typeparam>
        /// <returns>The builder, for chaining.</returns>
        public ContentHandlingBuilder AddSerializedContent<T>()
            where T : class
        {
            return this.AddSerializedContent<T>(ContentTypes.GetContentType<T>());
        }

        /// <summary>
        /// Registers a content type as an alias for an already-registered content type.
        /// </summary>
        /// <param name="contentType">The alias content type to register.</param>
        /// <param name="targetContentType">The existing content type the alias resolves to.</param>
        /// <returns>The builder, for chaining.</returns>
        /// <remarks>
        /// Resolving the alias delegates to the target registration, so a singleton target
        /// yields the same instance for both content types.
        /// </remarks>
        /// <exception cref="InvalidOperationException">The target content type is not registered.</exception>
        public ContentHandlingBuilder AddContentAlias(string contentType, string targetContentType)
        {
            ArgumentException.ThrowIfNullOrEmpty(contentType);
            ArgumentException.ThrowIfNullOrEmpty(targetContentType);

            if (!this.Registry.TryGet(targetContentType, out ContentRegistration? target))
            {
                throw new InvalidOperationException($"Cannot add an alias for content type '{targetContentType}' because it has not been registered.");
            }

            this.Registry.Add(contentType, target with { ContentType = contentType });

            if (target.Construction == ContentConstruction.FromServices)
            {
                Type implementingType = target.ImplementingType;
                string targetKey = target.ContentType;
                this.Services.Add(ServiceDescriptor.KeyedTransient(
                    implementingType,
                    contentType,
                    (sp, _) => sp.GetRequiredKeyedService(implementingType, targetKey)));
            }

            return this;
        }
    }
}
