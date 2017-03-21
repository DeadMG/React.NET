using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React.Core
{
    public class SolidColourElementProps : PrimitiveProps
    {
        public SolidColourElementProps(Colour colour, Func<Bounds, Bounds> location, Action<MouseEvent, Bounds> onMouseClick = null, Action<KeyboardEvent> onKeyboard = null)
            : base(onMouseClick, onKeyboard)
        {
            this.Colour = colour;
            this.Location = location;
        }

        public SolidColourElementProps(Colour colour, int width, int height, Action<MouseEvent, Bounds> onMouseClick = null, Action<KeyboardEvent> onKeyboard = null)
            : base(onMouseClick, onKeyboard)
        {
            this.Colour = colour;
            this.Location = (bounds) => new Bounds(x: bounds.X, y: bounds.Y, width: width, height: height);
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

        public IElementState Update(IElementState existing, RenderContext context)
        {
            return context.Renderer.UpdateSolidColourElementState(existing, this, context);
        }
    }
}
