using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React.Redux
{
    public interface IMiddleware<TState, TAction>
    {
        void Dispatch(IReduxStore<TState, TAction> store, Action<TAction> next, TAction action);
    }
}
