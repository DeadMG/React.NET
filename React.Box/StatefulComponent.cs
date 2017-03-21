using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using React.Core;

namespace React.Box
{
    public class StatefulComponentRenderContext<P, S>
    {
        public StatefulComponentRenderContext(P props, S state, IComponentContext context)
        {
            this.Props = props;
            this.Context = context;
            this.State = state;
        }

        public P Props { get; }
        public S State { get; }
        public IComponentContext Context { get; }
    }
    
    public abstract class StatefulComponent<P, S, C>
        where C : StatefulComponent<P, S, C>
    {
        internal S state;
        internal IUpdateContext updateContext;

        public StatefulComponent(S initialState)
        {
            state = initialState;
        }

        public static StatefulComponentElement<P, S, C> CreateElement(P currentProps)
        {
            return new StatefulComponentElement<P, S, C>(currentProps);
        }
        
        public abstract IElement Render(StatefulComponentRenderContext<P, S> context);

        public Task SetState(S newState)
        {
            var source = new TaskCompletionSource<object>();
            updateContext.OnNextUpdate(() =>
            {
                state = newState;
                source.TrySetResult(null);
            });
            return source.Task;
        }

        public Task WithState(Action<S> withState)
        {
            return WithState(state =>
            {
                withState(state);
                return 1;
            });
        }

        public Task<T> WithState<T>(Func<S, T> withState)
        {
            var source = new TaskCompletionSource<T>();
            updateContext.OnNextUpdate(() =>
            {
                try
                {
                    source.TrySetResult(withState(state));
                }
                catch (Exception e)
                {
                    source.TrySetException(e);
                }
            });
            return source.Task;
        }
    }

    public abstract class StatefulComponent<S, C> : StatefulComponent<EmptyProps, S, C>
        where C : StatefulComponent<S, C>
    {
        public StatefulComponent(S initialState)
            : base(initialState)
        {
        }
    }
}
