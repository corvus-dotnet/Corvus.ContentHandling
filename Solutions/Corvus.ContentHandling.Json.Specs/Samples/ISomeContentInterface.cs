// <copyright file="ISomeContentInterface.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json.Specs.Samples
{
    using System;

    /// <summary>
    /// An interface for a polymorphic content type.
    /// </summary>
    public interface ISomeContentInterface : IEquatable<ISomeContentInterface>
    {
        /// <summary>
        /// Gets the content type.
        /// </summary>
        string ContentType { get; }
    }
}