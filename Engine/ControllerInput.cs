﻿using Microsoft.Xna.Framework.Input;

namespace AdventureGame.Engine
{
    public static class ControllerInput
    {

        public static InputItem Start = new InputItem(button: Buttons.Start);
        public static InputItem Back = new InputItem(button: Buttons.Back);

        public static InputItem A = new InputItem(button: Buttons.A);
        public static InputItem B = new InputItem(button: Buttons.B);
        public static InputItem X = new InputItem(button: Buttons.X);
        public static InputItem Y = new InputItem(button: Buttons.Y);

        public static InputItem LeftThumbUp = new InputItem(button: Buttons.LeftThumbstickUp);
        public static InputItem LeftThumbLeft = new InputItem(button: Buttons.LeftThumbstickLeft);
        public static InputItem LeftThumbRight = new InputItem(button: Buttons.LeftThumbstickRight);
        public static InputItem LeftThumbDown = new InputItem(button: Buttons.LeftThumbstickDown);

    }

}
