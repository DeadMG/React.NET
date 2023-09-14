using System;

namespace React.Core
{
    public interface IUpdateContext
    {
        void OnNextUpdate(Action a);
        void OnNextUpdate(IEvent cause);
    }
}
