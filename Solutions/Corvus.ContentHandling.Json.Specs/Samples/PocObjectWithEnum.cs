// <copyright file="PocObjectWithEnum.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json.Specs.Samples
{
    using System;

    /// <summary>
    /// A plain old CLR object.
    /// </summary>
    public class PocObjectWithEnum : IEquatable<PocObjectWithEnum>
    {
        /// <summary>
        /// Gets or sets a simple value.
        /// </summary>
        public string? SomeValue { get; set; }

        /// <summary>
        /// Gets or sets an enum.
        /// </summary>
        public SomeEnum SomeEnum { get; set; }

        /// <summary>
        /// Compares two instances of <see cref="PocObjectWithEnum"/> for equality.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>True if the instances are equal, false otherwise.</returns>
        public static bool operator ==(PocObjectWithEnum left, PocObjectWithEnum right)
        {
            return left?.Equals(right) ?? false;
        }

        /// <summary>
        /// Compares two instances of <see cref="PocObjectWithEnum"/> for inequality.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>False if the instances are equal, true otherwise.</returns>
        public static bool operator !=(PocObjectWithEnum left, PocObjectWithEnum right)
        {
            return !(left == right);
        }

        /// <inheritdoc />
        public bool Equals(PocObjectWithEnum? other)
        {
            return other is not null && (this.SomeValue, this.SomeEnum) == (other.SomeValue, other.SomeEnum);
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            if (obj is PocObjectWithEnum sci)
            {
                return this.Equals(sci);
            }

            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode() => HashCode.Combine(this.SomeValue, this.SomeEnum);
    }
}