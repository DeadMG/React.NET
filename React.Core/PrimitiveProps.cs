using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React.Core
{
    public class PrimitiveProps
    {
        public PrimitiveProps(Action<ClickEvent> onMouseClick = null)
        {
            this.OnMouseClick = onMouseClick;
        }

        public Action<ClickEvent> OnMouseClick { get; }
    }
}
