using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectReact
{
    public class BackgroundElement : IElement
    {
        public BackgroundElement(Colour Colour, IElement Child)
        {
            this.Colour = Colour;
            this.Child = Child;
        }

        public IElementState Update(IElementState existing, UpdateContext context)
        {
            return context.Renderer.UpdateBackgroundElementState(existing, context.Bounds, this, context.Context);
        }

        public Colour Colour { get; }
        public IElement Child { get; }
        public Action<ClickEvent> OnMouseClick { get; }
    }
}
