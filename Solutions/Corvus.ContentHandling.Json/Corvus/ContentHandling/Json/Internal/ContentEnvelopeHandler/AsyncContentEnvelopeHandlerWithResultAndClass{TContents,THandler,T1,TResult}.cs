// <copyright file="AsyncContentEnvelopeHandlerWithResultAndClass{TContents,THandler,T1,TResult}.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json.Internal.ContentEnvelopeHandler
{
    using System.Threading.Tasks;

    /// <summary>
    /// A wrapper class for content handlers of specific types.
    /// </summary>
    /// <typeparam name="TContents">The concrete type of the payload.</typeparam>
    /// <typeparam name="THandler">The type of the handler.</typeparam>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public class AsyncContentEnvelopeHandlerWithResultAndClass<TContents, THandler, T1, TResult> : IAsyncContentHandlerWithResult<ContentEnvelope, T1, TResult>
        where THandler : IAsyncContentHandlerWithResult<TContents, T1, TResult>
    {
        private readonly THandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncContentEnvelopeHandlerWithResultAndClass{TPayload, THandler, T1, TResult}"/> class.
        /// </summary>
        /// <param name="handler">The instance to use to handle the payload.</param>
        public AsyncContentEnvelopeHandlerWithResultAndClass(THandler handler)
        {
            this.handler = handler;
        }

        /// <inheritdoc/>
        public Task<TResult> HandleAsync(ContentEnvelope payload, T1 param1)
        {
            if (payload is null)
            {
                throw new System.ArgumentNullException(nameof(payload));
            }

            return this.handler.HandleAsync(payload.GetContents<TContents>(), param1);
        }
    }
}
