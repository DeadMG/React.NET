using React.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace React.Box
{
    public class EventElement<TEvent> : IElement
        where TEvent : IEvent
    {
        public EventElement(Action<TEvent, IElementState> onEvent, IElement child)
        {
            this.OnEvent = onEvent;
            this.Child = child;
        }

        public Action<TEvent, IElementState> OnEvent { get; }
        public IElement Child { get; }

        public IElementState Update(IElementState existing, RenderContext renderContext, Bounds bounds)
        {
            return new EventElementState<TEvent>(this, existing, renderContext, bounds);
        }
    }

    public class EventElementState<TEvent> : IElementState
        where TEvent : IEvent
    {
        private readonly EventElement<TEvent> element;
        private readonly IElementState nested;

        public EventElementState(EventElement<TEvent> element, IElementState existing, RenderContext renderContext, Bounds bounds)
        {
            this.element = element;
            this.nested = element.Child.Update((existing as EventElementState<TEvent>)?.nested, renderContext, bounds);
            this.BoundingBox = nested.BoundingBox;
        }

        public Bounds BoundingBox { get; }

        public void FireEvents(IReadOnlyList<IEvent> events)
        {
            foreach (var @event in events.OfType<TEvent>())
            {
                this.element?.OnEvent(@event, nested);
            }
            nested.FireEvents(events);
        }

        public void Render(IRenderer r)
        {
            nested.Render(r);
        }

        public IElementState Nested => nested;
    }
}
