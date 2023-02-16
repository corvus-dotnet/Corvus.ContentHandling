// <copyright file="ContentHandlerWithAction{TPayloadBase,TPayload}.cs" company="Endjin Limited">
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
    public class ContentHandlerWithAction<TPayloadBase, TPayload> : IContentHandler<TPayloadBase>
        where TPayloadBase : notnull
        where TPayload : TPayloadBase
    {
        private readonly Action<TPayload> handle;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentHandlerWithAction{TPayloadBase, TPayload}"/> class.
        /// </summary>
        /// <param name="handle">The function to use to handle the payload.</param>
        public ContentHandlerWithAction(Action<TPayload> handle)
        {
            this.handle = handle ?? throw new ArgumentNullException(nameof(handle));
        }

        /// <inheritdoc/>
        public void Handle(TPayloadBase payload)
        {
            this.handle((TPayload)payload);
        }
    }
}