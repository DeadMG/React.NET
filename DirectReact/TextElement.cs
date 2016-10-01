using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectReact
{
    public class Text : IElement
    {
        private readonly string text;

        public Text(string text)
        {
            this.text = text;
        }
        
        public IElementState Update(IElementState existing, Bounds b, Renderer r)
        {
            var existingTextElementState = existing as TextElementState;
            if (existingTextElementState == null)
            {
                existing?.Dispose();
                return new TextElementState(text, b, r);
            }
            return new TextElementState(text, b, r);
        }
    }

    public class TextElementState : IElementState
    {
        private readonly SharpDX.DirectWrite.TextFormat format;
        private readonly SharpDX.DirectWrite.TextLayout layout;
        private readonly SharpDX.Direct2D1.SolidColorBrush textBrush;

        public TextElementState(string Text, Bounds b, Renderer r)
        {
            format = new SharpDX.DirectWrite.TextFormat(r.fontFactory, "Times New Roman", 18);
            layout = new SharpDX.DirectWrite.TextLayout(r.fontFactory, Text, format, b.Width, b.Height);
            textBrush = new SharpDX.Direct2D1.SolidColorBrush(r.d2dTarget, new SharpDX.Mathematics.Interop.RawColor4(1, 1, 1, 1));
            BoundingBox = new Bounds
            {
                X = b.X,
                Y = b.Y,
                Height = (int)layout.Metrics.Height,
                Width = (int)layout.Metrics.Width
            };
        }

        public Bounds BoundingBox { get; }
                
        public void Dispose()
        {
            format.Dispose();
            layout.Dispose();
            textBrush.Dispose();
        }
        
        public void Render(Renderer r)
        {
            r.d2dTarget.DrawTextLayout(new SharpDX.Mathematics.Interop.RawVector2(BoundingBox.X, BoundingBox.Y), layout, textBrush, SharpDX.Direct2D1.DrawTextOptions.Clip);
        }
    }
}
