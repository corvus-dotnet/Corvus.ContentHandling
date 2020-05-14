namespace Corvus.ContentHandling.DemoHandlerWithClasses
{
    #region textbox
    public class TextBox : IUIElement
    {
        public const string RegisteredContentType = UIElementContentType.Base + ".textbox";
        public string ContentType
        {
            get
            {
                return RegisteredContentType;
            }
        }
        public string Text { get; set; }
    }
    #endregion
}