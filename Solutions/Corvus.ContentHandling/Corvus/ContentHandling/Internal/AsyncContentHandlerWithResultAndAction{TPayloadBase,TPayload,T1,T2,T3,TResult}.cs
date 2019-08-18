// <copyright file="AsyncContentHandlerWithResultAndAction{TPayloadBase,TPayload,T1,T2,T3,TResult}.cs" company="Endjin Limited">
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
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public class AsyncContentHandlerWithResultAndAction<TPayloadBase, TPayload, T1, T2, T3, TResult> : IAsyncContentHandlerWithResult<TPayloadBase, T1, T2, T3, TResult>
        where TPayload : TPayloadBase
    {
        private readonly Func<TPayload, T1, T2, T3, Task<TResult>> handle;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncContentHandlerWithResultAndAction{TPayloadBase, TPayload, T1, T2, T3, TResult}"/> class.
        /// </summary>
        /// <param name="handle">The function to use to handle the payload.</param>
        public AsyncContentHandlerWithResultAndAction(Func<TPayload, T1, T2, T3, Task<TResult>> handle)
        {
            this.handle = handle;
        }

        /// <inheritdoc/>
        public Task<TResult> HandleAsync(TPayloadBase payload, T1 param1, T2 param2, T3 param3)
        {
            return this.handle((TPayload)payload, param1, param2, param3);
        }
    }
}
