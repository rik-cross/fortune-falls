/*
 *   KeyboardInput
 *   
 *   Defines all keyboard input as an InputItem
 */

using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    public static class KeyboardInput
    {
        public static List<Keys> keyList = new List<Keys>();

        public static InputItem Enter = new InputItem(key: Keys.Enter);
        public static InputItem Escape = new InputItem(key: Keys.Escape);

        public static InputItem LeftShift = new InputItem(key: Keys.LeftShift);

        public static InputItem W = new InputItem(key: Keys.W);
        public static InputItem A = new InputItem(key: Keys.A);
        public static InputItem S = new InputItem(key: Keys.S);
        public static InputItem D = new InputItem(key: Keys.D);

        // Scenes / menu input
        public static InputItem T = new InputItem(key: Keys.T);
        public static InputItem P = new InputItem(key: Keys.P);
        public static InputItem I = new InputItem(key: Keys.I);

        public static InputItem Q = new InputItem(key: Keys.Q);
        public static InputItem E = new InputItem(key: Keys.E);

        public static InputItem Up = new InputItem(key: Keys.Up);
        public static InputItem Left = new InputItem(key: Keys.Left);
        public static InputItem Down = new InputItem(key: Keys.Down);
        public static InputItem Right = new InputItem(key: Keys.Right);

    }

}
