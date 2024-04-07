/*
 *   ControllerInput
 *   
 *   Defines all controller input as an InputItem
 */

using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    public static class ControllerInput
    {

        public static List<Buttons> ButtonList = new List<Buttons>();

        public static InputItem Menu = new InputItem(button: Buttons.BigButton);
        public static InputItem Start = new InputItem(button: Buttons.Start);
        public static InputItem Back = new InputItem(button: Buttons.Back);

        public static InputItem LeftShoulder = new InputItem(button: Buttons.LeftShoulder);
        public static InputItem LeftTrigger = new InputItem(button: Buttons.LeftTrigger);
        public static InputItem RightShoulder = new InputItem(button: Buttons.RightShoulder);
        public static InputItem RightTrigger = new InputItem(button: Buttons.RightTrigger);

        public static InputItem A = new InputItem(button: Buttons.A);
        public static InputItem B = new InputItem(button: Buttons.B);
        public static InputItem X = new InputItem(button: Buttons.X);
        public static InputItem Y = new InputItem(button: Buttons.Y);

        public static InputItem LeftThumbUp = new InputItem(button: Buttons.LeftThumbstickUp);
        public static InputItem LeftThumbDown = new InputItem(button: Buttons.LeftThumbstickDown);
        public static InputItem LeftThumbLeft = new InputItem(button: Buttons.LeftThumbstickLeft);
        public static InputItem LeftThumbRight = new InputItem(button: Buttons.LeftThumbstickRight);

        public static InputItem RightThumbUp = new InputItem(button: Buttons.RightThumbstickUp);
        public static InputItem RightThumbDown = new InputItem(button: Buttons.RightThumbstickDown);
        public static InputItem RightThumbLeft = new InputItem(button: Buttons.RightThumbstickLeft);
        public static InputItem RightThumbRight = new InputItem(button: Buttons.RightThumbstickRight);

        public static InputItem DPadUp = new InputItem(button: Buttons.DPadUp);
        public static InputItem DPadDown = new InputItem(button: Buttons.DPadDown);
        public static InputItem DPadLeft = new InputItem(button: Buttons.DPadLeft);
        public static InputItem DPadRight = new InputItem(button: Buttons.DPadRight);

    }

}
