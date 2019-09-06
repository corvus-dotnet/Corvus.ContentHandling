// <copyright file="ContentHandlerWithResultAndAction{TPayloadBase,TPayload,TResult}.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Internal
{
    using System;

    /// <summary>
    /// A wrapper class for content handlers of specific types.
    /// </summary>
    /// <typeparam name="TPayloadBase">The base type of payloads for this handler class.</typeparam>
    /// <typeparam name="TPayload">The concrete type of the payload.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public class ContentHandlerWithResultAndAction<TPayloadBase, TPayload, TResult> : IContentHandlerWithResult<TPayloadBase, TResult>
        where TPayload : TPayloadBase
    {
        private readonly Func<TPayload, TResult> handle;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentHandlerWithResultAndAction{TPayloadBase, TPayload, TResult}"/> class.
        /// </summary>
        /// <param name="handle">The function to use to handle the payload.</param>
        public ContentHandlerWithResultAndAction(Func<TPayload, TResult> handle)
        {
            this.handle = handle ?? throw new ArgumentNullException(nameof(handle));
        }

        /// <inheritdoc/>
        public TResult Handle(TPayloadBase payload)
        {
            return this.handle((TPayload)payload);
        }
    }
}
