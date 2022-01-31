// <copyright file="SomeContentWithAbstractBaseAndPocChild.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json.Specs.Samples
{
    using System;

    /// <summary>
    /// A polymorphic content type based on an abstract base.
    /// </summary>
    public class SomeContentWithAbstractBaseAndPocChild : SomeContentAbstractBase, IEquatable<SomeContentWithAbstractBaseAndPocChild>
    {
        /// <summary>
        /// The content type.
        /// </summary>
        public const string RegisteredContentType = "application/vnd.corvus.somecontentwithabstractbaseandpocchild";

        /// <inheritdoc/>
        public override string ContentType => RegisteredContentType;

        /// <summary>
        /// Gets or sets a value.
        /// </summary>
        public string SomeValue { get; set; }

        /// <summary>
        /// Gets or sets a child.
        /// </summary>
        public PocObject Child { get; set; }

        /// <summary>
        /// Compares two instances of <see cref="SomeContentWithAbstractBaseAndPocChild"/> for equality.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>True if the instances are equal, false otherwise.</returns>
        public static bool operator ==(SomeContentWithAbstractBaseAndPocChild left, SomeContentWithAbstractBaseAndPocChild right)
        {
            return left?.Equals(right) ?? false;
        }

        /// <summary>
        /// Compares two instances of <see cref="SomeContentWithAbstractBaseAndPocChild"/> for inequality.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>False if the instances are equal, true otherwise.</returns>
        public static bool operator !=(SomeContentWithAbstractBaseAndPocChild left, SomeContentWithAbstractBaseAndPocChild right)
        {
            return !(left == right);
        }

        /// <inheritdoc />
        public bool Equals(SomeContentWithAbstractBaseAndPocChild other)
        {
            return this.SomeValue == other.SomeValue;
        }

        /// <inheritdoc />
        public override bool Equals(SomeContentAbstractBase other)
        {
            return this.Equals(other as SomeContentWithAbstractBaseAndPocChild);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is SomeContentWithAbstractBaseAndPocChild sci)
            {
                return this.Equals(sci);
            }

            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(this.SomeValue, this.Child);
        }
    }
}