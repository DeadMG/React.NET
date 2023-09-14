namespace React.Core
{
    public interface IKnownSizeElementState : ICanFireEvents
    {
        void Render(IRenderer r, Bounds bounds);

        Dimensions Size { get; }
    }
}
