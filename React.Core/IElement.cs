namespace React.Core
{
    public interface IElement<out ElementState>
        where ElementState : IElementState
    {
        ElementState Update(IElementState existing, RenderContext renderContext, Bounds bounds);
    }
}
