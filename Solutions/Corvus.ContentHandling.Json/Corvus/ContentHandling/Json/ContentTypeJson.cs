// <copyright file="ContentTypeJson.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Json
{
    /// <summary>
    /// The JSON wire-format conventions used by content-type-based serialization.
    /// </summary>
    /// <remarks>
    /// These property names are part of the wire format shared with earlier versions of this
    /// library: documents produced by v4 (and earlier) deserialize with v5 and vice versa.
    /// </remarks>
    public static class ContentTypeJson
    {
        /// <summary>
        /// The name of the JSON property carrying the content-type discriminator.
        /// </summary>
        public const string ContentTypePropertyName = "contentType";

        /// <summary>
        /// The name of the JSON property carrying the payload of a content envelope.
        /// </summary>
        public const string PayloadPropertyName = "payload";
    }
}
