// <copyright file="ContentEnvelopeAsyncMatcher.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// An asynchronous pattern match over a <see cref="ContentEnvelope"/>'s payload content type.
    /// </summary>
    /// <remarks>
    /// Cases are evaluated in the order added; the first case whose content type equals the
    /// envelope's <see cref="ContentEnvelope.PayloadContentType"/> wins.
    /// </remarks>
    public sealed class ContentEnvelopeAsyncMatcher
    {
        private readonly ContentEnvelope envelope;
        private readonly List<(string ContentType, Func<ContentEnvelope, Task> Handler)> cases = [];
        private Func<ContentEnvelope, Task>? elseHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentEnvelopeAsyncMatcher"/> class.
        /// </summary>
        /// <param name="envelope">The envelope to match over.</param>
        internal ContentEnvelopeAsyncMatcher(ContentEnvelope envelope)
        {
            this.envelope = envelope;
        }

        /// <summary>
        /// Adds a case for the content type derived from <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The payload type for this case.</typeparam>
        /// <param name="handler">The handler invoked with the deserialized payload.</param>
        /// <returns>The matcher, for chaining.</returns>
        public ContentEnvelopeAsyncMatcher When<T>(Func<T, Task> handler)
        {
            return this.When(ContentTypes.GetContentType<T>(), handler);
        }

        /// <summary>
        /// Adds a case for an explicit content type.
        /// </summary>
        /// <typeparam name="T">The payload type for this case.</typeparam>
        /// <param name="contentType">The content type this case matches.</param>
        /// <param name="handler">The handler invoked with the deserialized payload.</param>
        /// <returns>The matcher, for chaining.</returns>
        public ContentEnvelopeAsyncMatcher When<T>(string contentType, Func<T, Task> handler)
        {
            ArgumentException.ThrowIfNullOrEmpty(contentType);
            ArgumentNullException.ThrowIfNull(handler);

            this.cases.Add((contentType, e => handler(e.GetContents<T>())));
            return this;
        }

        /// <summary>
        /// Sets the handler invoked when no case matches.
        /// </summary>
        /// <param name="handler">The handler invoked with the envelope.</param>
        /// <returns>The matcher, for chaining.</returns>
        public ContentEnvelopeAsyncMatcher Else(Func<ContentEnvelope, Task> handler)
        {
            ArgumentNullException.ThrowIfNull(handler);

            this.elseHandler = handler;
            return this;
        }

        /// <summary>
        /// Executes the match.
        /// </summary>
        /// <returns>
        /// A task producing true if a <c>When</c> case matched; false otherwise (the
        /// <c>Else</c> handler, if set, has been invoked).
        /// </returns>
        public async Task<bool> ExecuteAsync()
        {
            string payloadContentType = this.envelope.PayloadContentType;
            foreach ((string contentType, Func<ContentEnvelope, Task> handler) in this.cases)
            {
                if (string.Equals(contentType, payloadContentType, StringComparison.Ordinal))
                {
                    await handler(this.envelope).ConfigureAwait(false);
                    return true;
                }
            }

            if (this.elseHandler is not null)
            {
                await this.elseHandler(this.envelope).ConfigureAwait(false);
            }

            return false;
        }
    }
}
