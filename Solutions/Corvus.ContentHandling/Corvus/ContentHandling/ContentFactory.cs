// <copyright file="ContentFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
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
    /// the <see cref="ContentFactoryServiceCollectionExtensions.AddContent"/> extension method.
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
        /// Gets the service collection associated with this factory.
        /// </summary>
        internal IServiceCollection Services { get; }

        /// <summary>
        /// Gets the dictionary from content types to registered details for that type.
        /// </summary>
        private ConcurrentDictionary<string, RegisteredContentType> ContentTypes { get; } = new ConcurrentDictionary<string, RegisteredContentType>();

        /// <summary>
        /// Gets the registered content type for a specified type.
        /// </summary>
        /// <param name="type">The type for which to get the content type.</param>
        /// <returns>The content type registered for the type.</returns>
        /// <remarks>The type must provide a static/const string called. <c>RegisteredContentType</c> which defines its content type.</remarks>
        public static string GetContentType(Type type)
        {
            ArgumentNullException.ThrowIfNull(type);

            if (!TryGetContentType(type, out string? contentType))
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
        public static bool TryGetContentType(object target, [NotNullWhen(true)] out string? contentType)
        {
            ArgumentNullException.ThrowIfNull(target);

            Type targetType = target.GetType();
            PropertyInfo? contentTypeProp = targetType.GetProperty("ContentType", BindingFlags.Public | BindingFlags.Instance);
            contentType = (string?)contentTypeProp?.GetValue(target);

            // If we have a content type return true, otherwise try to fall back to the RegisteredContentType for the class.
#pragma warning disable RCS1104 // Simplify conditional expression. - not an obvious improvement
#pragma warning disable IDE0075 // Simplify conditional expression - not an obvious improvement
            return string.IsNullOrEmpty(contentType) ? TryGetContentType(targetType, out contentType) : true;
#pragma warning restore IDE0075, RCS1104
        }

        /// <summary>
        /// Attempts to get the registered content type for a specified type.
        /// </summary>
        /// <param name="type">The type for which to get the content type.</param>
        /// <param name="contentType">The content type registered for the type.</param>
        /// <returns>A boolean indicating success.</returns>
        /// <remarks>The type must provide a static/const string called. <c>RegisteredContentType</c> which defines its content type.</remarks>
        public static bool TryGetContentType(Type type, [NotNullWhen(true)] out string? contentType)
        {
            ArgumentNullException.ThrowIfNull(type);

            FieldInfo? contentTypeField = type.GetTypeInfo().GetField("RegisteredContentType");
            contentType = null;

            if (contentTypeField?.IsStatic == true && contentTypeField.FieldType == typeof(string))
            {
                contentType = (string?)contentTypeField.GetValue(null);
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
        public static bool TryGetContentType<T>([NotNullWhen(true)] out string? contentType)
        {
            return TryGetContentType(typeof(T), out contentType);
        }

        /// <summary>
        /// Gets the implementation type for a content type.
        /// </summary>
        /// <param name="contentType">
        /// The content type for which to retrieve the implementation type.
        /// </param>
        /// <param name="implementingType">
        /// The implementation type, or null if the content type is not recognized.
        /// </param>
        /// <returns>
        /// True if the content type has been registered, false if not.
        /// </returns>
        public bool TryGetContentType(string contentType, [NotNullWhen(true)] out Type? implementingType)
        {
            if (this.ContentTypes.TryGetValue(contentType, out RegisteredContentType registration))
            {
                implementingType = registration.ImplementingType;
                return true;
            }

            implementingType = null;
            return false;
        }

        /// <summary>
        /// Gets the implementation type for a content type.
        /// </summary>
        /// <param name="contentType">
        /// The content type for which to retrieve the implementation type.
        /// </param>
        /// <param name="implementingType">
        /// The implementation type, or null if the content type is not recognized.
        /// </param>
        /// <param name="usesServices">
        /// True if the implementation type depends on services, thus needing to be initialized
        /// through dependency injection, false if it does not, or if the content type was not
        /// registered.
        /// </param>
        /// <returns>
        /// True if the content type has been registered, false if not.
        /// </returns>
        public bool TryGetContentType(string contentType, [NotNullWhen(true)] out Type? implementingType, out bool usesServices)
        {
            if (this.ContentTypes.TryGetValue(contentType, out RegisteredContentType registration))
            {
                implementingType = registration.ImplementingType;
                usesServices = registration.UseServices;
                return true;
            }

            implementingType = null;
            usesServices = false;
            return false;
        }

        /// <summary>
        /// Registers an implementation for a content type, where the implementation type requires
        /// services, and needs to be initialized through dependency injection.
        /// </summary>
        /// <param name="contentType">The content type.</param>
        /// <param name="implementationType">The implementation type for this content type.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown if an implementation for this content type has already been registered.
        /// </exception>
        public void AddTypeRequiringServices(string contentType, Type implementationType)
            => this.AddType(contentType, implementationType, true);

        /// <summary>
        /// Registers an implementation for a content type, where the implementation type does not
        /// require services, and so can be initialized through normal deserialization, enabling
        /// the use of constructor-based deserialization if required.
        /// </summary>
        /// <param name="contentType">The content type.</param>
        /// <param name="implementationType">The implementation type for this content type.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown if an implementation for this content type has already been registered.
        /// </exception>
        public void AddSimpleDeserializableType(string contentType, Type implementationType)
            => this.AddType(contentType, implementationType, false);

        /// <summary>
        /// Gets all of the registered content types, and their corresponding implementation types.
        /// </summary>
        /// <returns>
        /// A collection of all of the registered content types, and their corresponding implementation types.
        /// </returns>
        internal IEnumerable<KeyValuePair<string, Type>> GetRegisteredContentTypes() =>
            this.ContentTypes.Select(kv => new KeyValuePair<string, Type>(kv.Key, kv.Value.ImplementingType));

        private void AddType(string contentType, Type implementationType, bool usesServices)
        {
            if (!this.ContentTypes.TryAdd(contentType, new RegisteredContentType(implementationType, usesServices)))
            {
                throw new InvalidOperationException(string.Format(Resources.NamedTypeAlreadyAdded, contentType));
            }
        }

        private readonly struct RegisteredContentType
        {
            public RegisteredContentType(Type implementingType, bool useServices)
            {
                this.ImplementingType = implementingType;
                this.UseServices = useServices;
            }

            /// <summary>
            /// Gets the implementing type for this content type registration.
            /// </summary>
            public Type ImplementingType { get; }

            /// <summary>
            /// Gets a value indicating whether this implementation type needs access to services
            /// meaning that it needs to be initialized via dependency injection.
            /// </summary>
            public bool UseServices { get; }
        }
    }
}