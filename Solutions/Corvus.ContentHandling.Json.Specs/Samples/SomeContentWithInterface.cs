// <copyright file="SomeContentWithInterface.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json.Specs.Samples
{
    using System;

    /// <summary>
    /// A polymorphic content type based on an interface.
    /// </summary>
    public class SomeContentWithInterface : ISomeContentInterface, IEquatable<SomeContentWithInterface>
    {
        /// <summary>
        /// The content type.
        /// </summary>
        public const string RegisteredContentType = "application/vnd.corvus.somecontentwithinterface";

        /// <inheritdoc/>
        public string ContentType => RegisteredContentType;

        /// <summary>
        /// Gets or sets a value.
        /// </summary>
        public string SomeValue { get; set; }

        /// <summary>
        /// Compares two instances of SomeContentWithInterface for equality.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>True if the instances are equal, false otherwise.</returns>
        public static bool operator ==(SomeContentWithInterface left, SomeContentWithInterface right)
        {
            return left?.Equals(right) ?? false;
        }

        /// <summary>
        /// Compares two instances of SomeContentWithInterface for inequality.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>False if the instances are equal, true otherwise.</returns>
        public static bool operator !=(SomeContentWithInterface left, SomeContentWithInterface right)
        {
            return !(left == right);
        }

        /// <inheritdoc />
        public bool Equals(SomeContentWithInterface other)
        {
            return this.SomeValue == other.SomeValue;
        }

        /// <inheritdoc />
        public bool Equals(ISomeContentInterface other)
        {
            return this.Equals(other as SomeContentWithInterface);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is SomeContentWithInterface sci)
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