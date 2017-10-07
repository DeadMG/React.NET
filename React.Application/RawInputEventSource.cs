using React.Box;
using React.Core;
using SharpDX.RawInput;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace React.Application
{
    public class RawInputEventSource
    {
        public RawInputEventSource(Func<Point?> currentPoint)
        {
            Device.RegisterDevice(SharpDX.Multimedia.UsagePage.Generic, SharpDX.Multimedia.UsageId.GenericMouse, DeviceFlags.None);
            Device.RegisterDevice(SharpDX.Multimedia.UsagePage.Generic, SharpDX.Multimedia.UsageId.GenericKeyboard, DeviceFlags.None);

            var location = currentPoint();
            var currentState = new MouseState(location.Value.X, location.Value.Y, false);
            Device.MouseInput += (object sender, MouseInputEventArgs e) =>
            {
                var newLocation = currentPoint();
                if (newLocation == null) return;
                var isLeftDown = e.ButtonFlags == MouseButtonFlags.LeftButtonDown ? true : e.ButtonFlags == MouseButtonFlags.LeftButtonUp ? false : currentState.LeftButtonDown;
                var nextState = new MouseState(newLocation.Value.X, newLocation.Value.Y, isLeftDown);
                if (nextState.LeftButtonDown != currentState.LeftButtonDown || nextState.X != currentState.X || nextState.Y != currentState.Y)
                {
                    var originalState = currentState;
                    currentState = nextState;
                    Mouse?.Invoke(new ChangeEvent<MouseState>(originalState, nextState));
                }
            };

            Device.KeyboardInput += (object sender, KeyboardInputEventArgs e) =>
            {
                uint virtualKey = (uint)e.Key;
                uint scanCode = (uint)e.MakeCode;
                uint flags = (uint)e.ScanCodeFlags;

                if (virtualKey == 255) return;

                if (virtualKey == (uint)System.Windows.Forms.Keys.ShiftKey)
                {
                    virtualKey = MapVirtualKey(scanCode, 3);
                }
                else if (virtualKey == (uint)System.Windows.Forms.Keys.NumLock)
                {
                    scanCode = MapVirtualKey(virtualKey, 0) | 0x100;
                }

                var isE0 = ((flags & (uint)ScanCodeFlags.E0) != 0);
                var isE1 = ((flags & (uint)ScanCodeFlags.E1) != 0);

                if (isE1)
                {
                    // for escaped sequences, turn the virtual key into the correct scan code using MapVirtualKey.
                    // however, MapVirtualKey is unable to map VK_PAUSE (this is a known bug), hence we map that by hand.
                    if (virtualKey == (uint)System.Windows.Forms.Keys.Pause)
                        scanCode = 0x45;
                    else
                        scanCode = MapVirtualKey(virtualKey, 0);
                }

                switch((System.Windows.Forms.Keys)virtualKey)
                {
                    case System.Windows.Forms.Keys.Control:
                        virtualKey = isE0 ? (uint)System.Windows.Forms.Keys.RControlKey : (uint)System.Windows.Forms.Keys.LControlKey;
                        break;
                    case System.Windows.Forms.Keys.Menu:
                        virtualKey = isE0 ? (uint)System.Windows.Forms.Keys.RMenu : (uint)System.Windows.Forms.Keys.LMenu;
                        break;
                    //case System.Windows.Forms.Keys.Return:
                    //    if (isE0) virtualKey = System.Windows.Forms.Keys.NumpadEnter;
                    //    break;
                    case System.Windows.Forms.Keys.Insert:
                        if (!isE0) virtualKey = (uint)System.Windows.Forms.Keys.NumPad0;
                        break;
                    case System.Windows.Forms.Keys.Delete:
                        if (!isE0) virtualKey = (uint)System.Windows.Forms.Keys.Decimal;
                        break;
                    case System.Windows.Forms.Keys.Home:
                        if (!isE0) virtualKey = (uint)System.Windows.Forms.Keys.NumPad7;
                        break;
                    case System.Windows.Forms.Keys.End:
                        if (!isE0) virtualKey = (uint)System.Windows.Forms.Keys.NumPad1;
                        break;
                    case System.Windows.Forms.Keys.Prior:
                        if (!isE0) virtualKey = (uint)System.Windows.Forms.Keys.NumPad9;
                        break;
                    case System.Windows.Forms.Keys.Next:
                        if (!isE0) virtualKey = (uint)System.Windows.Forms.Keys.NumPad3;
                        break;
                    case System.Windows.Forms.Keys.Left:
                        if (!isE0) virtualKey = (uint)System.Windows.Forms.Keys.NumPad4;
                        break;
                    case System.Windows.Forms.Keys.Right:
                        if (!isE0) virtualKey = (uint)System.Windows.Forms.Keys.NumPad6;
                        break;
                    case System.Windows.Forms.Keys.Up:
                        if (!isE0) virtualKey = (uint)System.Windows.Forms.Keys.NumPad8;
                        break;
                    case System.Windows.Forms.Keys.Down:
                        if (!isE0) virtualKey = (uint)System.Windows.Forms.Keys.NumPad2;
                        break;
                    case System.Windows.Forms.Keys.Clear:
                        if (!isE0) virtualKey = (uint)System.Windows.Forms.Keys.NumPad5;
                        break;
                }
                var wasUp = ((flags & (uint)ScanCodeFlags.Break) != 0);

                var builder = new StringBuilder(512);
                GetKeyNameTextW((scanCode << 16) | ((isE0 ? 1u : 0u) << 24), builder, 512);
                
                var str = GetCharsFromKeys((uint)e.Key, (uint)e.MakeCode);

                Keyboard(new KeyboardEvent(builder.ToString(), str, wasUp));
            };
        }

        public static string GetCharsFromKeys(uint virtualKey, uint keys)
        {
            var buf = new StringBuilder(256);
            var keyboardState = new byte[256];
            GetKeyboardState(keyboardState);
            int x = ToUnicode(virtualKey, keys, keyboardState, buf, 256, 0);
            if (x == 0 || x == -1) return null;
            return buf.ToString();
        }

        public event Action<KeyboardEvent> Keyboard;
        public event Action<ChangeEvent<MouseState>> Mouse;

        [DllImport("user32.dll")]
        private static extern int GetKeyNameTextW(uint lParam, [Out, MarshalAs(UnmanagedType.LPWStr, SizeConst = 512)]StringBuilder receivingBuffer, int bufferSize);

        [DllImport("user32.dll")]
        private static extern uint MapVirtualKey(uint uCode, uint uMapType);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll")]
        private static extern int ToUnicode(uint virtualKeyCode, uint scanCode, byte[] keyboardState, [Out, MarshalAs(UnmanagedType.LPWStr, SizeConst = 64)] StringBuilder receivingBuffer, int bufferSize, uint flags);
    }
}
