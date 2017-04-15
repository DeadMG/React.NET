using React.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React.Redux
{
    public class ReduxStateUpdateEvent<TState> : IEvent<TState>
    {
        public TState NewState { get; }
        public TState OriginalState { get; }

        public ReduxStateUpdateEvent(TState originalState, TState newState)
        {
            this.NewState = newState;
            this.OriginalState = originalState;
        }
    }
}
