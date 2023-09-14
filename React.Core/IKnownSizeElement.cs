namespace React.Core
{
    public interface IKnownSizeElement
    {
        IKnownSizeElementState Update(IKnownSizeElementState existing, RenderContext renderContext);
    }
}
