namespace Corvus.ContentHandling.DemoHandlerWithClasses
{
    /// <summary>
    /// These are the types to be handled by the handlers
    /// </summary>
    #region button
    public class Button : IUIElement
    {
        public const string RegisteredContentType = UIElementContentType.Base + ".button";
        public string ContentType
        {
            get
            {
                return RegisteredContentType;
            }
        }
        public string Caption { get; set; }
    }
    #endregion
}