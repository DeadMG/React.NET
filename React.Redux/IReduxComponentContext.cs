using React.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React.Redux
{
    public interface IReduxComponentContext<out TState, in Action> : IComponentContext
    {
        void Dispatch(Action a);
        TState State { get; }
    }
}
