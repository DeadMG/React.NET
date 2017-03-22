using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using React.Core;
using React.Box;

namespace React.DirectRenderer
{
    public class SolidColourElementState : IElementState
    {
        private readonly SharpDX.Direct2D1.SolidColorBrush brush;
        private readonly Bounds boundingBox;
        private readonly SolidColourElementProps props;

        public SolidColourElementState(SolidColourElementState existing, SolidColourElement other, RenderContext context)
        {
            props = other.Props;
            brush = existing?.brush ?? new SharpDX.Direct2D1.SolidColorBrush(Renderer.AssertRendererType(context.Renderer).d2dTarget, new SharpDX.Mathematics.Interop.RawColor4
            {
                R = other.Props.Colour.R,
                G = other.Props.Colour.G,
                B = other.Props.Colour.B,
                A = other.Props.Colour.A
            });
            context.Disposables.Add(brush);
            boundingBox = other.Props.Location != null ? other.Props.Location(context.Bounds) : context.Bounds;
        }

        public Bounds BoundingBox => boundingBox;

        public void FireEvents(List<IEvent> events)
        {
            PrimitivePropsHelpers.FireEvents(props, BoundingBox, events);
        }

        public void Render(IRenderer r)
        {
            Renderer.AssertRendererType(r).d2dTarget.FillRectangle(new SharpDX.Mathematics.Interop.RawRectangleF(BoundingBox.X, BoundingBox.Y, BoundingBox.X + BoundingBox.Width, BoundingBox.Y + BoundingBox.Height), brush);
        }
    }
}
