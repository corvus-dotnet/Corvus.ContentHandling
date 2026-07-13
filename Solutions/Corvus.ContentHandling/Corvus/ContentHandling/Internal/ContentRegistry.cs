// <copyright file="ContentRegistry.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Internal
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    /// <summary>
    /// The registry of content type registrations, including the hierarchical fallback resolution.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Mutated only while the service collection is being configured (which is single-threaded
    /// by convention); reads after the provider is built never observe mutation.
    /// </para>
    /// <para>
    /// Note: a <see cref="System.Collections.Frozen.FrozenDictionary{TKey, TValue}"/> was
    /// benchmarked here and measured consistently slower than <see cref="Dictionary{TKey, TValue}"/>
    /// with an ordinal comparer for this key shape (long media-type strings sharing a common
    /// prefix and often a common length defeat its comparer-selection strategies).
    /// </para>
    /// </remarks>
    internal sealed class ContentRegistry : IContentRegistry
    {
        // Bounds the dispatch-path cache: registrations are finite, but requested content
        // types can be payload-driven and unbounded, so stop caching beyond this size.
        private const int MaxCachedHandlerResolutions = 1024;

        private readonly Dictionary<string, ContentRegistration> registrations = new(StringComparer.Ordinal);
        private readonly Dictionary<string, ContentRegistration>.AlternateLookup<ReadOnlySpan<char>> spanLookup;
        private readonly ConcurrentDictionary<(string ContentType, string HandlerClass), ContentRegistration?> handlerResolutions = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentRegistry"/> class.
        /// </summary>
        public ContentRegistry()
        {
            this.spanLookup = this.registrations.GetAlternateLookup<ReadOnlySpan<char>>();
        }

        /// <inheritdoc/>
        public IReadOnlyCollection<string> ContentTypes => this.registrations.Keys;

        /// <summary>
        /// Adds a registration for a content type.
        /// </summary>
        /// <param name="contentType">The content type to register.</param>
        /// <param name="registration">The registration details.</param>
        /// <exception cref="InvalidOperationException">The content type has already been registered.</exception>
        public void Add(string contentType, ContentRegistration registration)
        {
            if (!this.registrations.TryAdd(contentType, registration))
            {
                throw new InvalidOperationException($"Content with type '{contentType}' has already been registered.");
            }

            this.handlerResolutions.Clear();
        }

        /// <inheritdoc/>
        public bool TryGet(string contentType, [NotNullWhen(true)] out ContentRegistration? registration)
        {
            ArgumentException.ThrowIfNullOrEmpty(contentType);

            return this.registrations.TryGetValue(contentType, out registration);
        }

        /// <inheritdoc/>
        public bool TryResolve(string contentType, [NotNullWhen(true)] out ContentRegistration? registration)
        {
            ArgumentException.ThrowIfNullOrEmpty(contentType);

            if (this.registrations.TryGetValue(contentType, out registration))
            {
                return true;
            }

            // Fallback: strip the "+suffix" once (it is preserved across every hop), then
            // repeatedly drop the last dot segment of the stem, composing each candidate into
            // a single reusable buffer and probing via the dictionary's span-based lookup —
            // no per-hop string allocations.
            int indexOfPlusSuffix = contentType.LastIndexOf('+');
            ReadOnlySpan<char> suffix = indexOfPlusSuffix >= 0 ? contentType.AsSpan(indexOfPlusSuffix) : default;
            ReadOnlySpan<char> stem = indexOfPlusSuffix >= 0 ? contentType.AsSpan(0, indexOfPlusSuffix) : contentType;

            Span<char> candidate = contentType.Length <= 256 ? stackalloc char[contentType.Length] : new char[contentType.Length];

            while (true)
            {
                int indexOfLastDot = stem.LastIndexOf('.');
                if (indexOfLastDot <= 0)
                {
                    registration = null;
                    return false;
                }

                stem = stem[..indexOfLastDot];
                stem.CopyTo(candidate);
                suffix.CopyTo(candidate[stem.Length..]);

                if (this.spanLookup.TryGetValue(candidate[..(stem.Length + suffix.Length)], out registration))
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Resolves the handler registration for a (content type, handler class) pair,
        /// caching the outcome.
        /// </summary>
        /// <param name="contentType">The content type of the payload.</param>
        /// <param name="handlerClass">The class of handler.</param>
        /// <param name="registration">The resolved handler registration.</param>
        /// <returns>True if a handler registration was found.</returns>
        /// <remarks>
        /// Dispatch composes the handler key (an allocation) and probes the registry with
        /// fallback on every call; since the registry is immutable once the provider is
        /// built, the outcome per (content type, handler class) pair never changes and can
        /// be cached — including misses. The cache is bounded; beyond the cap resolution
        /// falls back to the uncached path.
        /// </remarks>
        public bool TryResolveHandler(string contentType, string handlerClass, [NotNullWhen(true)] out ContentRegistration? registration)
        {
            (string ContentType, string HandlerClass) cacheKey = (contentType, handlerClass);
            if (this.handlerResolutions.TryGetValue(cacheKey, out registration))
            {
                return registration is not null;
            }

            this.TryResolve(ContentHandlerKeys.For(contentType, handlerClass), out registration);

            if (this.handlerResolutions.Count < MaxCachedHandlerResolutions)
            {
                this.handlerResolutions.TryAdd(cacheKey, registration);
            }

            return registration is not null;
        }

        /// <inheritdoc/>
        public IEnumerable<string> GetContentTypes(string suffix)
        {
            ArgumentNullException.ThrowIfNull(suffix);

            return this.registrations.Keys.Where(k => k.EndsWith(suffix, StringComparison.Ordinal));
        }
    }
}
