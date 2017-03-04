using React.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React.Box
{
    public abstract class PrimitiveElementState : IElementState
    {
        protected readonly PrimitiveProps Props;
        protected readonly IEventLevel eventSource;

        public PrimitiveElementState(PrimitiveElementState existing, PrimitiveProps props, UpdateContext context)
        {
            Props = props;
            eventSource = context.EventSource;
            if (existing?.Props.OnMouse != null) existing.eventSource.Mouse -= existing.OnMouse;
            if (Props.OnMouse != null) context.EventSource.Mouse += OnMouse;
        }

        public virtual void Dispose()
        {
            if (Props.OnMouse != null) eventSource.Mouse -= OnMouse;
        }

        private void OnMouse(MouseEvent mouse)
        {
            Props.OnMouse?.Invoke(mouse, BoundingBox);
        }

        public abstract Bounds BoundingBox { get; }
        public abstract void Render(IRenderer r);
    }
}
