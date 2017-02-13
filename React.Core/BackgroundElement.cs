using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React
{
    public class BackgroundElementProps : PrimitiveProps
    {
        public Colour Colour { get; set; }
    }

    public class BackgroundElement : IElement
    {
        public BackgroundElement(BackgroundElementProps props, IElement child)
        {
            this.Props = props;
            this.Child = child;
        }

        public IElementState Update(IElementState existing, UpdateContext context)
        {
            return context.Renderer.UpdateBackgroundElementState(existing, context.Bounds, this, context.Context);
        }

        public BackgroundElementProps Props { get; }
        public IElement Child { get; }
    }
}
