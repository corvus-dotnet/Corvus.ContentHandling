// <copyright file="ContentHandlingBuilderExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling
{
    using System;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Lifetime-specific convenience methods for <see cref="ContentHandlingBuilder"/>.
    /// </summary>
    public static class ContentHandlingBuilderExtensions
    {
        /// <summary>
        /// Registers a content type implemented by <typeparamref name="T"/> with singleton lifetime.
        /// </summary>
        /// <typeparam name="T">The type implementing the content.</typeparam>
        /// <param name="builder">The content handling builder.</param>
        /// <param name="contentType">The content type to register.</param>
        /// <returns>The builder, for chaining.</returns>
        public static ContentHandlingBuilder AddSingletonContent<T>(this ContentHandlingBuilder builder, string contentType)
            where T : class
        {
            ArgumentNullException.ThrowIfNull(builder);
            return builder.AddContent<T>(contentType, ServiceLifetime.Singleton);
        }

        /// <summary>
        /// Registers a content type implemented by <typeparamref name="T"/> with singleton
        /// lifetime, deriving the content type from the type itself.
        /// </summary>
        /// <typeparam name="T">The type implementing the content.</typeparam>
        /// <param name="builder">The content handling builder.</param>
        /// <returns>The builder, for chaining.</returns>
        public static ContentHandlingBuilder AddSingletonContent<T>(this ContentHandlingBuilder builder)
            where T : class
        {
            ArgumentNullException.ThrowIfNull(builder);
            return builder.AddContent<T>(ServiceLifetime.Singleton);
        }

        /// <summary>
        /// Registers a content type implemented by <typeparamref name="T"/> with transient lifetime.
        /// </summary>
        /// <typeparam name="T">The type implementing the content.</typeparam>
        /// <param name="builder">The content handling builder.</param>
        /// <param name="contentType">The content type to register.</param>
        /// <returns>The builder, for chaining.</returns>
        public static ContentHandlingBuilder AddTransientContent<T>(this ContentHandlingBuilder builder, string contentType)
            where T : class
        {
            ArgumentNullException.ThrowIfNull(builder);
            return builder.AddContent<T>(contentType, ServiceLifetime.Transient);
        }

        /// <summary>
        /// Registers a content type implemented by <typeparamref name="T"/> with transient
        /// lifetime, deriving the content type from the type itself.
        /// </summary>
        /// <typeparam name="T">The type implementing the content.</typeparam>
        /// <param name="builder">The content handling builder.</param>
        /// <returns>The builder, for chaining.</returns>
        public static ContentHandlingBuilder AddTransientContent<T>(this ContentHandlingBuilder builder)
            where T : class
        {
            ArgumentNullException.ThrowIfNull(builder);
            return builder.AddContent<T>(ServiceLifetime.Transient);
        }

        /// <summary>
        /// Registers a content type implemented by <typeparamref name="T"/> with scoped lifetime.
        /// </summary>
        /// <typeparam name="T">The type implementing the content.</typeparam>
        /// <param name="builder">The content handling builder.</param>
        /// <param name="contentType">The content type to register.</param>
        /// <returns>The builder, for chaining.</returns>
        public static ContentHandlingBuilder AddScopedContent<T>(this ContentHandlingBuilder builder, string contentType)
            where T : class
        {
            ArgumentNullException.ThrowIfNull(builder);
            return builder.AddContent<T>(contentType, ServiceLifetime.Scoped);
        }

        /// <summary>
        /// Registers a content type implemented by <typeparamref name="T"/> with scoped
        /// lifetime, deriving the content type from the type itself.
        /// </summary>
        /// <typeparam name="T">The type implementing the content.</typeparam>
        /// <param name="builder">The content handling builder.</param>
        /// <returns>The builder, for chaining.</returns>
        public static ContentHandlingBuilder AddScopedContent<T>(this ContentHandlingBuilder builder)
            where T : class
        {
            ArgumentNullException.ThrowIfNull(builder);
            return builder.AddContent<T>(ServiceLifetime.Scoped);
        }
    }
}
