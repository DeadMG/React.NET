using React.Core;
using System.Collections.Generic;

namespace React.Box
{
    public class OverlayElementProps
    {
        public OverlayElementProps(IElement<IElementState> child, IElement<IElementState> overlay)
        {
            this.Overlay = overlay;
            this.Child = child;
        }

        public IElement<IElementState> Child { get; }
        public IElement<IElementState> Overlay { get; }
    }

    public class OverlayElement : Element<OverlayElementState, OverlayElement>
    {
        public OverlayElement(OverlayElementProps props)
        {
            this.Props = props;
        }

        public OverlayElementProps Props { get; }
    }

    public class OverlayElementState : IElementState
    {
        private readonly IElementState child;
        private readonly IElementState overlay;

        public OverlayElementState(OverlayElementState existing, OverlayElement element, RenderContext context, Bounds bounds)
        {
            child = element.Props.Child.Update(existing?.child, context, bounds);
            overlay = element.Props.Overlay.Update(existing?.overlay, context, bounds);
        }

        public Bounds BoundingBox => child.BoundingBox;

        public void FireEvents(IReadOnlyList<IEvent> events)
        {
            child.FireEvents(events);
            overlay.FireEvents(events);
        }
        
        public void Render(IRenderer r)
        {
            child?.Render(r);
            overlay?.Render(r);
        }
    }
}
