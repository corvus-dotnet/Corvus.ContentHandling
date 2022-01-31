// <copyright file="IAsyncContentHandler{TPayloadType,T1,T2,T3}.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling
{
    using System.Threading.Tasks;

    /// <summary>
    /// An async handler for content.
    /// </summary>
    /// <typeparam name="TPayloadType">The type of the payload.</typeparam>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    public interface IAsyncContentHandler<TPayloadType, T1, T2, T3>
    {
        /// <summary>
        /// Handle a particular content envelope.
        /// </summary>
        /// <param name="payload">The payload to handle.</param>
        /// <param name="param1">The first parameter.</param>
        /// <param name="param2">The second parameter.</param>
        /// <param name="param3">The third parameter.</param>
        /// <returns>A task which completes once the handler has completed.</returns>
        Task HandleAsync(TPayloadType payload, T1 param1, T2 param2, T3 param3);
    }
}