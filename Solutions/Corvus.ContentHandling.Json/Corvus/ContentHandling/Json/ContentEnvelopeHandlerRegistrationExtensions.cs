// <copyright file="ContentEnvelopeHandlerRegistrationExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Corvus.ContentHandling.Json.Internal;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    /// <summary>
    /// Registers handlers for payloads delivered in a <see cref="ContentEnvelope"/>, which
    /// transparently unwrap the envelope before invoking the handler.
    /// </summary>
    /// <remarks>
    /// Handlers registered this way are dispatched via
    /// <see cref="IContentDispatcher{TPayloadBase}"/> of <see cref="ContentEnvelope"/> — for
    /// example through <see cref="ContentEnvelope.DispatchToHandlerAsync"/>.
    /// </remarks>
    public static class ContentEnvelopeHandlerRegistrationExtensions
    {
        /// <summary>
        /// Registers an asynchronous delegate as the handler for envelopes carrying a content
        /// type, invoked with the unwrapped payload.
        /// </summary>
        /// <typeparam name="TPayload">The type of the payload inside the envelope.</typeparam>
        /// <param name="builder">The content handling builder.</param>
        /// <param name="contentType">The content type the handler handles.</param>
        /// <param name="handlerClass">The class of handler.</param>
        /// <param name="handler">The delegate which handles the unwrapped payload.</param>
        /// <returns>The builder, for chaining.</returns>
        public static ContentHandlingBuilder AddContentEnvelopeHandler<TPayload>(
            this ContentHandlingBuilder builder,
            string contentType,
            string handlerClass,
            Func<TPayload, CancellationToken, ValueTask> handler)
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(handler);

            return builder.AddContentHandler<ContentEnvelope, ContentEnvelope>(
                contentType,
                handlerClass,
                (envelope, cancellationToken) => handler(envelope.GetContents<TPayload>(), cancellationToken));
        }

        /// <summary>
        /// Registers an asynchronous delegate as the handler for envelopes carrying the content
        /// type derived from <typeparamref name="TPayload"/>, invoked with the unwrapped payload.
        /// </summary>
        /// <typeparam name="TPayload">The type of the payload inside the envelope.</typeparam>
        /// <param name="builder">The content handling builder.</param>
        /// <param name="handlerClass">The class of handler.</param>
        /// <param name="handler">The delegate which handles the unwrapped payload.</param>
        /// <returns>The builder, for chaining.</returns>
        public static ContentHandlingBuilder AddContentEnvelopeHandler<TPayload>(
            this ContentHandlingBuilder builder,
            string handlerClass,
            Func<TPayload, CancellationToken, ValueTask> handler)
        {
            return builder.AddContentEnvelopeHandler(ContentTypes.GetContentType<TPayload>(), handlerClass, handler);
        }

        /// <summary>
        /// Registers a synchronous delegate as the handler for envelopes carrying a content
        /// type, invoked with the unwrapped payload.
        /// </summary>
        /// <typeparam name="TPayload">The type of the payload inside the envelope.</typeparam>
        /// <param name="builder">The content handling builder.</param>
        /// <param name="contentType">The content type the handler handles.</param>
        /// <param name="handlerClass">The class of handler.</param>
        /// <param name="handler">The delegate which handles the unwrapped payload.</param>
        /// <returns>The builder, for chaining.</returns>
        public static ContentHandlingBuilder AddContentEnvelopeHandler<TPayload>(
            this ContentHandlingBuilder builder,
            string contentType,
            string handlerClass,
            Action<TPayload> handler)
        {
            ArgumentNullException.ThrowIfNull(handler);

            return builder.AddContentEnvelopeHandler<TPayload>(
                contentType,
                handlerClass,
                (payload, _) =>
                {
                    handler(payload);
                    return ValueTask.CompletedTask;
                });
        }

        /// <summary>
        /// Registers a synchronous delegate as the handler for envelopes carrying the content
        /// type derived from <typeparamref name="TPayload"/>, invoked with the unwrapped payload.
        /// </summary>
        /// <typeparam name="TPayload">The type of the payload inside the envelope.</typeparam>
        /// <param name="builder">The content handling builder.</param>
        /// <param name="handlerClass">The class of handler.</param>
        /// <param name="handler">The delegate which handles the unwrapped payload.</param>
        /// <returns>The builder, for chaining.</returns>
        public static ContentHandlingBuilder AddContentEnvelopeHandler<TPayload>(
            this ContentHandlingBuilder builder,
            string handlerClass,
            Action<TPayload> handler)
        {
            return builder.AddContentEnvelopeHandler(ContentTypes.GetContentType<TPayload>(), handlerClass, handler);
        }

        /// <summary>
        /// Registers a class-based handler for envelopes carrying a content type, invoked with
        /// the unwrapped payload.
        /// </summary>
        /// <typeparam name="TPayload">The type of the payload inside the envelope.</typeparam>
        /// <typeparam name="THandler">The type of the handler for the payload.</typeparam>
        /// <param name="builder">The content handling builder.</param>
        /// <param name="contentType">The content type the handler handles.</param>
        /// <param name="handlerClass">The class of handler.</param>
        /// <param name="lifetime">The service lifetime for the handler.</param>
        /// <returns>The builder, for chaining.</returns>
        public static ContentHandlingBuilder AddContentEnvelopeHandler<TPayload, THandler>(
            this ContentHandlingBuilder builder,
            string contentType,
            string handlerClass,
            ServiceLifetime lifetime = ServiceLifetime.Transient)
            where THandler : class, IContentHandler<TPayload>
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.Services.TryAdd(ServiceDescriptor.Describe(typeof(THandler), typeof(THandler), lifetime));
            return builder.AddContentHandler<ContentEnvelope, ContentEnvelope, ContentEnvelopeHandler<TPayload, THandler>>(contentType, handlerClass, lifetime);
        }

        /// <summary>
        /// Registers a class-based handler for envelopes carrying the content type derived from
        /// <typeparamref name="TPayload"/>, invoked with the unwrapped payload.
        /// </summary>
        /// <typeparam name="TPayload">The type of the payload inside the envelope.</typeparam>
        /// <typeparam name="THandler">The type of the handler for the payload.</typeparam>
        /// <param name="builder">The content handling builder.</param>
        /// <param name="handlerClass">The class of handler.</param>
        /// <param name="lifetime">The service lifetime for the handler.</param>
        /// <returns>The builder, for chaining.</returns>
        public static ContentHandlingBuilder AddContentEnvelopeHandler<TPayload, THandler>(
            this ContentHandlingBuilder builder,
            string handlerClass,
            ServiceLifetime lifetime = ServiceLifetime.Transient)
            where THandler : class, IContentHandler<TPayload>
        {
            return builder.AddContentEnvelopeHandler<TPayload, THandler>(ContentTypes.GetContentType<TPayload>(), handlerClass, lifetime);
        }
    }
}
