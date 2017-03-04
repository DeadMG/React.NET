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
        public BackgroundElementProps(Colour colour, Action<MouseEvent, Bounds> onMouse = null)
            : base(onMouse)
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

    public class BackgroundElementState : PrimitiveElementState
    {
        private readonly IElementState solidColourState;
        private readonly IElementState nestedState;

        public BackgroundElementState(BackgroundElementState existingState, BackgroundElement other, UpdateContext context)
            : base(existingState, other.Props, context)
        {
            nestedState = other.Child.Update(existingState?.nestedState, context);
            solidColourState = context.Renderer.UpdateSolidColourElementState(existingState?.solidColourState, context.Bounds, new SolidColourElement(new SolidColourElementProps(other.Props.Colour, b => nestedState.BoundingBox)), context.Context, context.EventSource);
        }

        public override Bounds BoundingBox => nestedState.BoundingBox;

        public override void Dispose()
        {
            solidColourState.Dispose();
            nestedState.Dispose();
            base.Dispose();
        }

        public override void Render(IRenderer r)
        {
            solidColourState.Render(r);
            nestedState.Render(r);
        }
    }
}
