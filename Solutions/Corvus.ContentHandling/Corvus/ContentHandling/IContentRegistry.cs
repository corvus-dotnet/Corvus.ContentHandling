// <copyright file="IContentRegistry.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// A read-only registry of the content types registered with the content handling framework.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Content instances are registered as keyed services in the container, keyed by their
    /// content-type string. Keyed services cannot be enumerated, nor can their keys be
    /// inspected, so this registry records the metadata needed for content type enumeration
    /// and for hierarchical fallback resolution.
    /// </para>
    /// <para>
    /// The registry is populated while the service collection is being configured, and is
    /// immutable once the service provider has been built.
    /// </para>
    /// </remarks>
    public interface IContentRegistry
    {
        /// <summary>
        /// Gets the registered content types.
        /// </summary>
        IReadOnlyCollection<string> ContentTypes { get; }

        /// <summary>
        /// Attempts to get the registration for an exact content type.
        /// </summary>
        /// <param name="contentType">The content type to look up.</param>
        /// <param name="registration">The registration for the content type.</param>
        /// <returns>True if the content type is registered.</returns>
        bool TryGet(string contentType, [NotNullWhen(true)] out ContentRegistration? registration);

        /// <summary>
        /// Attempts to resolve the registration for a content type, applying hierarchical fallback.
        /// </summary>
        /// <param name="contentType">The content type to resolve.</param>
        /// <param name="registration">The resolved registration.</param>
        /// <returns>True if a registration was found for the content type or one of its parents.</returns>
        /// <remarks>
        /// <para>
        /// If the content type is not registered, progressively more general content types are
        /// tried by removing trailing dot-delimited segments while preserving any
        /// <c>+suffix</c>: <c>application/vnd.corvus.a.b.c+suffix</c> falls back to
        /// <c>application/vnd.corvus.a.b+suffix</c>, then <c>application/vnd.corvus.a+suffix</c>,
        /// and so on.
        /// </para>
        /// <para>
        /// The returned <see cref="ContentRegistration.ContentType"/> is the key that matched —
        /// which may be a parent of the requested content type — and is the key to use for
        /// keyed service resolution.
        /// </para>
        /// </remarks>
        bool TryResolve(string contentType, [NotNullWhen(true)] out ContentRegistration? registration);

        /// <summary>
        /// Gets all registered content types ending with the given suffix.
        /// </summary>
        /// <param name="suffix">The suffix to match (ordinal comparison).</param>
        /// <returns>The matching content types.</returns>
        IEnumerable<string> GetContentTypes(string suffix);
    }
}
