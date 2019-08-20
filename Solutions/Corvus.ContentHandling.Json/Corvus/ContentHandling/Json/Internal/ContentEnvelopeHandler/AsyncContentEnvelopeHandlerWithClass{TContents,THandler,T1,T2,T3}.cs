﻿// <copyright file="AsyncContentEnvelopeHandlerWithClass{TContents,THandler,T1,T2,T3}.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json.Internal.ContentEnvelopeHandler
{
    using System.Threading.Tasks;

    /// <summary>
    /// A wrapper class for content handlers of specific types.
    /// </summary>
    /// <typeparam name="TContents">The concrete type of the contents of the envelope.</typeparam>
    /// <typeparam name="THandler">The type of the handler.</typeparam>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    public class AsyncContentEnvelopeHandlerWithClass<TContents, THandler, T1, T2, T3> : IAsyncContentHandler<ContentEnvelope, T1, T2, T3>
        where THandler : IAsyncContentHandler<TContents, T1, T2, T3>
    {
        private readonly THandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncContentEnvelopeHandlerWithClass{TPayload, THandler, T1, T2, T3}"/> class.
        /// </summary>
        /// <param name="handler">The instance to use to handle the payload.</param>
        public AsyncContentEnvelopeHandlerWithClass(THandler handler)
        {
            this.handler = handler;
        }

        /// <inheritdoc/>
        public Task HandleAsync(ContentEnvelope payload, T1 param1, T2 param2, T3 param3)
        {
            return this.handler.HandleAsync(payload.GetContents<TContents>(), param1, param2, param3);
        }
    }
}