// <copyright file="ContentEnvelopeContentFactoryExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling
{
    using System;
    using System.Threading.Tasks;
    using Corvus.ContentHandling.Json;
    using Corvus.ContentHandling.Json.Internal.ContentEnvelopeHandler;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Extensions to register handlers for a <see cref="ContentEnvelope"/>.
    /// </summary>
    public static class ContentEnvelopeContentFactoryExtensions
    {
        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handle">The function with which to handle the payload.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentEnvelopeHandler<TPayload>(this ContentFactory contentFactory, Action<TPayload> handle, string handlerClass)
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
            contentFactory.RegisterContentHandler<ContentEnvelope>(
                contentType,
                envelope => handle(envelope.GetContents<TPayload>()),
                handlerClass);
            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handle">The function with which to handle the payload.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentEnvelopeHandler<TPayload, T1>(this ContentFactory contentFactory, Action<TPayload, T1> handle, string handlerClass)
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
            contentFactory.RegisterContentHandler<ContentEnvelope, T1>(
                contentType,
                (envelope, param1) => handle(envelope.GetContents<TPayload>(), param1),
                handlerClass);

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handle">The function with which to handle the payload.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentEnvelopeHandler<TPayload, T1, T2>(this ContentFactory contentFactory, Action<TPayload, T1, T2> handle, string handlerClass)
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
            contentFactory.RegisterContentHandler<ContentEnvelope, T1, T2>(
                contentType,
                (envelope, param1, param2) => handle(envelope.GetContents<TPayload>(), param1, param2),
                handlerClass);

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <typeparam name="T3">The type of the third parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handle">The function with which to handle the payload.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentEnvelopeHandler<TPayload, T1, T2, T3>(this ContentFactory contentFactory, Action<TPayload, T1, T2, T3> handle, string handlerClass)
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
            contentFactory.RegisterContentHandler<ContentEnvelope, T1, T2, T3>(
                contentType,
                (envelope, param1, param2, param3) => handle(envelope.GetContents<TPayload>(), param1, param2, param3),
                handlerClass);

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentEnvelopeHandler<TPayload, THandler>(this ContentFactory contentFactory, string handlerClass)
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
            contentFactory.RegisterContentHandler<ContentEnvelope, ContentEnvelopeHandlerWithClass<TPayload, THandler>>(contentType, handlerClass);
            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentEnvelopeHandler<TPayload, THandler, T1>(this ContentFactory contentFactory, string handlerClass)
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
            contentFactory.RegisterContentHandler<ContentEnvelope, ContentEnvelopeHandlerWithClass<TPayload, THandler, T1>, T1>(contentType, handlerClass);

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentEnvelopeHandler<TPayload, THandler, T1, T2>(this ContentFactory contentFactory, string handlerClass)
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
            contentFactory.RegisterContentHandler<ContentEnvelope, ContentEnvelopeHandlerWithClass<TPayload, THandler, T1, T2>, T1, T2>(contentType, handlerClass);

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <typeparam name="T3">The type of the third parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentEnvelopeHandler<TPayload, THandler, T1, T2, T3>(this ContentFactory contentFactory, string handlerClass)
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
            contentFactory.RegisterContentHandler<ContentEnvelope, ContentEnvelopeHandlerWithClass<TPayload, THandler, T1, T2, T3>, T1, T2, T3>(contentType, handlerClass);

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <param name="handlerFactory">A factory function to create the singleton handler.</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentEnvelopeHandler<TPayload, THandler>(this ContentFactory contentFactory, string handlerClass, Func<IServiceProvider, THandler> handlerFactory)
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
            contentFactory.RegisterContentHandler<ContentEnvelope, ContentEnvelopeHandlerWithClass<TPayload, THandler>>(contentType, handlerClass);

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <param name="handlerFactory">A factory function to create the singleton handler.</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentEnvelopeHandler<TPayload, THandler, T1>(this ContentFactory contentFactory, string handlerClass, Func<IServiceProvider, THandler> handlerFactory)
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
            contentFactory.RegisterContentHandler<ContentEnvelope, ContentEnvelopeHandlerWithClass<TPayload, THandler, T1>, T1>(contentType, handlerClass);
            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <param name="handlerFactory">A factory function to create the singleton handler.</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentEnvelopeHandler<TPayload, THandler, T1, T2>(this ContentFactory contentFactory, string handlerClass, Func<IServiceProvider, THandler> handlerFactory)
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
            contentFactory.RegisterContentHandler<ContentEnvelope, ContentEnvelopeHandlerWithClass<TPayload, THandler, T1, T2>, T1, T2>(contentType, handlerClass);
            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <typeparam name="T3">The type of the third parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <param name="handlerFactory">A factory function to create the singleton handler.</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentEnvelopeHandler<TPayload, THandler, T1, T2, T3>(this ContentFactory contentFactory, string handlerClass, Func<IServiceProvider, THandler> handlerFactory)
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
            contentFactory.RegisterContentHandler<ContentEnvelope, ContentEnvelopeHandlerWithClass<TPayload, THandler, T1, T2, T3>, T1, T2, T3>(contentType, handlerClass);

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handle">The function with which to handle the payload.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentEnvelopeHandler<TPayload>(this ContentFactory contentFactory, Func<TPayload, Task> handle, string handlerClass)
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
            contentFactory.RegisterAsyncContentHandler<ContentEnvelope>(
                contentType,
                envelope => handle(envelope.GetContents<TPayload>()),
                handlerClass);

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handle">The function with which to handle the payload.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentEnvelopeHandler<TPayload, T1>(this ContentFactory contentFactory, Func<TPayload, T1, Task> handle, string handlerClass)
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
            contentFactory.RegisterAsyncContentHandler<ContentEnvelope, T1>(
                contentType,
                (envelope, param1) => handle(envelope.GetContents<TPayload>(), param1),
                handlerClass);
            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handle">The function with which to handle the payload.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentEnvelopeHandler<TPayload, T1, T2>(this ContentFactory contentFactory, Func<TPayload, T1, T2, Task> handle, string handlerClass)
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
            contentFactory.RegisterAsyncContentHandler<ContentEnvelope, T1, T2>(
                contentType,
                (envelope, param1, param2) => handle(envelope.GetContents<TPayload>(), param1, param2),
                handlerClass);

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <typeparam name="T3">The type of the third parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handle">The function with which to handle the payload.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentEnvelopeHandler<TPayload, T1, T2, T3>(this ContentFactory contentFactory, Func<TPayload, T1, T2, T3, Task> handle, string handlerClass)
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
            contentFactory.RegisterAsyncContentHandler<ContentEnvelope, T1, T2, T3>(
                contentType,
                (envelope, param1, param2, param3) => handle(envelope.GetContents<TPayload>(), param1, param2, param3),
                handlerClass);

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentEnvelopeHandler<TPayload, THandler>(this ContentFactory contentFactory, string handlerClass)
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
            contentFactory.RegisterAsyncContentHandler<ContentEnvelope, AsyncContentEnvelopeHandlerWithClass<TPayload, THandler>>(contentType, handlerClass);

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentEnvelopeHandler<TPayload, THandler, T1>(this ContentFactory contentFactory, string handlerClass)
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
            contentFactory.RegisterAsyncContentHandler<ContentEnvelope, AsyncContentEnvelopeHandlerWithClass<TPayload, THandler, T1>, T1>(contentType, handlerClass);
            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentEnvelopeHandler<TPayload, THandler, T1, T2>(this ContentFactory contentFactory, string handlerClass)
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
            contentFactory.RegisterAsyncContentHandler<ContentEnvelope, AsyncContentEnvelopeHandlerWithClass<TPayload, THandler, T1, T2>, T1, T2>(contentType, handlerClass);
            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <typeparam name="T3">The type of the third parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentEnvelopeHandler<TPayload, THandler, T1, T2, T3>(this ContentFactory contentFactory, string handlerClass)
            where THandler : class, IAsyncContentHandler<TPayload, T1, T2, T3>
        {
            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.Services.AddSingleton<THandler>();
            contentFactory.RegisterAsyncContentHandler<ContentEnvelope, AsyncContentEnvelopeHandlerWithClass<TPayload, THandler, T1, T2, T3>, T1, T2, T3>(contentType, handlerClass);
            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <param name="handlerFactory">A factory function to create the singleton handler.</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentEnvelopeHandler<TPayload, THandler>(this ContentFactory contentFactory, string handlerClass, Func<IServiceProvider, THandler> handlerFactory)
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
            contentFactory.RegisterAsyncContentHandler<ContentEnvelope, AsyncContentEnvelopeHandlerWithClass<TPayload, THandler>>(contentType, handlerClass);
            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <param name="handlerFactory">A factory function to create the singleton handler.</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentEnvelopeHandler<TPayload, THandler, T1>(this ContentFactory contentFactory, string handlerClass, Func<IServiceProvider, THandler> handlerFactory)
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
            contentFactory.RegisterAsyncContentHandler<ContentEnvelope, AsyncContentEnvelopeHandlerWithClass<TPayload, THandler, T1>, T1>(contentType, handlerClass);

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <param name="handlerFactory">A factory function to create the singleton handler.</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentEnvelopeHandler<TPayload, THandler, T1, T2>(this ContentFactory contentFactory, string handlerClass, Func<IServiceProvider, THandler> handlerFactory)
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
            contentFactory.RegisterAsyncContentHandler<ContentEnvelope, AsyncContentEnvelopeHandlerWithClass<TPayload, THandler, T1, T2>, T1, T2>(contentType, handlerClass);

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <typeparam name="T3">The type of the third parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <param name="handlerFactory">A factory function to create the singleton handler.</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentEnvelopeHandler<TPayload, THandler, T1, T2, T3>(this ContentFactory contentFactory, string handlerClass, Func<IServiceProvider, THandler> handlerFactory)
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
            contentFactory.RegisterAsyncContentHandler<ContentEnvelope, AsyncContentEnvelopeHandlerWithClass<TPayload, THandler, T1, T2, T3>, T1, T2, T3>(contentType, handlerClass);

            return contentFactory;
        }
    }
}