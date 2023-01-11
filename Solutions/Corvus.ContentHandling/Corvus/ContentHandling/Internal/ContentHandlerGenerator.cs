// <copyright file="ContentHandlerGenerator.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling.Internal
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Runtime.Loader;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    /// <summary>
    /// Registers custom subclasses of the <see cref="IContentHandler{TPayloadType}"/>
    /// as singletongs.
    /// </summary>
    internal static class ContentHandlerGenerator
    {
        private const string AsyncContentHandlerWithAction =
@"namespace Corvus.ContentHandling.Internal
{{
    using System;
    using System.Threading.Tasks;

    public class {0}<TPayloadBase, TPayload> : AsyncContentHandlerWithAction<TPayloadBase, TPayload>
        where TPayload : TPayloadBase
    {{
        public {0}(Func<TPayload, Task> handle)
            : base(handle)
        {{
        }}
    }}
}}";

        private const string AsyncContentHandlerWithActionT1 =
@"namespace Corvus.ContentHandling.Internal
{{
    using System;
    using System.Threading.Tasks;

    public class {0}<TPayloadBase, TPayload, T1> : AsyncContentHandlerWithAction<TPayloadBase, TPayload, T1>
        where TPayload : TPayloadBase
    {{
        public {0}(Func<TPayload, T1, Task> handle)
            : base(handle)
        {{
        }}
    }}
}}";

        private const string AsyncContentHandlerWithActionT1T2 =
@"namespace Corvus.ContentHandling.Internal
{{
    using System;
    using System.Threading.Tasks;

    public class {0}<TPayloadBase, TPayload, T1, T2> : AsyncContentHandlerWithAction<TPayloadBase, TPayload, T1, T2>
        where TPayload : TPayloadBase
    {{
        public {0}(Func<TPayload, T1, T2, Task> handle)
            : base(handle)
        {{
        }}
    }}
}}";

        private const string AsyncContentHandlerWithActionT1T2T3 =
@"namespace Corvus.ContentHandling.Internal
{{
    using System;
    using System.Threading.Tasks;

    public class {0}<TPayloadBase, TPayload, T1, T2, T3> : AsyncContentHandlerWithAction<TPayloadBase, TPayload, T1, T2, T3>
        where TPayload : TPayloadBase
    {{
        public {0}(Func<TPayload, T1, T2, T3, Task> handle)
            : base(handle)
        {{
        }}
    }}
}}";

        private const string ContentHandlerWithAction =
@"namespace Corvus.ContentHandling.Internal
{{
    using System;

    public class {0}<TPayloadBase, TPayload> : ContentHandlerWithAction<TPayloadBase, TPayload>
        where TPayload : TPayloadBase
    {{
        public {0}(Action<TPayload> handle)
            : base(handle)
        {{
        }}
    }}
}}";

        private const string ContentHandlerWithActionT1 =
@"namespace Corvus.ContentHandling.Internal
{{
    using System;

    public class {0}<TPayloadBase, TPayload, T1> : ContentHandlerWithAction<TPayloadBase, TPayload, T1>
        where TPayload : TPayloadBase
    {{
        public {0}(Action<TPayload, T1> handle)
            : base(handle)
        {{
        }}
    }}
}}";

        private const string ContentHandlerWithActionT1T2 =
@"namespace Corvus.ContentHandling.Internal
{{
    using System;

    public class {0}<TPayloadBase, TPayload, T1, T2> : ContentHandlerWithAction<TPayloadBase, TPayload, T1, T2>
        where TPayload : TPayloadBase
    {{
        public {0}(Action<TPayload, T1, T2> handle)
            : base(handle)
        {{
        }}
    }}
}}";

        private const string ContentHandlerWithActionT1T2T3 =
@"namespace Corvus.ContentHandling.Internal
{{
    using System;

    public class {0}<TPayloadBase, TPayload, T1, T2, T3> : ContentHandlerWithAction<TPayloadBase, TPayload, T1, T2, T3>
        where TPayload : TPayloadBase
    {{
        public {0}(Action<TPayload, T1, T2, T3> handle)
            : base(handle)
        {{
        }}
    }}
}}";

        /***/

        private const string AsyncContentHandlerWithResultAndAction =
@"namespace Corvus.ContentHandling.Internal
{{
    using System;
    using System.Threading.Tasks;

    public class {0}<TPayloadBase, TPayload, TResult> : AsyncContentHandlerWithResultAndAction<TPayloadBase, TPayload, TResult>
        where TPayload : TPayloadBase
    {{
        public {0}(Func<TPayload, Task<TResult>> handle)
            : base(handle)
        {{
        }}
    }}
}}";

        private const string AsyncContentHandlerWithResultAndActionT1 =
@"namespace Corvus.ContentHandling.Internal
{{
    using System;
    using System.Threading.Tasks;

    public class {0}<TPayloadBase, TPayload, T1, TResult> : AsyncContentHandlerWithResultAndAction<TPayloadBase, TPayload, T1, TResult>
        where TPayload : TPayloadBase
    {{
        public {0}(Func<TPayload, T1, Task<TResult>> handle)
            : base(handle)
        {{
        }}
    }}
}}";

        private const string AsyncContentHandlerWithResultAndActionT1T2 =
@"namespace Corvus.ContentHandling.Internal
{{
    using System;
    using System.Threading.Tasks;

    public class {0}<TPayloadBase, TPayload, T1, T2, TResult> : AsyncContentHandlerWithResultAndAction<TPayloadBase, TPayload, T1, T2, TResult>
        where TPayload : TPayloadBase
    {{
        public {0}(Func<TPayload, T1, T2, Task<TResult>> handle)
            : base(handle)
        {{
        }}
    }}
}}";

        private const string AsyncContentHandlerWithResultAndActionT1T2T3 =
@"namespace Corvus.ContentHandling.Internal
{{
    using System;
    using System.Threading.Tasks;

    public class {0}<TPayloadBase, TPayload, T1, T2, T3, TResult> : AsyncContentHandlerWithResultAndAction<TPayloadBase, TPayload, T1, T2, T3, TResult>
        where TPayload : TPayloadBase
    {{
        public {0}(Func<TPayload, T1, T2, T3, Task<TResult>> handle)
            : base(handle)
        {{
        }}
    }}
}}";

        private const string ContentHandlerWithResultAndAction =
@"namespace Corvus.ContentHandling.Internal
{{
    using System;

    public class {0}<TPayloadBase, TPayload, TResult> : ContentHandlerWithResultAndAction<TPayloadBase, TPayload, TResult>
        where TPayload : TPayloadBase
    {{
        public {0}(Func<TPayload, TResult> handle)
            : base(handle)
        {{
        }}
    }}
}}";

        private const string ContentHandlerWithResultAndActionT1 =
@"namespace Corvus.ContentHandling.Internal
{{
    using System;

    public class {0}<TPayloadBase, TPayload, T1, TResult> : ContentHandlerWithResultAndAction<TPayloadBase, TPayload, T1, TResult>
        where TPayload : TPayloadBase
    {{
        public {0}(Func<TPayload, T1, TResult> handle)
            : base(handle)
        {{
        }}
    }}
}}";

        private const string ContentHandlerWithResultAndActionT1T2 =
@"namespace Corvus.ContentHandling.Internal
{{
    using System;

    public class {0}<TPayloadBase, TPayload, T1, T2, TResult> : ContentHandlerWithResultAndAction<TPayloadBase, TPayload, T1, T2, TResult>
        where TPayload : TPayloadBase
    {{
        public {0}(Func<TPayload, T1, T2, TResult> handle)
            : base(handle)
        {{
        }}
    }}
}}";

        private const string ContentHandlerWithResultAndActionT1T2T3 =
@"namespace Corvus.ContentHandling.Internal
{{
    using System;

    public class {0}<TPayloadBase, TPayload, T1, T2, T3, TResult> : ContentHandlerWithResultAndAction<TPayloadBase, TPayload, T1, T2, T3, TResult>
        where TPayload : TPayloadBase
    {{
        public {0}(Func<TPayload, T1, T2, T3, TResult> handle)
            : base(handle)
        {{
        }}
    }}
}}";

        private static readonly ConcurrentDictionary<(string HandlerClass, string ContentType), Type> CompilationCache = new();
        private static readonly Dictionary<string, string> TrustedPlatformAssemblyMap = GetTrustedPlatformAssemblyMap();

        /// <summary>
        /// Register a unique singleton content handler for an action.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of the payload for this handler class.</typeparam>
        /// <typeparam name="TPayload">The specific type of the payload for this handler.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type to register.</param>
        /// <param name="handle">The handler function.</param>
        /// <param name="handlerClass">The handler class.</param>
        public static void RegisterContentHandlerWithAction<TPayloadBase, TPayload>(this ContentFactory contentFactory, string contentType, Action<TPayload> handle, string handlerClass)
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

            (string HandlerClass, string ContentType) key = (handlerClass, contentType);
            Type genericType = GetGenericHandlerType(handlerClass, "ContentHandlerWithAction", ContentHandlerWithAction, key, 0);
            Type constructedType = genericType.MakeGenericType(typeof(TPayloadBase), typeof(TPayload));
            object instance = Activator.CreateInstance(constructedType, handle)!;
            contentFactory.RegisterSingletonContent(ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass), constructedType, instance);
        }

        /// <summary>
        /// Register a unique singleton content handler for an action.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of the payload for this handler class.</typeparam>
        /// <typeparam name="TPayload">The specific type of the payload for this handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type to register.</param>
        /// <param name="handle">The handler function.</param>
        /// <param name="handlerClass">The handler class.</param>
        public static void RegisterContentHandlerWithAction<TPayloadBase, TPayload, T1>(this ContentFactory contentFactory, string contentType, Action<TPayload, T1> handle, string handlerClass)
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

            (string HandlerClass, string ContentType) key = (handlerClass, contentType);
            Type genericType = GetGenericHandlerType(handlerClass, "ContentHandlerWithAction", ContentHandlerWithActionT1, key, 1);
            Type constructedType = genericType.MakeGenericType(typeof(TPayloadBase), typeof(TPayload), typeof(T1));
            object instance = Activator.CreateInstance(constructedType, handle)!;
            contentFactory.RegisterSingletonContent(ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass), constructedType, instance);
        }

        /// <summary>
        /// Register a unique singleton content handler for an action.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of the payload for this handler class.</typeparam>
        /// <typeparam name="TPayload">The specific type of the payload for this handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type to register.</param>
        /// <param name="handle">The handler function.</param>
        /// <param name="handlerClass">The handler class.</param>
        public static void RegisterContentHandlerWithAction<TPayloadBase, TPayload, T1, T2>(this ContentFactory contentFactory, string contentType, Action<TPayload, T1, T2> handle, string handlerClass)
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

            (string HandlerClass, string ContentType) key = (handlerClass, contentType);
            Type genericType = GetGenericHandlerType(handlerClass, "ContentHandlerWithAction", ContentHandlerWithActionT1T2, key, 2);
            Type constructedType = genericType.MakeGenericType(typeof(TPayloadBase), typeof(TPayload), typeof(T1), typeof(T2));
            object instance = Activator.CreateInstance(constructedType, handle)!;
            contentFactory.RegisterSingletonContent(ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass), constructedType, instance);
        }

        /// <summary>
        /// Register a unique singleton content handler for an action.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of the payload for this handler class.</typeparam>
        /// <typeparam name="TPayload">The specific type of the payload for this handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <typeparam name="T3">The type of the third parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type to register.</param>
        /// <param name="handle">The handler function.</param>
        /// <param name="handlerClass">The handler class.</param>
        public static void RegisterContentHandlerWithAction<TPayloadBase, TPayload, T1, T2, T3>(this ContentFactory contentFactory, string contentType, Action<TPayload, T1, T2, T3> handle, string handlerClass)
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

            (string HandlerClass, string ContentType) key = (handlerClass, contentType);
            Type genericType = GetGenericHandlerType(handlerClass, "ContentHandlerWithAction", ContentHandlerWithActionT1T2T3, key, 3);
            Type constructedType = genericType.MakeGenericType(typeof(TPayloadBase), typeof(TPayload), typeof(T1), typeof(T2), typeof(T3));
            object instance = Activator.CreateInstance(constructedType, handle)!;
            contentFactory.RegisterSingletonContent(ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass), constructedType, instance);
        }

        /// <summary>
        /// Register a unique singleton content handler for an action.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of the payload for this handler class.</typeparam>
        /// <typeparam name="TPayload">The specific type of the payload for this handler.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type to register.</param>
        /// <param name="handle">The handler function.</param>
        /// <param name="handlerClass">The handler class.</param>
        public static void RegisterAsyncContentHandlerWithAction<TPayloadBase, TPayload>(this ContentFactory contentFactory, string contentType, Func<TPayload, Task> handle, string handlerClass)
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

            (string HandlerClass, string ContentType) key = (handlerClass, contentType);
            Type genericType = GetGenericHandlerType(handlerClass, "AsyncContentHandlerWithAction", AsyncContentHandlerWithAction, key, 0);
            Type constructedType = genericType.MakeGenericType(typeof(TPayloadBase), typeof(TPayload));
            object instance = Activator.CreateInstance(constructedType, handle)!;
            contentFactory.RegisterSingletonContent(ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass), constructedType, instance);
        }

        /// <summary>
        /// Register a unique singleton content handler for an action.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of the payload for this handler class.</typeparam>
        /// <typeparam name="TPayload">The specific type of the payload for this handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type to register.</param>
        /// <param name="handle">The handler function.</param>
        /// <param name="handlerClass">The handler class.</param>
        public static void RegisterAsyncContentHandlerWithAction<TPayloadBase, TPayload, T1>(this ContentFactory contentFactory, string contentType, Func<TPayload, T1, Task> handle, string handlerClass)
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

            (string HandlerClass, string ContentType) key = (handlerClass, contentType);
            Type genericType = GetGenericHandlerType(handlerClass, "AsyncContentHandlerWithAction", AsyncContentHandlerWithActionT1, key, 1);
            Type constructedType = genericType.MakeGenericType(typeof(TPayloadBase), typeof(TPayload), typeof(T1));
            object instance = Activator.CreateInstance(constructedType, handle)!;
            contentFactory.RegisterSingletonContent(ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass), constructedType, instance);
        }

        /// <summary>
        /// Register a unique singleton content handler for an action.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of the payload for this handler class.</typeparam>
        /// <typeparam name="TPayload">The specific type of the payload for this handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type to register.</param>
        /// <param name="handle">The handler function.</param>
        /// <param name="handlerClass">The handler class.</param>
        public static void RegisterAsyncContentHandlerWithAction<TPayloadBase, TPayload, T1, T2>(this ContentFactory contentFactory, string contentType, Func<TPayload, T1, T2, Task> handle, string handlerClass)
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

            (string HandlerClass, string ContentType) key = (handlerClass, contentType);
            Type genericType = GetGenericHandlerType(handlerClass, "AsyncContentHandlerWithAction", AsyncContentHandlerWithActionT1T2, key, 2);
            Type constructedType = genericType.MakeGenericType(typeof(TPayloadBase), typeof(TPayload), typeof(T1), typeof(T2));
            object instance = Activator.CreateInstance(constructedType, handle)!;
            contentFactory.RegisterSingletonContent(ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass), constructedType, instance);
        }

        /// <summary>
        /// Register a unique singleton content handler for an action.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of the payload for this handler class.</typeparam>
        /// <typeparam name="TPayload">The specific type of the payload for this handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <typeparam name="T3">The type of the third parameter.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type to register.</param>
        /// <param name="handle">The handler function.</param>
        /// <param name="handlerClass">The handler class.</param>
        public static void RegisterAsyncContentHandlerWithAction<TPayloadBase, TPayload, T1, T2, T3>(this ContentFactory contentFactory, string contentType, Func<TPayload, T1, T2, T3, Task> handle, string handlerClass)
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

            (string HandlerClass, string ContentType) key = (handlerClass, contentType);
            Type genericType = GetGenericHandlerType(handlerClass, "AsyncContentHandlerWithAction", AsyncContentHandlerWithActionT1T2T3, key, 3);
            Type constructedType = genericType.MakeGenericType(typeof(TPayloadBase), typeof(TPayload), typeof(T1), typeof(T2), typeof(T3));
            object instance = Activator.CreateInstance(constructedType, handle)!;
            contentFactory.RegisterSingletonContent(ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass), constructedType, instance);
        }

        /****/

        /// <summary>
        /// Register a unique singleton content handler for an action.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of the payload for this handler class.</typeparam>
        /// <typeparam name="TPayload">The specific type of the payload for this handler.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type to register.</param>
        /// <param name="handle">The handler function.</param>
        /// <param name="handlerClass">The handler class.</param>
        public static void RegisterContentHandlerWithResultAndAction<TPayloadBase, TPayload, TResult>(this ContentFactory contentFactory, string contentType, Func<TPayload, TResult> handle, string handlerClass)
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

            (string HandlerClass, string ContentType) key = (handlerClass, contentType);
            Type genericType = GetGenericHandlerType(handlerClass, "ContentHandlerWithResultAndAction", ContentHandlerWithResultAndAction, key, 1);
            Type constructedType = genericType.MakeGenericType(typeof(TPayloadBase), typeof(TPayload), typeof(TResult));
            object instance = Activator.CreateInstance(constructedType, handle)!;
            contentFactory.RegisterSingletonContent(ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass), constructedType, instance);
        }

        /// <summary>
        /// Register a unique singleton content handler for an action.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of the payload for this handler class.</typeparam>
        /// <typeparam name="TPayload">The specific type of the payload for this handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type to register.</param>
        /// <param name="handle">The handler function.</param>
        /// <param name="handlerClass">The handler class.</param>
        public static void RegisterContentHandlerWithResultAndAction<TPayloadBase, TPayload, T1, TResult>(this ContentFactory contentFactory, string contentType, Func<TPayload, T1, TResult> handle, string handlerClass)
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

            (string HandlerClass, string ContentType) key = (handlerClass, contentType);
            Type genericType = GetGenericHandlerType(handlerClass, "ContentHandlerWithResultAndAction", ContentHandlerWithResultAndActionT1, key, 2);
            Type constructedType = genericType.MakeGenericType(typeof(TPayloadBase), typeof(TPayload), typeof(T1), typeof(TResult));
            object instance = Activator.CreateInstance(constructedType, handle)!;
            contentFactory.RegisterSingletonContent(ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass), constructedType, instance);
        }

        /// <summary>
        /// Register a unique singleton content handler for an action.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of the payload for this handler class.</typeparam>
        /// <typeparam name="TPayload">The specific type of the payload for this handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type to register.</param>
        /// <param name="handle">The handler function.</param>
        /// <param name="handlerClass">The handler class.</param>
        public static void RegisterContentHandlerWithResultAndAction<TPayloadBase, TPayload, T1, T2, TResult>(this ContentFactory contentFactory, string contentType, Func<TPayload, T1, T2, TResult> handle, string handlerClass)
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

            (string HandlerClass, string ContentType) key = (handlerClass, contentType);
            Type genericType = GetGenericHandlerType(handlerClass, "ContentHandlerWithResultAndAction", ContentHandlerWithResultAndActionT1T2, key, 3);
            Type constructedType = genericType.MakeGenericType(typeof(TPayloadBase), typeof(TPayload), typeof(T1), typeof(T2), typeof(TResult));
            object instance = Activator.CreateInstance(constructedType, handle)!;
            contentFactory.RegisterSingletonContent(ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass), constructedType, instance);
        }

        /// <summary>
        /// Register a unique singleton content handler for an action.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of the payload for this handler class.</typeparam>
        /// <typeparam name="TPayload">The specific type of the payload for this handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <typeparam name="T3">The type of the third parameter.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type to register.</param>
        /// <param name="handle">The handler function.</param>
        /// <param name="handlerClass">The handler class.</param>
        public static void RegisterContentHandlerWithResultAndAction<TPayloadBase, TPayload, T1, T2, T3, TResult>(this ContentFactory contentFactory, string contentType, Func<TPayload, T1, T2, T3, TResult> handle, string handlerClass)
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

            (string HandlerClass, string ContentType) key = (handlerClass, contentType);
            Type genericType = GetGenericHandlerType(handlerClass, "ContentHandlerWithResultAndAction", ContentHandlerWithResultAndActionT1T2T3, key, 4);
            Type constructedType = genericType.MakeGenericType(typeof(TPayloadBase), typeof(TPayload), typeof(T1), typeof(T2), typeof(T3), typeof(TResult));
            object instance = Activator.CreateInstance(constructedType, handle)!;
            contentFactory.RegisterSingletonContent(ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass), constructedType, instance);
        }

        /// <summary>
        /// Register a unique singleton content handler for an action.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of the payload for this handler class.</typeparam>
        /// <typeparam name="TPayload">The specific type of the payload for this handler.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type to register.</param>
        /// <param name="handle">The handler function.</param>
        /// <param name="handlerClass">The handler class.</param>
        public static void RegisterAsyncContentHandlerWithResultAndAction<TPayloadBase, TPayload, TResult>(this ContentFactory contentFactory, string contentType, Func<TPayload, Task<TResult>> handle, string handlerClass)
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

            (string HandlerClass, string ContentType) key = (handlerClass, contentType);
            Type genericType = GetGenericHandlerType(handlerClass, "AsyncContentHandlerWithResultAndAction", AsyncContentHandlerWithResultAndAction, key, 1);
            Type constructedType = genericType.MakeGenericType(typeof(TPayloadBase), typeof(TPayload), typeof(TResult));
            object instance = Activator.CreateInstance(constructedType, handle)!;
            contentFactory.RegisterSingletonContent(ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass), constructedType, instance);
        }

        /// <summary>
        /// Register a unique singleton content handler for an action.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of the payload for this handler class.</typeparam>
        /// <typeparam name="TPayload">The specific type of the payload for this handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type to register.</param>
        /// <param name="handle">The handler function.</param>
        /// <param name="handlerClass">The handler class.</param>
        public static void RegisterAsyncContentHandlerWithResultAndAction<TPayloadBase, TPayload, T1, TResult>(this ContentFactory contentFactory, string contentType, Func<TPayload, T1, Task<TResult>> handle, string handlerClass)
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

            (string HandlerClass, string ContentType) key = (handlerClass, contentType);
            Type genericType = GetGenericHandlerType(handlerClass, "AsyncContentHandlerWithResultAndAction", AsyncContentHandlerWithResultAndActionT1, key, 2);
            Type constructedType = genericType.MakeGenericType(typeof(TPayloadBase), typeof(TPayload), typeof(T1), typeof(TResult));
            object instance = Activator.CreateInstance(constructedType, handle)!;
            contentFactory.RegisterSingletonContent(ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass), constructedType, instance);
        }

        /// <summary>
        /// Register a unique singleton content handler for an action.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of the payload for this handler class.</typeparam>
        /// <typeparam name="TPayload">The specific type of the payload for this handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type to register.</param>
        /// <param name="handle">The handler function.</param>
        /// <param name="handlerClass">The handler class.</param>
        public static void RegisterAsyncContentHandlerWithResultAndAction<TPayloadBase, TPayload, T1, T2, TResult>(this ContentFactory contentFactory, string contentType, Func<TPayload, T1, T2, Task<TResult>> handle, string handlerClass)
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

            (string HandlerClass, string ContentType) key = (handlerClass, contentType);
            Type genericType = GetGenericHandlerType(handlerClass, "AsyncContentHandlerWithResultAndAction", AsyncContentHandlerWithResultAndActionT1T2, key, 3);
            Type constructedType = genericType.MakeGenericType(typeof(TPayloadBase), typeof(TPayload), typeof(T1), typeof(T2), typeof(TResult));
            object instance = Activator.CreateInstance(constructedType, handle)!;
            contentFactory.RegisterSingletonContent(ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass), constructedType, instance);
        }

        /// <summary>
        /// Register a unique singleton content handler for an action.
        /// </summary>
        /// <typeparam name="TPayloadBase">The base type of the payload for this handler class.</typeparam>
        /// <typeparam name="TPayload">The specific type of the payload for this handler.</typeparam>
        /// <typeparam name="T1">The type of the first parameter.</typeparam>
        /// <typeparam name="T2">The type of the second parameter.</typeparam>
        /// <typeparam name="T3">The type of the third parameter.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="contentFactory">The content factory with which to register the handler.</param>
        /// <param name="contentType">The content type to register.</param>
        /// <param name="handle">The handler function.</param>
        /// <param name="handlerClass">The handler class.</param>
        public static void RegisterAsyncContentHandlerWithResultAndAction<TPayloadBase, TPayload, T1, T2, T3, TResult>(this ContentFactory contentFactory, string contentType, Func<TPayload, T1, T2, T3, Task<TResult>> handle, string handlerClass)
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

            (string HandlerClass, string ContentType) key = (handlerClass, contentType);
            Type genericType = GetGenericHandlerType(handlerClass, "AsyncContentHandlerWithResultAndAction", AsyncContentHandlerWithResultAndActionT1T2T3, key, 4);
            Type constructedType = genericType.MakeGenericType(typeof(TPayloadBase), typeof(TPayload), typeof(T1), typeof(T2), typeof(T3), typeof(TResult));
            object instance = Activator.CreateInstance(constructedType, handle)!;
            contentFactory.RegisterSingletonContent(ContentHandlerUtilities.GetHandlerContentType(contentType, handlerClass), constructedType, instance);
        }

        private static Type GetGenericHandlerType(string handlerClass, string baseName, string syntax, (string HandlerClass, string ContentType) key, int numberOfParameters)
        {
            return CompilationCache.GetOrAdd(key, _ =>
            {
                string uniqueKey = Guid.NewGuid().ToString("N");
                string typeName = $"{baseName}__{uniqueKey}";
                SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(string.Format(syntax, typeName));
                var compilation = CSharpCompilation.Create(
                    $"chg-{handlerClass}-{uniqueKey}",
                    new[] { syntaxTree },
                    new[] { MetadataReference.CreateFromFile(TrustedPlatformAssemblyMap["netstandard"]), MetadataReference.CreateFromFile(TrustedPlatformAssemblyMap["System.Runtime"]), MetadataReference.CreateFromFile(typeof(Task).GetTypeInfo().Assembly.Location), MetadataReference.CreateFromFile(typeof(ValueTuple).GetTypeInfo().Assembly.Location), MetadataReference.CreateFromFile(typeof(ContentHandlerGenerator).GetTypeInfo().Assembly.Location) },
                    new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, optimizationLevel: OptimizationLevel.Release));

                using var dllStream = new MemoryStream();
                Microsoft.CodeAnalysis.Emit.EmitResult emitResult = compilation.Emit(dllStream);
                if (!emitResult.Success)
                {
                    throw new Exception($"Compilation error building the content handler for the {handlerClass}");
                }

                dllStream.Flush();
                dllStream.Position = 0;

                // Make sure we load it into the same load context as our assembly
                Assembly assembly = AssemblyLoadContext.GetLoadContext(typeof(ContentHandlerGenerator).Assembly)!.LoadFromStream(dllStream);
                return assembly.GetType(GetTypeName(typeName, numberOfParameters))!;
            });
        }

        private static Dictionary<string, string> GetTrustedPlatformAssemblyMap()
        {
            var set = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            if (AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES") is string paths)
            {
                foreach (string path in paths.Split(Path.PathSeparator))
                {
                    if (Path.GetExtension(path) == ".dll")
                    {
                        string fileName = Path.GetFileNameWithoutExtension(path);
                        if (fileName.EndsWith(".ni", StringComparison.OrdinalIgnoreCase))
                        {
                            fileName = fileName.Substring(0, fileName.Length - ".ni".Length);
                        }

                        // last one wins:
                        set[fileName] = path;
                    }
                }
            }

            return set;
        }

        private static string GetTypeName(string typeName, int numberOfParameters)
        {
            return $"Corvus.ContentHandling.Internal.{typeName}`{numberOfParameters + 2}";
        }
    }
}