// <copyright file="SomeContentWithBaseAndPocChild.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json.Specs.Samples
{
    using System;

    /// <summary>
    /// A polymorphic content type based on an abstract base.
    /// </summary>
    public class SomeContentWithBaseAndPocChild : SomeContentBase, IEquatable<SomeContentWithBaseAndPocChild>
    {
        /// <summary>
        /// The content type.
        /// </summary>
        public const string RegisteredContentType = "application/vnd.corvus.somecontentwithbaseandpocchild";

        /// <summary>
        /// Initializes a new instance of the <see cref="SomeContentWithBaseAndPocChild"/> class.
        /// </summary>
        public SomeContentWithBaseAndPocChild()
            : base(RegisteredContentType)
        {
        }

        /// <summary>
        /// Gets or sets a value.
        /// </summary>
        public string SomeValue { get; set; }

        /// <summary>
        /// Gets or sets a child.
        /// </summary>
        public PocObject Child { get; set; }

        /// <summary>
        /// Compares two instances of <see cref="SomeContentWithBaseAndPocChild"/> for equality.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>True if the instances are equal, false otherwise.</returns>
        public static bool operator ==(SomeContentWithBaseAndPocChild left, SomeContentWithBaseAndPocChild right)
        {
            return left?.Equals(right) ?? false;
        }

        /// <summary>
        /// Compares two instances of <see cref="SomeContentWithBaseAndPocChild"/> for inequality.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>False if the instances are equal, true otherwise.</returns>
        public static bool operator !=(SomeContentWithBaseAndPocChild left, SomeContentWithBaseAndPocChild right)
        {
            return !(left == right);
        }

        /// <inheritdoc />
        public bool Equals(SomeContentWithBaseAndPocChild other)
        {
            return this.SomeValue == other.SomeValue;
        }

        /// <inheritdoc />
        public override bool Equals(SomeContentBase other)
        {
            return this.Equals(other as SomeContentWithBaseAndPocChild);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is SomeContentWithBaseAndPocChild sci)
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
