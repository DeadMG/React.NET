namespace React.Box
{
    public class MouseState
    {
        public MouseState(int x, int y, bool leftButtonDown)
        {
            this.X = x;
            this.Y = y;
            this.LeftButtonDown = leftButtonDown;
        }

        public int X { get; }
        public int Y { get; }
        public bool LeftButtonDown { get; }
    }
}
