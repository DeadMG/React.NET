namespace React.Core
{
    public interface ITextElementState : IElementState
    {
        int? GetTextIndex(int x, int y);
    }
}
