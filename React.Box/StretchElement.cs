using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using React.Core;

namespace React.Box
{
    public class StretchElement : Element<StretchElementState, StretchElement>
    {
        public StretchElement(IElement child)
        {
            this.Child = child;
        }

        public IElement Child { get; }
    }

    public class StretchElementState : IUpdatableElementState<StretchElement>
    {
        private IElementState nestedState;

        public StretchElementState(StretchElement element, UpdateContext context)
        {
            nestedState = element.Child.Update(null, context);
            BoundingBox = context.Bounds;
        }

        public Bounds BoundingBox { get; set; }

        public void Dispose()
        {
            nestedState.Dispose();
        }

        public void OnMouseClick(ClickEvent click)
        {
            if (nestedState.BoundingBox.IsInBounds(click))
                nestedState.OnMouseClick(click);
        }

        public void Render(IRenderer r)
        {
            nestedState.Render(r);
        }

        public void Update(StretchElement other, UpdateContext context)
        {
            BoundingBox = context.Bounds;
            nestedState = other.Child.Update(nestedState, context);
        }
    }
}
