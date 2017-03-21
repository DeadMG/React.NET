using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React.Core
{
    public interface IEvent
    {

    }

    public interface IEvent<State> : IEvent
    { 
        State OriginalState { get; }
        State NewState { get; }
    }
}
