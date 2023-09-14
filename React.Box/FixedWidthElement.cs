using React.Core;
using System.Collections.Generic;

namespace React.Box
{
    public class FixedWidthElement : IElement
    {
        public FixedWidthElement(int width)
        {
            this.Width = width;
        }

        public int Width { get; }

        public IElementState Update(IElementState existing, RenderContext renderContext, Bounds bounds)
        {
            return new FixedWidthElementState(Width, bounds);
        }
    }

    public class FixedWidthElementState : IElementState
    {
        public FixedWidthElementState(int width, Bounds bounds)
        {
            BoundingBox = new Bounds(bounds.X, bounds.Y, width, bounds.Height);
        }

        public Bounds BoundingBox { get; }

        public void FireEvents(IReadOnlyList<IEvent> events)
        {
        }

        public void Render(IRenderer r)
        {
        }
    }
}
