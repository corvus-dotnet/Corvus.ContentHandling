// <copyright file="ContentHandlerWithClass{TPayloadBase,TPayload,THandler,T1,T2}.cs" company="Endjin Limited">
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
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    public class ContentHandlerWithClass<TPayloadBase, TPayload, THandler, T1, T2> : IContentHandler<TPayloadBase, T1, T2>
        where TPayloadBase : notnull
        where TPayload : TPayloadBase
        where THandler : IContentHandler<TPayload, T1, T2>
    {
        private readonly THandler handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentHandlerWithClass{TPayloadBase, TPayload, THandler, T1, T2}"/> class.
        /// </summary>
        /// <param name="handler">The instance to use to handle the payload.</param>
        public ContentHandlerWithClass(THandler handler)
        {
            this.handler = handler;
        }

        /// <inheritdoc/>
        public void Handle(TPayloadBase payload, T1 parameter1, T2 parameter2)
        {
            this.handler.Handle((TPayload)payload, parameter1, parameter2);
        }
    }
}