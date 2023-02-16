// <copyright file="ContentHandlerContentFactoryExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling
{
    using System;
    using System.Threading.Tasks;
    using Corvus.ContentHandling.Internal;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Extensions to register handlers for a <see cref="IContentHandler{TPayloadType}"/> with a particular payload.
    /// </summary>
    public static class ContentHandlerContentFactoryExtensions
    {
        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handle">The function with which to handle the payload.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentHandler<TPayloadBase, TPayload>(this ContentFactory contentFactory, Action<TPayload> handle, string handlerClass)
            where TPayload : TPayloadBase
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (handle is null)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.RegisterContentHandlerWithAction<TPayloadBase, TPayload>(
                contentType,
                handle,
                handlerClass);
            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handle">The function with which to handle the payload.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentHandler<TPayloadBase, TPayload, T1>(this ContentFactory contentFactory, Action<TPayload, T1> handle, string handlerClass)
            where TPayload : TPayloadBase
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (handle is null)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.RegisterContentHandlerWithAction<TPayloadBase, TPayload, T1>(
                contentType,
                handle,
                handlerClass);

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handle">The function with which to handle the payload.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentHandler<TPayloadBase, TPayload, T1, T2>(this ContentFactory contentFactory, Action<TPayload, T1, T2> handle, string handlerClass)
            where TPayload : TPayloadBase
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (handle is null)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.RegisterContentHandlerWithAction<TPayloadBase, TPayload, T1, T2>(
                contentType,
                handle,
                handlerClass);

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <typeparam name="T3">The type of the third parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handle">The function with which to handle the payload.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentHandler<TPayloadBase, TPayload, T1, T2, T3>(this ContentFactory contentFactory, Action<TPayload, T1, T2, T3> handle, string handlerClass)
            where TPayload : TPayloadBase
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (handle is null)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.RegisterContentHandlerWithAction<TPayloadBase, TPayload, T1, T2, T3>(
                contentType,
                handle,
                handlerClass);

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type for which to register the handler.</param>
        /// <param name="handle">The function with which to handle the payload.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentHandler<TPayloadBase>(this ContentFactory contentFactory, string contentType, Action<TPayloadBase> handle, string handlerClass)
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (handle is null)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            contentFactory.RegisterContentHandlerWithAction<TPayloadBase, TPayloadBase>(
                contentType,
                handle,
                handlerClass);
            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type for which to register the handler.</param>
        /// <param name="handle">The function with which to handle the payload.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentHandler<TPayloadBase, T1>(this ContentFactory contentFactory, string contentType, Action<TPayloadBase, T1> handle, string handlerClass)
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (handle is null)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            contentFactory.RegisterContentHandlerWithAction<TPayloadBase, TPayloadBase, T1>(
                contentType,
                handle,
                handlerClass);
            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type for which to register the handler.</param>
        /// <param name="handle">The function with which to handle the payload.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentHandler<TPayloadBase, T1, T2>(this ContentFactory contentFactory, string contentType, Action<TPayloadBase, T1, T2> handle, string handlerClass)
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (handle is null)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            contentFactory.RegisterContentHandlerWithAction<TPayloadBase, TPayloadBase, T1, T2>(
                contentType,
                handle,
                handlerClass);
            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <typeparam name="T3">The type of the third parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type for which to register the handler.</param>
        /// <param name="handle">The function with which to handle the payload.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentHandler<TPayloadBase, T1, T2, T3>(this ContentFactory contentFactory, string contentType, Action<TPayloadBase, T1, T2, T3> handle, string handlerClass)
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (handle is null)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            contentFactory.RegisterContentHandlerWithAction<TPayloadBase, TPayloadBase, T1, T2, T3>(
                contentType,
                handle,
                handlerClass);

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentHandler<TPayloadBase, TPayload, THandler>(this ContentFactory contentFactory, string handlerClass)
            where TPayloadBase : notnull
            where TPayload : TPayloadBase
            where THandler : class, IContentHandler<TPayload>
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.Services.AddSingleton<THandler>();
            contentFactory.RegisterSingletonContent<ContentHandlerWithClass<TPayloadBase, TPayload, THandler>>(
                ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));
            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentHandler<TPayloadBase, TPayload, THandler, T1>(this ContentFactory contentFactory, string handlerClass)
            where TPayloadBase : notnull
            where TPayload : TPayloadBase
            where THandler : class, IContentHandler<TPayload, T1>
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.Services.AddSingleton<THandler>();
            contentFactory.RegisterSingletonContent<ContentHandlerWithClass<TPayloadBase, TPayload, THandler, T1>>(
                ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentHandler<TPayloadBase, TPayload, THandler, T1, T2>(this ContentFactory contentFactory, string handlerClass)
            where TPayloadBase : notnull
            where TPayload : TPayloadBase
            where THandler : class, IContentHandler<TPayload, T1, T2>
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.Services.AddSingleton<THandler>();
            contentFactory.RegisterSingletonContent<ContentHandlerWithClass<TPayloadBase, TPayload, THandler, T1, T2>>(
                ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <typeparam name="T3">The type of the third parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentHandler<TPayloadBase, TPayload, THandler, T1, T2, T3>(this ContentFactory contentFactory, string handlerClass)
            where TPayloadBase : notnull
            where TPayload : TPayloadBase
            where THandler : class, IContentHandler<TPayload, T1, T2, T3>
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.Services.AddSingleton<THandler>();
            contentFactory.RegisterSingletonContent<ContentHandlerWithClass<TPayloadBase, TPayload, THandler, T1, T2, T3>>(
                ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type for which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentHandler<TPayloadBase, THandler>(this ContentFactory contentFactory, string contentType, string handlerClass)
            where TPayloadBase : notnull
            where THandler : class, IContentHandler<TPayloadBase>
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (contentType is null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            contentFactory.Services.AddSingleton<THandler>();
            contentFactory.RegisterSingletonContent<ContentHandlerWithClass<TPayloadBase, TPayloadBase, THandler>>(
                ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type for which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentHandler<TPayloadBase, THandler, T1>(this ContentFactory contentFactory, string contentType, string handlerClass)
            where TPayloadBase : notnull
            where THandler : class, IContentHandler<TPayloadBase, T1>
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (contentType is null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            contentFactory.Services.AddSingleton<THandler>();
            contentFactory.RegisterSingletonContent<ContentHandlerWithClass<TPayloadBase, TPayloadBase, THandler, T1>>(
                ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type for which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentHandler<TPayloadBase, THandler, T1, T2>(this ContentFactory contentFactory, string contentType, string handlerClass)
            where TPayloadBase : notnull
            where THandler : class, IContentHandler<TPayloadBase, T1, T2>
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (contentType is null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            contentFactory.Services.AddSingleton<THandler>();
            contentFactory.RegisterSingletonContent<ContentHandlerWithClass<TPayloadBase, TPayloadBase, THandler, T1, T2>>(
                ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <typeparam name="T3">The type of the third parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type for which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentHandler<TPayloadBase, THandler, T1, T2, T3>(this ContentFactory contentFactory, string contentType, string handlerClass)
            where TPayloadBase : notnull
            where THandler : class, IContentHandler<TPayloadBase, T1, T2, T3>
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (contentType is null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            contentFactory.Services.AddSingleton<THandler>();
            contentFactory.RegisterSingletonContent<ContentHandlerWithClass<TPayloadBase, TPayloadBase, THandler, T1, T2, T3>>(
                ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <param name="handlerFactory">A factory function to create the singleton handler.</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentHandler<TPayloadBase, TPayload, THandler>(this ContentFactory contentFactory, string handlerClass, Func<IServiceProvider, THandler> handlerFactory)
            where TPayloadBase : notnull
            where TPayload : TPayloadBase
            where THandler : class, IContentHandler<TPayload>
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            if (handlerFactory is null)
            {
                throw new ArgumentNullException(nameof(handlerFactory));
            }

            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.Services.AddSingleton(handlerFactory);
            contentFactory.RegisterSingletonContent<ContentHandlerWithClass<TPayloadBase, TPayload, THandler>>(
                ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <param name="handlerFactory">A factory function to create the singleton handler.</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentHandler<TPayloadBase, TPayload, THandler, T1>(this ContentFactory contentFactory, string handlerClass, Func<IServiceProvider, THandler> handlerFactory)
            where TPayloadBase : notnull
            where TPayload : TPayloadBase
            where THandler : class, IContentHandler<TPayload, T1>
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            if (handlerFactory is null)
            {
                throw new ArgumentNullException(nameof(handlerFactory));
            }

            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.Services.AddSingleton(handlerFactory);
            contentFactory.RegisterSingletonContent<ContentHandlerWithClass<TPayloadBase, TPayload, THandler, T1>>(
                ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <param name="handlerFactory">A factory function to create the singleton handler.</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentHandler<TPayloadBase, TPayload, THandler, T1, T2>(this ContentFactory contentFactory, string handlerClass, Func<IServiceProvider, THandler> handlerFactory)
            where TPayloadBase : notnull
            where TPayload : TPayloadBase
            where THandler : class, IContentHandler<TPayload, T1, T2>
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            if (handlerFactory is null)
            {
                throw new ArgumentNullException(nameof(handlerFactory));
            }

            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.Services.AddSingleton(handlerFactory);
            contentFactory.RegisterSingletonContent<ContentHandlerWithClass<TPayloadBase, TPayload, THandler, T1, T2>>(
                ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <typeparam name="T3">The type of the third parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <param name="handlerFactory">A factory function to create the singleton handler.</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentHandler<TPayloadBase, TPayload, THandler, T1, T2, T3>(this ContentFactory contentFactory, string handlerClass, Func<IServiceProvider, THandler> handlerFactory)
            where TPayloadBase : notnull
            where TPayload : TPayloadBase
            where THandler : class, IContentHandler<TPayload, T1, T2, T3>
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            if (handlerFactory is null)
            {
                throw new ArgumentNullException(nameof(handlerFactory));
            }

            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.Services.AddSingleton(handlerFactory);
            contentFactory.RegisterSingletonContent<ContentHandlerWithClass<TPayloadBase, TPayload, THandler, T1, T2, T3>>(
                ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type for which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <param name="handlerFactory">A factory function to create the singleton handler.</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentHandler<TPayloadBase, THandler>(this ContentFactory contentFactory, string contentType, string handlerClass, Func<IServiceProvider, THandler> handlerFactory)
            where TPayloadBase : notnull
            where THandler : class, IContentHandler<TPayloadBase>
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (contentType is null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            if (handlerFactory is null)
            {
                throw new ArgumentNullException(nameof(handlerFactory));
            }

            contentFactory.Services.AddSingleton(handlerFactory);
            contentFactory.RegisterSingletonContent<ContentHandlerWithClass<TPayloadBase, TPayloadBase, THandler>>(
                ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type for which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <param name="handlerFactory">A factory function to create the singleton handler.</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentHandler<TPayloadBase, THandler, T1>(this ContentFactory contentFactory, string contentType, string handlerClass, Func<IServiceProvider, THandler> handlerFactory)
            where TPayloadBase : notnull
            where THandler : class, IContentHandler<TPayloadBase, T1>
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (contentType is null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            if (handlerFactory is null)
            {
                throw new ArgumentNullException(nameof(handlerFactory));
            }

            contentFactory.Services.AddSingleton(handlerFactory);
            contentFactory.RegisterSingletonContent<ContentHandlerWithClass<TPayloadBase, TPayloadBase, THandler, T1>>(
                ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));
            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type for which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <param name="handlerFactory">A factory function to create the singleton handler.</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentHandler<TPayloadBase, THandler, T1, T2>(this ContentFactory contentFactory, string contentType, string handlerClass, Func<IServiceProvider, THandler> handlerFactory)
            where TPayloadBase : notnull
            where THandler : class, IContentHandler<TPayloadBase, T1, T2>
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (contentType is null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            if (handlerFactory is null)
            {
                throw new ArgumentNullException(nameof(handlerFactory));
            }

            contentFactory.Services.AddSingleton(handlerFactory);
            contentFactory.RegisterSingletonContent<ContentHandlerWithClass<TPayloadBase, TPayloadBase, THandler, T1, T2>>(
                ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <typeparam name="T3">The type of the third parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type for which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <param name="handlerFactory">A factory function to create the singleton handler.</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentHandler<TPayloadBase, THandler, T1, T2, T3>(this ContentFactory contentFactory, string contentType, string handlerClass, Func<IServiceProvider, THandler> handlerFactory)
            where TPayloadBase : notnull
            where THandler : class, IContentHandler<TPayloadBase, T1, T2, T3>
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (contentType is null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            if (handlerFactory is null)
            {
                throw new ArgumentNullException(nameof(handlerFactory));
            }

            contentFactory.Services.AddSingleton(handlerFactory);
            contentFactory.RegisterSingletonContent<ContentHandlerWithClass<TPayloadBase, TPayloadBase, THandler, T1, T2, T3>>(
                ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handle">The function with which to handle the payload.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentHandler<TPayloadBase, TPayload>(this ContentFactory contentFactory, Func<TPayload, Task> handle, string handlerClass)
            where TPayload : TPayloadBase
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (handle is null)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.RegisterAsyncContentHandlerWithAction<TPayloadBase, TPayload>(
                contentType,
                handle,
                handlerClass);

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handle">The function with which to handle the payload.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentHandler<TPayloadBase, TPayload, T1>(this ContentFactory contentFactory, Func<TPayload, T1, Task> handle, string handlerClass)
            where TPayload : TPayloadBase
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (handle is null)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.RegisterAsyncContentHandlerWithAction<TPayloadBase, TPayload, T1>(
                contentType,
                handle,
                handlerClass);
            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handle">The function with which to handle the payload.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentHandler<TPayloadBase, TPayload, T1, T2>(this ContentFactory contentFactory, Func<TPayload, T1, T2, Task> handle, string handlerClass)
            where TPayload : TPayloadBase
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (handle is null)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.RegisterAsyncContentHandlerWithAction<TPayloadBase, TPayload, T1, T2>(
                contentType,
                handle,
                handlerClass);

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <typeparam name="T3">The type of the third parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handle">The function with which to handle the payload.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentHandler<TPayloadBase, TPayload, T1, T2, T3>(this ContentFactory contentFactory, Func<TPayload, T1, T2, T3, Task> handle, string handlerClass)
            where TPayload : TPayloadBase
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (handle is null)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.RegisterAsyncContentHandlerWithAction<TPayloadBase, TPayload, T1, T2, T3>(
                contentType,
                handle,
                handlerClass);

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type for which to register the handler.</param>
        /// <param name="handle">The function with which to handle the payload.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentHandler<TPayloadBase>(this ContentFactory contentFactory, string contentType, Func<TPayloadBase, Task> handle, string handlerClass)
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (contentType is null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (handle is null)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            contentFactory.RegisterAsyncContentHandlerWithAction<TPayloadBase, TPayloadBase>(
                contentType,
                handle,
                handlerClass);

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type for which to register the handler.</param>
        /// <param name="handle">The function with which to handle the payload.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentHandler<TPayloadBase, T1>(this ContentFactory contentFactory, string contentType, Func<TPayloadBase, T1, Task> handle, string handlerClass)
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (contentType is null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (handle is null)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            contentFactory.RegisterAsyncContentHandlerWithAction<TPayloadBase, TPayloadBase, T1>(
                contentType,
                handle,
                handlerClass);

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type for which to register the handler.</param>
        /// <param name="handle">The function with which to handle the payload.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentHandler<TPayloadBase, T1, T2>(this ContentFactory contentFactory, string contentType, Func<TPayloadBase, T1, T2, Task> handle, string handlerClass)
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (contentType is null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (handle is null)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            contentFactory.RegisterAsyncContentHandlerWithAction<TPayloadBase, TPayloadBase, T1, T2>(
                contentType,
                handle,
                handlerClass);

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <typeparam name="T3">The type of the third parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type for which to register the handler.</param>
        /// <param name="handle">The function with which to handle the payload.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentHandler<TPayloadBase, T1, T2, T3>(this ContentFactory contentFactory, string contentType, Func<TPayloadBase, T1, T2, T3, Task> handle, string handlerClass)
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (contentType is null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (handle is null)
            {
                throw new ArgumentNullException(nameof(handle));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            contentFactory.RegisterAsyncContentHandlerWithAction<TPayloadBase, TPayloadBase, T1, T2, T3>(
                contentType,
                handle,
                handlerClass);

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentHandler<TPayloadBase, TPayload, THandler>(this ContentFactory contentFactory, string handlerClass)
            where TPayloadBase : notnull
            where TPayload : TPayloadBase
            where THandler : class, IAsyncContentHandler<TPayload>
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.Services.AddSingleton<THandler>();
            contentFactory.RegisterSingletonContent<AsyncContentHandlerWithClass<TPayloadBase, TPayload, THandler>>(
                ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentHandler<TPayloadBase, TPayload, THandler, T1>(this ContentFactory contentFactory, string handlerClass)
            where TPayloadBase : notnull
            where TPayload : TPayloadBase
            where THandler : class, IAsyncContentHandler<TPayload, T1>
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.Services.AddSingleton<THandler>();
            contentFactory.RegisterSingletonContent<AsyncContentHandlerWithClass<TPayloadBase, TPayload, THandler, T1>>(
                ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));
            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentHandler<TPayloadBase, TPayload, THandler, T1, T2>(this ContentFactory contentFactory, string handlerClass)
            where TPayloadBase : notnull
            where TPayload : TPayloadBase
            where THandler : class, IAsyncContentHandler<TPayload, T1, T2>
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.Services.AddSingleton<THandler>();
            contentFactory.RegisterSingletonContent<AsyncContentHandlerWithClass<TPayloadBase, TPayload, THandler, T1, T2>>(
                ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));
            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <typeparam name="T3">The type of the third parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentHandler<TPayloadBase, TPayload, THandler, T1, T2, T3>(this ContentFactory contentFactory, string handlerClass)
            where TPayloadBase : notnull
            where TPayload : TPayloadBase
            where THandler : class, IAsyncContentHandler<TPayload, T1, T2, T3>
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.Services.AddSingleton<THandler>();
            contentFactory.RegisterSingletonContent<AsyncContentHandlerWithClass<TPayloadBase, TPayload, THandler, T1, T2, T3>>(
                ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));
            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type for which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentHandler<TPayloadBase, THandler>(this ContentFactory contentFactory, string contentType, string handlerClass)
            where TPayloadBase : notnull
            where THandler : class, IAsyncContentHandler<TPayloadBase>
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (contentType is null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            contentFactory.Services.AddSingleton<THandler>();
            contentFactory.RegisterSingletonContent<AsyncContentHandlerWithClass<TPayloadBase, TPayloadBase, THandler>>(
                ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));
            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type for which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentHandler<TPayloadBase, THandler, T1>(this ContentFactory contentFactory, string contentType, string handlerClass)
            where TPayloadBase : notnull
            where THandler : class, IAsyncContentHandler<TPayloadBase, T1>
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (contentType is null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            contentFactory.Services.AddSingleton<THandler>();
            contentFactory.RegisterSingletonContent<AsyncContentHandlerWithClass<TPayloadBase, TPayloadBase, THandler, T1>>(
                ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));
            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type for which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentHandler<TPayloadBase, THandler, T1, T2>(this ContentFactory contentFactory, string contentType, string handlerClass)
            where TPayloadBase : notnull
            where THandler : class, IAsyncContentHandler<TPayloadBase, T1, T2>
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (contentType is null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            contentFactory.Services.AddSingleton<THandler>();
            contentFactory.RegisterSingletonContent<AsyncContentHandlerWithClass<TPayloadBase, TPayloadBase, THandler, T1, T2>>(
                ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));
            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <typeparam name="T3">The type of the third parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type for which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentHandler<TPayloadBase, THandler, T1, T2, T3>(this ContentFactory contentFactory, string contentType, string handlerClass)
            where TPayloadBase : notnull
            where THandler : class, IAsyncContentHandler<TPayloadBase, T1, T2, T3>
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (contentType is null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            contentFactory.Services.AddSingleton<THandler>();
            contentFactory.RegisterSingletonContent<AsyncContentHandlerWithClass<TPayloadBase, TPayloadBase, THandler, T1, T2, T3>>(
                ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));
            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <param name="handlerFactory">A factory function to create the singleton handler.</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentHandler<TPayloadBase, TPayload, THandler>(this ContentFactory contentFactory, string handlerClass, Func<IServiceProvider, THandler> handlerFactory)
            where TPayloadBase : notnull
            where TPayload : TPayloadBase
            where THandler : class, IAsyncContentHandler<TPayload>
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            if (handlerFactory is null)
            {
                throw new ArgumentNullException(nameof(handlerFactory));
            }

            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.Services.AddSingleton(handlerFactory);
            contentFactory.RegisterSingletonContent<AsyncContentHandlerWithClass<TPayloadBase, TPayload, THandler>>(
                ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));
            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <param name="handlerFactory">A factory function to create the singleton handler.</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentHandler<TPayloadBase, TPayload, THandler, T1>(this ContentFactory contentFactory, string handlerClass, Func<IServiceProvider, THandler> handlerFactory)
            where TPayloadBase : notnull
            where TPayload : TPayloadBase
            where THandler : class, IAsyncContentHandler<TPayload, T1>
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            if (handlerFactory is null)
            {
                throw new ArgumentNullException(nameof(handlerFactory));
            }

            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.Services.AddSingleton(handlerFactory);
            contentFactory.RegisterSingletonContent<AsyncContentHandlerWithClass<TPayloadBase, TPayload, THandler, T1>>(
                ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <param name="handlerFactory">A factory function to create the singleton handler.</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentHandler<TPayloadBase, TPayload, THandler, T1, T2>(this ContentFactory contentFactory, string handlerClass, Func<IServiceProvider, THandler> handlerFactory)
            where TPayloadBase : notnull
            where TPayload : TPayloadBase
            where THandler : class, IAsyncContentHandler<TPayload, T1, T2>
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            if (handlerFactory is null)
            {
                throw new ArgumentNullException(nameof(handlerFactory));
            }

            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.Services.AddSingleton(handlerFactory);
            contentFactory.RegisterSingletonContent<AsyncContentHandlerWithClass<TPayloadBase, TPayload, THandler, T1, T2>>(
                ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <typeparam name="T3">The type of the third parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <param name="handlerFactory">A factory function to create the singleton handler.</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentHandler<TPayloadBase, TPayload, THandler, T1, T2, T3>(this ContentFactory contentFactory, string handlerClass, Func<IServiceProvider, THandler> handlerFactory)
            where TPayloadBase : notnull
            where TPayload : TPayloadBase
            where THandler : class, IAsyncContentHandler<TPayload, T1, T2, T3>
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            if (handlerFactory is null)
            {
                throw new ArgumentNullException(nameof(handlerFactory));
            }

            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.Services.AddSingleton(handlerFactory);
            contentFactory.RegisterSingletonContent<AsyncContentHandlerWithClass<TPayloadBase, TPayload, THandler, T1, T2, T3>>(
                ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type for which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <param name="handlerFactory">A factory function to create the singleton handler.</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentHandler<TPayloadBase, THandler>(this ContentFactory contentFactory, string contentType, string handlerClass, Func<IServiceProvider, THandler> handlerFactory)
            where TPayloadBase : notnull
            where THandler : class, IAsyncContentHandler<TPayloadBase>
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (contentType is null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            if (handlerFactory is null)
            {
                throw new ArgumentNullException(nameof(handlerFactory));
            }

            contentFactory.Services.AddSingleton(handlerFactory);
            contentFactory.RegisterSingletonContent<AsyncContentHandlerWithClass<TPayloadBase, TPayloadBase, THandler>>(
                ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type for which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <param name="handlerFactory">A factory function to create the singleton handler.</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentHandler<TPayloadBase, THandler, T1>(this ContentFactory contentFactory, string contentType, string handlerClass, Func<IServiceProvider, THandler> handlerFactory)
            where TPayloadBase : notnull
            where THandler : class, IAsyncContentHandler<TPayloadBase, T1>
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (contentType is null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            if (handlerFactory is null)
            {
                throw new ArgumentNullException(nameof(handlerFactory));
            }

            contentFactory.Services.AddSingleton(handlerFactory);
            contentFactory.RegisterSingletonContent<AsyncContentHandlerWithClass<TPayloadBase, TPayloadBase, THandler, T1>>(
                ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type for which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <param name="handlerFactory">A factory function to create the singleton handler.</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentHandler<TPayloadBase, THandler, T1, T2>(this ContentFactory contentFactory, string contentType, string handlerClass, Func<IServiceProvider, THandler> handlerFactory)
            where TPayloadBase : notnull
            where THandler : class, IAsyncContentHandler<TPayloadBase, T1, T2>
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (contentType is null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            if (handlerFactory is null)
            {
                throw new ArgumentNullException(nameof(handlerFactory));
            }

            contentFactory.Services.AddSingleton(handlerFactory);
            contentFactory.RegisterSingletonContent<AsyncContentHandlerWithClass<TPayloadBase, TPayloadBase, THandler, T1, T2>>(
                ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <typeparam name="T3">The type of the third parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type for which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <param name="handlerFactory">A factory function to create the singleton handler.</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentHandler<TPayloadBase, THandler, T1, T2, T3>(this ContentFactory contentFactory, string contentType, string handlerClass, Func<IServiceProvider, THandler> handlerFactory)
            where TPayloadBase : notnull
            where THandler : class, IAsyncContentHandler<TPayloadBase, T1, T2, T3>
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (contentType is null)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            if (handlerFactory is null)
            {
                throw new ArgumentNullException(nameof(handlerFactory));
            }

            contentFactory.Services.AddSingleton(handlerFactory);
            contentFactory.RegisterSingletonContent<AsyncContentHandlerWithClass<TPayloadBase, TPayloadBase, THandler, T1, T2, T3>>(
                ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));

            return contentFactory;
        }
    }
}