// <copyright file="AsyncContentHandlerWithResultAndAction{TPayloadBase,TPayload,T1,TResult}.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Internal
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// A wrapper class for content handlers of specific types.
    /// </summary>
    /// <typeparam name="TPayloadBase">The base type of payloads for this handler class.</typeparam>
    /// <typeparam name="TPayload">The concrete type of the payload.</typeparam>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public class AsyncContentHandlerWithResultAndAction<TPayloadBase, TPayload, T1, TResult> : IAsyncContentHandlerWithResult<TPayloadBase, T1, TResult>
        where TPayload : TPayloadBase
    {
        private readonly Func<TPayload, T1, Task<TResult>> handle;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncContentHandlerWithResultAndAction{TPayloadBase, TPayload, T1, TResult}"/> class.
        /// </summary>
        /// <param name="handle">The function to use to handle the payload.</param>
        public AsyncContentHandlerWithResultAndAction(Func<TPayload, T1, Task<TResult>> handle)
        {
            this.handle = handle;
        }

        /// <inheritdoc/>
        public Task<TResult> HandleAsync(TPayloadBase payload, T1 param1)
        {
            return this.handle((TPayload)payload, param1);
        }
    }
}
