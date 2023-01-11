// <copyright file="SomeContentBase.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json.Specs.Samples
{
    using System;

    /// <summary>
    /// An base for a polymorphic content type.
    /// </summary>
#pragma warning disable CA1067 // Override Object.Equals(object) when implementing IEquatable<T>
    public class SomeContentBase : IEquatable<SomeContentBase>
    {
#pragma warning restore CA1067 // Override Object.Equals(object) when implementing IEquatable<T>
        /// <summary>
        /// Initializes a new instance of the <see cref="SomeContentBase"/> class.
        /// </summary>
        /// <param name="contentType">The content type.</param>
        protected SomeContentBase(string contentType)
        {
            this.ContentType = contentType;
        }

        /// <summary>
        /// Gets the content type.
        /// </summary>
        public string ContentType { get; }

        /// <inheritdoc />
        public virtual bool Equals(SomeContentBase? other)
        {
            return this.Equals(other);
        }
    }
}