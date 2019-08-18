// <copyright file="ContentHandlerWithAction{TPayloadBase,TPayload,T1,T2,T3}.cs" company="Endjin Limited">
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
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    public class ContentHandlerWithAction<TPayloadBase, TPayload, T1, T2, T3> : IContentHandler<TPayloadBase, T1, T2, T3>
        where TPayload : TPayloadBase
    {
        private readonly Action<TPayload, T1, T2, T3> handle;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentHandlerWithAction{TPayloadBase, TPayload, T1, T2, T3}"/> class.
        /// </summary>
        /// <param name="handle">The function to use to handle the payload.</param>
        public ContentHandlerWithAction(Action<TPayload, T1, T2, T3> handle)
        {
            this.handle = handle;
        }

        /// <inheritdoc/>
        public void Handle(TPayloadBase payload, T1 parameter1, T2 parameter2, T3 parameter3)
        {
            this.handle((TPayload)payload, parameter1, parameter2, parameter3);
        }
    }
}
