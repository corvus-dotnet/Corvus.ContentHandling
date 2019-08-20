// <copyright file="SomeContentWithBaseAndChild.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json.Specs.Samples
{
    using System;

    /// <summary>
    /// A polymorphic content type based on an abstract base.
    /// </summary>
    public class SomeContentWithBaseAndChild : SomeContentBase, IEquatable<SomeContentWithBaseAndChild>
    {
        /// <summary>
        /// The content type.
        /// </summary>
        public const string RegisteredContentType = "application/vnd.corvus.somecontentwithbaseandchild";

        /// <summary>
        /// Initializes a new instance of the <see cref="SomeContentWithBaseAndChild"/> class.
        /// </summary>
        public SomeContentWithBaseAndChild()
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
        public SomeContentBase Child { get; set; }

        /// <summary>
        /// Compares two instances of <see cref="SomeContentWithBaseAndChild"/> for equality.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>True if the instances are equal, false otherwise.</returns>
        public static bool operator ==(SomeContentWithBaseAndChild left, SomeContentWithBaseAndChild right)
        {
            return left?.Equals(right) ?? false;
        }

        /// <summary>
        /// Compares two instances of <see cref="SomeContentWithBaseAndChild"/> for inequality.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>False if the instances are equal, true otherwise.</returns>
        public static bool operator !=(SomeContentWithBaseAndChild left, SomeContentWithBaseAndChild right)
        {
            return !(left == right);
        }

        /// <inheritdoc />
        public bool Equals(SomeContentWithBaseAndChild other)
        {
            return this.SomeValue == other.SomeValue;
        }

        /// <inheritdoc />
        public override bool Equals(SomeContentBase other)
        {
            return this.Equals(other as SomeContentWithBaseAndChild);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is SomeContentWithBaseAndChild sci)
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
