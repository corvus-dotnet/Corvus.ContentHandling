// <copyright file="MediaTypeDictionaryExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Extensions for an <see cref="IDictionary{MediaType, T}"/> which add logic about hierarchical
    /// media type handling.
    /// </summary>
    public static class MediaTypeDictionaryExtensions
    {
        /// <summary>
        /// Attempts to retrieve the entry for a media type.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type that is stored in the dictionary.</typeparam>
        /// <param name="dictionary">The dictionary to search.</param>
        /// <param name="mediaType">The media type to search for.</param>
        /// <param name="buildKey">The function with which to build the key from the media type.</param>
        /// <returns>The item. </returns>
        public static TValue GetRecursive<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, MediaType mediaType, Func<MediaType, TKey> buildKey)
        {
            if (dictionary is null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            if (buildKey is null)
            {
                throw new ArgumentNullException(nameof(buildKey));
            }

            if (dictionary.TryGetRecursive(mediaType, buildKey, out TValue result))
            {
                return result;
            }

            throw new KeyNotFoundException($"The specified key {mediaType} could not be found");
        }

        /// <summary>
        /// Attempts to retrieve the entry for a media type.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type that is stored in the dictionary.</typeparam>
        /// <param name="dictionary">The dictionary to search.</param>
        /// <param name="mediaType">The media type to search for.</param>
        /// <param name="buildKey">The function with which to build the key from the media type.</param>
        /// <param name="result">The value, if located.</param>
        /// <returns>True if a value was found, false otherwise.</returns>
        public static bool TryGetRecursive<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, MediaType mediaType, Func<MediaType, TKey> buildKey, out TValue result)
        {
            if (dictionary is null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            if (buildKey is null)
            {
                throw new ArgumentNullException(nameof(buildKey));
            }

            if (dictionary.TryGetValue(buildKey(mediaType), out result))
            {
                return true;
            }

            if (mediaType != MediaType.None)
            {
                return dictionary.TryGetRecursive(mediaType.GetParent(), buildKey, out result);
            }

            result = default;
            return false;
        }
    }
}