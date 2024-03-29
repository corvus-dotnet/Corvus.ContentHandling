﻿// <copyright file="ContentHandlerWithResultContentFactoryExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling
{
    using System;
    using System.Threading.Tasks;
    using Corvus.ContentHandling.Internal;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Extensions to register handlers for a <see cref="IContentHandlerWithResult{TPayloadType, TResult}"/> with a particular payload.
    /// </summary>
    public static class ContentHandlerWithResultContentFactoryExtensions
    {
        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handle">The function with which to handle the payload.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentHandlerWithResult<TPayloadBase, TPayload, TResult>(this ContentFactory contentFactory, Func<TPayload, TResult> handle, string handlerClass)
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
            contentFactory.RegisterContentHandlerWithResultAndAction<TPayloadBase, TPayload, TResult>(
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
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handle">The function with which to handle the payload.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentHandlerWithResult<TPayloadBase, TPayload, T1, TResult>(this ContentFactory contentFactory, Func<TPayload, T1, TResult> handle, string handlerClass)
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
            contentFactory.RegisterContentHandlerWithResultAndAction<TPayloadBase, TPayload, T1, TResult>(
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
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handle">The function with which to handle the payload.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentHandlerWithResult<TPayloadBase, TPayload, T1, T2, TResult>(this ContentFactory contentFactory, Func<TPayload, T1, T2, TResult> handle, string handlerClass)
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
            contentFactory.RegisterContentHandlerWithResultAndAction<TPayloadBase, TPayload, T1, T2, TResult>(
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
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handle">The function with which to handle the payload.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentHandlerWithResult<TPayloadBase, TPayload, T1, T2, T3, TResult>(this ContentFactory contentFactory, Func<TPayload, T1, T2, T3, TResult> handle, string handlerClass)
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
            contentFactory.RegisterContentHandlerWithResultAndAction<TPayloadBase, TPayload, T1, T2, T3, TResult>(
                contentType,
                handle,
                handlerClass);

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type for which to register the handler.</param>
        /// <param name="handle">The function with which to handle the payload.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentHandlerWithResult<TPayloadBase, TResult>(this ContentFactory contentFactory, string contentType, Func<TPayloadBase, TResult> handle, string handlerClass)
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

            contentFactory.RegisterContentHandlerWithResultAndAction<TPayloadBase, TPayloadBase, TResult>(
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
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type for which to register the handler.</param>
        /// <param name="handle">The function with which to handle the payload.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentHandlerWithResult<TPayloadBase, T1, TResult>(this ContentFactory contentFactory, string contentType, Func<TPayloadBase, T1, TResult> handle, string handlerClass)
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

            contentFactory.RegisterContentHandlerWithResultAndAction<TPayloadBase, TPayloadBase, T1, TResult>(
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
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type for which to register the handler.</param>
        /// <param name="handle">The function with which to handle the payload.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentHandlerWithResult<TPayloadBase, T1, T2, TResult>(this ContentFactory contentFactory, string contentType, Func<TPayloadBase, T1, T2, TResult> handle, string handlerClass)
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

            contentFactory.RegisterContentHandlerWithResultAndAction<TPayloadBase, TPayloadBase, T1, T2, TResult>(
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
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type for which to register the handler.</param>
        /// <param name="handle">The function with which to handle the payload.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentHandlerWithResult<TPayloadBase, T1, T2, T3, TResult>(this ContentFactory contentFactory, string contentType, Func<TPayloadBase, T1, T2, T3, TResult> handle, string handlerClass)
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

            contentFactory.RegisterContentHandlerWithResultAndAction<TPayloadBase, TPayloadBase, T1, T2, T3, TResult>(
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
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentHandlerWithResult<TPayloadBase, TPayload, THandler, TResult>(this ContentFactory contentFactory, string handlerClass)
            where TPayload : TPayloadBase
            where THandler : class, IContentHandlerWithResult<TPayload, TResult>
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
            contentFactory.RegisterSingletonContent<ContentHandlerWithResultAndClass<TPayloadBase, TPayload, THandler, TResult>>(
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
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentHandlerWithResult<TPayloadBase, TPayload, THandler, T1, TResult>(this ContentFactory contentFactory, string handlerClass)
            where TPayload : TPayloadBase
            where THandler : class, IContentHandlerWithResult<TPayload, T1, TResult>
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
            contentFactory.RegisterSingletonContent<ContentHandlerWithResultAndClass<TPayloadBase, TPayload, THandler, T1, TResult>>(
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
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentHandlerWithResult<TPayloadBase, TPayload, THandler, T1, T2, TResult>(this ContentFactory contentFactory, string handlerClass)
            where TPayload : TPayloadBase
            where THandler : class, IContentHandlerWithResult<TPayload, T1, T2, TResult>
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
            contentFactory.RegisterSingletonContent<ContentHandlerWithResultAndClass<TPayloadBase, TPayload, THandler, T1, T2, TResult>>(
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
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentHandlerWithResult<TPayloadBase, TPayload, THandler, T1, T2, T3, TResult>(this ContentFactory contentFactory, string handlerClass)
            where TPayload : TPayloadBase
            where THandler : class, IContentHandlerWithResult<TPayload, T1, T2, T3, TResult>
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
            contentFactory.RegisterSingletonContent<ContentHandlerWithResultAndClass<TPayloadBase, TPayload, THandler, T1, T2, T3, TResult>>(
                ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type for which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentHandlerWithResult<TPayloadBase, THandler, TResult>(this ContentFactory contentFactory, string contentType, string handlerClass)
            where THandler : class, IContentHandlerWithResult<TPayloadBase, TResult>
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
            contentFactory.RegisterSingletonContent<ContentHandlerWithResultAndClass<TPayloadBase, TPayloadBase, THandler, TResult>>(
                ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type for which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentHandlerWithResult<TPayloadBase, THandler, T1, TResult>(this ContentFactory contentFactory, string contentType, string handlerClass)
            where THandler : class, IContentHandlerWithResult<TPayloadBase, T1, TResult>
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
            contentFactory.RegisterSingletonContent<ContentHandlerWithResultAndClass<TPayloadBase, TPayloadBase, THandler, T1, TResult>>(
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
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type for which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentHandlerWithResult<TPayloadBase, THandler, T1, T2, TResult>(this ContentFactory contentFactory, string contentType, string handlerClass)
            where THandler : class, IContentHandlerWithResult<TPayloadBase, T1, T2, TResult>
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
            contentFactory.RegisterSingletonContent<ContentHandlerWithResultAndClass<TPayloadBase, TPayloadBase, THandler, T1, T2, TResult>>(
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
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type for which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentHandlerWithResult<TPayloadBase, THandler, T1, T2, T3, TResult>(this ContentFactory contentFactory, string contentType, string handlerClass)
            where THandler : class, IContentHandlerWithResult<TPayloadBase, T1, T2, T3, TResult>
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
            contentFactory.RegisterSingletonContent<ContentHandlerWithResultAndClass<TPayloadBase, TPayloadBase, THandler, T1, T2, T3, TResult>>(
                ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <param name="handlerFactory">A factory function to create the singleton handler.</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentHandlerWithResult<TPayloadBase, TPayload, THandler, TResult>(this ContentFactory contentFactory, string handlerClass, Func<IServiceProvider, THandler> handlerFactory)
            where TPayload : TPayloadBase
            where THandler : class, IContentHandlerWithResult<TPayload, TResult>
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
            contentFactory.RegisterSingletonContent<ContentHandlerWithResultAndClass<TPayloadBase, TPayload, THandler, TResult>>(
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
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <param name="handlerFactory">A factory function to create the singleton handler.</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentHandlerWithResult<TPayloadBase, TPayload, THandler, T1, TResult>(this ContentFactory contentFactory, string handlerClass, Func<IServiceProvider, THandler> handlerFactory)
            where TPayload : TPayloadBase
            where THandler : class, IContentHandlerWithResult<TPayload, T1, TResult>
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
            contentFactory.RegisterSingletonContent<ContentHandlerWithResultAndClass<TPayloadBase, TPayload, THandler, T1, TResult>>(
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
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <param name="handlerFactory">A factory function to create the singleton handler.</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentHandlerWithResult<TPayloadBase, TPayload, THandler, T1, T2, TResult>(this ContentFactory contentFactory, string handlerClass, Func<IServiceProvider, THandler> handlerFactory)
            where TPayload : TPayloadBase
            where THandler : class, IContentHandlerWithResult<TPayload, T1, T2, TResult>
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
            contentFactory.RegisterSingletonContent<ContentHandlerWithResultAndClass<TPayloadBase, TPayload, THandler, T1, T2, TResult>>(
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
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <param name="handlerFactory">A factory function to create the singleton handler.</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentHandlerWithResult<TPayloadBase, TPayload, THandler, T1, T2, T3, TResult>(this ContentFactory contentFactory, string handlerClass, Func<IServiceProvider, THandler> handlerFactory)
            where TPayload : TPayloadBase
            where THandler : class, IContentHandlerWithResult<TPayload, T1, T2, T3, TResult>
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
            contentFactory.RegisterSingletonContent<ContentHandlerWithResultAndClass<TPayloadBase, TPayload, THandler, T1, T2, T3, TResult>>(
                ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type for which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <param name="handlerFactory">A factory function to create the singleton handler.</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentHandlerWithResult<TPayloadBase, THandler, TResult>(this ContentFactory contentFactory, string contentType, string handlerClass, Func<IServiceProvider, THandler> handlerFactory)
            where THandler : class, IContentHandlerWithResult<TPayloadBase, TResult>
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
            contentFactory.RegisterSingletonContent<ContentHandlerWithResultAndClass<TPayloadBase, TPayloadBase, THandler, TResult>>(
                ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type for which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <param name="handlerFactory">A factory function to create the singleton handler.</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentHandlerWithResult<TPayloadBase, THandler, T1, TResult>(this ContentFactory contentFactory, string contentType, string handlerClass, Func<IServiceProvider, THandler> handlerFactory)
            where THandler : class, IContentHandlerWithResult<TPayloadBase, T1, TResult>
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
            contentFactory.RegisterSingletonContent<ContentHandlerWithResultAndClass<TPayloadBase, TPayloadBase, THandler, T1, TResult>>(
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
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type for which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <param name="handlerFactory">A factory function to create the singleton handler.</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentHandlerWithResult<TPayloadBase, THandler, T1, T2, TResult>(this ContentFactory contentFactory, string contentType, string handlerClass, Func<IServiceProvider, THandler> handlerFactory)
            where THandler : class, IContentHandlerWithResult<TPayloadBase, T1, T2, TResult>
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
            contentFactory.RegisterSingletonContent<ContentHandlerWithResultAndClass<TPayloadBase, TPayloadBase, THandler, T1, T2, TResult>>(
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
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type for which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <param name="handlerFactory">A factory function to create the singleton handler.</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterContentHandlerWithResult<TPayloadBase, THandler, T1, T2, T3, TResult>(this ContentFactory contentFactory, string contentType, string handlerClass, Func<IServiceProvider, THandler> handlerFactory)
            where THandler : class, IContentHandlerWithResult<TPayloadBase, T1, T2, T3, TResult>
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
            contentFactory.RegisterSingletonContent<ContentHandlerWithResultAndClass<TPayloadBase, TPayloadBase, THandler, T1, T2, T3, TResult>>(
                ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handle">The function with which to handle the payload.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentHandlerWithResult<TPayloadBase, TPayload, TResult>(this ContentFactory contentFactory, Func<TPayload, Task<TResult>> handle, string handlerClass)
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
            contentFactory.RegisterAsyncContentHandlerWithResultAndAction<TPayloadBase, TPayload, TResult>(
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
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handle">The function with which to handle the payload.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentHandlerWithResult<TPayloadBase, TPayload, T1, TResult>(this ContentFactory contentFactory, Func<TPayload, T1, Task<TResult>> handle, string handlerClass)
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
            contentFactory.RegisterAsyncContentHandlerWithResultAndAction<TPayloadBase, TPayload, T1, TResult>(
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
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handle">The function with which to handle the payload.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentHandlerWithResult<TPayloadBase, TPayload, T1, T2, TResult>(this ContentFactory contentFactory, Func<TPayload, T1, T2, Task<TResult>> handle, string handlerClass)
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
            contentFactory.RegisterAsyncContentHandlerWithResultAndAction<TPayloadBase, TPayload, T1, T2, TResult>(
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
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handle">The function with which to handle the payload.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentHandlerWithResult<TPayloadBase, TPayload, T1, T2, T3, TResult>(this ContentFactory contentFactory, Func<TPayload, T1, T2, T3, Task<TResult>> handle, string handlerClass)
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
            contentFactory.RegisterAsyncContentHandlerWithResultAndAction<TPayloadBase, TPayload, T1, T2, T3, TResult>(
                contentType,
                handle,
                handlerClass);

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type for which to register the handler.</param>
        /// <param name="handle">The function with which to handle the payload.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentHandlerWithResult<TPayloadBase, TResult>(this ContentFactory contentFactory, string contentType, Func<TPayloadBase, Task<TResult>> handle, string handlerClass)
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

            contentFactory.RegisterAsyncContentHandlerWithResultAndAction<TPayloadBase, TPayloadBase, TResult>(
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
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type for which to register the handler.</param>
        /// <param name="handle">The function with which to handle the payload.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentHandlerWithResult<TPayloadBase, T1, TResult>(this ContentFactory contentFactory, string contentType, Func<TPayloadBase, T1, Task<TResult>> handle, string handlerClass)
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

            contentFactory.RegisterAsyncContentHandlerWithResultAndAction<TPayloadBase, TPayloadBase, T1, TResult>(
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
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type for which to register the handler.</param>
        /// <param name="handle">The function with which to handle the payload.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentHandlerWithResult<TPayloadBase, T1, T2, TResult>(this ContentFactory contentFactory, string contentType, Func<TPayloadBase, T1, T2, Task<TResult>> handle, string handlerClass)
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

            contentFactory.RegisterAsyncContentHandlerWithResultAndAction<TPayloadBase, TPayloadBase, T1, T2, TResult>(
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
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type for which to register the handler.</param>
        /// <param name="handle">The function with which to handle the payload.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentHandlerWithResult<TPayloadBase, T1, T2, T3, TResult>(this ContentFactory contentFactory, string contentType, Func<TPayloadBase, T1, T2, T3, Task<TResult>> handle, string handlerClass)
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

            contentFactory.RegisterAsyncContentHandlerWithResultAndAction<TPayloadBase, TPayloadBase, T1, T2, T3, TResult>(
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
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentHandlerWithResult<TPayloadBase, TPayload, THandler, TResult>(this ContentFactory contentFactory, string handlerClass)
            where TPayload : TPayloadBase
            where THandler : class, IAsyncContentHandlerWithResult<TPayload, TResult>
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
            contentFactory.RegisterSingletonContent<AsyncContentHandlerWithResultAndClass<TPayloadBase, TPayload, THandler, TResult>>(
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
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentHandlerWithResult<TPayloadBase, TPayload, THandler, T1, TResult>(this ContentFactory contentFactory, string handlerClass)
            where TPayload : TPayloadBase
            where THandler : class, IAsyncContentHandlerWithResult<TPayload, T1, TResult>
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
            contentFactory.RegisterSingletonContent<AsyncContentHandlerWithResultAndClass<TPayloadBase, TPayload, THandler, T1, TResult>>(
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
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentHandlerWithResult<TPayloadBase, TPayload, THandler, T1, T2, TResult>(this ContentFactory contentFactory, string handlerClass)
            where TPayload : TPayloadBase
            where THandler : class, IAsyncContentHandlerWithResult<TPayload, T1, T2, TResult>
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
            contentFactory.RegisterSingletonContent<AsyncContentHandlerWithResultAndClass<TPayloadBase, TPayload, THandler, T1, T2, TResult>>(
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
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentHandlerWithResult<TPayloadBase, TPayload, THandler, T1, T2, T3, TResult>(this ContentFactory contentFactory, string handlerClass)
            where TPayload : TPayloadBase
            where THandler : class, IAsyncContentHandlerWithResult<TPayload, T1, T2, T3, TResult>
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
            contentFactory.RegisterSingletonContent<AsyncContentHandlerWithResultAndClass<TPayloadBase, TPayload, THandler, T1, T2, T3, TResult>>(
                ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));
            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type for which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentHandlerWithResult<TPayloadBase, THandler, TResult>(this ContentFactory contentFactory, string contentType, string handlerClass)
            where THandler : class, IAsyncContentHandlerWithResult<TPayloadBase, TResult>
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
            contentFactory.RegisterSingletonContent<AsyncContentHandlerWithResultAndClass<TPayloadBase, TPayloadBase, THandler, TResult>>(
                ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));
            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type for which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentHandlerWithResult<TPayloadBase, THandler, T1, TResult>(this ContentFactory contentFactory, string contentType, string handlerClass)
            where THandler : class, IAsyncContentHandlerWithResult<TPayloadBase, T1, TResult>
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
            contentFactory.RegisterSingletonContent<AsyncContentHandlerWithResultAndClass<TPayloadBase, TPayloadBase, THandler, T1, TResult>>(
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
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type for which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentHandlerWithResult<TPayloadBase, THandler, T1, T2, TResult>(this ContentFactory contentFactory, string contentType, string handlerClass)
            where THandler : class, IAsyncContentHandlerWithResult<TPayloadBase, T1, T2, TResult>
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
            contentFactory.RegisterSingletonContent<AsyncContentHandlerWithResultAndClass<TPayloadBase, TPayloadBase, THandler, T1, T2, TResult>>(
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
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type for which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentHandlerWithResult<TPayloadBase, THandler, T1, T2, T3, TResult>(this ContentFactory contentFactory, string contentType, string handlerClass)
            where THandler : class, IAsyncContentHandlerWithResult<TPayloadBase, T1, T2, T3, TResult>
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
            contentFactory.RegisterSingletonContent<AsyncContentHandlerWithResultAndClass<TPayloadBase, TPayloadBase, THandler, T1, T2, T3, TResult>>(
                ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));
            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="TPayload">The specific type of payload for this handler.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <param name="handlerFactory">A factory function to create the singleton handler.</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentHandlerWithResult<TPayloadBase, TPayload, THandler, TResult>(this ContentFactory contentFactory, string handlerClass, Func<IServiceProvider, THandler> handlerFactory)
            where TPayload : TPayloadBase
            where THandler : class, IAsyncContentHandlerWithResult<TPayload, TResult>
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.Services.AddSingleton(handlerFactory);
            contentFactory.RegisterSingletonContent<AsyncContentHandlerWithResultAndClass<TPayloadBase, TPayload, THandler, TResult>>(
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
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <param name="handlerFactory">A factory function to create the singleton handler.</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentHandlerWithResult<TPayloadBase, TPayload, THandler, T1, TResult>(this ContentFactory contentFactory, string handlerClass, Func<IServiceProvider, THandler> handlerFactory)
            where TPayload : TPayloadBase
            where THandler : class, IAsyncContentHandlerWithResult<TPayload, T1, TResult>
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.Services.AddSingleton(handlerFactory);
            contentFactory.RegisterSingletonContent<AsyncContentHandlerWithResultAndClass<TPayloadBase, TPayload, THandler, T1, TResult>>(
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
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <param name="handlerFactory">A factory function to create the singleton handler.</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentHandlerWithResult<TPayloadBase, TPayload, THandler, T1, T2, TResult>(this ContentFactory contentFactory, string handlerClass, Func<IServiceProvider, THandler> handlerFactory)
            where TPayload : TPayloadBase
            where THandler : class, IAsyncContentHandlerWithResult<TPayload, T1, T2, TResult>
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.Services.AddSingleton(handlerFactory);
            contentFactory.RegisterSingletonContent<AsyncContentHandlerWithResultAndClass<TPayloadBase, TPayload, THandler, T1, T2, TResult>>(
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
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <param name="handlerFactory">A factory function to create the singleton handler.</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentHandlerWithResult<TPayloadBase, TPayload, THandler, T1, T2, T3, TResult>(this ContentFactory contentFactory, string handlerClass, Func<IServiceProvider, THandler> handlerFactory)
            where TPayload : TPayloadBase
            where THandler : class, IAsyncContentHandlerWithResult<TPayload, T1, T2, T3, TResult>
        {
            if (contentFactory is null)
            {
                throw new ArgumentNullException(nameof(contentFactory));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            if (handlerClass is null)
            {
                throw new ArgumentNullException(nameof(handlerClass));
            }

            string contentType = ContentFactory.GetContentType<TPayload>();
            contentFactory.Services.AddSingleton(handlerFactory);
            contentFactory.RegisterSingletonContent<AsyncContentHandlerWithResultAndClass<TPayloadBase, TPayload, THandler, T1, T2, T3, TResult>>(
                ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type for which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <param name="handlerFactory">A factory function to create the singleton handler.</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentHandlerWithResult<TPayloadBase, THandler, TResult>(this ContentFactory contentFactory, string contentType, string handlerClass, Func<IServiceProvider, THandler> handlerFactory)
            where THandler : class, IAsyncContentHandlerWithResult<TPayloadBase, TResult>
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
            contentFactory.RegisterSingletonContent<AsyncContentHandlerWithResultAndClass<TPayloadBase, TPayloadBase, THandler, TResult>>(
                ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));

            return contentFactory;
        }

        /// <summary>
        /// Registers a content handler for a particular type of payload.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of payload for all handlers of this class.</typeparam>
        /// <typeparam name="THandler">The type of the handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type for which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <param name="handlerFactory">A factory function to create the singleton handler.</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentHandlerWithResult<TPayloadBase, THandler, T1, TResult>(this ContentFactory contentFactory, string contentType, string handlerClass, Func<IServiceProvider, THandler> handlerFactory)
            where THandler : class, IAsyncContentHandlerWithResult<TPayloadBase, T1, TResult>
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
            contentFactory.RegisterSingletonContent<AsyncContentHandlerWithResultAndClass<TPayloadBase, TPayloadBase, THandler, T1, TResult>>(
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
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type for which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <param name="handlerFactory">A factory function to create the singleton handler.</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentHandlerWithResult<TPayloadBase, THandler, T1, T2, TResult>(this ContentFactory contentFactory, string contentType, string handlerClass, Func<IServiceProvider, THandler> handlerFactory)
            where THandler : class, IAsyncContentHandlerWithResult<TPayloadBase, T1, T2, TResult>
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
            contentFactory.RegisterSingletonContent<AsyncContentHandlerWithResultAndClass<TPayloadBase, TPayloadBase, THandler, T1, T2, TResult>>(
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
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type for which to register the handler.</param>
        /// <param name="handlerClass">The class of handler (e.g. "viewFactory", "messageDispatcher").</param>
        /// <param name="handlerFactory">A factory function to create the singleton handler.</param>
        /// <returns>The content factory.</returns>
        public static ContentFactory RegisterAsyncContentHandlerWithResult<TPayloadBase, THandler, T1, T2, T3, TResult>(this ContentFactory contentFactory, string contentType, string handlerClass, Func<IServiceProvider, THandler> handlerFactory)
            where THandler : class, IAsyncContentHandlerWithResult<TPayloadBase, T1, T2, T3, TResult>
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
            contentFactory.RegisterSingletonContent<AsyncContentHandlerWithResultAndClass<TPayloadBase, TPayloadBase, THandler, T1, T2, T3, TResult>>(
                ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass));

            return contentFactory;
        }
    }
}