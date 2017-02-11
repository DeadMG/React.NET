using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectReact
{
    public interface ITextElement
    {
        Action<ClickEvent> OnMouseClick { get; }
        string Text { get; }
    }

    public class TextElement : IElement, ITextElement
    {
        public TextElement(string text)
        {
            this.Text = text;
        }

        public IElementState Update(IElementState existing, UpdateContext context)
        {
            return context.Renderer.UpdateTextElementState(existing, context.Bounds, this, context.Context);
        }

        public Action<ClickEvent> OnMouseClick { get; set; }
        public string Text { get; }
    }
}
