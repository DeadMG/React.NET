using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React.Core
{
    public interface IUpdateContext
    {
        void OnNextUpdate(Action a);
        void OnNextUpdate(IEvent cause);

        event Action<List<IEvent>> OnUpdatesFinished;
    }
}
