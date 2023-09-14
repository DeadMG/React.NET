using React.Core;

namespace React.Redux
{
    public interface IReduxComponentContext<out TState, in Action> : IComponentContext
    {
        void Dispatch(Action a);
        TState State { get; }
    }
}
