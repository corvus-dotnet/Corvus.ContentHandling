// <copyright file="AsyncContentEnvelopeHandlerWithClass{TContents,THandler}.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json.Internal.ContentEnvelopeHandler
{
    using System.Threading.Tasks;

    /// <summary>
    /// A wrapper class for content handlers of specific types.
    /// </summary>
    /// <typeparam name="TContents">The concrete type of the contents of the envelope.</typeparam>
    /// <typeparam name="THandler">The concrete type of the handler.</typeparam>
    public class AsyncContentEnvelopeHandlerWithClass<TContents, THandler> : IAsyncContentHandler<ContentEnvelope>
        where THandler : IAsyncContentHandler<TContents>
    {
        private readonly THandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncContentEnvelopeHandlerWithClass{TPayload, THandler}"/> class.
        /// </summary>
        /// <param name="handler">The instance to use to handle the payload.</param>
        public AsyncContentEnvelopeHandlerWithClass(THandler handler)
        {
            this.handler = handler;
        }

        /// <inheritdoc/>
        public Task HandleAsync(ContentEnvelope payload)
        {
            if (payload is null)
            {
                throw new System.ArgumentNullException(nameof(payload));
            }

            return this.handler.HandleAsync(payload.GetContents<TContents>());
        }
    }
}
