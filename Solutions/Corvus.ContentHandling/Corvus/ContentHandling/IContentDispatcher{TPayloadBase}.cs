// <copyright file="IContentDispatcher{TPayloadBase}.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Dispatches payloads to content handlers registered for the payload's content type and
    /// a handler class.
    /// </summary>
    /// <typeparam name="TPayloadBase">The common base type of payloads dispatched through this dispatcher.</typeparam>
    /// <remarks>
    /// <para>
    /// Handlers are registered for a (content type, handler class) pair. The handler class
    /// partitions handlers by purpose: you might register a <c>"renderer"</c> handler and an
    /// <c>"auditor"</c> handler for the same content type.
    /// </para>
    /// <para>
    /// Handler resolution applies the same hierarchical content-type fallback as content
    /// resolution: a payload with content type <c>application/vnd.corvus.a.b.c</c> is handled
    /// by a handler registered for <c>application/vnd.corvus.a.b</c> when no more specific
    /// handler exists.
    /// </para>
    /// <para>
    /// Overloads without an explicit content type discover it from the payload instance
    /// (a public <c>ContentType</c> property, a <see cref="ContentTypeAttribute"/>, or a
    /// static <c>RegisteredContentType</c> field).
    /// </para>
    /// <para>
    /// Note on overload resolution: when <typeparamref name="TPayloadBase"/> is dispatched with
    /// a string context, calls like <c>DispatchAsync(payload, "a", "b")</c> bind to the
    /// (contentType, handlerClass) overload; pass the type argument explicitly
    /// (<c>DispatchAsync&lt;string&gt;(payload, context, handlerClass)</c>) to use a string context.
    /// </para>
    /// </remarks>
    public interface IContentDispatcher<TPayloadBase>
        where TPayloadBase : notnull
    {
        /// <summary>
        /// Dispatches a payload to the handler for its content type, discovered from the payload.
        /// </summary>
        /// <param name="payload">The payload to dispatch.</param>
        /// <param name="handlerClass">The class of handler to dispatch to.</param>
        /// <param name="cancellationToken">A token to observe for cancellation.</param>
        /// <returns>A task which completes when the payload has been handled.</returns>
        ValueTask DispatchAsync(TPayloadBase payload, string handlerClass, CancellationToken cancellationToken = default);

        /// <summary>
        /// Dispatches a payload to the handler for the specified content type.
        /// </summary>
        /// <param name="payload">The payload to dispatch.</param>
        /// <param name="contentType">The content type determining the handler.</param>
        /// <param name="handlerClass">The class of handler to dispatch to.</param>
        /// <param name="cancellationToken">A token to observe for cancellation.</param>
        /// <returns>A task which completes when the payload has been handled.</returns>
        ValueTask DispatchAsync(TPayloadBase payload, string contentType, string handlerClass, CancellationToken cancellationToken = default);

        /// <summary>
        /// Dispatches a payload and context to the handler for its content type, discovered from the payload.
        /// </summary>
        /// <typeparam name="TContext">The type of the context passed alongside the payload.</typeparam>
        /// <param name="payload">The payload to dispatch.</param>
        /// <param name="context">The context for this dispatch.</param>
        /// <param name="handlerClass">The class of handler to dispatch to.</param>
        /// <param name="cancellationToken">A token to observe for cancellation.</param>
        /// <returns>A task which completes when the payload has been handled.</returns>
        ValueTask DispatchAsync<TContext>(TPayloadBase payload, TContext context, string handlerClass, CancellationToken cancellationToken = default);

        /// <summary>
        /// Dispatches a payload and context to the handler for the specified content type.
        /// </summary>
        /// <typeparam name="TContext">The type of the context passed alongside the payload.</typeparam>
        /// <param name="payload">The payload to dispatch.</param>
        /// <param name="context">The context for this dispatch.</param>
        /// <param name="contentType">The content type determining the handler.</param>
        /// <param name="handlerClass">The class of handler to dispatch to.</param>
        /// <param name="cancellationToken">A token to observe for cancellation.</param>
        /// <returns>A task which completes when the payload has been handled.</returns>
        ValueTask DispatchAsync<TContext>(TPayloadBase payload, TContext context, string contentType, string handlerClass, CancellationToken cancellationToken = default);

        /// <summary>
        /// Dispatches a payload to the result-producing handler for its content type, discovered from the payload.
        /// </summary>
        /// <typeparam name="TResult">The type of the result produced by the handler.</typeparam>
        /// <param name="payload">The payload to dispatch.</param>
        /// <param name="handlerClass">The class of handler to dispatch to.</param>
        /// <param name="cancellationToken">A token to observe for cancellation.</param>
        /// <returns>A task producing the result of handling the payload.</returns>
        ValueTask<TResult> DispatchWithResultAsync<TResult>(TPayloadBase payload, string handlerClass, CancellationToken cancellationToken = default);

        /// <summary>
        /// Dispatches a payload to the result-producing handler for the specified content type.
        /// </summary>
        /// <typeparam name="TResult">The type of the result produced by the handler.</typeparam>
        /// <param name="payload">The payload to dispatch.</param>
        /// <param name="contentType">The content type determining the handler.</param>
        /// <param name="handlerClass">The class of handler to dispatch to.</param>
        /// <param name="cancellationToken">A token to observe for cancellation.</param>
        /// <returns>A task producing the result of handling the payload.</returns>
        ValueTask<TResult> DispatchWithResultAsync<TResult>(TPayloadBase payload, string contentType, string handlerClass, CancellationToken cancellationToken = default);

        /// <summary>
        /// Dispatches a payload and context to the result-producing handler for its content
        /// type, discovered from the payload.
        /// </summary>
        /// <typeparam name="TContext">The type of the context passed alongside the payload.</typeparam>
        /// <typeparam name="TResult">The type of the result produced by the handler.</typeparam>
        /// <param name="payload">The payload to dispatch.</param>
        /// <param name="context">The context for this dispatch.</param>
        /// <param name="handlerClass">The class of handler to dispatch to.</param>
        /// <param name="cancellationToken">A token to observe for cancellation.</param>
        /// <returns>A task producing the result of handling the payload.</returns>
        ValueTask<TResult> DispatchWithResultAsync<TContext, TResult>(TPayloadBase payload, TContext context, string handlerClass, CancellationToken cancellationToken = default);

        /// <summary>
        /// Dispatches a payload and context to the result-producing handler for the specified content type.
        /// </summary>
        /// <typeparam name="TContext">The type of the context passed alongside the payload.</typeparam>
        /// <typeparam name="TResult">The type of the result produced by the handler.</typeparam>
        /// <param name="payload">The payload to dispatch.</param>
        /// <param name="context">The context for this dispatch.</param>
        /// <param name="contentType">The content type determining the handler.</param>
        /// <param name="handlerClass">The class of handler to dispatch to.</param>
        /// <param name="cancellationToken">A token to observe for cancellation.</param>
        /// <returns>A task producing the result of handling the payload.</returns>
        ValueTask<TResult> DispatchWithResultAsync<TContext, TResult>(TPayloadBase payload, TContext context, string contentType, string handlerClass, CancellationToken cancellationToken = default);
    }
}
