using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame.Engine
{
    public static class Theme
    {
        public static Color main = Color.SaddleBrown;
        public static Color mid = Color.LightGray;
        public static Color low = Color.AntiqueWhite;

        public static Color primaryText = Color.DarkSlateGray;

        public static SpriteFont primaryFont = Globals.font;
        public static SpriteFont secondaryFont = Globals.fontSmall;

        public static int largeBorder = 20; // const instead of static?
        public static int smallBorder = 5;
    }
}