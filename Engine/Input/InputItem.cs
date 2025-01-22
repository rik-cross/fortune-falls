using Microsoft.Xna.Framework.Input;

namespace Engine
{
    /*
     *   InputItem
     *   
     *   Encapulates an input, to be associated with an InputMethod   
     *   e.g. InputMethod.button1 = InputItem Enter
     */

    public class InputItem
    {
        // stores a key (for keyboard input only)
        public Keys? Key { get; set; }
        // stores a button (for controller input only)
        public Buttons? Button { get; set; }
        // stores a mouse button (for mouse input only)
        public MouseButtons? MouseButton { get; set; }

        public InputItem(Keys? key = null, Buttons? button = null, MouseButtons? mouseButton = null)
        {
            Key = key;
            Button = button;
            MouseButton = mouseButton;
        }

    }

}
