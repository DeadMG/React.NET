using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React.Core
{
    public class TextLeftMouseUpEvent : LeftMouseUpEvent
    {
        public TextLeftMouseUpEvent(int x, int y, int textIndex) : base(x, y)
        {
            this.TextIndex = textIndex;
        }

        public int TextIndex { get; }
    }
}
