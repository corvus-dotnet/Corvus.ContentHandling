// <copyright file="AsyncContentHandlerWithResultAndClass{TPayloadBase,TPayload,THandler,TResult}.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Internal
{
    using System.Threading.Tasks;

    /// <summary>
    /// A wrapper class for content handlers of specific types.
    /// </summary>
    /// <typeparam name="TPayloadBase">The base type of payloads for this handler class.</typeparam>
    /// <typeparam name="TPayload">The concrete type of the payload.</typeparam>
    /// <typeparam name="THandler">The concrete type of the handler.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public class AsyncContentHandlerWithResultAndClass<TPayloadBase, TPayload, THandler, TResult> : IAsyncContentHandlerWithResult<TPayloadBase, TResult>
        where TPayload : TPayloadBase
        where THandler : IAsyncContentHandlerWithResult<TPayload, TResult>
    {
        private readonly THandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncContentHandlerWithResultAndClass{TPayloadBase, TPayload, THandler, TResult}"/> class.
        /// </summary>
        /// <param name="handler">The instance to use to handle the payload.</param>
        public AsyncContentHandlerWithResultAndClass(THandler handler)
        {
            this.handler = handler;
        }

        /// <inheritdoc/>
        public Task<TResult> HandleAsync(TPayloadBase payload)
        {
            return this.handler.HandleAsync((TPayload)payload);
        }
    }
}
