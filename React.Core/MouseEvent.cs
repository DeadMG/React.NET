using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React.Core
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

    public class MouseEvent
    {
        public MouseEvent(MouseState originalState, MouseState newState)
        {
            this.OriginalState = originalState;
            this.NewState = newState;
        }

        public MouseState OriginalState { get; }
        public MouseState NewState { get; }
    }
}
