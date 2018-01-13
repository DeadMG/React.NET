using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using React.Core;

namespace React.Box
{
    public class LineProps
    {
        public LineProps(LineDirection direction)
        {
            this.Direction = direction;
        }

        public LineDirection Direction { get; }
    }

    public class Line : Element<LineState, Line>
    {
        public Line(LineProps props, params IElement<IElementState>[] children)
        {
            if (props == null) throw new InvalidOperationException();
            this.Children = children;
            this.Props = props;
        }

        public IElement<IElementState>[] Children { get; }
        public LineProps Props { get; }
    }

    public class LineState : IElementState
    {
        private readonly List<IElementState> nestedElementStates;
        private readonly LineProps props;

        public LineState(LineState existing, Line e, RenderContext context)
        {
            props = e.Props;
            var bounds = context.Bounds;
            nestedElementStates = e.Children.Select((elem, index) =>
            {
                var result = elem?.Update(existing?.nestedElementStates?[index], context.WithBounds(bounds));
                if (result != null) bounds = bounds.Remaining(e.Props.Direction, result.BoundingBox);
                return result;
            }).ToList();

            BoundingBox = context.Bounds.Sum(e.Props.Direction, nestedElementStates.Where(p => p != null).Select(p => p.BoundingBox));
        }
        
        public Bounds BoundingBox { get; }

        public void FireEvents(IReadOnlyList<IEvent> events)
        {
            foreach (var child in nestedElementStates)
            {
                child.FireEvents(events);
            }
        }

        public void Render(IRenderer r)
        {
            foreach (var element in nestedElementStates)
            {
                element?.Render(r);
            }
        }
    }
}
