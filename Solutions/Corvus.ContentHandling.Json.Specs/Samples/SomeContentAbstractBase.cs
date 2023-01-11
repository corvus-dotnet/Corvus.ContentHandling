// <copyright file="SomeContentAbstractBase.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json.Specs.Samples
{
    using System;

    /// <summary>
    /// An abstract base for a polymorphic content type.
    /// </summary>
#pragma warning disable CA1067 // Override Object.Equals(object) when implementing IEquatable<T>
    public abstract class SomeContentAbstractBase : IEquatable<SomeContentAbstractBase>
#pragma warning restore CA1067 // Override Object.Equals(object) when implementing IEquatable<T>
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