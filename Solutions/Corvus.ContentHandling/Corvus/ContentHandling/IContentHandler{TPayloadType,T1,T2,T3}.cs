// <copyright file="IContentHandler{TPayloadType,T1,T2,T3}.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling
{
    /// <summary>
    /// A handler for content.
    /// </summary>
    /// <typeparam name="TPayloadType">The type of the payload.</typeparam>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    public interface IContentHandler<TPayloadType, T1, T2, T3>
    {
        /// <summary>
        /// Dispatch a content payload to a handler.
        /// </summary>
        /// <param name="payload">The payload to handle.</param>
        /// <param name="parameter1">The first parameter for the handler.</param>
        /// <param name="parameter2">The second parameter for the handler.</param>
        /// <param name="parameter3">The third parameter for the handler.</param>
        void Handle(TPayloadType payload, T1 parameter1, T2 parameter2, T3 parameter3);
    }
}
