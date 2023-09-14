﻿using React.Box;
using React.Core;
using Redux.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace React.Redux
{
    public class ReduxProviderElement<TState, TAction> : IElement
        where TState : class
    {        
        public ReduxProviderElement(IReduxStore<TState, TAction> store, IElement child)
        {
            this.Store = store;
            this.Child = child;
        }
        
        public IElementState Update(IElementState existing, RenderContext renderContext, Bounds bounds)
        {
            return new ReduxProviderElementState<TState, TAction>(this, existing, renderContext, bounds);
        }

        public IReduxStore<TState, TAction> Store { get; }
        public IElement Child { get; }

        public static ReduxProviderElement<TState2, TAction2> Create<TState2, TAction2>(IReduxStore<TState2, TAction2> store, IElement child)
            where TState2 : class
        {
            return new ReduxProviderElement<TState2, TAction2>(store, child);
        }
    }

    public class ReduxProviderElementState<TState, TAction> : IElementState
        where TState : class
    {
        private TState state;
        private readonly IElementState nested;

        public ReduxProviderElementState(ReduxProviderElement<TState, TAction> element, IElementState existing, RenderContext renderContext, Bounds bounds)
        {
            var existingProviderState = existing as ReduxProviderElementState<TState, TAction>;
            this.state = existingProviderState?.state ?? element.Store.State;
            var nestedContext = new RenderContext(
                renderContext.Renderer, 
                new ReduxProviderComponentContext<TState, TAction>(() => state, action => element.Store.Dispatch(action)), 
                renderContext.UpdateContext,
                renderContext.Disposables);
            this.nested = element.Child.Update(existingProviderState?.nested, nestedContext, bounds);
        }

        public Bounds BoundingBox => nested.BoundingBox;

        public void FireEvents(IReadOnlyList<IEvent> events)
        {
            foreach(var @event in events.OfType<ChangeEvent<TState>>())
            {
                state = @event.NewState;
            }
            nested.FireEvents(events);
        }

        public void Render(IRenderer r)
        {
            nested.Render(r);
        }
    }

    public class ReduxProviderComponentContext<TState, TAction> : IReduxComponentContext<TState, TAction>
    {
        private readonly Func<TState> getState;
        private readonly Action<TAction> dispatch;

        public ReduxProviderComponentContext(Func<TState> getState, Action<TAction> dispatch)
        {
            this.getState = getState;
            this.dispatch = dispatch;
        }

        public TState State => getState();

        public void Dispatch(TAction a)
        {
            dispatch(a);
        }
    }
}
