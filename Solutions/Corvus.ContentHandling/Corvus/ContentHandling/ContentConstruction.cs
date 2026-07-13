// <copyright file="ContentConstruction.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling
{
    /// <summary>
    /// Describes how instances of a registered content type are constructed.
    /// </summary>
    public enum ContentConstruction
    {
        /// <summary>
        /// Instances are resolved from the service provider, via a keyed service
        /// registration whose key is the content type.
        /// </summary>
        FromServices,

        /// <summary>
        /// Instances are constructed outside the container — by a serializer during
        /// deserialization, or via the type's public parameterless constructor.
        /// </summary>
        BySerializer,
    }
}
