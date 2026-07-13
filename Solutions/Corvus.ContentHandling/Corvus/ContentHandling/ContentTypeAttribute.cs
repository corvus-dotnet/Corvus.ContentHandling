// <copyright file="ContentTypeAttribute.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling
{
    using System;

    /// <summary>
    /// Declares the content type for a class or interface, in the media-type-like form
    /// used by the content handling framework (e.g. <c>application/vnd.corvus.example</c>).
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is the preferred way to associate a content type with a type. For compatibility
    /// with types written for earlier versions of this library, a <c>static</c> or <c>const</c>
    /// string field named <c>RegisteredContentType</c> is also recognized; the attribute takes
    /// precedence when both are present.
    /// </para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
    public sealed class ContentTypeAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentTypeAttribute"/> class.
        /// </summary>
        /// <param name="contentType">The content type for the decorated type.</param>
        public ContentTypeAttribute(string contentType)
        {
            ArgumentException.ThrowIfNullOrEmpty(contentType);
            this.ContentType = contentType;
        }

        /// <summary>
        /// Gets the content type for the decorated type.
        /// </summary>
        public string ContentType { get; }
    }
}
