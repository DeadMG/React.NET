using React.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React.Box
{
    public class OverlayElementProps
    {
        public OverlayElementProps(IElement child, IElement overlay)
        {
            this.Overlay = overlay;
            this.Child = child;
        }

        public IElement Child { get; }
        public IElement Overlay { get; }
    }

    public class OverlayElement : Element<OverlayElementState, OverlayElement>
    {
        public OverlayElement(OverlayElementProps props)
        {
            this.Props = props;
        }

        public OverlayElementProps Props { get; }
    }

    public class OverlayElementState : IUpdatableElementState<OverlayElement>
    {
        private IElementState child;
        private IElementState overlay;

        public OverlayElementState(OverlayElement element, UpdateContext context)
        {
            child = element.Props.Child.Update(null, context);
            overlay = element.Props.Overlay.Update(null, context);
        }

        public Bounds BoundingBox => child.BoundingBox;

        public void Dispose()
        {
            child?.Dispose();
            overlay?.Dispose();
        }

        public void OnMouseClick(LeftMouseUpEvent click)
        {
            child.OnMouseClick(click);
        }

        public void Render(IRenderer r)
        {
            child?.Render(r);
            overlay?.Render(r);
        }

        public void Update(OverlayElement other, UpdateContext context)
        {
            child = other.Props.Child.Update(child, context);
            overlay = other.Props.Overlay.Update(overlay, context);
        }
    }
}
