﻿// <copyright file="PocObjectWithDictionary.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json.Specs.Samples
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A plain old CLR object.
    /// </summary>
    public class PocObjectWithDictionary : IEquatable<PocObjectWithDictionary>
    {
        /// <summary>
        /// Gets or sets a standard string value.
        /// </summary>
        public string SomeValue { get; set; }

        /// <summary>
        /// Gets or sets a dictionary of values.
        /// </summary>
        public Dictionary<string, string> Dictionary { get; set; }

        /// <summary>
        /// Compares two instances of PocObjectWithDictionary for equality.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>True if the instances are equal, false otherwise.</returns>
        public static bool operator ==(PocObjectWithDictionary left, PocObjectWithDictionary right)
        {
            return left?.Equals(right) ?? false;
        }

        /// <summary>
        /// Compares two instances of PocObject for inequality.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>False if the instances are equal, true otherwise.</returns>
        public static bool operator !=(PocObjectWithDictionary left, PocObjectWithDictionary right)
        {
            return !(left == right);
        }

        /// <inheritdoc />
        public bool Equals(PocObjectWithDictionary other)
        {
            if (this.SomeValue != other.SomeValue)
            {
                return false;
            }

            if (this.Dictionary == null && other.Dictionary == null)
            {
                return true;
            }

            if (this.Dictionary != null && other.Dictionary != null && this.Dictionary.Count == other.Dictionary.Count)
            {
                foreach (string current in this.Dictionary.Keys)
                {
                    if (!other.Dictionary.ContainsKey(current) || this.Dictionary[current] != other.Dictionary[current])
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is PocObjectWithDictionary sci)
            {
                return this.Equals(sci);
            }

            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(this.SomeValue, this.Dictionary);
        }
    }
}