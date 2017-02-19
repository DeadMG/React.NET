using React.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React.Box
{
    public class EmptyElement : IElement
    {
        public IElementState Update(IElementState existing, UpdateContext context)
        {
            existing?.Dispose();
            return new EmptyElementState(context.Bounds);
        }
    }

    public class EmptyElementState : IElementState
    {
        public EmptyElementState(Bounds boundingBox)
        {
            this.BoundingBox = new Bounds(x: boundingBox.X, y: boundingBox.Y, width: 0, height: 0);
        }

        public Bounds BoundingBox { get; set; }

        public void Dispose()
        {
        }

        public void OnMouseClick(LeftMouseUpEvent click)
        {
        }

        public void Render(IRenderer r)
        {
        }
    }
}
