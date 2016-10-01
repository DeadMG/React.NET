using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectReact
{
    public class TextElement<Renderer> : IElement<Renderer>
        where Renderer : IRenderer<Renderer>
    {
        public TextElement(string text)
        {
            this.text = text;
        }

        public IElementState<Renderer> Update(IElementState<Renderer> existing, Bounds b, Renderer r)
        {
            return r.UpdateTextElementState(existing, b, this);
        }

        public Action<ClickEvent> OnMouseClick { get; set; }
        public string text { get; }
    }
}
