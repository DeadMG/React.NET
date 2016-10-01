using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectReact
{
    public enum LineDirection
    {
        Horizontal,
        Vertical
    }

    public class Line<Renderer> : Element<LineState<Renderer>, Line<Renderer>, Renderer>
        where Renderer : IRenderer<Renderer>
    {
        public Line(LineDirection direction, params IElement<Renderer>[] children)
        {
            this.Children = children;
            this.Direction = direction;
        }

        public IElement<Renderer>[] Children { get; }
        public LineDirection Direction { get; }
        public Action<ClickEvent> OnMouseClick { get; set; }
    }

    public class LineState<Renderer> : IUpdatableElementState<Line<Renderer>, Renderer>
        where Renderer : IRenderer<Renderer>
    {
        private List<IElementState<Renderer>> nestedElementStates;
        private Action<ClickEvent> onMouseClick;

        public LineState(Line<Renderer> e, Bounds b, Renderer r)
        {
            var originalBounds = b;
            nestedElementStates = new List<IElementState<Renderer>>();
            foreach (var child in e.Children)
            {
                var newState = child.Update(null, b, r);
                nestedElementStates.Add(newState);
                b = Bounds.Remaining(e.Direction, b, newState.BoundingBox);
            }
            BoundingBox = Bounds.Sum(e.Direction, originalBounds, nestedElementStates.Select(p => p.BoundingBox));
            onMouseClick = e.OnMouseClick;
        }

        public void Update(Line<Renderer> other, Bounds b, Renderer r)
        {
            var originalBounds = b;
            var newStates = new List<IElementState<Renderer>>();
            for (int i = 0; i < Math.Max(nestedElementStates.Count, other.Children.Length); ++i)
            {
                if (i >= other.Children.Length)
                {
                    nestedElementStates[i].Dispose();
                    continue;
                }
                IElementState<Renderer> existingState = i >= nestedElementStates.Count ? null : nestedElementStates[i];
                var newState = other.Children[i].Update(existingState, b, r);
                newStates.Add(newState);
                b = Bounds.Remaining(other.Direction, b, newState.BoundingBox);
            }
            nestedElementStates = newStates;
            BoundingBox = Bounds.Sum(other.Direction, originalBounds, nestedElementStates.Select(p => p.BoundingBox));
            onMouseClick = other.OnMouseClick;
        }

        public Bounds BoundingBox { get; private set; }

        public void Dispose()
        {
            foreach (var state in nestedElementStates)
                state.Dispose();
        }

        public void Render(Renderer r)
        {
            foreach (var element in nestedElementStates)
            {
                element.Render(r);
            }
        }

        public void OnMouseClick(ClickEvent click)
        {
            foreach (var child in nestedElementStates)
            {
                if (Bounds.IsInBounds(child.BoundingBox, click))
                {
                    child.OnMouseClick(click);
                    break;
                }
            }
            onMouseClick?.Invoke(click);
        }
    }
}
