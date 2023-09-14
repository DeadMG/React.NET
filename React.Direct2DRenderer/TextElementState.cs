using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using React.Core;
using SharpDX.Mathematics.Interop;

namespace React.DirectRenderer
{
    public class TextElementState : ITextElementState
    {
        private readonly SharpDX.DirectWrite.TextFormat format;
        private readonly SharpDX.DirectWrite.TextLayout layout;
        private readonly SharpDX.Direct2D1.SolidColorBrush textBrush;
        private readonly List<IElementState> children;
        private readonly TextElementProps props;

        public TextElementState(TextElementState existing, TextElement element, RenderContext context, Bounds bounds)
        {
            props = element.Props;
            format = existing?.format ?? new SharpDX.DirectWrite.TextFormat(Renderer.AssertRendererType(context.Renderer).fontFactory, "Times New Roman", 18);
            layout = new SharpDX.DirectWrite.TextLayout(Renderer.AssertRendererType(context.Renderer).fontFactory, element.Props.Text, format, bounds.Width, bounds.Height);
            textBrush = existing?.textBrush ?? new SharpDX.Direct2D1.SolidColorBrush(Renderer.AssertRendererType(context.Renderer).d2dTarget, new RawColor4(1, 1, 1, 1));
            BoundingBox = new Bounds(x: bounds.X, y: bounds.Y, width: (int)layout.Metrics.WidthIncludingTrailingWhitespace, height: (int)layout.Metrics.Height);

            context.Disposables.Add(format);
            context.Disposables.Add(layout);
            context.Disposables.Add(textBrush);

            var childStates = new List<IElementState>();
            foreach(var child in element.Children)
            {
                var locations = layout.HitTestTextRange(child.Location.StartIndex, child.Location.EndIndex - child.Location.StartIndex, BoundingBox.X, BoundingBox.Y);
                foreach(var location in locations)
                {
                    childStates.Add(child.Child.Update(null, context, new Bounds(x: (int)location.Left, y: (int)location.Top, width: (int)location.Width, height: (int)location.Height)));
                }
            }
            children = childStates;
        }

        public Bounds BoundingBox { get; }
        
        public void Render(IRenderer r)
        {
            Renderer.AssertRendererType(r).d2dTarget.DrawTextLayout(new RawVector2(BoundingBox.X, BoundingBox.Y), layout, textBrush, SharpDX.Direct2D1.DrawTextOptions.Clip);
            foreach (var child in children)
                child.Render(r);
        }

        public void FireEvents(IReadOnlyList<IEvent> events)
        {
            if (props.OnEvent == null) return;
            foreach (var @event in events)
            {
                props.OnEvent(@event, this);
            }
        }

        public int? GetTextIndex(int x, int y)
        {
            RawBool isTrailing;
            RawBool isInside;
            var metrics = layout.HitTestPoint(x - BoundingBox.X, y - BoundingBox.Y, out isTrailing, out isInside);
            if (!isInside) return null;
            return isTrailing ? metrics.TextPosition + 1 : metrics.TextPosition;
        }
    }
}
