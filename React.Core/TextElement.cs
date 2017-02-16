using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React.Core
{
    public class TextElementProps : PrimitiveProps
    {
        public TextElementProps(string text, Action<ClickEvent> onMouseClick = null)
            : base(onMouseClick)
        {
            this.Text = text;
        }

        public string Text { get; }
    }
    
    public class TextElement : IElement
    {
        public TextElement(TextElementProps props)
        {
            this.Props = props;
        }

        public IElementState Update(IElementState existing, UpdateContext context)
        {
            return context.Renderer.UpdateTextElementState(existing, context.Bounds, this, context.Context);
        }

        public TextElementProps Props { get; }
    }
}
