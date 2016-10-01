using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectReact
{
    public class BackgroundElement<Renderer> : IElement<Renderer>
        where Renderer : IRenderer<Renderer>
    {
        public BackgroundElement(Colour Colour, IElement<Renderer> Child)
        {
            this.Colour = Colour;
            this.Child = Child;
        }

        public IElementState<Renderer> Update(IElementState<Renderer> existing, Bounds b, Renderer r)
        {
            return r.UpdateBackgroundElementState(existing, b, this);
        }

        public Colour Colour { get; }
        public IElement<Renderer> Child { get; }
        public Action<ClickEvent> OnMouseClick { get; set; }
    }
}
