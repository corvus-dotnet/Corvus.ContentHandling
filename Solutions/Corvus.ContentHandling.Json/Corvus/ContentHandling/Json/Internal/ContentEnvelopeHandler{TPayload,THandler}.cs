// <copyright file="ContentEnvelopeHandler{TPayload,THandler}.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json.Internal
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Adapts a payload handler to an envelope handler: unwraps the
    /// <see cref="ContentEnvelope"/> payload before invoking the inner handler.
    /// </summary>
    /// <typeparam name="TPayload">The type of the payload inside the envelope.</typeparam>
    /// <typeparam name="THandler">The type of the handler for the payload.</typeparam>
    internal sealed class ContentEnvelopeHandler<TPayload, THandler> : IContentHandler<ContentEnvelope>
        where THandler : IContentHandler<TPayload>
    {
        private readonly THandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentEnvelopeHandler{TPayload, THandler}"/> class.
        /// </summary>
        /// <param name="handler">The handler for the unwrapped payload.</param>
        public ContentEnvelopeHandler(THandler handler)
        {
            this.handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        /// <inheritdoc/>
        public ValueTask HandleAsync(ContentEnvelope payload, CancellationToken cancellationToken = default)
        {
            return this.handler.HandleAsync(payload.GetContents<TPayload>(), cancellationToken);
        }
    }
}
