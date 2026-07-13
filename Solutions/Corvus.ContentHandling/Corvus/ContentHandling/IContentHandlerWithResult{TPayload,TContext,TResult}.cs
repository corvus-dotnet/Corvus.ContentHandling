// <copyright file="IContentHandlerWithResult{TPayload,TContext,TResult}.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// A handler for content of a particular content type, receiving an additional context
    /// value and producing a result.
    /// </summary>
    /// <typeparam name="TPayload">The type of the payload to handle.</typeparam>
    /// <typeparam name="TContext">
    /// The type of the context passed alongside the payload. Use a tuple to pass multiple values.
    /// </typeparam>
    /// <typeparam name="TResult">The type of the result produced by the handler.</typeparam>
    public interface IContentHandlerWithResult<in TPayload, in TContext, TResult>
    {
        /// <summary>
        /// Handles the payload and produces a result.
        /// </summary>
        /// <param name="payload">The payload to handle.</param>
        /// <param name="context">The context for this dispatch.</param>
        /// <param name="cancellationToken">A token to observe for cancellation.</param>
        /// <returns>A task producing the result of handling the payload.</returns>
        ValueTask<TResult> HandleAsync(TPayload payload, TContext context, CancellationToken cancellationToken = default);
    }
}
