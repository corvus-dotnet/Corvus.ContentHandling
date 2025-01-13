// <copyright file="ContentEnvelopeHandlerWithClass{TContents,THandler}.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json.Internal.ContentEnvelopeHandler
{
    /// <summary>
    /// A wrapper class for content handlers of specific types.
    /// </summary>
    /// <typeparam name="TContents">The concrete type of the contents of the envelope.</typeparam>
    /// <typeparam name="THandler">The type of the handler.</typeparam>
    public class ContentEnvelopeHandlerWithClass<TContents, THandler> : IContentHandler<ContentEnvelope>
        where THandler : IContentHandler<TContents>
    {
        private readonly IContentHandler<TContents> handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentEnvelopeHandlerWithClass{TPayload, THandler}"/> class.
        /// </summary>
        /// <param name="handler">The instance to use to handle the payload.</param>
        public ContentEnvelopeHandlerWithClass(THandler handler)
        {
            this.handler = handler;
        }

        /// <inheritdoc/>
        public void Handle(ContentEnvelope payload)
        {
            System.ArgumentNullException.ThrowIfNull(payload);

            this.handler.Handle(payload.GetContents<TContents>());
        }
    }
}