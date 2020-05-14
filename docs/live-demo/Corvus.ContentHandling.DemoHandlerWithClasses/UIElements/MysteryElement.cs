namespace Corvus.ContentHandling.DemoHandlerWithClasses
{
    public class MysteryElement : IUIElement
    {
        public const string RegisteredContentType = UIElementContentType.Base + ".mysteryelement";
        public string ContentType
        {
            get
            {
                return RegisteredContentType;
            }
        }
    }
}