﻿// <copyright file="AsyncContentHandlerWithAction{TPayloadBase,TPayload,T1,T2}.cs" company="Endjin Limited">
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
    public class AsyncContentHandlerWithAction<TPayloadBase, TPayload, T1, T2> : IAsyncContentHandler<TPayloadBase, T1, T2>
        where TPayloadBase : notnull
        where TPayload : TPayloadBase
    {
        private readonly Func<TPayload, T1, T2, Task> handle;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncContentHandlerWithAction{TPayloadBase, TPayload, T1, T2}"/> class.
        /// </summary>
        /// <param name="handle">The function to use to handle the payload.</param>
        public AsyncContentHandlerWithAction(Func<TPayload, T1, T2, Task> handle)
        {
            this.handle = handle ?? throw new ArgumentNullException(nameof(handle));
        }

        /// <inheritdoc/>
        public Task HandleAsync(TPayloadBase payload, T1 param1, T2 param2)
        {
            return this.handle((TPayload)payload, param1, param2);
        }
    }
}