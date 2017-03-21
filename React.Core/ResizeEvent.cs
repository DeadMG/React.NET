using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React.Core
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

    public class ResizeEvent : IEvent<ResizeState>
    {
        public ResizeEvent(ResizeState originalState, ResizeState newState)
        {
            this.OriginalState = originalState;
            this.NewState = newState;
        }

        public ResizeState NewState { get; }
        public ResizeState OriginalState { get; }
    }
}
