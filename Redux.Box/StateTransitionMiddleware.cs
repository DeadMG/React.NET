using React.Redux;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redux.Box
{
    public class StateTransitionMiddleware<TState, TAction> : IMiddleware<TState, TAction>
    {
        private readonly Action<TState, TState> transitionHandler;

        public StateTransitionMiddleware(Action<TState, TState> transitionHandler)
        {
            this.transitionHandler = transitionHandler;
        }

        public void Dispatch(IReduxStore<TState, TAction> store, Action<TAction> next, TAction action)
        {
            var state = store.State;
            next(action);
            if (!ReferenceEquals(state, store.State))
            {
                transitionHandler?.Invoke(state, store.State);
            }
        }
    }
}
