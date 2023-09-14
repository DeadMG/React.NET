namespace React.Core
{
    public interface IElement
    {
        IElementState Update(IElementState existing, RenderContext renderContext, Bounds bounds);
    }
}
