using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React.Redux
{
    public interface IReduxStore<TState, Action>
    {
        void Dispatch(Action a);
        TState State { get; }
    }
}
