// <copyright file="IContentHandler{TPayloadType}.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling
{
    /// <summary>
    /// A handler for content.
    /// </summary>
    /// <typeparam name="TPayloadType">The type of the payload.</typeparam>
    public interface IContentHandler<TPayloadType>
    {
        /// <summary>
        /// Dispatch a content payload to a handler.
        /// </summary>
        /// <param name="payload">The payload to handle.</param>
        void Handle(TPayloadType payload);
    }
}
