using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React.Core
{
    public class PrimitiveProps
    {
        public PrimitiveProps(Action<MouseEvent, Bounds> onMouse, Action<KeyboardEvent> onKeyboard)
        {
            this.OnMouse = onMouse;
            this.OnKeyboard = onKeyboard;
        }

        public Action<MouseEvent, Bounds> OnMouse { get; }
        public Action<KeyboardEvent> OnKeyboard { get; }
    }
}
