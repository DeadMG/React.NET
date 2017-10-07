using React.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React.Box
{
    public class KeyboardEvent : IEvent
    {
        public KeyboardEvent(string keyName, string textValue, bool wasUp) 
        {
            this.KeyName = keyName;
            this.TextValue = textValue;
            this.WasUp = wasUp;
        }

        public string KeyName { get; }
        public string TextValue { get; }
        public bool WasUp { get; }
    }
}
