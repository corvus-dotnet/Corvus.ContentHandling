// <copyright file="ContentHandlerRegistrationExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Corvus.ContentHandling.Internal;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    /// <summary>
    /// Registers content handlers with the content handling framework.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Handlers are registered as keyed services against the closed handler interface for the
    /// dispatcher's payload base type, keyed by
    /// <c>"{contentType}+{handlerClass.ToLowerInvariant()}"</c>. Because keyed service identity
    /// is the (service type, key) pair, any number of delegate handlers can be registered as
    /// distinct instances of the same adapter type — no unique wrapper types are needed.
    /// </para>
    /// <para>
    /// Overloads without an explicit content type derive it from the concrete payload type
    /// via <see cref="ContentTypes.GetContentType{T}"/>.
    /// </para>
    /// </remarks>
    public static class ContentHandlerRegistrationExtensions
    {
        /// <summary>
        /// Registers an asynchronous delegate as the handler for a content type and handler class.
        /// </summary>
        /// <typeparam name="TPayloadBase">The common base type of payloads for the dispatcher.</typeparam>
        /// <typeparam name="TPayload">The concrete payload type handled by the delegate.</typeparam>
        /// <param name="builder">The content handling builder.</param>
        /// <param name="contentType">The content type the handler handles.</param>
        /// <param name="handlerClass">The class of handler.</param>
        /// <param name="handler">The delegate which handles the payload.</param>
        /// <returns>The builder, for chaining.</returns>
        public static ContentHandlingBuilder AddContentHandler<TPayloadBase, TPayload>(
            this ContentHandlingBuilder builder,
            string contentType,
            string handlerClass,
            Func<TPayload, CancellationToken, ValueTask> handler)
            where TPayloadBase : notnull
            where TPayload : TPayloadBase
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(handler);

            string key = ContentHandlerKeys.For(contentType, handlerClass);
            builder.Registry.Add(key, new ContentRegistration(key, typeof(IContentHandler<TPayloadBase>), ContentConstruction.FromServices, ServiceLifetime.Singleton));
            builder.Services.AddKeyedSingleton<IContentHandler<TPayloadBase>>(key, new DelegateContentHandler<TPayloadBase, TPayload>(handler));
            return builder;
        }

        /// <summary>
        /// Registers an asynchronous delegate as the handler for a handler class, deriving the
        /// content type from <typeparamref name="TPayload"/>.
        /// </summary>
        /// <typeparam name="TPayloadBase">The common base type of payloads for the dispatcher.</typeparam>
        /// <typeparam name="TPayload">The concrete payload type handled by the delegate.</typeparam>
        /// <param name="builder">The content handling builder.</param>
        /// <param name="handlerClass">The class of handler.</param>
        /// <param name="handler">The delegate which handles the payload.</param>
        /// <returns>The builder, for chaining.</returns>
        public static ContentHandlingBuilder AddContentHandler<TPayloadBase, TPayload>(
            this ContentHandlingBuilder builder,
            string handlerClass,
            Func<TPayload, CancellationToken, ValueTask> handler)
            where TPayloadBase : notnull
            where TPayload : TPayloadBase
        {
            return builder.AddContentHandler<TPayloadBase, TPayload>(ContentTypes.GetContentType<TPayload>(), handlerClass, handler);
        }

        /// <summary>
        /// Registers a synchronous delegate as the handler for a content type and handler class.
        /// </summary>
        /// <typeparam name="TPayloadBase">The common base type of payloads for the dispatcher.</typeparam>
        /// <typeparam name="TPayload">The concrete payload type handled by the delegate.</typeparam>
        /// <param name="builder">The content handling builder.</param>
        /// <param name="contentType">The content type the handler handles.</param>
        /// <param name="handlerClass">The class of handler.</param>
        /// <param name="handler">The delegate which handles the payload.</param>
        /// <returns>The builder, for chaining.</returns>
        public static ContentHandlingBuilder AddContentHandler<TPayloadBase, TPayload>(
            this ContentHandlingBuilder builder,
            string contentType,
            string handlerClass,
            Action<TPayload> handler)
            where TPayloadBase : notnull
            where TPayload : TPayloadBase
        {
            ArgumentNullException.ThrowIfNull(handler);

            return builder.AddContentHandler<TPayloadBase, TPayload>(
                contentType,
                handlerClass,
                (payload, _) =>
                {
                    handler(payload);
                    return ValueTask.CompletedTask;
                });
        }

        /// <summary>
        /// Registers a synchronous delegate as the handler for a handler class, deriving the
        /// content type from <typeparamref name="TPayload"/>.
        /// </summary>
        /// <typeparam name="TPayloadBase">The common base type of payloads for the dispatcher.</typeparam>
        /// <typeparam name="TPayload">The concrete payload type handled by the delegate.</typeparam>
        /// <param name="builder">The content handling builder.</param>
        /// <param name="handlerClass">The class of handler.</param>
        /// <param name="handler">The delegate which handles the payload.</param>
        /// <returns>The builder, for chaining.</returns>
        public static ContentHandlingBuilder AddContentHandler<TPayloadBase, TPayload>(
            this ContentHandlingBuilder builder,
            string handlerClass,
            Action<TPayload> handler)
            where TPayloadBase : notnull
            where TPayload : TPayloadBase
        {
            return builder.AddContentHandler<TPayloadBase, TPayload>(ContentTypes.GetContentType<TPayload>(), handlerClass, handler);
        }

        /// <summary>
        /// Registers an asynchronous delegate receiving a context as the handler for a content
        /// type and handler class.
        /// </summary>
        /// <typeparam name="TPayloadBase">The common base type of payloads for the dispatcher.</typeparam>
        /// <typeparam name="TPayload">The concrete payload type handled by the delegate.</typeparam>
        /// <typeparam name="TContext">The type of the context passed alongside the payload.</typeparam>
        /// <param name="builder">The content handling builder.</param>
        /// <param name="contentType">The content type the handler handles.</param>
        /// <param name="handlerClass">The class of handler.</param>
        /// <param name="handler">The delegate which handles the payload.</param>
        /// <returns>The builder, for chaining.</returns>
        public static ContentHandlingBuilder AddContentHandler<TPayloadBase, TPayload, TContext>(
            this ContentHandlingBuilder builder,
            string contentType,
            string handlerClass,
            Func<TPayload, TContext, CancellationToken, ValueTask> handler)
            where TPayloadBase : notnull
            where TPayload : TPayloadBase
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(handler);

            string key = ContentHandlerKeys.For(contentType, handlerClass);
            builder.Registry.Add(key, new ContentRegistration(key, typeof(IContentHandler<TPayloadBase, TContext>), ContentConstruction.FromServices, ServiceLifetime.Singleton));
            builder.Services.AddKeyedSingleton<IContentHandler<TPayloadBase, TContext>>(key, new DelegateContentHandler<TPayloadBase, TPayload, TContext>(handler));
            return builder;
        }

        /// <summary>
        /// Registers a class-based handler for a content type and handler class.
        /// </summary>
        /// <typeparam name="TPayloadBase">The common base type of payloads for the dispatcher.</typeparam>
        /// <typeparam name="TPayload">The concrete payload type handled by the handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <param name="builder">The content handling builder.</param>
        /// <param name="contentType">The content type the handler handles.</param>
        /// <param name="handlerClass">The class of handler.</param>
        /// <param name="lifetime">The service lifetime for the handler.</param>
        /// <returns>The builder, for chaining.</returns>
        /// <remarks>
        /// The handler type is also registered unkeyed (if not already registered), so it
        /// receives ordinary constructor injection.
        /// </remarks>
        public static ContentHandlingBuilder AddContentHandler<TPayloadBase, TPayload, THandler>(
            this ContentHandlingBuilder builder,
            string contentType,
            string handlerClass,
            ServiceLifetime lifetime = ServiceLifetime.Transient)
            where TPayloadBase : notnull
            where TPayload : TPayloadBase
            where THandler : class, IContentHandler<TPayload>
        {
            ArgumentNullException.ThrowIfNull(builder);

            string key = ContentHandlerKeys.For(contentType, handlerClass);
            builder.Registry.Add(key, new ContentRegistration(key, typeof(IContentHandler<TPayloadBase>), ContentConstruction.FromServices, lifetime));
            builder.Services.TryAdd(ServiceDescriptor.Describe(typeof(THandler), typeof(THandler), lifetime));

            // Registering the adapter by type (rather than a factory) lets the container
            // construct it — and resolve THandler into it — through one compiled call site,
            // instead of a factory delegate making a nested GetRequiredService round-trip.
            builder.Services.Add(ServiceDescriptor.DescribeKeyed(
                typeof(IContentHandler<TPayloadBase>),
                key,
                typeof(ClassContentHandler<TPayloadBase, TPayload, THandler>),
                lifetime));
            return builder;
        }

        /// <summary>
        /// Registers a class-based handler for a handler class, deriving the content type from
        /// <typeparamref name="TPayload"/>.
        /// </summary>
        /// <typeparam name="TPayloadBase">The common base type of payloads for the dispatcher.</typeparam>
        /// <typeparam name="TPayload">The concrete payload type handled by the handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <param name="builder">The content handling builder.</param>
        /// <param name="handlerClass">The class of handler.</param>
        /// <param name="lifetime">The service lifetime for the handler.</param>
        /// <returns>The builder, for chaining.</returns>
        public static ContentHandlingBuilder AddContentHandler<TPayloadBase, TPayload, THandler>(
            this ContentHandlingBuilder builder,
            string handlerClass,
            ServiceLifetime lifetime = ServiceLifetime.Transient)
            where TPayloadBase : notnull
            where TPayload : TPayloadBase
            where THandler : class, IContentHandler<TPayload>
        {
            return builder.AddContentHandler<TPayloadBase, TPayload, THandler>(ContentTypes.GetContentType<TPayload>(), handlerClass, lifetime);
        }

        /// <summary>
        /// Registers a class-based handler receiving a context for a content type and handler class.
        /// </summary>
        /// <typeparam name="TPayloadBase">The common base type of payloads for the dispatcher.</typeparam>
        /// <typeparam name="TPayload">The concrete payload type handled by the handler.</typeparam>
        /// <typeparam name="TContext">The type of the context passed alongside the payload.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <param name="builder">The content handling builder.</param>
        /// <param name="contentType">The content type the handler handles.</param>
        /// <param name="handlerClass">The class of handler.</param>
        /// <param name="lifetime">The service lifetime for the handler.</param>
        /// <returns>The builder, for chaining.</returns>
        public static ContentHandlingBuilder AddContentHandler<TPayloadBase, TPayload, TContext, THandler>(
            this ContentHandlingBuilder builder,
            string contentType,
            string handlerClass,
            ServiceLifetime lifetime = ServiceLifetime.Transient)
            where TPayloadBase : notnull
            where TPayload : TPayloadBase
            where THandler : class, IContentHandler<TPayload, TContext>
        {
            ArgumentNullException.ThrowIfNull(builder);

            string key = ContentHandlerKeys.For(contentType, handlerClass);
            builder.Registry.Add(key, new ContentRegistration(key, typeof(IContentHandler<TPayloadBase, TContext>), ContentConstruction.FromServices, lifetime));
            builder.Services.TryAdd(ServiceDescriptor.Describe(typeof(THandler), typeof(THandler), lifetime));
            builder.Services.Add(ServiceDescriptor.DescribeKeyed(
                typeof(IContentHandler<TPayloadBase, TContext>),
                key,
                typeof(ClassContentHandler<TPayloadBase, TPayload, TContext, THandler>),
                lifetime));
            return builder;
        }

        /// <summary>
        /// Registers an asynchronous delegate as the result-producing handler for a content
        /// type and handler class.
        /// </summary>
        /// <typeparam name="TPayloadBase">The common base type of payloads for the dispatcher.</typeparam>
        /// <typeparam name="TPayload">The concrete payload type handled by the delegate.</typeparam>
        /// <typeparam name="TResult">The type of the result produced by the handler.</typeparam>
        /// <param name="builder">The content handling builder.</param>
        /// <param name="contentType">The content type the handler handles.</param>
        /// <param name="handlerClass">The class of handler.</param>
        /// <param name="handler">The delegate which handles the payload.</param>
        /// <returns>The builder, for chaining.</returns>
        public static ContentHandlingBuilder AddContentHandlerWithResult<TPayloadBase, TPayload, TResult>(
            this ContentHandlingBuilder builder,
            string contentType,
            string handlerClass,
            Func<TPayload, CancellationToken, ValueTask<TResult>> handler)
            where TPayloadBase : notnull
            where TPayload : TPayloadBase
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(handler);

            string key = ContentHandlerKeys.For(contentType, handlerClass);
            builder.Registry.Add(key, new ContentRegistration(key, typeof(IContentHandlerWithResult<TPayloadBase, TResult>), ContentConstruction.FromServices, ServiceLifetime.Singleton));
            builder.Services.AddKeyedSingleton<IContentHandlerWithResult<TPayloadBase, TResult>>(key, new DelegateContentHandlerWithResult<TPayloadBase, TPayload, TResult>(handler));
            return builder;
        }

        /// <summary>
        /// Registers a synchronous delegate as the result-producing handler for a content type
        /// and handler class.
        /// </summary>
        /// <typeparam name="TPayloadBase">The common base type of payloads for the dispatcher.</typeparam>
        /// <typeparam name="TPayload">The concrete payload type handled by the delegate.</typeparam>
        /// <typeparam name="TResult">The type of the result produced by the handler.</typeparam>
        /// <param name="builder">The content handling builder.</param>
        /// <param name="contentType">The content type the handler handles.</param>
        /// <param name="handlerClass">The class of handler.</param>
        /// <param name="handler">The delegate which handles the payload.</param>
        /// <returns>The builder, for chaining.</returns>
        public static ContentHandlingBuilder AddContentHandlerWithResult<TPayloadBase, TPayload, TResult>(
            this ContentHandlingBuilder builder,
            string contentType,
            string handlerClass,
            Func<TPayload, TResult> handler)
            where TPayloadBase : notnull
            where TPayload : TPayloadBase
        {
            ArgumentNullException.ThrowIfNull(handler);

            return builder.AddContentHandlerWithResult<TPayloadBase, TPayload, TResult>(
                contentType,
                handlerClass,
                (payload, _) => ValueTask.FromResult(handler(payload)));
        }

        /// <summary>
        /// Registers an asynchronous delegate receiving a context as the result-producing
        /// handler for a content type and handler class.
        /// </summary>
        /// <typeparam name="TPayloadBase">The common base type of payloads for the dispatcher.</typeparam>
        /// <typeparam name="TPayload">The concrete payload type handled by the delegate.</typeparam>
        /// <typeparam name="TContext">The type of the context passed alongside the payload.</typeparam>
        /// <typeparam name="TResult">The type of the result produced by the handler.</typeparam>
        /// <param name="builder">The content handling builder.</param>
        /// <param name="contentType">The content type the handler handles.</param>
        /// <param name="handlerClass">The class of handler.</param>
        /// <param name="handler">The delegate which handles the payload.</param>
        /// <returns>The builder, for chaining.</returns>
        public static ContentHandlingBuilder AddContentHandlerWithResult<TPayloadBase, TPayload, TContext, TResult>(
            this ContentHandlingBuilder builder,
            string contentType,
            string handlerClass,
            Func<TPayload, TContext, CancellationToken, ValueTask<TResult>> handler)
            where TPayloadBase : notnull
            where TPayload : TPayloadBase
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(handler);

            string key = ContentHandlerKeys.For(contentType, handlerClass);
            builder.Registry.Add(key, new ContentRegistration(key, typeof(IContentHandlerWithResult<TPayloadBase, TContext, TResult>), ContentConstruction.FromServices, ServiceLifetime.Singleton));
            builder.Services.AddKeyedSingleton<IContentHandlerWithResult<TPayloadBase, TContext, TResult>>(key, new DelegateContentHandlerWithResult<TPayloadBase, TPayload, TContext, TResult>(handler));
            return builder;
        }

        /// <summary>
        /// Registers a class-based result-producing handler for a content type and handler class.
        /// </summary>
        /// <typeparam name="TPayloadBase">The common base type of payloads for the dispatcher.</typeparam>
        /// <typeparam name="TPayload">The concrete payload type handled by the handler.</typeparam>
        /// <typeparam name="TResult">The type of the result produced by the handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <param name="builder">The content handling builder.</param>
        /// <param name="contentType">The content type the handler handles.</param>
        /// <param name="handlerClass">The class of handler.</param>
        /// <param name="lifetime">The service lifetime for the handler.</param>
        /// <returns>The builder, for chaining.</returns>
        public static ContentHandlingBuilder AddContentHandlerWithResult<TPayloadBase, TPayload, TResult, THandler>(
            this ContentHandlingBuilder builder,
            string contentType,
            string handlerClass,
            ServiceLifetime lifetime = ServiceLifetime.Transient)
            where TPayloadBase : notnull
            where TPayload : TPayloadBase
            where THandler : class, IContentHandlerWithResult<TPayload, TResult>
        {
            ArgumentNullException.ThrowIfNull(builder);

            string key = ContentHandlerKeys.For(contentType, handlerClass);
            builder.Registry.Add(key, new ContentRegistration(key, typeof(IContentHandlerWithResult<TPayloadBase, TResult>), ContentConstruction.FromServices, lifetime));
            builder.Services.TryAdd(ServiceDescriptor.Describe(typeof(THandler), typeof(THandler), lifetime));
            builder.Services.Add(ServiceDescriptor.DescribeKeyed(
                typeof(IContentHandlerWithResult<TPayloadBase, TResult>),
                key,
                typeof(ClassContentHandlerWithResult<TPayloadBase, TPayload, TResult, THandler>),
                lifetime));
            return builder;
        }

        /// <summary>
        /// Registers a class-based result-producing handler receiving a context for a content
        /// type and handler class.
        /// </summary>
        /// <typeparam name="TPayloadBase">The common base type of payloads for the dispatcher.</typeparam>
        /// <typeparam name="TPayload">The concrete payload type handled by the handler.</typeparam>
        /// <typeparam name="TContext">The type of the context passed alongside the payload.</typeparam>
        /// <typeparam name="TResult">The type of the result produced by the handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <param name="builder">The content handling builder.</param>
        /// <param name="contentType">The content type the handler handles.</param>
        /// <param name="handlerClass">The class of handler.</param>
        /// <param name="lifetime">The service lifetime for the handler.</param>
        /// <returns>The builder, for chaining.</returns>
        public static ContentHandlingBuilder AddContentHandlerWithResult<TPayloadBase, TPayload, TContext, TResult, THandler>(
            this ContentHandlingBuilder builder,
            string contentType,
            string handlerClass,
            ServiceLifetime lifetime = ServiceLifetime.Transient)
            where TPayloadBase : notnull
            where TPayload : TPayloadBase
            where THandler : class, IContentHandlerWithResult<TPayload, TContext, TResult>
        {
            ArgumentNullException.ThrowIfNull(builder);

            string key = ContentHandlerKeys.For(contentType, handlerClass);
            builder.Registry.Add(key, new ContentRegistration(key, typeof(IContentHandlerWithResult<TPayloadBase, TContext, TResult>), ContentConstruction.FromServices, lifetime));
            builder.Services.TryAdd(ServiceDescriptor.Describe(typeof(THandler), typeof(THandler), lifetime));
            builder.Services.Add(ServiceDescriptor.DescribeKeyed(
                typeof(IContentHandlerWithResult<TPayloadBase, TContext, TResult>),
                key,
                typeof(ClassContentHandlerWithResult<TPayloadBase, TPayload, TContext, TResult, THandler>),
                lifetime));
            return builder;
        }
    }
}
