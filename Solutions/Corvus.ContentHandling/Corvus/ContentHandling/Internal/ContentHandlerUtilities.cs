// <copyright file="ContentHandlerUtilities.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Internal
{
    /// <summary>
    /// Common utility functions for content handlers.
    /// </summary>
    internal static class ContentHandlerUtilities
    {
        /// <summary>
        /// Get the handler content type, given a payload content type and a handler class.
        /// </summary>
        /// <param name="payloadContentType">The content type of the payload.</param>
        /// <param name="handlerClass">The class of handler.</param>
        /// <returns>The content type.</returns>
        internal static string GetHandlerContentType(string payloadContentType, string handlerClass)
        {
            return $"{payloadContentType}+{handlerClass.ToLowerInvariant()}";
        }
    }
}
