namespace React.Core
{
    public class SolidColourElementProps
    {
        public SolidColourElementProps(Colour colour)
        {
            this.Colour = colour;
        }
        
        public Colour Colour { get; }
    }

    public class SolidColourElement : IElement<IElementState>
    {
        public SolidColourElement(SolidColourElementProps props, IElement<IElementState> child)
        {
            this.Props = props;
            this.Child = child;
        }

        public SolidColourElementProps Props { get; }
        public IElement<IElementState> Child { get; }

        public IElementState Update(IElementState existing, RenderContext context, Bounds bounds)
        {
            return context.Renderer.UpdateSolidColourElementState(existing, this, context, bounds);
        }
    }
}
