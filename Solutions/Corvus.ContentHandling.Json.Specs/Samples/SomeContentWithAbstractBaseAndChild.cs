// <copyright file="SomeContentWithAbstractBaseAndChild.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json.Specs.Samples
{
    using System;

    /// <summary>
    /// A polymorphic content type based on an abstract base.
    /// </summary>
    public class SomeContentWithAbstractBaseAndChild : SomeContentAbstractBase, IEquatable<SomeContentWithAbstractBaseAndChild>
    {
        /// <summary>
        /// The content type.
        /// </summary>
        public const string RegisteredContentType = "application/vnd.corvus.somecontentwithabstractbaseandchild";

        /// <inheritdoc/>
        public override string ContentType => RegisteredContentType;

        /// <summary>
        /// Gets or sets a value.
        /// </summary>
        public string SomeValue { get; set; }

        /// <summary>
        /// Gets or sets a child.
        /// </summary>
        public SomeContentAbstractBase Child { get; set; }

        /// <summary>
        /// Compares two instances of SomeContentWithAbstractBaseAndChild for equality.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>True if the instances are equal, false otherwise.</returns>
        public static bool operator ==(SomeContentWithAbstractBaseAndChild left, SomeContentWithAbstractBaseAndChild right)
        {
            return left?.Equals(right) ?? false;
        }

        /// <summary>
        /// Compares two instances of SomeContentWithAbstractBaseAndChild for inequality.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>False if the instances are equal, true otherwise.</returns>
        public static bool operator !=(SomeContentWithAbstractBaseAndChild left, SomeContentWithAbstractBaseAndChild right)
        {
            return !(left == right);
        }

        /// <inheritdoc />
        public bool Equals(SomeContentWithAbstractBaseAndChild other)
        {
            return this.SomeValue == other.SomeValue && ((this.Child == null && other.Child == null) || (this.Child?.Equals(other.Child) == true));
        }

        /// <inheritdoc />
        public override bool Equals(SomeContentAbstractBase other)
        {
            return this.Equals(other as SomeContentWithAbstractBaseAndChild);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is SomeContentWithAbstractBaseAndChild sci)
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