using PaisleyPark.Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Input;

namespace PaisleyPark.ViewModels
{
    public static class Hotkeys
    {
        public static List<Hotkey> hotkeys = new List<Hotkey>();
    }

    public class Hotkey {
        public Keys key;
        public HotKeyModifiers modifiers;
        public Action<Preset> func;
        public Preset preset;

        public Hotkey(Keys key, HotKeyModifiers modifiers, Action<Preset> func, Preset preset) {
            this.key = key;
            this.modifiers = modifiers;
            this.func = func;
            this.preset = preset;
            Hotkeys.hotkeys.Add(this);
        }
    }
}
