// <copyright file="ContentEnvelopeHandlerWithClass{TContents,THandler,T1}.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json.Internal.ContentEnvelopeHandler
{
    /// <summary>
    /// A wrapper class for content handlers of specific types.
    /// </summary>
    /// <typeparam name="TContents">The concrete type of the payload.</typeparam>
    /// <typeparam name="THandler">The type of the handler.</typeparam>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    public class ContentEnvelopeHandlerWithClass<TContents, THandler, T1> : IContentHandler<ContentEnvelope, T1>
        where THandler : IContentHandler<TContents, T1>
    {
        private readonly THandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentEnvelopeHandlerWithClass{TPayload, THandler, T1}"/> class.
        /// </summary>
        /// <param name="handler">The instance to use to handle the payload.</param>
        public ContentEnvelopeHandlerWithClass(THandler handler)
        {
            this.handler = handler;
        }

        /// <inheritdoc/>
        public void Handle(ContentEnvelope payload, T1 parameter)
        {
            if (payload is null)
            {
                throw new System.ArgumentNullException(nameof(payload));
            }

            this.handler.Handle(payload.GetContents<TContents>(), parameter);
        }
    }
}
