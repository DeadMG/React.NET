using System.Collections.Generic;

namespace React.Core
{
    public interface ICanFireEvents
    {
        void FireEvents(IReadOnlyList<IEvent> events);
    }
}
