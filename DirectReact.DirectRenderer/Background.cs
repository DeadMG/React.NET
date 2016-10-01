using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectReact.DirectRenderer
{
    public class Background : Element<BackgroundState, Background, Renderer>
    {
        public Background(SharpDX.Mathematics.Interop.RawColor4 Colour, IElement<Renderer> Child)
        {
            this.Colour = Colour;
            this.Child = Child;
        }

        public SharpDX.Mathematics.Interop.RawColor4 Colour { get; }
        public IElement<Renderer> Child { get; }
        public Action<ClickEvent> OnMouseClick { get; set; }
    }

    public class BackgroundState : IUpdatableElementState<Background, Renderer>
    {
        private SharpDX.Direct2D1.SolidColorBrush brush;
        private IElementState<Renderer> nestedState;
        private Action<ClickEvent> onMouseClick;

        public BackgroundState(Background other, Bounds b, Renderer r)
        {
            brush = new SharpDX.Direct2D1.SolidColorBrush(r.d2dTarget, other.Colour);
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

        public void Update(Background other, Bounds b, Renderer r)
        {
            brush.Dispose();
            brush = new SharpDX.Direct2D1.SolidColorBrush(r.d2dTarget, other.Colour);
            nestedState = other.Child.Update(nestedState, b, r);
            onMouseClick = other.OnMouseClick;
        }
    }
}
