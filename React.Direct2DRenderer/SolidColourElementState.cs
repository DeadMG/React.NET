using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using React.Core;
using React.Box;

namespace React.DirectRenderer
{
    public class SolidColourElementState : PrimitiveElementState
    {
        private readonly SharpDX.Direct2D1.SolidColorBrush brush;
        private readonly Bounds boundingBox;

        public SolidColourElementState(SolidColourElementState existing, SolidColourElement other, UpdateContext context)
            : base(existing, other.Props, context)
        {
            brush = existing?.brush ?? new SharpDX.Direct2D1.SolidColorBrush(Renderer.AssertRendererType(context.Renderer).d2dTarget, new SharpDX.Mathematics.Interop.RawColor4
            {
                R = other.Props.Colour.R,
                G = other.Props.Colour.G,
                B = other.Props.Colour.B,
                A = other.Props.Colour.A
            });
            boundingBox = other.Props.Location != null ? other.Props.Location(context.Bounds) : context.Bounds;
        }

        public override Bounds BoundingBox => boundingBox;

        public override void Dispose()
        {
            brush.Dispose();
            base.Dispose();
        }
        
        public override void Render(IRenderer r)
        {
            Renderer.AssertRendererType(r).d2dTarget.FillRectangle(new SharpDX.Mathematics.Interop.RawRectangleF(BoundingBox.X, BoundingBox.Y, BoundingBox.X + BoundingBox.Width, BoundingBox.Y + BoundingBox.Height), brush);
        }
    }
}
