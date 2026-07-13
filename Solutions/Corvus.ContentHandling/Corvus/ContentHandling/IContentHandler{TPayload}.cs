// <copyright file="IContentHandler{TPayload}.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// A handler for content of a particular content type.
    /// </summary>
    /// <typeparam name="TPayload">The type of the payload to handle.</typeparam>
    /// <remarks>
    /// Handlers are registered with a content type and a handler class via the
    /// <c>AddContentHandler</c> methods on <see cref="ContentHandlingBuilder"/>, and are
    /// dispatched to via <see cref="IContentDispatcher{TPayloadBase}"/>. Synchronous handlers
    /// simply return <see cref="ValueTask.CompletedTask"/>.
    /// </remarks>
    public interface IContentHandler<in TPayload>
    {
        /// <summary>
        /// Handles the payload.
        /// </summary>
        /// <param name="payload">The payload to handle.</param>
        /// <param name="cancellationToken">A token to observe for cancellation.</param>
        /// <returns>A task which completes when the payload has been handled.</returns>
        ValueTask HandleAsync(TPayload payload, CancellationToken cancellationToken = default);
    }
}
