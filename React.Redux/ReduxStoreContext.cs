using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React.Redux
{
    public class ReduxStoreContext<TState, TAction> : IReduxComponentContext<TState, TAction>
    {
        private readonly IReduxStore<TState, TAction> store;
        
        public ReduxStoreContext(IReduxStore<TState, TAction> store)
        {
            this.store = store;
        }

        public TState State => store.State;
        public void Dispatch(TAction a) => store.Dispatch(a);
    }
}
