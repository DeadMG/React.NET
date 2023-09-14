namespace React.Core
{
    public interface IElementState : ICanFireEvents
    {
        void Render(IRenderer r);

        Bounds BoundingBox { get; }
    }
}
