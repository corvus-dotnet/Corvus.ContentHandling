﻿// <copyright file="ContentHandlerWithResultAndAction{TPayloadBase,TPayload,T1,T2,TResult}.cs" company="Endjin Limited">
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
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public class ContentHandlerWithResultAndAction<TPayloadBase, TPayload, T1, T2, TResult> : IContentHandlerWithResult<TPayloadBase, T1, T2, TResult>
        where TPayloadBase : notnull
        where TPayload : TPayloadBase
    {
        private readonly Func<TPayload, T1, T2, TResult> handle;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentHandlerWithResultAndAction{TPayloadBase, TPayload, T1, T2, TResult}"/> class.
        /// </summary>
        /// <param name="handle">The function to use to handle the payload.</param>
        public ContentHandlerWithResultAndAction(Func<TPayload, T1, T2, TResult> handle)
        {
            this.handle = handle ?? throw new ArgumentNullException(nameof(handle));
        }

        /// <inheritdoc/>
        public TResult Handle(TPayloadBase payload, T1 param1, T2 param2)
        {
            return this.handle((TPayload)payload, param1, param2);
        }
    }
}