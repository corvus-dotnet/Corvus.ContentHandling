// <copyright file="IAsyncContentHandlerWithResult{TPayloadType,T1,T2,T3,TResult}.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling
{
    using System.Threading.Tasks;

    /// <summary>
    /// A handler for content.
    /// </summary>
    /// <typeparam name="TPayloadType">The type of the payload.</typeparam>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <typeparam name="T3">The type of the third parameter.</typeparam>
    /// <typeparam name="TResult">The result of the payload.</typeparam>
    public interface IAsyncContentHandlerWithResult<TPayloadType, T1, T2, T3, TResult>
    {
        /// <summary>
        /// Dispatch a content payload to a handler.
        /// </summary>
        /// <param name="payload">The payload to handle.</param>
        /// <param name="param1">The first parameter.</param>
        /// <param name="param2">The second parameter.</param>
        /// <param name="param3">The third parameter.</param>
        /// <returns>The result of the handler.</returns>
        Task<TResult> HandleAsync(TPayloadType payload, T1 param1, T2 param2, T3 param3);
    }
}