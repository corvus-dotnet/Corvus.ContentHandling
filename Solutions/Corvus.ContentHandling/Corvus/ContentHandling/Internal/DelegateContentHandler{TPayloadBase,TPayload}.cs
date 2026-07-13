// <copyright file="DelegateContentHandler{TPayloadBase,TPayload}.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Internal
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Adapts a delegate to <see cref="IContentHandler{TPayload}"/> for the dispatcher's payload base type.
    /// </summary>
    /// <typeparam name="TPayloadBase">The common base type of payloads for the dispatcher.</typeparam>
    /// <typeparam name="TPayload">The concrete payload type handled by the delegate.</typeparam>
    /// <remarks>
    /// Multiple delegate handlers are distinct keyed instances of this same closed generic
    /// type, registered under different keys — service identity for keyed services is the
    /// (service type, key) pair, so no unique wrapper type per registration is required.
    /// </remarks>
    internal sealed class DelegateContentHandler<TPayloadBase, TPayload> : IContentHandler<TPayloadBase>
        where TPayloadBase : notnull
        where TPayload : TPayloadBase
    {
        private readonly Func<TPayload, CancellationToken, ValueTask> handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateContentHandler{TPayloadBase, TPayload}"/> class.
        /// </summary>
        /// <param name="handler">The delegate which handles the payload.</param>
        public DelegateContentHandler(Func<TPayload, CancellationToken, ValueTask> handler)
        {
            this.handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        /// <inheritdoc/>
        public ValueTask HandleAsync(TPayloadBase payload, CancellationToken cancellationToken = default)
        {
            return this.handler((TPayload)payload, cancellationToken);
        }
    }
}
