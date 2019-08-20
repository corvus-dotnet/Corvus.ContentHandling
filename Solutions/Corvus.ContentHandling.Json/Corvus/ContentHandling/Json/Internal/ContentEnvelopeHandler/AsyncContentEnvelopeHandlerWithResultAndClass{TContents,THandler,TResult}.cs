// <copyright file="AsyncContentEnvelopeHandlerWithResultAndClass{TContents,THandler,TResult}.cs" company="Endjin Limited">
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
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public class AsyncContentEnvelopeHandlerWithResultAndClass<TContents, THandler, TResult> : IAsyncContentHandlerWithResult<ContentEnvelope, TResult>
        where THandler : IAsyncContentHandlerWithResult<TContents, TResult>
    {
        private readonly THandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncContentEnvelopeHandlerWithResultAndClass{TPayload, THandler, TResult}"/> class.
        /// </summary>
        /// <param name="handler">The instance to use to handle the payload.</param>
        public AsyncContentEnvelopeHandlerWithResultAndClass(THandler handler)
        {
            this.handler = handler;
        }

        /// <inheritdoc/>
        public Task<TResult> HandleAsync(ContentEnvelope payload)
        {
            return this.handler.HandleAsync(payload.GetContents<TContents>());
        }
    }
}
