namespace React.Box
{
    public class ResizeState
    {
        public ResizeState(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        public int Width { get; }
        public int Height { get; }
    }
}
