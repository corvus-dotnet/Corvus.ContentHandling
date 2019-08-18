// <copyright file="IAsyncContentHandler{TPayloadType}.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling
{
    using System.Threading.Tasks;

    /// <summary>
    /// An async handler for content.
    /// </summary>
    /// <typeparam name="TPayloadType">The type of the payload.</typeparam>
    public interface IAsyncContentHandler<TPayloadType>
    {
        /// <summary>
        /// Handle a particular content envelope.
        /// </summary>
        /// <param name="payload">The payload to handle.</param>
        /// <returns>A task which completes once the handler has completed.</returns>
        Task HandleAsync(TPayloadType payload);
    }
}
