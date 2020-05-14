namespace Corvus.ContentHandling.DemoHandlerWithClasses
{
    using System;
    using Corvus.ContentHandling;

    public class TextBoxHandler : IContentHandler<TextBox>
    {
        public void Handle(TextBox payload)
        {
            Console.WriteLine($"The TextBoxHandler class handled a TextBox with text '{payload.Text}'");
        }
    }
}