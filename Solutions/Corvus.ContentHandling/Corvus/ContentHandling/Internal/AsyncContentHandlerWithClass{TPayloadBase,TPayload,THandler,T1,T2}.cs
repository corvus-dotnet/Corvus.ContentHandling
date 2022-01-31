// <copyright file="AsyncContentHandlerWithClass{TPayloadBase,TPayload,THandler,T1,T2}.cs" company="Endjin Limited">
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
    /// <typeparam name="THandler">The type of the handler.</typeparam>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    public class AsyncContentHandlerWithClass<TPayloadBase, TPayload, THandler, T1, T2> : IAsyncContentHandler<TPayloadBase, T1, T2>
        where TPayload : TPayloadBase
        where THandler : IAsyncContentHandler<TPayload, T1, T2>
    {
        private readonly THandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncContentHandlerWithClass{TPayloadBase, TPayload, THandler, T1, T2}"/> class.
        /// </summary>
        /// <param name="handler">The instance to use to handle the payload.</param>
        public AsyncContentHandlerWithClass(THandler handler)
        {
            this.handler = handler;
        }

        /// <inheritdoc/>
        public Task HandleAsync(TPayloadBase payload, T1 param1, T2 param2)
        {
            return this.handler.HandleAsync((TPayload)payload, param1, param2);
        }
    }
}