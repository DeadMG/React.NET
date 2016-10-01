using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectReact
{
    public class Background : Element<BackgroundState, Background>
    {
        public Background(SharpDX.Mathematics.Interop.RawColor4 Colour, IElement Child)
        {
            this.Colour = Colour;
            this.Child = Child;
        }

        public SharpDX.Mathematics.Interop.RawColor4 Colour { get; }
        public IElement Child { get; }
    }

    public class BackgroundState : IUpdatableElementState<Background>
    {
        private SharpDX.Direct2D1.SolidColorBrush brush;
        private IElementState nestedState;

        public BackgroundState(Background other, Bounds b, Renderer r)
        {
            brush = new SharpDX.Direct2D1.SolidColorBrush(r.d2dTarget, other.Colour);
            nestedState = other.Child.Update(null, b, r);
        }

        public Bounds BoundingBox => nestedState.BoundingBox;
        public void Dispose()
        {
            brush.Dispose();
            nestedState.Dispose();
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
        }
    }
}
