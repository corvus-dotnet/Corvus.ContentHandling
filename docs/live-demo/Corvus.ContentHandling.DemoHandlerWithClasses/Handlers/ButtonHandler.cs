namespace Corvus.ContentHandling.DemoHandlerWithClasses
{
    using System;
    using Corvus.ContentHandling;

    // These are the class-based handlers
    public class ButtonHandler : IContentHandler<Button>
    {
        public void Handle(Button payload)
        {
            Console.WriteLine($"The ButtonHandler class handled a Button with caption '{payload.Caption}'");
        }
    }
}