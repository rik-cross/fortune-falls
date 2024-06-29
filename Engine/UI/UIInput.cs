using System;
using System.Collections.Generic;
using S = System.Diagnostics.Debug;

using Microsoft.Xna.Framework.Input;

namespace AdventureGame.Engine
{
    /*
     * Used for the UI input controls
     * Maps an input string to an InputItem
     * 
     * An InputItem can contain multiple inputs
     * e.g. Keys.E, Buttons.RightTrigger and MouseButtons.LeftMouseButton
     */
    public static class UIInput
    {
        private static Dictionary<string, InputItem> _inputs = new Dictionary<string, InputItem>();

        public static InputItem Get(string input)
        {
            if (_inputs.TryGetValue(input, out InputItem value))
                return value;
            else
                return null;
        }

        public static void Set(string input, InputItem value)
        {
            _inputs[input] = value;
        }

        public static bool Contains(string input)
        {
            return _inputs.ContainsKey(input);
        }

        // Set the basic menu controls
        public static void InitialiseUIControls()
        {
            Set("up", new Engine.InputItem(key: Keys.W, button: Buttons.LeftThumbstickUp));
            Set("down", new Engine.InputItem(key: Keys.S, button: Buttons.LeftThumbstickDown));
            Set("left", new Engine.InputItem(key: Keys.A, button: Buttons.LeftThumbstickLeft));
            Set("right", new Engine.InputItem(key: Keys.D, button: Buttons.LeftThumbstickRight));
            Set("back", new Engine.InputItem(key: Keys.Escape, button: Buttons.Back));
            Set("select", new Engine.InputItem(key: Keys.Enter, button: Buttons.A));
        }
    }
}
