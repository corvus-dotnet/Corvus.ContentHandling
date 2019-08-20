// <copyright file="StandardContractResolver.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json
{
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// A standard endjin contract resolver.
    /// </summary>
    public class StandardContractResolver : CamelCasePropertyNamesContractResolver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StandardContractResolver"/> class.
        /// </summary>
        public StandardContractResolver()
        {
            this.NamingStrategy.ProcessDictionaryKeys = false;
        }

        /// <summary>
        /// Gets a standard endjin contract resolver.
        /// </summary>
        public static StandardContractResolver Instance { get; } = new StandardContractResolver();
    }
}