namespace React.Box
{
    public class ChangeEvent<State> : IChangeEvent<State>
    {
        public ChangeEvent(State originalState, State newState)
        {
            this.OriginalState = originalState;
            this.NewState = newState;
        }

        public State OriginalState { get; }
        public State NewState { get; }
    }
}
