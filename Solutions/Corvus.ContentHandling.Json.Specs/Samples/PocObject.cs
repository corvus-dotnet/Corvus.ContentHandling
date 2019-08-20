// <copyright file="PocObject.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json.Specs.Samples
{
    using System;

    /// <summary>
    /// A plain old CLR object.
    /// </summary>
    public class PocObject : IEquatable<PocObject>
    {
        /// <summary>
        /// Gets or sets a simple value.
        /// </summary>
        public string SomeValue { get; set; }

        /// <summary>
        /// Compares two instances of PocObject for equality.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>True if the instances are equal, false otherwise.</returns>
        public static bool operator ==(PocObject left, PocObject right)
        {
            return left?.Equals(right) ?? false;
        }

        /// <summary>
        /// Compares two instances of PocObject for inequality.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>False if the instances are equal, true otherwise.</returns>
        public static bool operator !=(PocObject left, PocObject right)
        {
            return !(left == right);
        }

        /// <inheritdoc />
        public bool Equals(PocObject other)
        {
            return this.SomeValue == other.SomeValue;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is PocObject sci)
            {
                return this.Equals(sci);
            }

            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return this.SomeValue.GetHashCode();
        }
    }
}