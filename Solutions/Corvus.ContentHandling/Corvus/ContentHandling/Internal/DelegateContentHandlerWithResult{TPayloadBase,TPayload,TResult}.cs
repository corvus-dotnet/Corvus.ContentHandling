// <copyright file="DelegateContentHandlerWithResult{TPayloadBase,TPayload,TResult}.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Internal
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Adapts a delegate to <see cref="IContentHandlerWithResult{TPayload, TResult}"/> for the dispatcher's payload base type.
    /// </summary>
    /// <typeparam name="TPayloadBase">The common base type of payloads for the dispatcher.</typeparam>
    /// <typeparam name="TPayload">The concrete payload type handled by the delegate.</typeparam>
    /// <typeparam name="TResult">The type of the result produced by the handler.</typeparam>
    internal sealed class DelegateContentHandlerWithResult<TPayloadBase, TPayload, TResult> : IContentHandlerWithResult<TPayloadBase, TResult>
        where TPayloadBase : notnull
        where TPayload : TPayloadBase
    {
        private readonly Func<TPayload, CancellationToken, ValueTask<TResult>> handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateContentHandlerWithResult{TPayloadBase, TPayload, TResult}"/> class.
        /// </summary>
        /// <param name="handler">The delegate which handles the payload.</param>
        public DelegateContentHandlerWithResult(Func<TPayload, CancellationToken, ValueTask<TResult>> handler)
        {
            this.handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        /// <inheritdoc/>
        public ValueTask<TResult> HandleAsync(TPayloadBase payload, CancellationToken cancellationToken = default)
        {
            return this.handler((TPayload)payload, cancellationToken);
        }
    }
}
