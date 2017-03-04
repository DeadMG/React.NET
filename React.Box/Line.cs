using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using React.Core;

namespace React.Box
{
    public class LineProps : PrimitiveProps
    {
        public LineProps(LineDirection direction, Action<MouseEvent, Bounds> onMouse = null)
            : base(onMouse)
        {
            this.Direction = direction;
        }

        public LineDirection Direction { get; }
    }

    public class Line : Element<LineState, Line>
    {
        public Line(LineProps props, params IElement[] children)
        {
            if (props == null) throw new InvalidOperationException();
            this.Children = children;
            this.Props = props;
        }

        public IElement[] Children { get; }
        public LineProps Props { get; }
    }

    public class LineState : PrimitiveElementState
    {
        private readonly List<IElementState> nestedElementStates;
        private readonly IEventLevel nestedEventLevel;

        public LineState(LineState existing, Line e, UpdateContext context)
            : base(existing, e.Props, context)
        {
            nestedEventLevel = context.EventSource.CreateNestedEventLevel();
            var bounds = context.Bounds;
            nestedElementStates = e.Children.Select((elem, index) =>
            {
                var result = elem?.Update(existing?.nestedElementStates?[index], new UpdateContext(bounds, context.Renderer, context.Context, nestedEventLevel));
                if (result != null) bounds = bounds.Remaining(e.Props.Direction, result.BoundingBox);
                return result;
            }).ToList();
            BoundingBox = context.Bounds.Sum(e.Props.Direction, nestedElementStates.Where(p => p != null).Select(p => p.BoundingBox));
        }
        
        public override Bounds BoundingBox { get; }

        public override void Dispose()
        {
            foreach (var state in nestedElementStates)
                state?.Dispose();
            nestedEventLevel.Dispose();
            base.Dispose();
        }

        public override void Render(IRenderer r)
        {
            foreach (var element in nestedElementStates)
            {
                element?.Render(r);
            }
        }
    }
}
