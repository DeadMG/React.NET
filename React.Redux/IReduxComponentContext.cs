using React.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React.Redux
{
    public interface IReduxComponentContext<TState, Action> : IComponentContext
    {
        void Dispatch(Action a);
        TState State { get; }
    }
}
