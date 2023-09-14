using System.Collections.Generic;

namespace React.Box
{
    public interface IKey
    {

    }

    public class ContentKey : IKey
    {
        public string Content { get; }
    }

    public class ModifierKey : IKey
    {
        public static readonly ModifierKey Ctrl = new ModifierKey();
        public static readonly ModifierKey Shift = new ModifierKey();
    }

    public class ToggleKey : IKey
    {
        public static readonly ToggleKey CapsLock = new ToggleKey();
        public static readonly ToggleKey NumLock = new ToggleKey();
        public static readonly ToggleKey ScrollLock = new ToggleKey();
    }
    
    public class KeyboardState
    {
        public Dictionary<ToggleKey, bool> Toggles { get; }
        public Dictionary<IKey, bool> KeyStates { get; }
    }
}
