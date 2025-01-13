// <copyright file="ContentEnvelopeHandlerWithResultAndClass{TContents,THandler,TResult}.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json.Internal.ContentEnvelopeHandler
{
    /// <summary>
    /// A wrapper class for content handlers of specific types.
    /// </summary>
    /// <typeparam name="TContents">The concrete type of the contents of the envelope.</typeparam>
    /// <typeparam name="THandler">The type of the handler.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public class ContentEnvelopeHandlerWithResultAndClass<TContents, THandler, TResult> : IContentHandlerWithResult<ContentEnvelope, TResult>
        where THandler : IContentHandlerWithResult<TContents, TResult>
    {
        private readonly IContentHandlerWithResult<TContents, TResult> handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentEnvelopeHandlerWithResultAndClass{TPayload, THandler, TResult}"/> class.
        /// </summary>
        /// <param name="handler">The instance to use to handle the payload.</param>
        public ContentEnvelopeHandlerWithResultAndClass(THandler handler)
        {
            this.handler = handler;
        }

        /// <inheritdoc/>
        public TResult Handle(ContentEnvelope payload)
        {
            System.ArgumentNullException.ThrowIfNull(payload);

            return this.handler.Handle(payload.GetContents<TContents>());
        }
    }
}