/*
 *   MouseInput
 *   
 *   Defines all mouse input as an InputItem
 */

namespace AdventureGame.Engine
{
    public static class MouseInput
    {
        public static InputItem LeftButton = new InputItem(mouseButton: MouseButtons.LeftMouseButton);
        public static InputItem RightButton = new InputItem(mouseButton: MouseButtons.RightMouseButton);
        public static InputItem MiddleButton = new InputItem(mouseButton: MouseButtons.MiddleMouseButton);

    }

}
