using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React.Core
{
    public class TextuallyPositionedChild
    {
        public TextuallyPositionedChild(TextSelection location, IElement child)
        {
            this.Location = location;
            this.Child = child;
        }

        public TextSelection Location { get; }
        public IElement Child { get; }
    }

    public class TextElementProps
    {
        public TextElementProps(string text, Action<TextMouseEvent, Bounds> onMouse = null)
        {
            this.Text = text;
            this.OnMouse = onMouse;
        }

        public string Text { get; }
        public Action<TextMouseEvent, Bounds> OnMouse { get; }
    }
    
    public class TextElement : IElement
    {
        public TextElement(TextElementProps props, params TextuallyPositionedChild[] children)
        {
            this.Props = props;
            this.Children = children ?? new TextuallyPositionedChild[0];
        }

        public IElementState Update(IElementState existing, UpdateContext context)
        {
            return context.Renderer.UpdateTextElementState(existing, context.Bounds, this, context.Context, context.EventSource);
        }

        public TextuallyPositionedChild[] Children { get; }
        public TextElementProps Props { get; }
    }
}
