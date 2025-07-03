// <copyright file="ContentHandlerWithAction{TPayloadBase,TPayload,T1}.cs" company="Endjin Limited">
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
    public class ContentHandlerWithAction<TPayloadBase, TPayload, T1> : IContentHandler<TPayloadBase, T1>
        where TPayloadBase : notnull
        where TPayload : TPayloadBase
    {
        private readonly Action<TPayload, T1> handle;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentHandlerWithAction{TPayloadBase, TPayload, T1}"/> class.
        /// </summary>
        /// <param name="handle">The function to use to handle the payload.</param>
        public ContentHandlerWithAction(Action<TPayload, T1> handle)
        {
            this.handle = handle ?? throw new ArgumentNullException(nameof(handle));
        }

        /// <inheritdoc/>
        public void Handle(TPayloadBase payload, T1 parameter)
        {
            this.handle((TPayload)payload, parameter);
        }
    }
}