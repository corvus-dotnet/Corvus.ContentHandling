// <copyright file="DelegateContentHandler{TPayloadBase,TPayload,TContext}.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Internal
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Adapts a delegate to <see cref="IContentHandler{TPayload, TContext}"/> for the dispatcher's payload base type.
    /// </summary>
    /// <typeparam name="TPayloadBase">The common base type of payloads for the dispatcher.</typeparam>
    /// <typeparam name="TPayload">The concrete payload type handled by the delegate.</typeparam>
    /// <typeparam name="TContext">The type of the context passed alongside the payload.</typeparam>
    internal sealed class DelegateContentHandler<TPayloadBase, TPayload, TContext> : IContentHandler<TPayloadBase, TContext>
        where TPayloadBase : notnull
        where TPayload : TPayloadBase
    {
        private readonly Func<TPayload, TContext, CancellationToken, ValueTask> handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateContentHandler{TPayloadBase, TPayload, TContext}"/> class.
        /// </summary>
        /// <param name="handler">The delegate which handles the payload.</param>
        public DelegateContentHandler(Func<TPayload, TContext, CancellationToken, ValueTask> handler)
        {
            this.handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        /// <inheritdoc/>
        public ValueTask HandleAsync(TPayloadBase payload, TContext context, CancellationToken cancellationToken = default)
        {
            return this.handler((TPayload)payload, context, cancellationToken);
        }
    }
}
