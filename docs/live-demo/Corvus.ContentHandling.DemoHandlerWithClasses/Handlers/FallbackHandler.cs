namespace Corvus.ContentHandling.DemoHandlerWithClasses
{
    using System;
    using Corvus.ContentHandling;

    public class FallbackHandler : IContentHandler<IUIElement>
    {
        public void Handle(IUIElement payload)
        {
            Console.WriteLine($"The FallbackHandler class handled a UIElement with content type '{payload.ContentType}'");
        }
    }
}