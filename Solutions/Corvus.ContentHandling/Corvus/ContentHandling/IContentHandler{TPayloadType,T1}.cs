// <copyright file="IContentHandler{TPayloadType,T1}.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling
{
    /// <summary>
    /// A handler for content.
    /// </summary>
    /// <typeparam name="TPayloadType">The type of the payload.</typeparam>
    /// <typeparam name="T1">The type of the parameter.</typeparam>
    public interface IContentHandler<TPayloadType, T1>
    {
        /// <summary>
        /// Dispatch a content payload to a handler.
        /// </summary>
        /// <param name="payload">The payload to handle.</param>
        /// <param name="parameter">The parameter for the handler.</param>
        void Handle(TPayloadType payload, T1 parameter);
    }
}