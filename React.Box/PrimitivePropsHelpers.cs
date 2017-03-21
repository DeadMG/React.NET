using React.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React.Box
{
    public class PrimitivePropsHelpers
    {
        public static void FireEvents(PrimitiveProps props, Bounds bounds, List<IEvent> events)
        {
            foreach (var mouseEvent in events.OfType<MouseEvent>())
                props.OnMouse?.Invoke(mouseEvent, bounds);
            foreach (var keyboardEvent in events.OfType<KeyboardEvent>())
                props.OnKeyboard?.Invoke(keyboardEvent);
        }
    }
}
