// <copyright file="ContentHandlerWithClass{TPayloadBase,TPayload,THandler}.cs" company="Endjin Limited">
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
    public class ContentHandlerWithClass<TPayloadBase, TPayload, THandler> : IContentHandler<TPayloadBase>
        where TPayloadBase : notnull
        where TPayload : TPayloadBase
        where THandler : IContentHandler<TPayload>
    {
        private readonly IContentHandler<TPayload> handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentHandlerWithClass{TPayloadBase, TPayload, THandler}"/> class.
        /// </summary>
        /// <param name="handler">The instance to use to handle the payload.</param>
        public ContentHandlerWithClass(THandler handler)
        {
            this.handler = handler;
        }

        /// <inheritdoc/>
        public void Handle(TPayloadBase payload)
        {
            this.handler.Handle((TPayload)payload);
        }
    }
}