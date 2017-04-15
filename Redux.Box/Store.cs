using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React.Redux
{
    public class Store<TState, Action> : IReduxStore<TState, Action>
    {
        private readonly Func<TState, Action, TState> reducer;
        private readonly IMiddleware<TState, Action>[] middleware;

        public Store(TState initial, Func<TState, Action, TState> reducer, params IMiddleware<TState, Action>[] middleware)
        {
            this.reducer = reducer;
            this.middleware = middleware.Where(x => x != null).ToArray();
            State = initial;
        }
        
        public TState State { get; private set; }

        public void Dispatch(Action a)
        {
            var existing = State;
            try
            {
                Dispatch(middleware, a);
            }
            catch (Exception)
            {
                State = existing;
                throw;
            }
        }

        private void Dispatch(IMiddleware<TState, Action>[] remainingMiddlewares, Action a)
        {
            if (remainingMiddlewares.Length == 0)
            {
                State = reducer(State, a);
                return;
            }
            remainingMiddlewares[0].Dispatch(this, action => Dispatch(remainingMiddlewares.Take(1).ToArray(), action), a);
        }
    }
}
