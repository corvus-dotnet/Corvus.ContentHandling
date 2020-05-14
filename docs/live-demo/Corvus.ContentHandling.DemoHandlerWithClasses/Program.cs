namespace Corvus.ContentHandling.DemoHandlerWithClasses
{
    using System;
    using Corvus.ContentHandling;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Playground
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Entry point
        /// </summary>
        /// <param name="args">Arguments</param>
        private static void Main(string[] args)
        {
            IServiceProvider serviceProvider = BuildServiceProvider();

            var dispatcher = serviceProvider.GetRequiredService<IContentHandlerDispatcher<IUIElement>>();

            // Imagine this was a e.g. a deserialized DOM model.
            var uiElements = new IUIElement[]
            {
                new Button { Caption = "Hello world!" },
                new TextBox { Text = "What are you doing here?" },
                new MysteryElement(),
            };

            foreach (var uiElement in uiElements)
            {
                dispatcher.DispatchPayloadToHandler(uiElement, Renderers.HandlerClass);
            }
        }

        private static IServiceProvider BuildServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddContent(factory =>
            {
                // This registers the class-based handlers
                factory.RegisterContentHandler<IUIElement, Button, ButtonHandler>(Renderers.HandlerClass);
                factory.RegisterContentHandler<IUIElement, TextBox, TextBoxHandler>(Renderers.HandlerClass);
                // This registers our fallback handler
                factory.RegisterContentHandler<IUIElement, FallbackHandler>(UIElementContentType.Base, Renderers.HandlerClass);
            });

            return serviceCollection.BuildServiceProvider();
        }
    }
}