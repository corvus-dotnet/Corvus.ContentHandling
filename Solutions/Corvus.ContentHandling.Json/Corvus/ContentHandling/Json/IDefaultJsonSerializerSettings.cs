// <copyright file="IDefaultJsonSerializerSettings.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json
{
    using Newtonsoft.Json;

    /// <summary>
    /// A factory to get the default <see cref="JsonSerializerSettings"/> for the context.
    /// </summary>
    public interface IDefaultJsonSerializerSettings
    {
        /// <summary>
        /// Gets an instance of the default JsonSerializerSettings.
        /// </summary>
        JsonSerializerSettings Instance { get; }
    }
}