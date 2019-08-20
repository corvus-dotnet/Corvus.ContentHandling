// <copyright file="SomeContentWithInterfaceAndPocChild.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json.Specs.Samples
{
    using System;

    /// <summary>
    /// A polymorphic content type based on an interface.
    /// </summary>
    public class SomeContentWithInterfaceAndPocChild : ISomeContentInterface, IEquatable<SomeContentWithInterfaceAndPocChild>
    {
        /// <summary>
        /// The content type.
        /// </summary>
        public const string RegisteredContentType = "application/vnd.corvus.somecontentwithinterfaceandpocchild";

        /// <inheritdoc/>
        public string ContentType => RegisteredContentType;

        /// <summary>
        /// Gets or sets a value.
        /// </summary>
        public string SomeValue { get; set; }

        /// <summary>
        /// Gets or sets a child.
        /// </summary>
        public PocObject Child { get; set; }

        /// <summary>
        /// Compares two instances of SomeContentWithInterfaceAndPocChild for equality.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>True if the instances are equal, false otherwise.</returns>
        public static bool operator ==(SomeContentWithInterfaceAndPocChild left, SomeContentWithInterfaceAndPocChild right)
        {
            return left?.Equals(right) ?? false;
        }

        /// <summary>
        /// Compares two instances of SomeContentWithInterfaceAndPocChild for inequality.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>False if the instances are equal, true otherwise.</returns>
        public static bool operator !=(SomeContentWithInterfaceAndPocChild left, SomeContentWithInterfaceAndPocChild right)
        {
            return !(left == right);
        }

        /// <inheritdoc />
        public bool Equals(SomeContentWithInterfaceAndPocChild other)
        {
            return this.SomeValue == other.SomeValue && this.Child == other.Child;
        }

        /// <inheritdoc />
        public bool Equals(ISomeContentInterface other)
        {
            return this.Equals(other as SomeContentWithInterfaceAndPocChild);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is SomeContentWithInterfaceAndPocChild sci)
            {
                return this.Equals(sci);
            }

            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return (this.SomeValue, this.Child).GetHashCode();
        }
    }
}
