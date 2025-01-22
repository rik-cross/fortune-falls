/*
 * Maps an input string to an InputItem
 * 
 * An InputItem can contain multiple inputs
 * e.g. Keys.E, Buttons.RightTrigger and MouseButtons.LeftMouseButton
 */

using System;
using System.Collections.Generic;

namespace Engine
{
    class PlayerControlComponent : Component
    {
        private Dictionary<string, InputItem> _inputs;

        public PlayerControlComponent()
        {
            _inputs = new Dictionary<string, InputItem>();
        }

        public InputItem Get(string input)
        {
            if (_inputs.TryGetValue(input, out InputItem value))
                return value;
            else
                return null;
        }

        public void Set(string input, InputItem value)
        {
            _inputs[input] = value;
        }

        public bool Contains(string input)
        {
            return _inputs.ContainsKey(input);
        }
    }
}
