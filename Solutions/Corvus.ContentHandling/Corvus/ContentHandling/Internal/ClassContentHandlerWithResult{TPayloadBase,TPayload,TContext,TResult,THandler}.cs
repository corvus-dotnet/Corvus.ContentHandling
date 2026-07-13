// <copyright file="ClassContentHandlerWithResult{TPayloadBase,TPayload,TContext,TResult,THandler}.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Internal
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Adapts a handler implementing <see cref="IContentHandlerWithResult{TPayload, TContext, TResult}"/>
    /// for a concrete payload type to the dispatcher's payload base type.
    /// </summary>
    /// <typeparam name="TPayloadBase">The common base type of payloads for the dispatcher.</typeparam>
    /// <typeparam name="TPayload">The concrete payload type handled by the handler.</typeparam>
    /// <typeparam name="TContext">The type of the context passed alongside the payload.</typeparam>
    /// <typeparam name="TResult">The type of the result produced by the handler.</typeparam>
    /// <typeparam name="THandler">The type of the handler.</typeparam>
    internal sealed class ClassContentHandlerWithResult<TPayloadBase, TPayload, TContext, TResult, THandler> : IContentHandlerWithResult<TPayloadBase, TContext, TResult>
        where TPayloadBase : notnull
        where TPayload : TPayloadBase
        where THandler : IContentHandlerWithResult<TPayload, TContext, TResult>
    {
        private readonly THandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassContentHandlerWithResult{TPayloadBase, TPayload, TContext, TResult, THandler}"/> class.
        /// </summary>
        /// <param name="handler">The handler to adapt.</param>
        public ClassContentHandlerWithResult(THandler handler)
        {
            this.handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        /// <inheritdoc/>
        public ValueTask<TResult> HandleAsync(TPayloadBase payload, TContext context, CancellationToken cancellationToken = default)
        {
            return this.handler.HandleAsync((TPayload)payload, context, cancellationToken);
        }
    }
}
