using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React.Core
{
    public interface ICanFireEvents
    {
        void FireEvents(IReadOnlyList<IEvent> events);
    }
}
