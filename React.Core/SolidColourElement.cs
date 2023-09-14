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

    public class SolidColourElement : IElement
    {
        public SolidColourElement(SolidColourElementProps props, IElement child)
        {
            this.Props = props;
            this.Child = child;
        }

        public SolidColourElementProps Props { get; }
        public IElement Child { get; }

        public IElementState Update(IElementState existing, RenderContext context, Bounds bounds)
        {
            return context.Renderer.UpdateSolidColourElementState(existing, this, context, bounds);
        }
    }
}
