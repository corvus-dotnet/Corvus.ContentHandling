// <copyright file="SomeContentAbstractBase.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json.Specs.Samples
{
    using System;

    /// <summary>
    /// An abstract base for a polymorphic content type.
    /// </summary>
    public abstract class SomeContentAbstractBase : IEquatable<SomeContentAbstractBase>
    {
        /// <summary>
        /// Gets the content type.
        /// </summary>
        public abstract string ContentType { get; }

        /// <inheritdoc />
        public virtual bool Equals(SomeContentAbstractBase? other)
        {
            return this.Equals(other);
        }
    }
}