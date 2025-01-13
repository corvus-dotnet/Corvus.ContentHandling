// <copyright file="ContentEnvelopeHandlerWithClass{TContents,THandler,T1,T2,T3}.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json.Internal.ContentEnvelopeHandler
{
    /// <summary>
    /// A wrapper class for content handlers of specific types.
    /// </summary>
    /// <typeparam name="TContents">The concrete type of the contents of the envelope.</typeparam>
    /// <typeparam name="THandler">The type of the handler.</typeparam>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    public class ContentEnvelopeHandlerWithClass<TContents, THandler, T1, T2, T3> : IContentHandler<ContentEnvelope, T1, T2, T3>
        where THandler : IContentHandler<TContents, T1, T2, T3>
    {
        private readonly THandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentEnvelopeHandlerWithClass{TPayload, THandler, T1, T2, T3}"/> class.
        /// </summary>
        /// <param name="handler">The instance to use to handle the payload.</param>
        public ContentEnvelopeHandlerWithClass(THandler handler)
        {
            this.handler = handler;
        }

        /// <inheritdoc/>
        public void Handle(ContentEnvelope payload, T1 parameter1, T2 parameter2, T3 parameter3)
        {
            System.ArgumentNullException.ThrowIfNull(payload);

            this.handler.Handle(payload.GetContents<TContents>(), parameter1, parameter2, parameter3);
        }
    }
}