// <copyright file="ContentHandlerKeys.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Internal
{
    using System;

    /// <summary>
    /// Computes the keyed-service keys under which content handlers are registered.
    /// </summary>
    internal static class ContentHandlerKeys
    {
        /// <summary>
        /// Gets the handler registration key for a content type and handler class.
        /// </summary>
        /// <param name="contentType">The content type of the payload.</param>
        /// <param name="handlerClass">The class of handler.</param>
        /// <returns>The key under which the handler is registered.</returns>
        /// <remarks>
        /// The key appends the handler class, lowercased, as a media-type <c>+suffix</c>
        /// (e.g. <c>application/vnd.corvus.example+renderer</c>), which is what makes
        /// hierarchical content-type fallback work for handler dispatch. Composed with a
        /// single allocation — this runs on every dispatch.
        /// </remarks>
        public static string For(string contentType, string handlerClass)
        {
            ArgumentException.ThrowIfNullOrEmpty(contentType);
            ArgumentException.ThrowIfNullOrEmpty(handlerClass);

            return string.Create(
                contentType.Length + 1 + handlerClass.Length,
                (contentType, handlerClass),
                static (span, state) =>
                {
                    state.contentType.CopyTo(span);
                    span[state.contentType.Length] = '+';
                    state.handlerClass.AsSpan().ToLowerInvariant(span[(state.contentType.Length + 1)..]);
                });
        }
    }
}
