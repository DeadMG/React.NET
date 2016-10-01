using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectReact
{
    public class TextElement : IElement
    {
        public TextElement(string text)
        {
            this.Text = text;
        }

        public string Text { get; }

        public IElementState Update(IElementState existing, Renderer r)
        {
            var existingTextElementState = existing as TextElementState;
            if (existingTextElementState == null)
            {
                existing?.Dispose();
                return new TextElementState(this.Text, r);
            }
            return new TextElementState(this.Text, r);
        }
    }

    public class TextElementState : IElementState
    {
        private readonly SharpDX.DirectWrite.TextFormat format;
        private readonly SharpDX.Direct2D1.SolidColorBrush textBrush;

        public TextElementState(string Text, Renderer r)
        {
            format = new SharpDX.DirectWrite.TextFormat(r.fontFactory, "Times New Roman", 18);
            textBrush = new SharpDX.Direct2D1.SolidColorBrush(r.d2dTarget, new SharpDX.Mathematics.Interop.RawColor4(1, 1, 1, 1));
            this.Text = Text;
        }

        public string Text { get; }
        
        public void Dispose()
        {
            format.Dispose();
            textBrush.Dispose();
        }
        
        public void Render(Renderer r)
        {
            r.d2dTarget.DrawText(Text, format, new SharpDX.Mathematics.Interop.RawRectangleF(0, 0, 100, 100), textBrush);
        }
    }
}
