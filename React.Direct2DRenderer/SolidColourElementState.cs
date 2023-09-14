using React.Core;
using System.Collections.Generic;

namespace React.DirectRenderer
{
    public class SolidColourElementState : IElementState
    {
        private readonly SharpDX.Direct2D1.SolidColorBrush brush;
        private readonly IElementState child;
        private readonly SolidColourElement element;

        public SolidColourElementState(SolidColourElementState existing, SolidColourElement other, RenderContext context, Bounds bounds)
        {
            element = other;
            brush = existing?.brush ?? new SharpDX.Direct2D1.SolidColorBrush(Renderer.AssertRendererType(context.Renderer).d2dTarget, new SharpDX.Mathematics.Interop.RawColor4
            {
                R = other.Props.Colour.R,
                G = other.Props.Colour.G,
                B = other.Props.Colour.B,
                A = other.Props.Colour.A
            });
            context.Disposables.Add(brush);
            child = element.Child.Update(existing?.child, context, bounds);
        }

        public Bounds BoundingBox => child.BoundingBox;

        public void FireEvents(IReadOnlyList<IEvent> events)
        {
            child.FireEvents(events);
        }

        public void Render(IRenderer r)
        {
            Renderer.AssertRendererType(r).d2dTarget.FillRectangle(new SharpDX.Mathematics.Interop.RawRectangleF(BoundingBox.X, BoundingBox.Y, BoundingBox.X + BoundingBox.Width, BoundingBox.Y + BoundingBox.Height), brush);
            child.Render(r);
        }
    }
}
