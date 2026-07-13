// <copyright file="IContentHandler{TPayload,TContext}.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// A handler for content of a particular content type, receiving an additional context value.
    /// </summary>
    /// <typeparam name="TPayload">The type of the payload to handle.</typeparam>
    /// <typeparam name="TContext">
    /// The type of the context passed alongside the payload. Use a tuple to pass multiple values.
    /// </typeparam>
    public interface IContentHandler<in TPayload, in TContext>
    {
        /// <summary>
        /// Handles the payload.
        /// </summary>
        /// <param name="payload">The payload to handle.</param>
        /// <param name="context">The context for this dispatch.</param>
        /// <param name="cancellationToken">A token to observe for cancellation.</param>
        /// <returns>A task which completes when the payload has been handled.</returns>
        ValueTask HandleAsync(TPayload payload, TContext context, CancellationToken cancellationToken = default);
    }
}
