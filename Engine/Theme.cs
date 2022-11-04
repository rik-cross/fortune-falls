using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame.Engine
{
    public static class Theme
    {
        // CHANGE to colorPrimary, textColorPrimary, borderSizeSmall etc
        public static Color primary = Color.SaddleBrown;
        public static Color secondary = Color.LightGray;
        public static Color tertiary = Color.AntiqueWhite;

        public static Color healthLevelLow = Color.Red;
        public static Color healthLevelMedium = Color.Orange;
        public static Color healthLevelHigh = Color.Green;

        public static Color primaryText = Color.DarkSlateGray;
        public static Color secondaryText = Color.Gray;
        public static Color tertiaryText = Color.Yellow;

        public static SpriteFont primaryFont = Globals.content.Load<SpriteFont>("File");
        public static SpriteFont secondaryFont = Globals.content.Load<SpriteFont>("small");
        public static SpriteFont tertiaryFont = Globals.content.Load<SpriteFont>("small");

        public static Color borderPrimary = Color.Black;
        public static Color borderSecondary = Color.DarkGray;

        public static Color borderHighlightPrimary = Color.Goldenrod; // or highlightPrimary?
        public static Color borderHighlightSecondary = Color.Red;

        public static readonly int tinyBorder = 1;
        public static readonly int smallBorder = 3;
        public static readonly int mediumBorder = 5;
        public static readonly int largeBorder = 12;
        public static readonly int extraLargeBorder = 20;
    }
}