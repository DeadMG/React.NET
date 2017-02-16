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
        public Colour Colour { get; set; }
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

    public class BackgroundElementState : IUpdatableElementState<BackgroundElement>
    {
        private IElementState solidColourState;
        private IElementState nestedState;
        private Action<ClickEvent> onMouseClick;

        public BackgroundElementState(BackgroundElement other, UpdateContext context)
        {
            nestedState = other.Child.Update(null, context);
            onMouseClick = other.Props.OnMouseClick;
            solidColourState = context.Renderer.UpdateSolidColourElementState(solidColourState, context.Bounds, new SolidColourElement(new SolidColourElementProps(other.Props.Colour, b => nestedState.BoundingBox)), context.Context);
        }

        public Bounds BoundingBox => nestedState.BoundingBox;

        public void Dispose()
        {
            solidColourState.Dispose();
            nestedState.Dispose();
        }

        public void OnMouseClick(ClickEvent click)
        {
            if (nestedState.BoundingBox.IsInBounds(click))
                nestedState.OnMouseClick(click);
            onMouseClick?.Invoke(click);
        }

        public void Render(IRenderer r)
        {
            solidColourState.Render(r);
            nestedState.Render(r);
        }

        public void Update(BackgroundElement other, UpdateContext context)
        {
            onMouseClick = other.Props.OnMouseClick;
            nestedState = other.Child.Update(nestedState, context);
            solidColourState = context.Renderer.UpdateSolidColourElementState(solidColourState, context.Bounds, new SolidColourElement(new SolidColourElementProps(other.Props.Colour, b => nestedState.BoundingBox)), context.Context);
        }
    }
}
