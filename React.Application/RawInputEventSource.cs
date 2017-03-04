using React.Core;
using SharpDX.RawInput;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React.Application
{
    public class RawInputEventSource
    {
        private class RawInputEventLevel : IEventLevel
        {
            private readonly RawInputEventLevel parent;
            private readonly HashSet<RawInputEventLevel> children = new HashSet<RawInputEventLevel>();
            private readonly HashSet<Action<MouseEvent>> mouseListeners = new HashSet<Action<MouseEvent>>();

            public RawInputEventLevel(RawInputEventLevel parent)
            {
                this.parent = parent;
                children = new HashSet<RawInputEventLevel>();
            }

            public event Action<KeyboardEvent> Keyboard;
            public event Action<MouseEvent> Mouse
            {
                add
                {
                    mouseListeners.Add(value);
                }
                remove
                {
                    mouseListeners.Remove(value);
                }
            }

            public IEventLevel CreateNestedEventLevel()
            {
                var level = new RawInputEventLevel(this);
                children.Add(level);
                return level;
            }

            public void Dispose()
            {
                parent?.children?.Remove(this);
            }

            public void Invoke(MouseEvent mouse)
            {
                var currentListeners = new HashSet<Action<MouseEvent>>(mouseListeners);
                foreach(var listener in currentListeners)
                {
                    if (mouseListeners.Contains(listener))
                        listener?.Invoke(mouse);
                }
                var currentChildren = new HashSet<RawInputEventLevel>(children);
                foreach (var child in currentChildren)
                {
                    if (children.Contains(child))
                        child.Invoke(mouse);
                }
            }
        }

        public RawInputEventSource(Func<Point> currentPoint)
        {
            var rootLevel = new RawInputEventLevel(null);
            Root = rootLevel;

            Device.RegisterDevice(SharpDX.Multimedia.UsagePage.Generic, SharpDX.Multimedia.UsageId.GenericMouse, DeviceFlags.None);

            var location = currentPoint();
            var currentState = new MouseState(location.X, location.Y, false);
            Device.MouseInput += (object sender, MouseInputEventArgs e) =>
            {
                var newLocation = currentPoint();
                var isLeftDown = e.ButtonFlags == MouseButtonFlags.LeftButtonDown ? true : e.ButtonFlags == MouseButtonFlags.LeftButtonUp ? false : currentState.LeftButtonDown;
                var nextState = new MouseState(newLocation.X, newLocation.Y, isLeftDown);

                rootLevel.Invoke(new MouseEvent(currentState, nextState));
                currentState = nextState;
            };
        }
        
        public IEventLevel Root { get; }
    }
}
