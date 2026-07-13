// <copyright file="ContentTypes.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Discovers the content type associated with a type or instance, by convention.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Type-level discovery looks for a <see cref="ContentTypeAttribute"/> first, then falls
    /// back to a <c>static</c>/<c>const</c> string field named <c>RegisteredContentType</c>
    /// (the convention used by earlier versions of this library).
    /// </para>
    /// <para>
    /// Object-level discovery looks for a public instance property named <c>ContentType</c>
    /// returning a non-empty string, then falls back to type-level discovery. The instance
    /// property is also the value serialized as the <c>contentType</c> discriminator by
    /// the JSON support library, so types participating in polymorphic serialization should
    /// provide it.
    /// </para>
    /// <para>
    /// Discovery results are cached per type, so the reflection cost is paid once per type
    /// per process; repeat lookups (registration, dispatch-by-payload, envelope creation)
    /// are dictionary hits.
    /// </para>
    /// </remarks>
    public static class ContentTypes
    {
        private static readonly ConcurrentDictionary<Type, string?> TypeContentTypes = new();
        private static readonly ConcurrentDictionary<Type, Func<object, string?>?> ContentTypeGetters = new();

        /// <summary>
        /// Gets the content type for a specified type.
        /// </summary>
        /// <param name="type">The type for which to get the content type.</param>
        /// <returns>The content type declared by the type.</returns>
        /// <exception cref="InvalidOperationException">
        /// The type declares no content type via <see cref="ContentTypeAttribute"/> or a
        /// static string field named <c>RegisteredContentType</c>.
        /// </exception>
        public static string GetContentType(Type type)
        {
            ArgumentNullException.ThrowIfNull(type);

            if (!TryGetContentType(type, out string? contentType))
            {
                throw new InvalidOperationException($"The type '{type}' does not declare a content type. Add a [ContentType(\"...\")] attribute, or a static string field named 'RegisteredContentType'.");
            }

            return contentType;
        }

        /// <summary>
        /// Gets the content type for a specified type.
        /// </summary>
        /// <typeparam name="T">The type for which to get the content type.</typeparam>
        /// <returns>The content type declared by the type.</returns>
        /// <exception cref="InvalidOperationException">
        /// The type declares no content type via <see cref="ContentTypeAttribute"/> or a
        /// static string field named <c>RegisteredContentType</c>.
        /// </exception>
        public static string GetContentType<T>()
        {
            return GetContentType(typeof(T));
        }

        /// <summary>
        /// Attempts to get the content type for a specified type.
        /// </summary>
        /// <param name="type">The type for which to get the content type.</param>
        /// <param name="contentType">The content type declared by the type.</param>
        /// <returns>True if the type declares a content type.</returns>
        public static bool TryGetContentType(Type type, [NotNullWhen(true)] out string? contentType)
        {
            ArgumentNullException.ThrowIfNull(type);

            contentType = TypeContentTypes.GetOrAdd(type, static t => DiscoverContentType(t));
            return !string.IsNullOrEmpty(contentType);
        }

        /// <summary>
        /// Attempts to get the content type for a specified type.
        /// </summary>
        /// <typeparam name="T">The type for which to get the content type.</typeparam>
        /// <param name="contentType">The content type declared by the type.</param>
        /// <returns>True if the type declares a content type.</returns>
        public static bool TryGetContentType<T>([NotNullWhen(true)] out string? contentType)
        {
            return TryGetContentType(typeof(T), out contentType);
        }

        /// <summary>
        /// Attempts to get the content type for an instance.
        /// </summary>
        /// <param name="target">The instance for which to get the content type.</param>
        /// <param name="contentType">The content type of the instance.</param>
        /// <returns>True if a content type could be determined.</returns>
        /// <remarks>
        /// A public instance property named <c>ContentType</c> returning a non-empty string
        /// takes precedence; otherwise the instance's type is inspected as for
        /// <see cref="TryGetContentType(Type, out string?)"/>.
        /// </remarks>
        public static bool TryGetContentType(object target, [NotNullWhen(true)] out string? contentType)
        {
            ArgumentNullException.ThrowIfNull(target);

            Type targetType = target.GetType();
            Func<object, string?>? getter = ContentTypeGetters.GetOrAdd(targetType, static t => BuildContentTypeGetter(t));
            contentType = getter?.Invoke(target);

            return string.IsNullOrEmpty(contentType)
                ? TryGetContentType(targetType, out contentType)
                : true;
        }

        /// <summary>
        /// Gets the content type for an instance.
        /// </summary>
        /// <param name="target">The instance for which to get the content type.</param>
        /// <returns>The content type of the instance.</returns>
        /// <exception cref="InvalidOperationException">
        /// No content type could be determined for the instance.
        /// </exception>
        public static string GetContentType(object target)
        {
            if (!TryGetContentType(target, out string? contentType))
            {
                throw new InvalidOperationException($"The instance of type '{target.GetType()}' does not provide a content type. Add a public 'ContentType' string property, a [ContentType(\"...\")] attribute, or a static string field named 'RegisteredContentType'.");
            }

            return contentType;
        }

        private static string? DiscoverContentType(Type type)
        {
            ContentTypeAttribute? attribute = type.GetCustomAttribute<ContentTypeAttribute>(inherit: false);
            if (attribute is not null)
            {
                return attribute.ContentType;
            }

            FieldInfo? contentTypeField = type.GetField("RegisteredContentType");
            if (contentTypeField?.IsStatic == true && contentTypeField.FieldType == typeof(string))
            {
                return (string?)contentTypeField.GetValue(null);
            }

            return null;
        }

        private static Func<object, string?>? BuildContentTypeGetter(Type type)
        {
            PropertyInfo? contentTypeProperty = type.GetProperty("ContentType", BindingFlags.Public | BindingFlags.Instance);
            if (contentTypeProperty?.PropertyType != typeof(string) || contentTypeProperty.GetMethod is null)
            {
                return null;
            }

            ParameterExpression target = Expression.Parameter(typeof(object));
            return Expression.Lambda<Func<object, string?>>(
                Expression.Property(Expression.Convert(target, type), contentTypeProperty),
                target).Compile();
        }
    }
}
