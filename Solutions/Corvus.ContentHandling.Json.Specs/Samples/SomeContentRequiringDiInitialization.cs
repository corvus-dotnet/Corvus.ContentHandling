// <copyright file="SomeContentRequiringDiInitialization.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json.Specs.Samples
{
    using System;

    /// <summary>
    /// A polymorphic content type based on an interface, with a constructor that should be
    /// populated via DI.
    /// </summary>
    public class SomeContentRequiringDiInitialization : ISomeContentInterface, IEquatable<SomeContentRequiringDiInitialization>
    {
        /// <summary>
        /// The content type.
        /// </summary>
        public const string RegisteredContentType = "application/vnd.corvus.somecontentrequiringdiinitialization";

        /// <summary>
        /// DI-style constructor.
        /// </summary>
        /// <param name="cf">
        /// A dependency. (Used only to verify that it works.
        /// </param>
        public SomeContentRequiringDiInitialization(ContentFactory cf)
        {
            ArgumentNullException.ThrowIfNull(cf);
        }

        /// <inheritdoc/>
        public string ContentType => RegisteredContentType;

        /// <summary>
        /// Gets or sets a value.
        /// </summary>
        public string? SomeValue { get; set; }

        /// <summary>
        /// Compares two instances of SomeContentRequiringDiInitialization for equality.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>True if the instances are equal, false otherwise.</returns>
        public static bool operator ==(SomeContentRequiringDiInitialization left, SomeContentRequiringDiInitialization right)
        {
            return left?.Equals(right) ?? false;
        }

        /// <summary>
        /// Compares two instances of SomeContentRequiringDiInitialization for inequality.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>False if the instances are equal, true otherwise.</returns>
        public static bool operator !=(SomeContentRequiringDiInitialization left, SomeContentRequiringDiInitialization right)
        {
            return !(left == right);
        }

        /// <inheritdoc />
        public bool Equals(SomeContentRequiringDiInitialization? other)
        {
            return other is not null && this.SomeValue == other.SomeValue;
        }

        /// <inheritdoc />
        public bool Equals(ISomeContentInterface? other)
        {
            return this.Equals(other as SomeContentRequiringDiInitialization);
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            if (obj is SomeContentRequiringDiInitialization sci)
            {
                return this.Equals(sci);
            }

            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return this.SomeValue?.GetHashCode() ?? 0;
        }
    }
}