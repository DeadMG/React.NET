using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redux.Core
{
    public interface IReduxStore<out TState, in TAction>
    {
        void Dispatch(TAction a);
        TState State { get; }
    }
}
