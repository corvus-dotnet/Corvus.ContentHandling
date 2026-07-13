// <copyright file="ContentRegistration.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling
{
    using System;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Describes a content type registration.
    /// </summary>
    /// <param name="ContentType">
    /// The content type under which the registration was made. When a registration is found
    /// via hierarchical fallback, this is the key that actually exists — use it (not the
    /// requested content type) for keyed service resolution.
    /// </param>
    /// <param name="ImplementingType">
    /// The type implementing the content. For content handlers this is the closed handler
    /// interface the handler was registered against.
    /// </param>
    /// <param name="Construction">How instances of the content are constructed.</param>
    /// <param name="Lifetime">
    /// The service lifetime for <see cref="ContentConstruction.FromServices"/> registrations;
    /// null for <see cref="ContentConstruction.BySerializer"/> registrations.
    /// </param>
    public sealed record ContentRegistration(
        string ContentType,
        Type ImplementingType,
        ContentConstruction Construction,
        ServiceLifetime? Lifetime);
}
