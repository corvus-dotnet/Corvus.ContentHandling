// <copyright file="IContentHandlerWithResult{TPayloadType,TResult}.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling
{
    /// <summary>
    /// A handler for content.
    /// </summary>
    /// <typeparam name="TPayloadType">The type of the payload.</typeparam>
    /// <typeparam name="TResult">The result of the payload.</typeparam>
    public interface IContentHandlerWithResult<TPayloadType, TResult>
    {
        /// <summary>
        /// Dispatch a content payload to a handler.
        /// </summary>
        /// <param name="payload">The payload to handle.</param>
        /// <returns>The result of the handler.</returns>
        TResult Handle(TPayloadType payload);
    }
}
