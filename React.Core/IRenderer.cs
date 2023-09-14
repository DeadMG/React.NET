namespace React.Core
{
    public interface IRenderer
    {
        ITextElementState UpdateTextElementState(IElementState existing, TextElement element, RenderContext context, Bounds bounds);
        IElementState UpdateSolidColourElementState(IElementState existing, SolidColourElement element, RenderContext context, Bounds bounds);
        IElementState UpdateImageElementState(IElementState existing, ImageElement element, RenderContext context, Bounds bounds);

        void RenderFrame(IElementState root);
    }
}
