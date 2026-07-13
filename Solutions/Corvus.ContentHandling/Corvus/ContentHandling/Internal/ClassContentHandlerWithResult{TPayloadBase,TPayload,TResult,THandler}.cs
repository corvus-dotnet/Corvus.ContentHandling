// <copyright file="ClassContentHandlerWithResult{TPayloadBase,TPayload,TResult,THandler}.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Internal
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Adapts a handler implementing <see cref="IContentHandlerWithResult{TPayload, TResult}"/>
    /// for a concrete payload type to the dispatcher's payload base type.
    /// </summary>
    /// <typeparam name="TPayloadBase">The common base type of payloads for the dispatcher.</typeparam>
    /// <typeparam name="TPayload">The concrete payload type handled by the handler.</typeparam>
    /// <typeparam name="TResult">The type of the result produced by the handler.</typeparam>
    /// <typeparam name="THandler">The type of the handler.</typeparam>
    internal sealed class ClassContentHandlerWithResult<TPayloadBase, TPayload, TResult, THandler> : IContentHandlerWithResult<TPayloadBase, TResult>
        where TPayloadBase : notnull
        where TPayload : TPayloadBase
        where THandler : IContentHandlerWithResult<TPayload, TResult>
    {
        private readonly THandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassContentHandlerWithResult{TPayloadBase, TPayload, TResult, THandler}"/> class.
        /// </summary>
        /// <param name="handler">The handler to adapt.</param>
        public ClassContentHandlerWithResult(THandler handler)
        {
            this.handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        /// <inheritdoc/>
        public ValueTask<TResult> HandleAsync(TPayloadBase payload, CancellationToken cancellationToken = default)
        {
            return this.handler.HandleAsync((TPayload)payload, cancellationToken);
        }
    }
}
