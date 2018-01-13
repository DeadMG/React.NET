using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React.Core
{
    public class SolidColourElementProps
    {
        public SolidColourElementProps(Colour colour, Func<Bounds, Bounds> location)
        {
            this.Colour = colour;
            this.Location = location;
        }

        public SolidColourElementProps(Colour colour, int width, int height)
            : this(colour, (bounds) => new Bounds(x: bounds.X, y: bounds.Y, width: width, height: height))
        {
        }

        public Colour Colour { get; }
        public Func<Bounds, Bounds> Location { get; }
    }

    public class SolidColourElement : IElement<ISolidColourElementState>
    {
        public SolidColourElement(SolidColourElementProps props)
        {
            this.Props = props;
        }

        public SolidColourElementProps Props { get; }

        public ISolidColourElementState Update(IElementState existing, RenderContext context, Bounds bounds)
        {
            return context.Renderer.UpdateSolidColourElementState(existing, this, context, bounds);
        }
    }
}
