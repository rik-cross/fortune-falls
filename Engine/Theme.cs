using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame.Engine
{
    public static class Theme
    {
        public static Color primary = Color.SaddleBrown;
        public static Color secondary = Color.LightGray;
        public static Color tertiary = Color.AntiqueWhite;

        public static Color primaryText = Color.DarkSlateGray;
        public static Color secondaryText = Color.Gray;
        public static Color tertiaryText = Color.Yellow;

        public static SpriteFont primaryFont = Globals.content.Load<SpriteFont>("File");
        public static SpriteFont secondaryFont = Globals.content.Load<SpriteFont>("small");
        public static SpriteFont tertiaryFont = Globals.content.Load<SpriteFont>("small");

        public static readonly int smallBorder = 5;
        public static readonly int mediumBorder = 10;
        public static readonly int largeBorder = 20;
    }
}