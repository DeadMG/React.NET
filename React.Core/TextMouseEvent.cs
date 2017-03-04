using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React.Core
{
    public class TextMouseState
    {
        public TextMouseState(MouseState mouseState, int? textIndex)
        {
            this.Mouse = mouseState;
            this.TextIndex = textIndex;
        }

        public MouseState Mouse { get; }
        public int? TextIndex { get; }
    }

    public class TextMouseEvent
    {
        public TextMouseEvent(TextMouseState originalState, TextMouseState newState)
        {
            this.OriginalState = originalState;
            this.NewState = newState;
        }

        public TextMouseState OriginalState { get; }
        public TextMouseState NewState { get; }
    }
}
