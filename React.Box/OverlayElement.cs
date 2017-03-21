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

    public class OverlayElementState : IElementState
    {
        private readonly IElementState child;
        private readonly IElementState overlay;

        public OverlayElementState(OverlayElementState existing, OverlayElement element, RenderContext context)
        {
            child = element.Props.Child.Update(existing?.child, context);
            overlay = element.Props.Overlay.Update(existing?.overlay, context);
        }

        public Bounds BoundingBox => child.BoundingBox;
                
        public void Render(IRenderer r)
        {
            child?.Render(r);
            overlay?.Render(r);
        }
    }
}
