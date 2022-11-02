/*
 *   MouseInput
 *   
 *   Defines all mouse input as an InputItem
 */

using Microsoft.Xna.Framework.Input;

namespace AdventureGame.Engine
{
    public static class MouseInput
    {
        public static InputItem LeftButton = new InputItem(mouseButton: MouseButtons.LeftMouseButton);
        public static InputItem RightButton = new InputItem(mouseButton: MouseButtons.RightMouseButton);
        public static InputItem MiddleButton = new InputItem(mouseButton: MouseButtons.MiddleMouseButton);

        //public static InputItem LeftButton = new InputItem(leftMouse: Mouse.GetState().LeftButton);
        //public static InputItem RightButton = new InputItem(rightMouse: Mouse.GetState().RightButton);
        //public static InputItem MiddleButton = new InputItem(middleMouse: Mouse.GetState().MiddleButton);

    }

}
