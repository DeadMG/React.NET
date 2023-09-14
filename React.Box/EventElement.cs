using React.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace React.Box
{
    public class EventElement<TEvent> : IElement<IEventElementState<TEvent>>
        where TEvent : IEvent
    {
        public EventElement(Action<TEvent> onEvent)
        {
            this.OnEvent = onEvent;
        }

        public Action<TEvent> OnEvent { get; }

        public IEventElementState<TEvent> Update(IElementState existing, RenderContext renderContext, Bounds bounds)
        {
            return new EventElementState<TEvent>(this, bounds);
        }
    }

    public interface IEventElementState<TEvent> : IElementState
        where TEvent : IEvent
    {

    }

    public class EventElementState<TEvent> : IEventElementState<TEvent>
        where TEvent : IEvent
    {
        private readonly EventElement<TEvent> element;

        public EventElementState(EventElement<TEvent> element, Bounds bounds)
        {
            this.element = element;
            this.BoundingBox = new Bounds(bounds.X, bounds.Y, 0, 0);
        }

        public Bounds BoundingBox { get; }

        public void FireEvents(IReadOnlyList<IEvent> events)
        {
            foreach (var @event in events.OfType<TEvent>())
            {
                this.element?.OnEvent(@event);
            }
        }

        public void Render(IRenderer r)
        {
        }
    }

    public class EventElement<TEvent, TElement> : IElement<IEventElementState<TEvent, TElement>>
        where TElement : class, IElementState
        where TEvent : IEvent
    {
        public EventElement(Action<TEvent, TElement> onEvent, IElement<TElement> child)
        {
            this.OnEvent = onEvent;
            this.Child = child;
        }

        public Action<TEvent, TElement> OnEvent { get; }
        public IElement<TElement> Child { get; }

        public IEventElementState<TEvent, TElement> Update(IElementState existing, RenderContext renderContext, Bounds bounds)
        {
            return new EventElementState<TEvent, TElement>(this, existing, renderContext, bounds);
        }
    }

    public interface IEventElementState<TEvent, TElement> : IElementState
        where TEvent : IEvent
        where TElement : class, IElementState
    {
        TElement Nested { get; }
    }

    public class EventElementState<TEvent, TElement> : IEventElementState<TEvent, TElement>
        where TEvent : IEvent
        where TElement : class, IElementState
    {
        private readonly EventElement<TEvent, TElement> element;
        private readonly TElement nested;

        public EventElementState(EventElement<TEvent, TElement> element, IElementState existing, RenderContext renderContext, Bounds bounds)
        {
            this.element = element;
            this.nested = element.Child.Update((existing as EventElementState<TEvent, TElement>)?.nested, renderContext, bounds);
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

        public TElement Nested => nested;
    }
}
