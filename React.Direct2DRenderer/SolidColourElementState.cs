using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using React.Core;

namespace React.DirectRenderer
{
    public class SolidColourElementState : IElementState
    {
        private SharpDX.Direct2D1.SolidColorBrush brush;
        private Action<LeftMouseUpEvent> onMouseClick;
        private Bounds boundingBox;

        public SolidColourElementState(SolidColourElement other, UpdateContext context)
        {
            brush = new SharpDX.Direct2D1.SolidColorBrush(Renderer.AssertRendererType(context.Renderer).d2dTarget, new SharpDX.Mathematics.Interop.RawColor4
            {
                R = other.Props.Colour.R,
                G = other.Props.Colour.G,
                B = other.Props.Colour.B,
                A = other.Props.Colour.A
            });
            onMouseClick = other.Props.OnMouseClick;
            boundingBox = other.Props.Location != null ? other.Props.Location(context.Bounds) : context.Bounds;
        }

        public Bounds BoundingBox => boundingBox;

        public void Dispose()
        {
            brush.Dispose();
        }

        public void OnMouseClick(LeftMouseUpEvent click)
        {
            onMouseClick?.Invoke(click);
        }

        public void Render(IRenderer r)
        {
            Renderer.AssertRendererType(r).d2dTarget.FillRectangle(new SharpDX.Mathematics.Interop.RawRectangleF(BoundingBox.X, BoundingBox.Y, BoundingBox.X + BoundingBox.Width, BoundingBox.Y + BoundingBox.Height), brush);
        }

        public void Update(SolidColourElement other, UpdateContext context)
        {
            brush.Dispose();
            brush = new SharpDX.Direct2D1.SolidColorBrush(Renderer.AssertRendererType(context.Renderer).d2dTarget, new SharpDX.Mathematics.Interop.RawColor4
            {
                R = other.Props.Colour.R,
                G = other.Props.Colour.G,
                B = other.Props.Colour.B,
                A = other.Props.Colour.A
            });
            onMouseClick = other.Props.OnMouseClick;
            boundingBox = other.Props.Location(context.Bounds);
        }
    }
}
