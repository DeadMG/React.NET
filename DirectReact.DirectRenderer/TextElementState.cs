using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectReact.DirectRenderer
{

    public class TextElementState : IElementState<Renderer>
    {
        private readonly SharpDX.DirectWrite.TextFormat format;
        private readonly SharpDX.DirectWrite.TextLayout layout;
        private readonly SharpDX.Direct2D1.SolidColorBrush textBrush;
        private readonly Action<ClickEvent> onMouseClick;

        public TextElementState(TextElement<Renderer> element, Bounds b, Renderer r)
        {
            format = new SharpDX.DirectWrite.TextFormat(r.fontFactory, "Times New Roman", 18);
            layout = new SharpDX.DirectWrite.TextLayout(r.fontFactory, element.text, format, b.Width, b.Height);
            textBrush = new SharpDX.Direct2D1.SolidColorBrush(r.d2dTarget, new SharpDX.Mathematics.Interop.RawColor4(1, 1, 1, 1));
            BoundingBox = new Bounds
            {
                X = b.X,
                Y = b.Y,
                Height = (int)layout.Metrics.Height,
                Width = (int)layout.Metrics.Width
            };
            onMouseClick = element.OnMouseClick;
        }

        public Bounds BoundingBox { get; }
                
        public void Dispose()
        {
            format.Dispose();
            layout.Dispose();
            textBrush.Dispose();
        }

        public void OnMouseClick(ClickEvent click)
        {
            onMouseClick?.Invoke(click);
        }
        
        public void Render(Renderer r)
        {
            r.d2dTarget.DrawTextLayout(new SharpDX.Mathematics.Interop.RawVector2(BoundingBox.X, BoundingBox.Y), layout, textBrush, SharpDX.Direct2D1.DrawTextOptions.Clip);
        }
    }
}
