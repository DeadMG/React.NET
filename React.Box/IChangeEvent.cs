using React.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React.Box
{
    public interface IChangeEvent<State> : IEvent
    {
        State OriginalState { get; }
        State NewState { get; }
    }
}
