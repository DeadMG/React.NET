namespace React.Core
{
    public class TextSelection
    {
        public TextSelection(int startIndex, int endIndex)
        {
            this.StartIndex = startIndex;
            this.EndIndex = endIndex;
        }

        public int StartIndex { get; }
        public int EndIndex { get; }
    }
}
