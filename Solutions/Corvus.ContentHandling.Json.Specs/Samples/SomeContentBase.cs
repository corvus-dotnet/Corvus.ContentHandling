// <copyright file="SomeContentBase.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json.Specs.Samples
{
    using System;

    /// <summary>
    /// An base for a polymorphic content type.
    /// </summary>
    public class SomeContentBase : IEquatable<SomeContentBase>
    {
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
        public virtual bool Equals(SomeContentBase other)
        {
            return this.Equals(other);
        }
    }
}
