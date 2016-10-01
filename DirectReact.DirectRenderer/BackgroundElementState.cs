using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectReact.DirectRenderer
{
    public class BackgroundElementState : IElementState<Renderer>
    {
        private SharpDX.Direct2D1.SolidColorBrush brush;
        private IElementState<Renderer> nestedState;
        private Action<ClickEvent> onMouseClick;

        public BackgroundElementState(BackgroundElement<Renderer> other, Bounds b, Renderer r)
        {
            brush = new SharpDX.Direct2D1.SolidColorBrush(r.d2dTarget, new SharpDX.Mathematics.Interop.RawColor4
            {
                R = other.Colour.R,
                G = other.Colour.G,
                B = other.Colour.B,
                A = other.Colour.A
            });
            nestedState = other.Child.Update(null, b, r);
            onMouseClick = other.OnMouseClick;
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

        public void Render(Renderer r)
        {
            r.d2dTarget.FillRectangle(new SharpDX.Mathematics.Interop.RawRectangleF(BoundingBox.X, BoundingBox.Y, BoundingBox.X + BoundingBox.Width, BoundingBox.Y + BoundingBox.Height), brush);
            nestedState.Render(r);
        }

        public void Update(BackgroundElement<Renderer> other, Bounds b, Renderer r)
        {
            brush.Dispose();
            brush = new SharpDX.Direct2D1.SolidColorBrush(r.d2dTarget, new SharpDX.Mathematics.Interop.RawColor4
            {
                R = other.Colour.R,
                G = other.Colour.G,
                B = other.Colour.B,
                A = other.Colour.A
            });
            onMouseClick = other.OnMouseClick;
            nestedState = other.Child.Update(nestedState, b, r);
        }
    }
}
