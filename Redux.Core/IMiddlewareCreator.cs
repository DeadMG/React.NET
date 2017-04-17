using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redux.Core
{
    public interface IMiddlewareCreator<in TState, TAction>
    {
        IMiddleware<TAction> Create(IReduxStore<TState, TAction> store);
    }
}
