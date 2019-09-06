// <copyright file="ContentFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling
{
    using System;
    using System.Collections.Concurrent;
    using System.Reflection;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// A content factory for registered content types and their handlers.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The content factory supports two distinct usage patterns.
    /// <list type="bullet">
    /// <item>the creation of instances of types, defined by a content-type similar to that used in HTTP media types.</item>
    /// <item>dispatching objects to handlers defined by a content-type similar to that used in HTTP media types.</item>
    /// </list>
    /// </para>
    /// <para>
    /// You obtain an instance of the content factory by adding it to the <see cref="IServiceCollection"/> using
    /// the <see cref="ContentFactoryServiceCollectionExtensions.AddContentFactory"/> extension method.
    /// </para>
    /// <para>You can then register dotnet types for media types, using the various RegisterContent extensions. They follow the similar singleton/transient pattern to any other container.</para>
    /// <para>While you can explicitly specify a content type string for the content, it can also be derived from a static constant string <c>RegisteredContentType</c> field.</para>
    /// <para>Similarly, you can register handlers which can dispatch content to appropriate handlers through an <see cref="IContentHandlerDispatcher{TPayloadBaseType}"/>.</para>
    /// </remarks>
    public class ContentFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentFactory"/> class.
        /// </summary>
        /// <param name="services">The service collection for which to create a content factory.</param>
        internal ContentFactory(IServiceCollection services)
        {
            this.Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        /// <summary>
        /// Gets the dictionary of handlers to the type of the handler.
        /// </summary>
        internal ConcurrentDictionary<string, Type> Handlers { get; } = new ConcurrentDictionary<string, Type>();

        /// <summary>
        /// Gets the service collection associated with this factory.
        /// </summary>
        internal IServiceCollection Services { get; }

        /// <summary>
        /// Gets the registered content type for a specified type.
        /// </summary>
        /// <param name="type">The type for which to get the content type.</param>
        /// <returns>The content type registered for the type.</returns>
        /// <remarks>The type must provide a static/const string called. <c>RegisteredContentType</c> which defines its content type.</remarks>
        public static string GetContentType(Type type)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (!TryGetContentType(type, out string contentType))
            {
                throw new InvalidOperationException(string.Format(Resources.TypeNeedsStaticStringContentTypeField, type));
            }

            return contentType;
        }

        /// <summary>
        /// Attempts to get the content type for a specified type.
        /// </summary>
        /// <param name="target">The object for which to get the content type.</param>
        /// <param name="contentType">The content type registered for the type.</param>
        /// <returns>A boolean indicating success.</returns>
        /// <remarks>The type must provide an instance property called. <c>ContentType</c> which contains its content type.</remarks>
        public static bool TryGetContentType(object target, out string contentType)
        {
            if (target is null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            Type targetType = target.GetType();
            PropertyInfo contentTypeProp = targetType.GetProperty("ContentType", BindingFlags.Public | BindingFlags.Instance);
            contentType = (string)contentTypeProp?.GetValue(target);

            // If we have a content type return true, otherwise try to fall back to the RegisteredContentType for the class.
            return string.IsNullOrEmpty(contentType) ? TryGetContentType(targetType, out contentType) : true;
        }

        /// <summary>
        /// Attempts to get the registered content type for a specified type.
        /// </summary>
        /// <param name="type">The type for which to get the content type.</param>
        /// <param name="contentType">The content type registered for the type.</param>
        /// <returns>A boolean indicating success.</returns>
        /// <remarks>The type must provide a static/const string called. <c>RegisteredContentType</c> which defines its content type.</remarks>
        public static bool TryGetContentType(Type type, out string contentType)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            FieldInfo contentTypeField = type.GetTypeInfo().GetField("RegisteredContentType");
            contentType = null;

            if (contentTypeField?.IsStatic == true && contentTypeField.FieldType == typeof(string))
            {
                contentType = (string)contentTypeField.GetValue(null);
            }

            return !string.IsNullOrEmpty(contentType);
        }

        /// <summary>
        /// Get the content type for a Type.
        /// </summary>
        /// <typeparam name="T">The type for which to retrieve the content type.</typeparam>
        /// <returns>The content type of the type.</returns>
        /// <remarks>The type must provide a static/const string called. <c>RegisteredContentType</c> which defines its content type.</remarks>
        public static string GetContentType<T>()
        {
            return GetContentType(typeof(T));
        }

        /// <summary>
        /// Get the content type for a Type.
        /// </summary>
        /// <typeparam name="T">The type for which to retrieve the content type.</typeparam>
        /// <param name="contentType">The content type of the type.</param>
        /// <returns>A boolean indicating success or failure.</returns>
        /// <remarks>The type must provide a static/const string called. <c>RegisteredContentType</c> which defines its content type.</remarks>
        public static bool TryGetContentType<T>(out string contentType)
        {
            return TryGetContentType(typeof(T), out contentType);
        }
    }
}