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
        public StretchElement(IElement<IElementState> child)
        {
            this.Child = child;
        }

        public IElement<IElementState> Child { get; }
    }

    public class StretchElementState : IElementState
    {
        private readonly IElementState nestedState;
        
        public StretchElementState(StretchElementState existing, StretchElement element, RenderContext context, Bounds bounds)
        {
            nestedState = element.Child?.Update(existing?.nestedState, context, bounds);
            BoundingBox = bounds;
        }

        public Bounds BoundingBox { get; set; }

        public void FireEvents(IReadOnlyList<IEvent> events)
        {
            nestedState?.FireEvents(events);
        }

        public void Render(IRenderer r)
        {
            nestedState?.Render(r);
        }
    }
}
