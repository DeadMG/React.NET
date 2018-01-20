using React.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React.Box
{
    public class SizeBindingElementState<T> : IElementState
        where T : IKnownSizeElementState
    {
        private readonly T knownSizeElementState;
        private readonly Bounds knownBounds;

        public SizeBindingElementState(T knownSizeElementState, Bounds knownBounds)
        {
            this.knownSizeElementState = knownSizeElementState;
            this.knownBounds = knownBounds;
            Wrapped = knownSizeElementState;
        }

        public Bounds BoundingBox => new Bounds(knownBounds.X, knownBounds.Y, knownSizeElementState.Size.Width, knownSizeElementState.Size.Height);

        public T Wrapped { get; }

        public void FireEvents(IReadOnlyList<IEvent> events)
        {
            knownSizeElementState.FireEvents(events);
        }

        public void Render(IRenderer r)
        {
            knownSizeElementState.Render(r, BoundingBox);
        }
    }
}
