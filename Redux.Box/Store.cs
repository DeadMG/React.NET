using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redux.Core;

namespace Redux.Box
{
    public class Store<TState, Action> : IReduxStore<TState, Action>
    {
        private readonly Func<TState, Action, TState> reducer;
        private readonly IMiddleware<Action>[] middleware;

        public Store(TState initial, Func<TState, Action, TState> reducer, params IMiddlewareCreator<TState, Action>[] middleware)
        {
            this.reducer = reducer;
            this.middleware = middleware.Select(x => x?.Create(this)).Where(x => x != null).ToArray();
            State = initial;
        }
        
        public TState State { get; private set; }

        public void Dispatch(Action a)
        {
            var existing = State;
            try
            {
                Dispatch(middleware, a);
                if (!ReferenceEquals(existing, State))
                {
                    StateChanged?.Invoke(existing, State);
                }
            }
            catch (Exception)
            {
                State = existing;
                throw;
            }
        }

        private void Dispatch(IMiddleware<Action>[] remainingMiddlewares, Action a)
        {
            if (remainingMiddlewares.Length == 0)
            {
                State = reducer(State, a);
                return;
            }
            remainingMiddlewares[0].Dispatch(action => Dispatch(remainingMiddlewares.Skip(1).ToArray(), action), a);
        }

        public event Action<TState, TState> StateChanged;
    }
}
