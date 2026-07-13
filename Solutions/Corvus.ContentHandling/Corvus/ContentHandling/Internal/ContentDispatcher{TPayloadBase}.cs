// <copyright file="ContentDispatcher{TPayloadBase}.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Internal
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Dispatches payloads to content handlers resolved as keyed services.
    /// </summary>
    /// <typeparam name="TPayloadBase">The common base type of payloads dispatched through this dispatcher.</typeparam>
    /// <remarks>
    /// Registered as a transient open generic, so it resolves handlers from the provider of
    /// the scope it was itself resolved from — scoped handlers get correct scope semantics.
    /// </remarks>
    internal sealed class ContentDispatcher<TPayloadBase> : IContentDispatcher<TPayloadBase>
        where TPayloadBase : notnull
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IContentRegistry registry;
        private System.Collections.Concurrent.ConcurrentDictionary<ContentRegistration, object>? resolvedHandlers;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentDispatcher{TPayloadBase}"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider from which handlers are resolved.</param>
        /// <param name="registry">The content registry.</param>
        public ContentDispatcher(IServiceProvider serviceProvider, IContentRegistry registry)
        {
            this.serviceProvider = serviceProvider;
            this.registry = registry;
        }

        /// <inheritdoc/>
        public ValueTask DispatchAsync(TPayloadBase payload, string handlerClass, CancellationToken cancellationToken = default)
        {
            return this.DispatchAsync(payload, GetPayloadContentType(payload), handlerClass, cancellationToken);
        }

        /// <inheritdoc/>
        public ValueTask DispatchAsync(TPayloadBase payload, string contentType, string handlerClass, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(payload);

            return this.GetHandler<IContentHandler<TPayloadBase>>(contentType, handlerClass)
                .HandleAsync(payload, cancellationToken);
        }

        /// <inheritdoc/>
        public ValueTask DispatchAsync<TContext>(TPayloadBase payload, TContext context, string handlerClass, CancellationToken cancellationToken = default)
        {
            return this.DispatchAsync(payload, context, GetPayloadContentType(payload), handlerClass, cancellationToken);
        }

        /// <inheritdoc/>
        public ValueTask DispatchAsync<TContext>(TPayloadBase payload, TContext context, string contentType, string handlerClass, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(payload);

            return this.GetHandler<IContentHandler<TPayloadBase, TContext>>(contentType, handlerClass)
                .HandleAsync(payload, context, cancellationToken);
        }

        /// <inheritdoc/>
        public ValueTask<TResult> DispatchWithResultAsync<TResult>(TPayloadBase payload, string handlerClass, CancellationToken cancellationToken = default)
        {
            return this.DispatchWithResultAsync<TResult>(payload, GetPayloadContentType(payload), handlerClass, cancellationToken);
        }

        /// <inheritdoc/>
        public ValueTask<TResult> DispatchWithResultAsync<TResult>(TPayloadBase payload, string contentType, string handlerClass, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(payload);

            return this.GetHandler<IContentHandlerWithResult<TPayloadBase, TResult>>(contentType, handlerClass)
                .HandleAsync(payload, cancellationToken);
        }

        /// <inheritdoc/>
        public ValueTask<TResult> DispatchWithResultAsync<TContext, TResult>(TPayloadBase payload, TContext context, string handlerClass, CancellationToken cancellationToken = default)
        {
            return this.DispatchWithResultAsync<TContext, TResult>(payload, context, GetPayloadContentType(payload), handlerClass, cancellationToken);
        }

        /// <inheritdoc/>
        public ValueTask<TResult> DispatchWithResultAsync<TContext, TResult>(TPayloadBase payload, TContext context, string contentType, string handlerClass, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(payload);

            return this.GetHandler<IContentHandlerWithResult<TPayloadBase, TContext, TResult>>(contentType, handlerClass)
                .HandleAsync(payload, context, cancellationToken);
        }

        private static string GetPayloadContentType(TPayloadBase payload)
        {
            ArgumentNullException.ThrowIfNull(payload);

            return ContentTypes.GetContentType(payload);
        }

        private THandler GetHandler<THandler>(string contentType, string handlerClass)
            where THandler : class
        {
            // The registry caches handler resolution per (content type, handler class) pair,
            // avoiding the key composition and fallback probe on repeat dispatches.
            bool found = this.registry is ContentRegistry concreteRegistry
                ? concreteRegistry.TryResolveHandler(contentType, handlerClass, out ContentRegistration? registration)
                : this.registry.TryResolve(ContentHandlerKeys.For(contentType, handlerClass), out registration);

            if (!found)
            {
                throw new InvalidOperationException($"No content handler is registered for content type '{contentType}' and handler class '{handlerClass}'.");
            }

            // Handlers with singleton or scoped lifetime are stable for the provider this
            // dispatcher was resolved from, so cache them per registration (reference-keyed —
            // ContentRegistration is a record, and value hashing would re-hash the key string).
            // Transient handlers must be resolved fresh on every dispatch.
            if (registration!.Lifetime is not (ServiceLifetime.Singleton or ServiceLifetime.Scoped))
            {
                return this.serviceProvider.GetRequiredKeyedService<THandler>(registration.ContentType);
            }

            System.Collections.Concurrent.ConcurrentDictionary<ContentRegistration, object> cache =
                this.resolvedHandlers ??= new(System.Collections.Generic.ReferenceEqualityComparer.Instance);

            if (cache.TryGetValue(registration, out object? cached))
            {
                return (THandler)cached;
            }

            THandler handler = this.serviceProvider.GetRequiredKeyedService<THandler>(registration.ContentType);
            cache.TryAdd(registration, handler);
            return handler;
        }
    }
}
