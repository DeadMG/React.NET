using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React.Core
{
    public class SolidColourElementProps : PrimitiveProps
    {
        public SolidColourElementProps(Colour colour, Func<Bounds, Bounds> location, Action<ClickEvent> onMouseClick = null)
            : base(onMouseClick)
        {
            this.Colour = colour;
            this.Location = location;
        }

        public Colour Colour { get; }
        public Func<Bounds, Bounds> Location { get; }
    }

    public class SolidColourElement : IElement
    {
        public SolidColourElement(SolidColourElementProps props)
        {
            this.Props = props;
        }

        public SolidColourElementProps Props { get; }

        public IElementState Update(IElementState existing, UpdateContext context)
        {
            return context.Renderer.UpdateSolidColourElementState(existing, context.Bounds, this, context.Context);
        }
    }
}
