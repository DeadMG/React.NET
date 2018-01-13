using React.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React.Box
{
    public class BackgroundElementProps
    {
        public BackgroundElementProps(Colour colour)
        {
            this.Colour = colour;
        }

        public Colour Colour { get; }
    }

    public class BackgroundElement : Element<BackgroundElementState, BackgroundElement>
    {
        public BackgroundElement(BackgroundElementProps props, IElement<IElementState> child)
        {
            this.Props = props;
            this.Child = child;
        }
        
        public BackgroundElementProps Props { get; }
        public IElement<IElementState> Child { get; }
    }

    public class BackgroundElementState : IElementState
    {
        private readonly IElementState solidColourState;
        private readonly IElementState nestedState;
        private readonly BackgroundElementProps props;

        public BackgroundElementState(BackgroundElementState existingState, BackgroundElement other, RenderContext context)
        {
            props = other.Props;
            nestedState = other.Child.Update(existingState?.nestedState, context);
            solidColourState = context.Renderer.UpdateSolidColourElementState(existingState?.solidColourState, new SolidColourElement(new SolidColourElementProps(other.Props.Colour, b => nestedState.BoundingBox)), context);
            
        }

        public Bounds BoundingBox => nestedState.BoundingBox;
        
        public void Render(IRenderer r)
        {
            solidColourState.Render(r);
            nestedState.Render(r);
        }

        public void FireEvents(IReadOnlyList<IEvent> events)
        {
            solidColourState.FireEvents(events);
            nestedState.FireEvents(events);
        }
    }
}
