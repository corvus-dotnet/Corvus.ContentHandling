// <copyright file="MediaType.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling
{
    using System;

    /// <summary>
    /// Represents a MediaType, which is a combination of a content type and an encoding separated by a "+".
    /// </summary>
    public readonly struct MediaType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaType"/> struct.
        /// </summary>
        /// <param name="typeAndSubtype">The content type and subtype portion of the media type.</param>
        /// <param name="structuredSyntaxSuffix">The (optional) encoding for the media type.</param>
        public MediaType(string typeAndSubtype, string structuredSyntaxSuffix = "")
        {
            this.TypeAndSubtype = typeAndSubtype ?? throw new ArgumentNullException(nameof(typeAndSubtype));
            this.StructuredSyntaxSuffix = structuredSyntaxSuffix ?? throw new ArgumentNullException(nameof(structuredSyntaxSuffix));
        }

        /// <summary>
        /// Gets an empty MediaType.
        /// </summary>
        public static MediaType None { get; } = new(string.Empty, string.Empty);

        /// <summary>
        /// Gets the content type and subtype for this media type.
        /// </summary>
        /// <remarks>This will generally be a hierarchical string, using dot separators.</remarks>
        /// <example>application/vnd.Corvus.data-catalog.data-store.data-lake-store.</example>
        public string TypeAndSubtype { get; }

        /// <summary>
        /// Gets the encoding for the media type. Internally, we are often using this to specify a handler
        /// class when routing requests based on a content type.
        /// </summary>
        public string StructuredSyntaxSuffix { get; }

        /// <summary>
        /// Explicit conversion from a media type to a string.
        /// </summary>
        /// <param name="mediaType">The media type to convert.</param>
        public static explicit operator string(MediaType mediaType)
        {
            return mediaType.ToString();
        }

        /// <summary>
        /// Explicit conversion from a string to a media type.
        /// </summary>
        /// <param name="mediaType">The string to convert.</param>
        /// <remarks>
        /// If the string contains more than one <c>+</c> separator, the conversion will fail with an
        /// <see cref="InvalidOperationException"/>. If there is no <c>+</c> separator, the encoding will
        /// be assumed to be null.
        /// </remarks>
        public static explicit operator MediaType(string mediaType)
        {
            if (string.IsNullOrEmpty(mediaType))
            {
                return MediaType.None;
            }

            string[] parts = mediaType.Split('+');

            if (parts.Length > 2)
            {
                throw new InvalidOperationException($"Cannot cast the specified string \"{mediaType}\" to a MediaType as it contains more than one \"+\" character. Media types must consist of a content type, with an optional encoding separated by a \"+\" character.");
            }

            string contentType = parts[0];
            string encoding = parts.Length > 1 ? parts[1] : string.Empty;

            return new MediaType(contentType, encoding);
        }

        /// <summary>
        /// CHecks equality of two MediaType instances.
        /// </summary>
        /// <param name="lhs">The left hand side of the expression.</param>
        /// <param name="rhs">The right hand side of the expression.</param>
        /// <returns>True if the two instances are equal, false otherwise.</returns>
        public static bool operator ==(MediaType lhs, MediaType rhs)
        {
            return lhs.Equals(rhs);
        }

        /// <summary>
        /// CHecks inequality of two MediaType instances.
        /// </summary>
        /// <param name="lhs">The left hand side of the expression.</param>
        /// <param name="rhs">The right hand side of the expression.</param>
        /// <returns>False if the two instances are equal, true otherwise.</returns>
        public static bool operator !=(MediaType lhs, MediaType rhs)
        {
            return !lhs.Equals(rhs);
        }

        /// <summary>
        /// Attempts to find the "parent" media type for this media type.
        /// </summary>
        /// <returns>The parent if it can be determined, <see cref="None"/> if not.</returns>
        /// <remarks>Parent media type is determined by trying to remove the last section of the ContentType up
        /// to the last <c>.</c>. If no <c>.</c> is present, there is no parent and
        /// <see cref="None"/>will be returned.
        /// </remarks>
        public MediaType GetParent()
        {
            int indexOfLastDot = this.TypeAndSubtype.LastIndexOf('.');
            if (indexOfLastDot > 0)
            {
                string parentContentType = this.TypeAndSubtype.Substring(0, indexOfLastDot);
                return new MediaType(parentContentType, this.StructuredSyntaxSuffix);
            }

            return None;
        }

        /// <summary>
        /// Converts the media type to a string representation.
        /// </summary>
        /// <returns>The string representation of the media type.</returns>
        public override string ToString()
        {
            if (this == None)
            {
                return string.Empty;
            }

            if (string.IsNullOrEmpty(this.StructuredSyntaxSuffix))
            {
                return this.TypeAndSubtype;
            }

            return $"{this.TypeAndSubtype}+{this.StructuredSyntaxSuffix}";
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            if (obj is MediaType link)
            {
                return (this.TypeAndSubtype, this.StructuredSyntaxSuffix) == (link.TypeAndSubtype, link.StructuredSyntaxSuffix);
            }

            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(this.TypeAndSubtype, this.StructuredSyntaxSuffix);
        }
    }
}