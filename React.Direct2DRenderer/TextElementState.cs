using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using React.Core;
using SharpDX.Mathematics.Interop;

namespace React.DirectRenderer
{
    public class TextElementState : IElementState
    {
        private readonly string text;
        private readonly SharpDX.DirectWrite.TextFormat format;
        private readonly SharpDX.DirectWrite.TextLayout layout;
        private readonly SharpDX.Direct2D1.SolidColorBrush textBrush;
        private readonly List<IElementState> children;

        public TextElementState(TextElementState existing, TextElement element, RenderContext context)
        {
            text = element.Props.Text;
            format = existing?.format ?? new SharpDX.DirectWrite.TextFormat(Renderer.AssertRendererType(context.Renderer).fontFactory, "Times New Roman", 18);
            layout = new SharpDX.DirectWrite.TextLayout(Renderer.AssertRendererType(context.Renderer).fontFactory, element.Props.Text, format, context.Bounds.Width, context.Bounds.Height);
            textBrush = existing?.textBrush ?? new SharpDX.Direct2D1.SolidColorBrush(Renderer.AssertRendererType(context.Renderer).d2dTarget, new RawColor4(1, 1, 1, 1));
            BoundingBox = new Bounds(x: context.Bounds.X, y: context.Bounds.Y, width: (int)layout.Metrics.Width, height: (int)layout.Metrics.Height);

            context.Disposables.Add(format);
            context.Disposables.Add(layout);
            context.Disposables.Add(textBrush);

            var childStates = new List<IElementState>();
            foreach(var child in element.Children)
            {
                var locations = layout.HitTestTextRange(child.Location.StartIndex, child.Location.EndIndex - child.Location.StartIndex, BoundingBox.X, BoundingBox.Y);
                if (locations.Length == 0)
                {
                    continue;
                }
                var location = locations[0];
                childStates.Add(child.Child.Update(null, context.WithBounds(new Bounds(x: (int)location.Left, y: (int)location.Top, width: (int)location.Width, height: (int)location.Height))));
            }
            children = childStates;

            foreach(var mouseEvent in context.Events.OfType<MouseEvent>())
            {
                element.Props.OnMouse?.Invoke(new TextMouseEvent(new TextMouseState(mouseEvent.OriginalState, GetTextIndex(mouseEvent.OriginalState)), new TextMouseState(mouseEvent.NewState, GetTextIndex(mouseEvent.NewState))), BoundingBox);
            }
        }

        public Bounds BoundingBox { get; }
        
        private int? GetTextIndex(MouseState state)
        {
            RawBool isTrailing;
            RawBool isInside;
            var metrics = layout.HitTestPoint(state.X - BoundingBox.X, state.Y - BoundingBox.Y, out isTrailing, out isInside);
            if (!isInside) return null;
            return isTrailing ? metrics.TextPosition + 1 : metrics.TextPosition;
        }
        
        public void Render(IRenderer r)
        {
            Renderer.AssertRendererType(r).d2dTarget.DrawTextLayout(new RawVector2(BoundingBox.X, BoundingBox.Y), layout, textBrush, SharpDX.Direct2D1.DrawTextOptions.Clip);
            foreach (var child in children)
                child.Render(r);
        }
    }
}
