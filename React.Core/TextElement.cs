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
        public TextElementProps(string text, TextuallyPositionedChild[] children = null, Action<TextClickEvent> onMouseClick = null)
        {
            this.Text = text;
            this.OnMouseClick = onMouseClick;
            this.Children = children ?? new TextuallyPositionedChild[0];
        }

        public string Text { get; }
        public Action<TextClickEvent> OnMouseClick { get; }
        public TextuallyPositionedChild[] Children { get; }
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
