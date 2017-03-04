using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React.Core
{
    public class PrimitiveProps
    {
        public PrimitiveProps(Action<MouseEvent, Bounds> onMouse = null)
        {
            this.OnMouse = onMouse;
        }

        public Action<MouseEvent, Bounds> OnMouse { get; }
    }
}
