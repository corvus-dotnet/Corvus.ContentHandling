﻿// <copyright file="ContentEnvelopeHandlerWithResultAndClass{TContents,THandler,T1,T2,TResult}.cs" company="Endjin Limited">
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
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public class ContentEnvelopeHandlerWithResultAndClass<TContents, THandler, T1, T2, TResult> : IContentHandlerWithResult<ContentEnvelope, T1, T2, TResult>
        where THandler : IContentHandlerWithResult<TContents, T1, T2, TResult>
    {
        private readonly THandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentEnvelopeHandlerWithResultAndClass{TPayload, THandler, T1, T2, TResult}"/> class.
        /// </summary>
        /// <param name="handler">The instance to use to handle the payload.</param>
        public ContentEnvelopeHandlerWithResultAndClass(THandler handler)
        {
            this.handler = handler;
        }

        /// <inheritdoc/>
        public TResult Handle(ContentEnvelope payload, T1 param1, T2 param2)
        {
            System.ArgumentNullException.ThrowIfNull(payload);

            return this.handler.Handle(payload.GetContents<TContents>(), param1, param2);
        }
    }
}