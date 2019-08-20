// <copyright file="ContentEnvelopeWithResultServiceCollectionExtensions.cs" company="Endjin Limited">
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
    public static class ContentEnvelopeWithResultServiceCollectionExtensions
    {
        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The service collection with which to register the handler.</param>
        /// <param name="handle">The function with which to handle the payload.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The service collection.</returns>
        public static ContentFactory RegisterContentEnvelopeHandlerWithResult<TPayload, TResult>(this ContentFactory contentFactory, Func<TPayload, TResult> handle, string handlerClass)
        {
            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.RegisterContentHandlerWithResult<ContentEnvelope, TResult>(
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
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The service collection with which to register the handler.</param>
        /// <param name="handle">The function with which to handle the payload.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The service collection.</returns>
        public static ContentFactory RegisterContentEnvelopeHandlerWithResult<TPayload, T1, TResult>(this ContentFactory contentFactory, Func<TPayload, T1, TResult> handle, string handlerClass)
        {
            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.RegisterContentHandlerWithResult<ContentEnvelope, T1, TResult>(
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
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The service collection with which to register the handler.</param>
        /// <param name="handle">The function with which to handle the payload.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The service collection.</returns>
        public static ContentFactory RegisterContentEnvelopeHandlerWithResult<TPayload, T1, T2, TResult>(this ContentFactory contentFactory, Func<TPayload, T1, T2, TResult> handle, string handlerClass)
        {
            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.RegisterContentHandlerWithResult<ContentEnvelope, T1, T2, TResult>(
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
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The service collection with which to register the handler.</param>
        /// <param name="handle">The function with which to handle the payload.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The service collection.</returns>
        public static ContentFactory RegisterContentEnvelopeHandlerWithResult<TPayload, T1, T2, T3, TResult>(this ContentFactory contentFactory, Func<TPayload, T1, T2, T3, TResult> handle, string handlerClass)
        {
            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.RegisterContentHandlerWithResult<ContentEnvelope, T1, T2, T3, TResult>(
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
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The service collection with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The service collection.</returns>
        public static ContentFactory RegisterContentEnvelopeHandlerWithResult<TPayload, THandler, TResult>(this ContentFactory contentFactory, string handlerClass)
            where THandler : class, IContentHandlerWithResult<TPayload, TResult>
        {
            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.Services.AddSingleton<THandler>();
            contentFactory.RegisterContentHandlerWithResult<ContentEnvelope, ContentEnvelopeHandlerWithResultAndClass<TPayload, THandler, TResult>, TResult>(contentType, handlerClass);
            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The service collection with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The service collection.</returns>
        public static ContentFactory RegisterContentEnvelopeHandlerWithResult<TPayload, THandler, T1, TResult>(this ContentFactory contentFactory, string handlerClass)
            where THandler : class, IContentHandlerWithResult<TPayload, T1, TResult>
        {
            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.Services.AddSingleton<THandler>();
            contentFactory.RegisterContentHandlerWithResult<ContentEnvelope, ContentEnvelopeHandlerWithResultAndClass<TPayload, THandler, T1, TResult>, T1, TResult>(contentType, handlerClass);

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The service collection with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The service collection.</returns>
        public static ContentFactory RegisterContentEnvelopeHandlerWithResult<TPayload, THandler, T1, T2, TResult>(this ContentFactory contentFactory, string handlerClass)
            where THandler : class, IContentHandlerWithResult<TPayload, T1, T2, TResult>
        {
            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.Services.AddSingleton<THandler>();
            contentFactory.RegisterContentHandlerWithResult<ContentEnvelope, ContentEnvelopeHandlerWithResultAndClass<TPayload, THandler, T1, T2, TResult>, T1, T2, TResult>(contentType, handlerClass);

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
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The service collection with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The service collection.</returns>
        public static ContentFactory RegisterContentEnvelopeHandlerWithResult<TPayload, THandler, T1, T2, T3, TResult>(this ContentFactory contentFactory, string handlerClass)
            where THandler : class, IContentHandlerWithResult<TPayload, T1, T2, T3, TResult>
        {
            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.Services.AddSingleton<THandler>();
            contentFactory.RegisterContentHandlerWithResult<ContentEnvelope, ContentEnvelopeHandlerWithResultAndClass<TPayload, THandler, T1, T2, T3, TResult>, T1, T2, T3, TResult>(contentType, handlerClass);

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The service collection with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <param name="handlerFactory">A factory function to create the singleton handler.</param>
        /// <returns>The service collection.</returns>
        public static ContentFactory RegisterContentEnvelopeHandlerWithResult<TPayload, THandler, TResult>(this ContentFactory contentFactory, string handlerClass, Func<IServiceProvider, THandler> handlerFactory)
            where THandler : class, IContentHandlerWithResult<TPayload, TResult>
        {
            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.Services.AddSingleton(handlerFactory);
            contentFactory.RegisterContentHandlerWithResult<ContentEnvelope, ContentEnvelopeHandlerWithResultAndClass<TPayload, THandler, TResult>, TResult>(contentType, handlerClass);

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The service collection with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <param name="handlerFactory">A factory function to create the singleton handler.</param>
        /// <returns>The service collection.</returns>
        public static ContentFactory RegisterContentEnvelopeHandlerWithResult<TPayload, THandler, T1, TResult>(this ContentFactory contentFactory, string handlerClass, Func<IServiceProvider, THandler> handlerFactory)
            where THandler : class, IContentHandlerWithResult<TPayload, T1, TResult>
        {
            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.Services.AddSingleton(handlerFactory);
            contentFactory.RegisterContentHandlerWithResult<ContentEnvelope, ContentEnvelopeHandlerWithResultAndClass<TPayload, THandler, T1, TResult>, T1, TResult>(contentType, handlerClass);
            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The service collection with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <param name="handlerFactory">A factory function to create the singleton handler.</param>
        /// <returns>The service collection.</returns>
        public static ContentFactory RegisterContentEnvelopeHandlerWithResult<TPayload, THandler, T1, T2, TResult>(this ContentFactory contentFactory, string handlerClass, Func<IServiceProvider, THandler> handlerFactory)
            where THandler : class, IContentHandlerWithResult<TPayload, T1, T2, TResult>
        {
            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.Services.AddSingleton(handlerFactory);
            contentFactory.RegisterContentHandlerWithResult<ContentEnvelope, ContentEnvelopeHandlerWithResultAndClass<TPayload, THandler, T1, T2, TResult>, T1, T2, TResult>(contentType, handlerClass);
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
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The service collection with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <param name="handlerFactory">A factory function to create the singleton handler.</param>
        /// <returns>The service collection.</returns>
        public static ContentFactory RegisterContentEnvelopeHandlerWithResult<TPayload, THandler, T1, T2, T3, TResult>(this ContentFactory contentFactory, string handlerClass, Func<IServiceProvider, THandler> handlerFactory)
            where THandler : class, IContentHandlerWithResult<TPayload, T1, T2, T3, TResult>
        {
            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.Services.AddSingleton(handlerFactory);
            contentFactory.RegisterContentHandlerWithResult<ContentEnvelope, ContentEnvelopeHandlerWithResultAndClass<TPayload, THandler, T1, T2, T3, TResult>, T1, T2, T3, TResult>(contentType, handlerClass);

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The service collection with which to register the handler.</param>
        /// <param name="handle">The function with which to handle the payload.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The service collection.</returns>
        public static ContentFactory RegisterAsyncContentEnvelopeHandlerWithResult<TPayload, TResult>(this ContentFactory contentFactory, Func<TPayload, Task<TResult>> handle, string handlerClass)
        {
            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.RegisterAsyncContentHandlerWithResult<ContentEnvelope, TResult>(
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
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The service collection with which to register the handler.</param>
        /// <param name="handle">The function with which to handle the payload.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The service collection.</returns>
        public static ContentFactory RegisterAsyncContentEnvelopeHandlerWithResult<TPayload, T1, TResult>(this ContentFactory contentFactory, Func<TPayload, T1, Task<TResult>> handle, string handlerClass)
        {
            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.RegisterAsyncContentHandlerWithResult<ContentEnvelope, T1, TResult>(
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
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The service collection with which to register the handler.</param>
        /// <param name="handle">The function with which to handle the payload.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The service collection.</returns>
        public static ContentFactory RegisterAsyncContentEnvelopeHandlerWithResult<TPayload, T1, T2, TResult>(this ContentFactory contentFactory, Func<TPayload, T1, T2, Task<TResult>> handle, string handlerClass)
        {
            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.RegisterAsyncContentHandlerWithResult<ContentEnvelope, T1, T2, TResult>(
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
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The service collection with which to register the handler.</param>
        /// <param name="handle">The function with which to handle the payload.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The service collection.</returns>
        public static ContentFactory RegisterAsyncContentEnvelopeHandlerWithResult<TPayload, T1, T2, T3, TResult>(this ContentFactory contentFactory, Func<TPayload, T1, T2, T3, Task<TResult>> handle, string handlerClass)
        {
            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.RegisterAsyncContentHandlerWithResult<ContentEnvelope, T1, T2, T3, TResult>(
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
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The service collection with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The service collection.</returns>
        public static ContentFactory RegisterAsyncContentEnvelopeHandlerWithResult<TPayload, THandler, TResult>(this ContentFactory contentFactory, string handlerClass)
            where THandler : class, IAsyncContentHandlerWithResult<TPayload, TResult>
        {
            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.Services.AddSingleton<THandler>();
            contentFactory.RegisterAsyncContentHandlerWithResult<ContentEnvelope, AsyncContentEnvelopeHandlerWithResultAndClass<TPayload, THandler, TResult>, TResult>(contentType, handlerClass);

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The service collection with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The service collection.</returns>
        public static ContentFactory RegisterAsyncContentEnvelopeHandlerWithResult<TPayload, THandler, T1, TResult>(this ContentFactory contentFactory, string handlerClass)
            where THandler : class, IAsyncContentHandlerWithResult<TPayload, T1, TResult>
        {
            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.Services.AddSingleton<THandler>();
            contentFactory.RegisterAsyncContentHandlerWithResult<ContentEnvelope, AsyncContentEnvelopeHandlerWithResultAndClass<TPayload, THandler, T1, TResult>, T1, TResult>(contentType, handlerClass);
            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The service collection with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The service collection.</returns>
        public static ContentFactory RegisterAsyncContentEnvelopeHandlerWithResult<TPayload, THandler, T1, T2, TResult>(this ContentFactory contentFactory, string handlerClass)
            where THandler : class, IAsyncContentHandlerWithResult<TPayload, T1, T2, TResult>
        {
            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.Services.AddSingleton<THandler>();
            contentFactory.RegisterAsyncContentHandlerWithResult<ContentEnvelope, AsyncContentEnvelopeHandlerWithResultAndClass<TPayload, THandler, T1, T2, TResult>, T1, T2, TResult>(contentType, handlerClass);
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
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The service collection with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The service collection.</returns>
        public static ContentFactory RegisterAsyncContentEnvelopeHandlerWithResult<TPayload, THandler, T1, T2, T3, TResult>(this ContentFactory contentFactory, string handlerClass)
            where THandler : class, IAsyncContentHandlerWithResult<TPayload, T1, T2, T3, TResult>
        {
            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.Services.AddSingleton<THandler>();
            contentFactory.RegisterAsyncContentHandlerWithResult<ContentEnvelope, AsyncContentEnvelopeHandlerWithResultAndClass<TPayload, THandler, T1, T2, T3, TResult>, T1, T2, T3, TResult>(contentType, handlerClass);
            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The service collection with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <param name="handlerFactory">A factory function to create the singleton handler.</param>
        /// <returns>The service collection.</returns>
        public static ContentFactory RegisterAsyncContentEnvelopeHandlerWithResult<TPayload, THandler, TResult>(this ContentFactory contentFactory, string handlerClass, Func<IServiceProvider, THandler> handlerFactory)
            where THandler : class, IAsyncContentHandlerWithResult<TPayload, TResult>
        {
            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.Services.AddSingleton(handlerFactory);
            contentFactory.RegisterAsyncContentHandlerWithResult<ContentEnvelope, AsyncContentEnvelopeHandlerWithResultAndClass<TPayload, THandler, TResult>, TResult>(contentType, handlerClass);
            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The service collection with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <param name="handlerFactory">A factory function to create the singleton handler.</param>
        /// <returns>The service collection.</returns>
        public static ContentFactory RegisterAsyncContentEnvelopeHandlerWithResult<TPayload, THandler, T1, TResult>(this ContentFactory contentFactory, string handlerClass, Func<IServiceProvider, THandler> handlerFactory)
            where THandler : class, IAsyncContentHandlerWithResult<TPayload, T1, TResult>
        {
            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.Services.AddSingleton(handlerFactory);
            contentFactory.RegisterAsyncContentHandlerWithResult<ContentEnvelope, AsyncContentEnvelopeHandlerWithResultAndClass<TPayload, THandler, T1, TResult>, T1, TResult>(contentType, handlerClass);

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The service collection with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <param name="handlerFactory">A factory function to create the singleton handler.</param>
        /// <returns>The service collection.</returns>
        public static ContentFactory RegisterAsyncContentEnvelopeHandlerWithResult<TPayload, THandler, T1, T2, TResult>(this ContentFactory contentFactory, string handlerClass, Func<IServiceProvider, THandler> handlerFactory)
            where THandler : class, IAsyncContentHandlerWithResult<TPayload, T1, T2, TResult>
        {
            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.Services.AddSingleton(handlerFactory);
            contentFactory.RegisterAsyncContentHandlerWithResult<ContentEnvelope, AsyncContentEnvelopeHandlerWithResultAndClass<TPayload, THandler, T1, T2, TResult>, T1, T2, TResult>(contentType, handlerClass);

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
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The service collection with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <param name="handlerFactory">A factory function to create the singleton handler.</param>
        /// <returns>The service collection.</returns>
        public static ContentFactory RegisterAsyncContentEnvelopeHandlerWithResult<TPayload, THandler, T1, T2, T3, TResult>(this ContentFactory contentFactory, string handlerClass, Func<IServiceProvider, THandler> handlerFactory)
            where THandler : class, IAsyncContentHandlerWithResult<TPayload, T1, T2, T3, TResult>
        {
            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.Services.AddSingleton(handlerFactory);
            contentFactory.RegisterAsyncContentHandlerWithResult<ContentEnvelope, AsyncContentEnvelopeHandlerWithResultAndClass<TPayload, THandler, T1, T2, T3, TResult>, T1, T2, T3, TResult>(contentType, handlerClass);

            return contentFactory;
        }
    }
}
