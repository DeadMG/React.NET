using React.Core;
using System.Collections.Generic;

namespace React.DirectRenderer
{
    public class KnownSizeSolidColourElementState : IKnownSizeElementState
    {
        private readonly SharpDX.Direct2D1.SolidColorBrush brush;
        private readonly KnownSizeSolidColourElementProps props;

        public KnownSizeSolidColourElementState(KnownSizeSolidColourElementState existing, KnownSizeSolidColourElement other, RenderContext context)
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
        }

        public Dimensions Size => props.Size;

        public void FireEvents(IReadOnlyList<IEvent> events)
        {
        }

        public void Render(IRenderer r, Bounds bounds)
        {
            Renderer.AssertRendererType(r).d2dTarget.FillRectangle(new SharpDX.Mathematics.Interop.RawRectangleF(bounds.X, bounds.Y, Size.Width + bounds.X, bounds.Y + Size.Height), brush);
        }
    }
}
