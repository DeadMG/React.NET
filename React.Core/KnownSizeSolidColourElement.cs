namespace React.Core
{
    public class KnownSizeSolidColourElementProps
    {
        public KnownSizeSolidColourElementProps(Colour colour, Dimensions size)
        {
            this.Colour = colour;
            this.Size = size;
        }

        public Dimensions Size { get; }
        public Colour Colour { get; }
    }

    public class KnownSizeSolidColourElement : IKnownSizeElement<IKnownSizeElementState>
    {
        public KnownSizeSolidColourElement(KnownSizeSolidColourElementProps props)
        {
            this.Props = props;
        }

        public KnownSizeSolidColourElementProps Props { get; }

        public IKnownSizeElementState Update(IKnownSizeElementState existing, RenderContext renderContext)
        {
            return renderContext.Renderer.UpdateSolidColourElementState(existing, this, renderContext);
        }
    }
}
