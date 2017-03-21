using React.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React.Box
{
    public class BackgroundElementProps : PrimitiveProps
    {
        public BackgroundElementProps(Colour colour, Action<MouseEvent, Bounds> onMouse = null, Action<KeyboardEvent> onKeyboard = null)
            : base(onMouse, onKeyboard)
        {
            this.Colour = colour;
        }

        public Colour Colour { get; }
    }

    public class BackgroundElement : Element<BackgroundElementState, BackgroundElement>
    {
        public BackgroundElement(BackgroundElementProps props, IElement child)
        {
            this.Props = props;
            this.Child = child;
        }
        
        public BackgroundElementProps Props { get; }
        public IElement Child { get; }
    }

    public class BackgroundElementState : IElementState
    {
        private readonly IElementState solidColourState;
        private readonly IElementState nestedState;

        public BackgroundElementState(BackgroundElementState existingState, BackgroundElement other, RenderContext context)
        {
            Element = other;
            nestedState = other.Child.Update(existingState?.nestedState, context);
            solidColourState = context.Renderer.UpdateSolidColourElementState(existingState?.solidColourState, new SolidColourElement(new SolidColourElementProps(other.Props.Colour, b => nestedState.BoundingBox)), context);
            PrimitivePropsHelpers.FireEvents(other.Props, nestedState.BoundingBox, context.Events);
        }

        public Bounds BoundingBox => nestedState.BoundingBox;
        
        public void Render(IRenderer r)
        {
            solidColourState.Render(r);
            nestedState.Render(r);
        }

        public BackgroundElement Element { get; }
    }
}
