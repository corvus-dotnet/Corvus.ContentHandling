// <copyright file="ContentHandlerWithResultAndClass{TPayloadBase,TPayload,THandler,TResult}.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Internal
{
    /// <summary>
    /// A wrapper class for content handlers of specific types.
    /// </summary>
    /// <typeparam name="TPayloadBase">The base type of payloads for this handler class.</typeparam>
    /// <typeparam name="TPayload">The concrete type of the payload.</typeparam>
    /// <typeparam name="THandler">The type of the handler.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public class ContentHandlerWithResultAndClass<TPayloadBase, TPayload, THandler, TResult> : IContentHandlerWithResult<TPayloadBase, TResult>
        where TPayloadBase : notnull
        where TPayload : TPayloadBase
        where THandler : IContentHandlerWithResult<TPayload, TResult>
    {
        private readonly IContentHandlerWithResult<TPayload, TResult> handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentHandlerWithResultAndClass{TPayloadBase, TPayload, THandler, TResult}"/> class.
        /// </summary>
        /// <param name="handler">The instance to use to handle the payload.</param>
        public ContentHandlerWithResultAndClass(THandler handler)
        {
            this.handler = handler;
        }

        /// <inheritdoc/>
        public TResult Handle(TPayloadBase payload)
        {
            return this.handler.Handle((TPayload)payload);
        }
    }
}