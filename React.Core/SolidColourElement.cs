using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React.Core
{
    public class SolidColourElementProps : PrimitiveProps
    {
        public Colour Colour { get; set; }
        public Func<Bounds, Bounds> Location { get; set; }
    }

    public class SolidColourElement : IElement
    {
        public SolidColourElement(SolidColourElementProps props)
        {
            this.Props = props;
        }

        public SolidColourElementProps Props { get; }

        public IElementState Update(IElementState existing, UpdateContext context)
        {
            return context.Renderer.UpdateSolidColourElementState(existing, context.Bounds, this, context.Context);
        }
    }
}
