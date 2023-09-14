using React.Core;

namespace React.Box
{
    public interface IChangeEvent<State> : IEvent
    {
        State OriginalState { get; }
        State NewState { get; }
    }
}
