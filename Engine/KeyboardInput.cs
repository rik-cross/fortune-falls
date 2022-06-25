using Microsoft.Xna.Framework.Input;

namespace AdventureGame.Engine
{
    public static class KeyboardInput
    {

        public static InputItem Enter = new InputItem(key: Keys.Enter);
        public static InputItem Escape = new InputItem(key: Keys.Escape);

        public static InputItem W = new InputItem(key: Keys.W);
        public static InputItem A = new InputItem(key: Keys.A);
        public static InputItem S = new InputItem(key: Keys.S);
        public static InputItem D = new InputItem(key: Keys.D);

        public static InputItem P = new InputItem(key: Keys.P);

        public static InputItem Up = new InputItem(key: Keys.Up);
        public static InputItem Left = new InputItem(key: Keys.Left);
        public static InputItem Down = new InputItem(key: Keys.Down);
        public static InputItem Right = new InputItem(key: Keys.Right);

    }

}
