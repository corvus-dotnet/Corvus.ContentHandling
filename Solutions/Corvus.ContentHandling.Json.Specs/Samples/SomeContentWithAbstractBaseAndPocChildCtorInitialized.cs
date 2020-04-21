// <copyright file="SomeContentWithAbstractBaseAndPocChildCtorInitialized.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json.Specs.Samples
{
    using System;

    /// <summary>
    /// A polymorphic content type based on an abstract base.
    /// </summary>
    public class SomeContentWithAbstractBaseAndPocChildCtorInitialized : SomeContentAbstractBase, IEquatable<SomeContentWithAbstractBaseAndPocChildCtorInitialized>
    {
        /// <summary>
        /// The content type.
        /// </summary>
        public const string RegisteredContentType = "application/vnd.corvus.somecontentwithabstractbaseandpocchildctorinit";

        /// <summary>
        /// Creates a <see cref="SomeContentWithAbstractBaseAndPocChildCtorInitialized"/>.
        /// </summary>
        /// <param name="someValue"><see cref="SomeValue"/>.</param>
        /// <param name="child"><see cref="Child"/>.</param>
        public SomeContentWithAbstractBaseAndPocChildCtorInitialized(
            string someValue,
            PocObjectWithCtorInitialization child)
        {
            this.SomeValue = someValue;
            this.Child = child;
        }

        /// <inheritdoc/>
        public override string ContentType => RegisteredContentType;

        /// <summary>
        /// Gets a value.
        /// </summary>
        public string SomeValue { get; }

        /// <summary>
        /// Gets a child.
        /// </summary>
        public PocObjectWithCtorInitialization Child { get; }

        /// <summary>
        /// Compares two instances of <see cref="SomeContentWithAbstractBaseAndPocChildCtorInitialized"/> for equality.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>True if the instances are equal, false otherwise.</returns>
        public static bool operator ==(SomeContentWithAbstractBaseAndPocChildCtorInitialized left, SomeContentWithAbstractBaseAndPocChildCtorInitialized right)
        {
            return left?.Equals(right) ?? false;
        }

        /// <summary>
        /// Compares two instances of <see cref="SomeContentWithAbstractBaseAndPocChildCtorInitialized"/> for inequality.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>False if the instances are equal, true otherwise.</returns>
        public static bool operator !=(SomeContentWithAbstractBaseAndPocChildCtorInitialized left, SomeContentWithAbstractBaseAndPocChildCtorInitialized right)
        {
            return !(left == right);
        }

        /// <inheritdoc />
        public bool Equals(SomeContentWithAbstractBaseAndPocChildCtorInitialized other)
        {
            return this.SomeValue == other.SomeValue;
        }

        /// <inheritdoc />
        public override bool Equals(SomeContentAbstractBase other)
        {
            return this.Equals(other as SomeContentWithAbstractBaseAndPocChildCtorInitialized);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is SomeContentWithAbstractBaseAndPocChildCtorInitialized sci)
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
