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
        private readonly Action<TextMouseEvent, Bounds> onMouse;
        private readonly List<IElementState> children;
        private readonly IEventLevel eventSource;
        private readonly IEventLevel childEventLevel;

        public TextElementState(TextElement element, Bounds b, IRenderer r, IComponentContext o, IEventLevel eventSource)
        {
            if (element.Props.OnMouse != null)
            {
                this.eventSource = eventSource;
                onMouse = element.Props.OnMouse;
                eventSource.Mouse += OnMouse;
            }

            text = element.Props.Text;
            format = new SharpDX.DirectWrite.TextFormat(Renderer.AssertRendererType(r).fontFactory, "Times New Roman", 18);
            layout = new SharpDX.DirectWrite.TextLayout(Renderer.AssertRendererType(r).fontFactory, element.Props.Text, format, b.Width, b.Height);
            textBrush = new SharpDX.Direct2D1.SolidColorBrush(Renderer.AssertRendererType(r).d2dTarget, new RawColor4(1, 1, 1, 1));
            BoundingBox = new Bounds(x: b.X, y: b.Y, width: (int)layout.Metrics.Width, height: (int)layout.Metrics.Height);

            childEventLevel = eventSource.CreateNestedEventLevel();
            var childStates = new List<IElementState>();
            foreach(var child in element.Children)
            {
                var locations = layout.HitTestTextRange(child.Location.StartIndex, child.Location.EndIndex - child.Location.StartIndex, BoundingBox.X, BoundingBox.Y);
                if (locations.Length == 0)
                {
                    continue;
                }
                var location = locations[0];
                childStates.Add(child.Child.Update(null, new UpdateContext(new Bounds(x: (int)location.Left, y: (int)location.Top, width: (int)location.Width, height: (int)location.Height), r, o, childEventLevel)));
            }
            children = childStates;
        }

        public Bounds BoundingBox { get; }
                
        public void Dispose()
        {
            format.Dispose();
            layout.Dispose();
            textBrush.Dispose();
            foreach (var child in children)
                child.Dispose();
            if (eventSource != null) eventSource.Mouse -= OnMouse;
            childEventLevel.Dispose();
        }

        private void OnMouse(MouseEvent click)
        {
            onMouse?.Invoke(new TextMouseEvent(new TextMouseState(click.OriginalState, GetTextIndex(click.OriginalState)), new TextMouseState(click.NewState, GetTextIndex(click.NewState))), BoundingBox);
        }

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
