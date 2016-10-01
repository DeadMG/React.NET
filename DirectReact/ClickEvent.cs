using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectReact
{
    public class ClickEvent : IPositionedEvent
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
