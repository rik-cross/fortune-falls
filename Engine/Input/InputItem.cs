/*
 *   InputItem
 *   
 *   Encapulates an input, to be associated with an InputMethod   
 *   e.g. InputMethod.button1 = InputItem Enter
 */

using Microsoft.Xna.Framework.Input;

namespace AdventureGame.Engine
{
    public class InputItem
    {

        // stores a key (for keyboard input only)
        public Keys? key;
        // stores a button (for controller input only)
        public Buttons? button;
        // stores a mouse button (for mouse input only)
        public MouseButtons? mouseButton;

        public InputItem(Keys? key = null, Buttons? button = null,
            MouseButtons? mouseButton = null)
        {
            this.key = key;
            this.button = button;
            this.mouseButton = mouseButton;
        }

    }

}
