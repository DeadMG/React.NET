using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using React.Core;

namespace React.DirectRenderer
{
    public class BackgroundElementState : IElementState
    {
        private SharpDX.Direct2D1.SolidColorBrush brush;
        private IElementState nestedState;
        private Action<ClickEvent> onMouseClick;

        public BackgroundElementState(BackgroundElement other, UpdateContext context)
        {
            brush = new SharpDX.Direct2D1.SolidColorBrush(Renderer.AssertRendererType(context.Renderer).d2dTarget, new SharpDX.Mathematics.Interop.RawColor4
            {
                R = other.Props.Colour.R,
                G = other.Props.Colour.G,
                B = other.Props.Colour.B,
                A = other.Props.Colour.A
            });
            nestedState = other.Child.Update(null, context);
            onMouseClick = other.Props.OnMouseClick;
        }

        public Bounds BoundingBox => nestedState.BoundingBox;

        public void Dispose()
        {
            brush.Dispose();
            nestedState.Dispose();
        }

        public void OnMouseClick(ClickEvent click)
        {
            if (Bounds.IsInBounds(nestedState.BoundingBox, click))
                nestedState.OnMouseClick(click);
            onMouseClick?.Invoke(click);
        }

        public void Render(IRenderer r)
        {
            Renderer.AssertRendererType(r).d2dTarget.FillRectangle(new SharpDX.Mathematics.Interop.RawRectangleF(BoundingBox.X, BoundingBox.Y, BoundingBox.X + BoundingBox.Width, BoundingBox.Y + BoundingBox.Height), brush);
            nestedState.Render(r);
        }

        public void Update(BackgroundElement other, UpdateContext context)
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
            nestedState = other.Child.Update(nestedState, context);
        }
    }
}
