namespace React.Core
{
    public interface IKnownSizeElement<out ElementState>
        where ElementState : IKnownSizeElementState
    {
        ElementState Update(IKnownSizeElementState existing, RenderContext renderContext);
    }
}
