// <copyright file="ContentEnvelopeMatcher.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A synchronous pattern match over a <see cref="ContentEnvelope"/>'s payload content type.
    /// </summary>
    /// <remarks>
    /// Cases are evaluated in the order added; the first case whose content type equals the
    /// envelope's <see cref="ContentEnvelope.PayloadContentType"/> wins. This replaces the
    /// fixed-arity <c>Match</c> overloads of earlier versions with an unbounded fluent form.
    /// </remarks>
    public sealed class ContentEnvelopeMatcher
    {
        private readonly ContentEnvelope envelope;
        private readonly List<(string ContentType, Action<ContentEnvelope> Handler)> cases = [];
        private Action<ContentEnvelope>? elseHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentEnvelopeMatcher"/> class.
        /// </summary>
        /// <param name="envelope">The envelope to match over.</param>
        internal ContentEnvelopeMatcher(ContentEnvelope envelope)
        {
            this.envelope = envelope;
        }

        /// <summary>
        /// Adds a case for the content type derived from <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The payload type for this case.</typeparam>
        /// <param name="handler">The handler invoked with the deserialized payload.</param>
        /// <returns>The matcher, for chaining.</returns>
        public ContentEnvelopeMatcher When<T>(Action<T> handler)
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
        public ContentEnvelopeMatcher When<T>(string contentType, Action<T> handler)
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
        public ContentEnvelopeMatcher Else(Action<ContentEnvelope> handler)
        {
            ArgumentNullException.ThrowIfNull(handler);

            this.elseHandler = handler;
            return this;
        }

        /// <summary>
        /// Executes the match.
        /// </summary>
        /// <returns>
        /// True if a <c>When</c> case matched; false otherwise (the <c>Else</c> handler, if
        /// set, has been invoked).
        /// </returns>
        public bool Execute()
        {
            string payloadContentType = this.envelope.PayloadContentType;
            foreach ((string contentType, Action<ContentEnvelope> handler) in this.cases)
            {
                if (string.Equals(contentType, payloadContentType, StringComparison.Ordinal))
                {
                    handler(this.envelope);
                    return true;
                }
            }

            this.elseHandler?.Invoke(this.envelope);
            return false;
        }
    }
}
