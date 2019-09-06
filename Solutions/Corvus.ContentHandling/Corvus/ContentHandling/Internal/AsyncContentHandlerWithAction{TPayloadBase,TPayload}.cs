// <copyright file="AsyncContentHandlerWithAction{TPayloadBase,TPayload}.cs" company="Endjin Limited">
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
    public class AsyncContentHandlerWithAction<TPayloadBase, TPayload> : IAsyncContentHandler<TPayloadBase>
        where TPayload : TPayloadBase
    {
        private readonly Func<TPayload, Task> handle;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncContentHandlerWithAction{TPayloadBase, TPayload}"/> class.
        /// </summary>
        /// <param name="handle">The function to use to handle the payload.</param>
        public AsyncContentHandlerWithAction(Func<TPayload, Task> handle)
        {
            this.handle = handle ?? throw new ArgumentNullException(nameof(handle));
        }

        /// <inheritdoc/>
        public Task HandleAsync(TPayloadBase payload)
        {
            return this.handle((TPayload)payload);
        }
    }
}
