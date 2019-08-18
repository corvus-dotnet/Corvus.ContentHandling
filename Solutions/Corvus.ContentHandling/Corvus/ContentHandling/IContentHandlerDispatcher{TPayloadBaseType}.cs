// <copyright file="IContentHandlerDispatcher{TPayloadBaseType}.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Corvus.ContentHandling
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// An interface implemented by types which can dispatch a payload
    /// to a handler of a particular handler class.
    /// </summary>
    /// <typeparam name="TPayloadBaseType">The common base type of the payloads for this class of handler.</typeparam>
    /// <remarks>
    /// <para>It is a common pattern to have a set of types of some common base, and operations that you can carry out on those types.</para>
    /// <para>Each operation needs to be specialised for the specific type, and there may be multiple different classes of operation you can carry out on those types - e.g. for UI Element types you may want to render them to HTML, or render them to WPF, or log their state.</para>
    /// <para>The <see cref="IContentHandlerDispatcher{TPayloadBaseType}"/> gives us the means to register handlers of particular "handler classes" for these types, and dispatch instances of those types when we need to.</para>
    /// <para>
    /// Consider the case where you have a number of types which describe user interface elements
    /// which implement some base type <c>UIElement</c>.
    /// </para>
    /// <para>
    /// <code>
    /// public interface IUIElement
    /// {
    ///     string ContentType { get; }
    /// }
    ///
    /// public class Button : IUIElement
    /// {
    ///     public const string contentFactoryType = "application/vnd.Corvus.uielement.button";
    ///
    ///     public string Caption { get; set; }
    ///
    ///     public string ContentType => contentFactoryType;
    /// }
    ///
    /// public class TextBox : IUIElement
    /// {
    ///     public const string contentFactoryType = "application/vnd.Corvus.uielement.button";
    ///
    ///     public string Text { get; set; }
    ///
    ///     public string ContentType => contentFactoryType;
    /// }
    ///
    /// </code>
    /// </para>
    /// <para>
    /// Imagine we want to provide a means of generating views for those UI elements - what we might call a 'renderer'.
    /// We can create <see cref="IContentHandler{TPayloadType}"/> instances and register tham as handlers for this
    /// content.
    /// </para>
    /// <para>
    /// <code>
    /// public class TextBoxRenderer : IContentHandler&lt;TextBox&gt;
    /// {
    ///     private readonly IRenderContext renderContext;
    ///
    ///     public TextBoxRenderer(IRenderContext renderContext)
    ///     {
    ///         this.renderContext = renderContext;
    ///     }
    ///
    ///     public void Handle(TextBox textBox)
    ///     {
    ///         this.renderContext.EmitTextBox(textBox.Text);
    ///     }
    /// }
    ///
    /// public class ButtonRenderer : IContentHandler&lt;Button&gt;
    /// {
    ///     private readonly IRenderContext renderContext;
    ///
    ///     public TextBoxRenderer(IRenderContext renderContext)
    ///     {
    ///         this.renderContext = renderContext;
    ///     }
    ///
    ///     public void Handle(Button button)
    ///     {
    ///         this.renderContext.EmitButton(button.Caption);
    ///     }
    /// }
    /// </code>
    /// </para>
    /// <para>
    /// Next, we register these renderers as content handlers for the "UIElementRenderer.HandlerClass" - this is a unique string indicating the type of handler that we are registering.
    /// </para>
    /// <para>
    /// <code>
    /// serviceCollection.RegisterContentHandler&lt;IUIElement, TextBox, TextBoxRenderer&gt;(UIElementRenderer.RendererClass);
    /// serviceCollection.RegisterContentHandler&lt;IUIElement, Button, ButtonRenderer&gt;(UIElementRenderer.RendererClass);
    /// </code>
    /// </para>
    /// <para>
    /// Now, when faced with a list of IUIElements for which to generate views, we use the <see cref="IContentHandlerDispatcher{TPayloadType}"/>
    /// to render the elements.
    /// </para>
    /// <para>
    /// <code>
    /// public class UIElementRenderer
    /// {
    ///     // We need to ensure our handler class is globally unique, but tolerably human-readable
    ///     public const string RendererClass = "renderer-b2ade175-7fa4-4c45-8427-f3e31e3051b3";
    ///
    ///     private readonly IContentHandlerDispatcher&lt;IUIElement&gt; dispatcher;
    ///
    ///     public UIElementRenderer(IContentHandlerDispatcher&lt;IUIElement&gt; dispatcher)
    ///     {
    ///         this.dispatcher = dispatcher;
    ///     }
    ///
    ///     public void RenderUI(IEnumerable&lt;IUIElement&gt; itemsToRender)
    ///     {
    ///         foreach(var item in itemsToRender)
    ///         {
    ///             // We are asking the dispatcher to dispatch the item to a handler in the UIElementRenderer.RendererClass
    ///             // not any other classes of handler that may be registered for this content type
    ///             this.dispatcher.DispatchPayloadToHandler(item, RendererClass);
    ///         }
    ///     }
    /// }
    /// </code>
    /// </para>
    /// <para>
    /// This will correctly dispatch the payload to the relevant handler for rendering.
    /// </para>
    /// <para>
    /// Imagine now that we wanted to register loggers for the UIElements. We will create a specialised logger for the <c>Button</c>
    /// but want to fall back on a generic logger for all other UIElements.
    /// </para>
    /// <para>
    /// First, let's update our renderer to add logging.
    /// </para>
    /// <para>
    /// <code>
    /// public class UIElementRenderer
    /// {
    ///     // We need to ensure our handler class is globally unique, but tolerably human-readable
    ///     public const string RendererClass = "renderer-b2ade175-7fa4-4c45-8427-f3e31e3051b3";
    ///     public const string LoggerClass = "logger-b2ade175-7fa4-4c45-8427-f3e31e3051b3";
    ///
    ///     private readonly IContentHandlerDispatcher&lt;IUIElement&gt; dispatcher;
    ///
    ///     public UIElementRenderer(IContentHandlerDispatcher&lt;IUIElement&gt; dispatcher)
    ///     {
    ///         this.dispatcher = dispatcher;
    ///     }
    ///
    ///     public void RenderUI(IEnumerable&lt;IUIElement&gt; itemsToRender)
    ///     {
    ///         foreach(var item in itemsToRender)
    ///         {
    ///             // We are asking the dispatcher to dispatch the item to a handler in the UIElementRenderer.RendererClass
    ///             // not any other classes of handler that may be registered for this content type
    ///             this.dispatcher.DispatchPayloadToHandler(item, LoggerClass, "About to rendered");
    ///             this.dispatcher.DispatchPayloadToHandler(item, RendererClass);
    ///             this.dispatcher.DispatchPayloadToHandler(item, LoggerClass, "Rendered");
    ///         }
    ///     }
    /// }
    /// </code>
    /// </para>
    /// <para>
    /// Note that we have added a new handler class - the <c>LoggerClass</c> and that we've added a couple of lines to dispatch
    /// the payload to the registered loggers. This time you'll notice that we've added a parameter to the call to the dispatcher, too.
    /// </para>
    /// <para>
    /// So, let's create the logging handlers for this.
    /// </para>
    /// <para>
    /// <code>
    /// public class UIElementLogger : IContentHandler&lt;IUIElement, string&gt;
    /// {
    ///     private readonly ILogger&lt;IUIELement&gt; logger;
    ///
    ///     public UIElementLogger( ILogger&lt;IUIELement&gt; logger)
    ///     {
    ///         this.logger = logger;
    ///     }
    ///
    ///     public void Handle(IUIElement uiElement, string message)
    ///     {
    ///         this.logger.LogInformation($"{message}: {uiElement.ContentType}");
    ///     }
    /// }
    ///
    /// public class ButtonLogger : IContentHandler&lt;Button, string&gt;
    /// {
    ///     private readonly ILogger&lt;IUIELement&gt; logger;
    ///
    ///     public UIElementLogger( ILogger&lt;IUIELement&gt; logger)
    ///     {
    ///         this.logger = logger;
    ///     }
    ///
    ///     public void Handle(Button button, string message)
    ///     {
    ///         this.logger.LogInformation($"{message}: {button.ContentType} [{button.Caption}]");
    ///     }
    /// }
    /// </code>
    /// </para>
    /// <para>
    /// And, we can register these renderers as content handlers for the "UIElementRenderer.LoggerClass". Notice how we explicitly use the
    /// common root of the content type "application/vnd.Corvus.uielement" for our fallback <c>IUIElement</c> logger. This is our standard
    /// content fallback mechanism support by <see cref="ContentFactory"/>.
    /// </para>
    /// <para>
    /// <code>
    /// serviceCollection.RegisterContentHandler&lt;IUIElement, UIElementLogger, string&gt;("application/vnd.Corvus.uielement", UIElementRenderer.LoggerClass);
    /// serviceCollection.RegisterContentHandler&lt;IUIElement, Button, ButtonLogger, string&gt;(UIElementRenderer.LoggerClass);
    /// </code>
    /// </para>
    /// <para>
    /// There are additional overloads that support passing parameters alongside the payload; using a <see cref="Func{T}"/> or <see cref="Action"/> instead of the handler type; asynchronous handlers; and returning results from your handler.
    /// </para>
    /// <para>
    /// Commonly, you will want to enforce that your parameters, handler class and return types are all consistent for a particular family of handlers. One
    /// easy way to do this is to define a static class of extension methods, like this.
    /// </para>
    /// <para>
    /// <code>
    /// public static class ResultRendererHandlerExtensions
    /// {
    ///     public const string HandlerClass = "resultRenderers";
    ///
    ///     public static IServiceCollection RegisterResultRenderer&lt;TPayload&gt;(this IServiceCollection services, Func&lt;TPayload, string, string&gt; handler)
    ///         where TPayload : IUIElement
    ///     {
    ///         services.RegisterContentHandlerWithResult&lt;IUIElement, TPayload, string, string&gt;(handler, HandlerClass);
    ///         return services;
    ///     }
    ///
    ///     public static IServiceCollection RegisterResultRenderer&lt;TPayload, THandler&gt;(this IServiceCollection services)
    ///         where TPayload : IUIElement
    ///         where THandler : class, IContentHandlerWithResult&lt;TPayload, string, string&gt;
    ///     {
    ///         services.RegisterContentHandlerWithResult&lt;IUIElement, TPayload, THandler, string, string&gt;(HandlerClass);
    ///         return services;
    ///     }
    ///
    ///     public static string DispatchResultRenderer(this IContentHandlerDispatcher&lt;IUIElement&gt; dispatcher, IUIElement uiElement, string parameter)
    ///     {
    ///         return dispatcher.DispatchPayloadToHandler&lt;string, string&gt;(uiElement, HandlerClass, parameter);
    ///     }
    /// }
    /// </code>
    /// </para>
    /// <para>
    /// With this, your clients can now register handlers for this handler class with.
    /// </para>
    /// <para>
    /// <code>
    /// serviceCollection.RegisterResultRenderer&lt;Button&gt;(
    ///     (payload, source) =&gt; $"The result handled a Button  with caption: '{payload.Caption}' for '{source}'");
    ///
    /// serviceCollection.RegisterResultRenderer&lt;TextBox&gt;(
    ///     (payload, source) =&gt; $"The result handled a TextBox with text: '{payload.Text}' for '{source}'");
    /// </code>
    ///
    /// (or, if you created your handler as a class).
    ///
    /// <code>
    /// serviceCollection.RegisterResultRenderer&lt;TextBox, TextBoxRenderer&gt;():
    /// serviceCollection.RegisterResultRenderer&lt;Button, ButtonRenderer&gt;():
    /// </code>
    /// </para>
    /// <para>
    /// And you can dispatch to this family of handlers with:
    /// </para>
    /// <para>
    /// <code>
    /// dispatcher.DispatchResultRenderer(uiElement, "WhateverSourceWeSpecifyExampleDispatcher")
    /// </code>
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// namespace Corvus.Playground
    /// {
    ///     using System;
    ///     using Corvus.ContentHandling;
    ///     using Microsoft.Extensions.DependencyInjection;
    ///
    ///     /// &lt;summary&gt;
    ///     /// Playground
    ///     /// &lt;/summary&gt;
    ///     internal class Program
    ///     {
    ///         /// &lt;summary&gt;
    ///         /// Entry point
    ///         /// &lt;/summary&gt;
    ///         /// &lt;param name="args"&gt;Arguments&lt;/param&gt;
    ///         private static void Main(string[] args)
    ///         {
    ///             var serviceCollection = new ServiceCollection();
    ///
    ///             const string rendererHandlerClass = "renderers";
    ///             const string actionRendererHandlerClass = "actionRenderers";
    ///             const string resultRendererHandlerClass = "resultRenderers";
    ///
    ///             serviceCollection.RegisterContentHandler&lt;IUIElement, Button, ButtonHandler&gt;(rendererHandlerClass);
    ///             serviceCollection.RegisterContentHandler&lt;IUIElement, TextBox, TextBoxHandler&gt;(rendererHandlerClass);
    ///
    ///             serviceCollection.RegisterContentHandler&lt;IUIElement, Button&gt;(
    ///                 payload =&gt; Console.WriteLine($"The action handled a Button  with caption: '{payload.Caption}'"),
    ///                 actionRendererHandlerClass);
    ///
    ///             serviceCollection.RegisterContentHandler&lt;IUIElement, TextBox&gt;(
    ///                 payload =&gt; Console.WriteLine($"The action handled a TextBox with text: '{payload.Text}'"),
    ///                 actionRendererHandlerClass);
    ///
    ///             serviceCollection.RegisterContentHandlerWithResult&lt;IUIElement, Button, string, string&gt;(
    ///                 (payload, source) =&gt; $"The result handled a Button  with caption: '{payload.Caption}' for '{source}'",
    ///                 resultRendererHandlerClass);
    ///
    ///             serviceCollection.RegisterContentHandlerWithResult&lt;IUIElement, TextBox, string, string&gt;(
    ///                 (payload, source) =&gt; $"The result handled a TextBox with text: '{payload.Text}' for '{source}'",
    ///                 resultRendererHandlerClass);
    ///
    ///             var serviceProvider = serviceCollection.BuildServices();
    ///             var dispatcher = serviceProvider.GetRequiredService&lt;IContentHandlerDispatcher&lt;IUIElement&gt;&gt;();
    ///
    ///             var uiElements = new IUIElement[]
    ///             {
    ///                 new Button { Caption = "Hello world!" },
    ///                 new TextBox { Text = "What are you doing here?" },
    ///             };
    ///
    ///             foreach (var uiElement in uiElements)
    ///             {
    ///                 dispatcher.DispatchPayloadToHandler(uiElement, rendererHandlerClass);
    ///                 dispatcher.DispatchPayloadToHandler(uiElement, actionRendererHandlerClass);
    ///                 Console.WriteLine(dispatcher.DispatchPayloadToHandler&lt;string, string&gt;(uiElement, resultRendererHandlerClass, "ExampleDispatcher"));
    ///             }
    ///
    ///             Console.ReadKey();
    ///         }
    ///     }
    ///
    ///     public interface IUIElement
    ///     {
    ///         string ContentType { get; }
    ///     }
    ///
    ///     public class ButtonHandler : IContentHandler&lt;Button&gt;
    ///     {
    ///         public void Handle(Button payload)
    ///         {
    ///             Console.WriteLine($"The class handled a Button with caption '{payload.Caption}'");
    ///         }
    ///     }
    ///
    ///     public class TextBoxHandler : IContentHandler&lt;TextBox&gt;
    ///     {
    ///         public void Handle(TextBox payload)
    ///         {
    ///             Console.WriteLine($"The class handled a TextBox with text '{payload.Text}'");
    ///         }
    ///     }
    ///
    ///     public class Button : IUIElement
    ///     {
    ///         public const string contentFactoryType = "application/vnd.Corvus.uielement.button";
    ///
    ///         public string ContentType
    ///         {
    ///             get
    ///             {
    ///                 return contentFactoryType;
    ///             }
    ///         }
    ///
    ///         public string Caption { get; set; }
    ///     }
    ///
    ///     public class TextBox : IUIElement
    ///     {
    ///         public const string contentFactoryType = "application/vnd.Corvus.uielement.textbox";
    ///
    ///         public string ContentType
    ///         {
    ///             get
    ///             {
    ///                 return contentFactoryType;
    ///             }
    ///         }
    ///
    ///         public string Text { get; set; }
    ///     }
    /// }
    /// </code>
    /// </example>
    public interface IContentHandlerDispatcher<TPayloadBaseType>
    {
        /// <summary>
        /// Handle the given payload.
        /// </summary>
        /// <param name="payload">The payload to handle.</param>
        /// <param name="handlerClass">The class of handler (e.g. "view", "messageProcessor").</param>
        void DispatchPayloadToHandler(TPayloadBaseType payload, string handlerClass);

        /// <summary>
        /// Handle the given payload.
        /// </summary>
        /// <typeparam name="TParam1">The type of the first parameter.</typeparam>
        /// <param name="payload">The payload to handle.</param>
        /// <param name="handlerClass">The class of handler (e.g. "view", "messageProcessor").</param>
        /// <param name="param1">The first parameter.</param>
        void DispatchPayloadToHandler<TParam1>(TPayloadBaseType payload, string handlerClass, TParam1 param1);

        /// <summary>
        /// Handle the given payload.
        /// </summary>
        /// <typeparam name="TParam1">The type of the first parameter.</typeparam>
        /// <typeparam name="TParam2">The type of the second parameter.</typeparam>
        /// <param name="payload">The payload to handle.</param>
        /// <param name="handlerClass">The class of handler (e.g. "view", "messageProcessor").</param>
        /// <param name="param1">The first parameter.</param>
        /// <param name="param2">The second parameter.</param>
        void DispatchPayloadToHandler<TParam1, TParam2>(TPayloadBaseType payload, string handlerClass, TParam1 param1, TParam2 param2);

        /// <summary>
        /// Handle the given payload.
        /// </summary>
        /// <typeparam name="TParam1">The type of the first parameter.</typeparam>
        /// <typeparam name="TParam2">The type of the second parameter.</typeparam>
        /// <typeparam name="TParam3">The type of the third parameter.</typeparam>
        /// <param name="payload">The payload to handle.</param>
        /// <param name="handlerClass">The class of handler (e.g. "view", "messageProcessor").</param>
        /// <param name="param1">The first parameter.</param>
        /// <param name="param2">The second parameter.</param>
        /// <param name="param3">The thirdparameter.</param>
        void DispatchPayloadToHandler<TParam1, TParam2, TParam3>(TPayloadBaseType payload, string handlerClass, TParam1 param1, TParam2 param2, TParam3 param3);

        /// <summary>
        /// Handle the given payload.
        /// </summary>
        /// <param name="payload">The payload to handle.</param>
        /// <param name="contentType">The content type of the handler to retrieve.</param>
        /// <param name="handlerClass">The class of handler (e.g. "view", "messageProcessor").</param>
        void DispatchPayloadToHandler(TPayloadBaseType payload, string contentType, string handlerClass);

        /// <summary>
        /// Handle the given payload.
        /// </summary>
        /// <typeparam name="TParam1">The type of the first parameter.</typeparam>
        /// <param name="payload">The payload to handle.</param>
        /// <param name="contentType">The content type of the handler to retrieve.</param>
        /// <param name="handlerClass">The class of handler (e.g. "view", "messageProcessor").</param>
        /// <param name="param1">The first parameter.</param>
        void DispatchPayloadToHandler<TParam1>(TPayloadBaseType payload, string contentType, string handlerClass, TParam1 param1);

        /// <summary>
        /// Handle the given payload.
        /// </summary>
        /// <typeparam name="TParam1">The type of the first parameter.</typeparam>
        /// <typeparam name="TParam2">The type of the second parameter.</typeparam>
        /// <param name="payload">The payload to handle.</param>
        /// <param name="contentType">The content type of the handler to retrieve.</param>
        /// <param name="handlerClass">The class of handler (e.g. "view", "messageProcessor").</param>
        /// <param name="param1">The first parameter.</param>
        /// <param name="param2">The second parameter.</param>
        void DispatchPayloadToHandler<TParam1, TParam2>(TPayloadBaseType payload, string contentType, string handlerClass, TParam1 param1, TParam2 param2);

        /// <summary>
        /// Handle the given payload.
        /// </summary>
        /// <typeparam name="TParam1">The type of the first parameter.</typeparam>
        /// <typeparam name="TParam2">The type of the second parameter.</typeparam>
        /// <typeparam name="TParam3">The type of the third parameter.</typeparam>
        /// <param name="payload">The payload to handle.</param>
        /// <param name="contentType">The content type of the handler to retrieve.</param>
        /// <param name="handlerClass">The class of handler (e.g. "view", "messageProcessor").</param>
        /// <param name="param1">The first parameter.</param>
        /// <param name="param2">The second parameter.</param>
        /// <param name="param3">The third parameter.</param>
        void DispatchPayloadToHandler<TParam1, TParam2, TParam3>(TPayloadBaseType payload, string contentType, string handlerClass, TParam1 param1, TParam2 param2, TParam3 param3);

        /// <summary>
        /// Handle the given payload.
        /// </summary>
        /// <param name="payload">The payload to handle.</param>
        /// <param name="handlerClass">The class of handler (e.g. "view", "messageProcessor").</param>
        /// <returns>A <see cref="Task"/> which completes when the content is handled.</returns>
        Task DispatchPayloadToHandlerAsync(TPayloadBaseType payload, string handlerClass);

        /// <summary>
        /// Handle the given payload.
        /// </summary>
        /// <typeparam name="TParam1">The type of the first parameter.</typeparam>
        /// <param name="payload">The payload to handle.</param>
        /// <param name="handlerClass">The class of handler (e.g. "view", "messageProcessor").</param>
        /// <param name="param1">The first parameter.</param>
        /// <returns>A <see cref="Task"/> which completes when the content is handled.</returns>
        Task DispatchPayloadToHandlerAsync<TParam1>(TPayloadBaseType payload, string handlerClass, TParam1 param1);

        /// <summary>
        /// Handle the given payload.
        /// </summary>
        /// <typeparam name="TParam1">The type of the first parameter.</typeparam>
        /// <typeparam name="TParam2">The type of the second parameter.</typeparam>
        /// <param name="payload">The payload to handle.</param>
        /// <param name="handlerClass">The class of handler (e.g. "view", "messageProcessor").</param>
        /// <param name="param1">The first parameter.</param>
        /// <param name="param2">The second parameter.</param>
        /// <returns>A <see cref="Task"/> which completes when the content is handled.</returns>
        Task DispatchPayloadToHandlerAsync<TParam1, TParam2>(TPayloadBaseType payload, string handlerClass, TParam1 param1, TParam2 param2);

        /// <summary>
        /// Handle the given payload.
        /// </summary>
        /// <typeparam name="TParam1">The type of the first parameter.</typeparam>
        /// <typeparam name="TParam2">The type of the second parameter.</typeparam>
        /// <typeparam name="TParam3">The type of the third parameter.</typeparam>
        /// <param name="payload">The payload to handle.</param>
        /// <param name="handlerClass">The class of handler (e.g. "view", "messageProcessor").</param>
        /// <param name="param1">The first parameter.</param>
        /// <param name="param2">The second parameter.</param>
        /// <param name="param3">The third parameter.</param>
        /// <returns>A <see cref="Task"/> which completes when the content is handled.</returns>
        Task DispatchPayloadToHandlerAsync<TParam1, TParam2, TParam3>(TPayloadBaseType payload, string handlerClass, TParam1 param1, TParam2 param2, TParam3 param3);

        /// <summary>
        /// Handle the given payload.
        /// </summary>
        /// <param name="payload">The payload to handle.</param>
        /// <param name="contentType">The content type of the handler to retrieve.</param>
        /// <param name="handlerClass">The class of handler (e.g. "view", "messageProcessor").</param>
        /// <returns>A <see cref="Task"/> which completes when the content is handled.</returns>
        Task DispatchPayloadToHandlerAsync(TPayloadBaseType payload, string contentType, string handlerClass);

        /// <summary>
        /// Handle the given payload.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="payload">The payload to handle.</param>
        /// <param name="handlerClass">The class of handler (e.g. "view", "messageProcessor").</param>
        /// <returns>The result of the handler.</returns>
        TResult DispatchPayloadToHandler<TResult>(TPayloadBaseType payload, string handlerClass);

        /// <summary>
        /// Handle the given payload.
        /// </summary>
        /// <typeparam name="TParam1">The type of the first parameter.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="payload">The payload to handle.</param>
        /// <param name="handlerClass">The class of handler (e.g. "view", "messageProcessor").</param>
        /// <param name="param1">The first parameter.</param>
        /// <returns>The result of the handler.</returns>
        TResult DispatchPayloadToHandler<TParam1, TResult>(TPayloadBaseType payload, string handlerClass, TParam1 param1);

        /// <summary>
        /// Handle the given payload.
        /// </summary>
        /// <typeparam name="TParam1">The type of the first parameter.</typeparam>
        /// <typeparam name="TParam2">The type of the second parameter.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="payload">The payload to handle.</param>
        /// <param name="handlerClass">The class of handler (e.g. "view", "messageProcessor").</param>
        /// <param name="param1">The first parameter.</param>
        /// <param name="param2">The second parameter.</param>
        /// <returns>The result of the handler.</returns>
        TResult DispatchPayloadToHandler<TParam1, TParam2, TResult>(TPayloadBaseType payload, string handlerClass, TParam1 param1, TParam2 param2);

        /// <summary>
        /// Handle the given payload.
        /// </summary>
        /// <typeparam name="TParam1">The type of the first parameter.</typeparam>
        /// <typeparam name="TParam2">The type of the second parameter.</typeparam>
        /// <typeparam name="TParam3">The type of the third parameter.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="payload">The payload to handle.</param>
        /// <param name="handlerClass">The class of handler (e.g. "view", "messageProcessor").</param>
        /// <param name="param1">The first parameter.</param>
        /// <param name="param2">The second parameter.</param>
        /// <param name="param3">The thirdparameter.</param>
        /// <returns>The result of the handler.</returns>
        TResult DispatchPayloadToHandler<TParam1, TParam2, TParam3, TResult>(TPayloadBaseType payload, string handlerClass, TParam1 param1, TParam2 param2, TParam3 param3);

        /// <summary>
        /// Handle the given payload.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="payload">The payload to handle.</param>
        /// <param name="contentType">The content type of the handler to retrieve.</param>
        /// <param name="handlerClass">The class of handler (e.g. "view", "messageProcessor").</param>
        /// <returns>The result of the handler.</returns>
        TResult DispatchPayloadToHandler<TResult>(TPayloadBaseType payload, string contentType, string handlerClass);

        /// <summary>
        /// Handle the given payload.
        /// </summary>
        /// <typeparam name="TParam1">The type of the first parameter.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="payload">The payload to handle.</param>
        /// <param name="contentType">The content type of the handler to retrieve.</param>
        /// <param name="handlerClass">The class of handler (e.g. "view", "messageProcessor").</param>
        /// <param name="param1">The first parameter.</param>
        /// <returns>The result of the handler.</returns>
        TResult DispatchPayloadToHandler<TParam1, TResult>(TPayloadBaseType payload, string contentType, string handlerClass, TParam1 param1);

        /// <summary>
        /// Handle the given payload.
        /// </summary>
        /// <typeparam name="TParam1">The type of the first parameter.</typeparam>
        /// <typeparam name="TParam2">The type of the second parameter.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="payload">The payload to handle.</param>
        /// <param name="contentType">The content type of the handler to retrieve.</param>
        /// <param name="handlerClass">The class of handler (e.g. "view", "messageProcessor").</param>
        /// <param name="param1">The first parameter.</param>
        /// <param name="param2">The second parameter.</param>
        /// <returns>The result of the handler.</returns>
        TResult DispatchPayloadToHandler<TParam1, TParam2, TResult>(TPayloadBaseType payload, string contentType, string handlerClass, TParam1 param1, TParam2 param2);

        /// <summary>
        /// Handle the given payload.
        /// </summary>
        /// <typeparam name="TParam1">The type of the first parameter.</typeparam>
        /// <typeparam name="TParam2">The type of the second parameter.</typeparam>
        /// <typeparam name="TParam3">The type of the third parameter.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="payload">The payload to handle.</param>
        /// <param name="contentType">The content type of the handler to retrieve.</param>
        /// <param name="handlerClass">The class of handler (e.g. "view", "messageProcessor").</param>
        /// <param name="param1">The first parameter.</param>
        /// <param name="param2">The second parameter.</param>
        /// <param name="param3">The third parameter.</param>
        /// <returns>The result of the handler.</returns>
        TResult DispatchPayloadToHandler<TParam1, TParam2, TParam3, TResult>(TPayloadBaseType payload, string contentType, string handlerClass, TParam1 param1, TParam2 param2, TParam3 param3);

        /// <summary>
        /// Handle the given payload.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="payload">The payload to handle.</param>
        /// <param name="handlerClass">The class of handler (e.g. "view", "messageProcessor").</param>
        /// <returns>A <see cref="Task{TResult}"/> which completes when the content is handled.</returns>
        Task<TResult> DispatchPayloadToHandlerAsync<TResult>(TPayloadBaseType payload, string handlerClass);

        /// <summary>
        /// Handle the given payload.
        /// </summary>
        /// <typeparam name="TParam1">The type of the first parameter.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="payload">The payload to handle.</param>
        /// <param name="handlerClass">The class of handler (e.g. "view", "messageProcessor").</param>
        /// <param name="param1">The first parameter.</param>
        /// <returns>A <see cref="Task{TResult}"/> which completes when the content is handled.</returns>
        Task<TResult> DispatchPayloadToHandlerAsync<TParam1, TResult>(TPayloadBaseType payload, string handlerClass, TParam1 param1);

        /// <summary>
        /// Handle the given payload.
        /// </summary>
        /// <typeparam name="TParam1">The type of the first parameter.</typeparam>
        /// <typeparam name="TParam2">The type of the second parameter.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="payload">The payload to handle.</param>
        /// <param name="handlerClass">The class of handler (e.g. "view", "messageProcessor").</param>
        /// <param name="param1">The first parameter.</param>
        /// <param name="param2">The second parameter.</param>
        /// <returns>A <see cref="Task{TResult}"/> which completes when the content is handled.</returns>
        Task<TResult> DispatchPayloadToHandlerAsync<TParam1, TParam2, TResult>(TPayloadBaseType payload, string handlerClass, TParam1 param1, TParam2 param2);

        /// <summary>
        /// Handle the given payload.
        /// </summary>
        /// <typeparam name="TParam1">The type of the first parameter.</typeparam>
        /// <typeparam name="TParam2">The type of the second parameter.</typeparam>
        /// <typeparam name="TParam3">The type of the third parameter.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="payload">The payload to handle.</param>
        /// <param name="handlerClass">The class of handler (e.g. "view", "messageProcessor").</param>
        /// <param name="param1">The first parameter.</param>
        /// <param name="param2">The second parameter.</param>
        /// <param name="param3">The third parameter.</param>
        /// <returns>A <see cref="Task{TResult}"/> which completes when the content is handled.</returns>
        Task<TResult> DispatchPayloadToHandlerAsync<TParam1, TParam2, TParam3, TResult>(TPayloadBaseType payload, string handlerClass, TParam1 param1, TParam2 param2, TParam3 param3);

        /// <summary>
        /// Handle the given payload.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="payload">The payload to handle.</param>
        /// <param name="contentType">The content type of the handler to retrieve.</param>
        /// <param name="handlerClass">The class of handler (e.g. "view", "messageProcessor").</param>
        /// <returns>A <see cref="Task{TResult}"/> which completes when the content is handled.</returns>
        Task<TResult> DispatchPayloadToHandlerAsync<TResult>(TPayloadBaseType payload, string contentType, string handlerClass);

        /// <summary>
        /// Handle the given payload.
        /// </summary>
        /// <typeparam name="TParam1">The type of the first parameter.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="payload">The payload to handle.</param>
        /// <param name="contentType">The content type of the handler to retrieve.</param>
        /// <param name="handlerClass">The class of handler (e.g. "view", "messageProcessor").</param>
        /// <param name="param1">The first parameter.</param>
        /// <returns>A <see cref="Task{TResult}"/> which completes when the content is handled.</returns>
        Task<TResult> DispatchPayloadToHanderAsync<TParam1, TResult>(TPayloadBaseType payload, string contentType, string handlerClass, TParam1 param1);

        /// <summary>
        /// Handle the given payload.
        /// </summary>
        /// <typeparam name="TParam1">The type of the first parameter.</typeparam>
        /// <typeparam name="TParam2">The type of the second parameter.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="payload">The payload to handle.</param>
        /// <param name="contentType">The content type of the handler to retrieve.</param>
        /// <param name="handlerClass">The class of handler (e.g. "view", "messageProcessor").</param>
        /// <param name="param1">The first parameter.</param>
        /// <param name="param2">The second parameter.</param>
        /// <returns>A <see cref="Task{TResult}"/> which completes when the content is handled.</returns>
        Task<TResult> DispatchPayloadToHandlerAsync<TParam1, TParam2, TResult>(TPayloadBaseType payload, string contentType, string handlerClass, TParam1 param1, TParam2 param2);

        /// <summary>
        /// Handle the given payload.
        /// </summary>
        /// <typeparam name="TParam1">The type of the first parameter.</typeparam>
        /// <typeparam name="TParam2">The type of the second parameter.</typeparam>
        /// <typeparam name="TParam3">The type of the third parameter.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="payload">The payload to handle.</param>
        /// <param name="contentType">The content type of the handler to retrieve.</param>
        /// <param name="handlerClass">The class of handler (e.g. "view", "messageProcessor").</param>
        /// <param name="param1">The first parameter.</param>
        /// <param name="param2">The second parameter.</param>
        /// <param name="param3">The third parameter.</param>
        /// <returns>A <see cref="Task{TResult}"/> which completes when the content is handled.</returns>
        Task<TResult> DispatchPayloadToHandlerAsync<TParam1, TParam2, TParam3, TResult>(TPayloadBaseType payload, string contentType, string handlerClass, TParam1 param1, TParam2 param2, TParam3 param3);
    }
}