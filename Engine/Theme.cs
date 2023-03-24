using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame.Engine
{
    public static class Theme
    {
        public static Color ColorPrimary = Color.SaddleBrown;
        public static Color ColorSecondary = Color.LightGray;
        public static Color ColorTertiary = Color.AntiqueWhite;

        public static Color TextColorPrimary = Color.DarkSlateGray;
        public static Color TextColorSecondary = Color.Gray;
        public static Color TextColorTertiary = Color.Yellow;

        public static readonly SpriteFont FontTitle = Globals.content.Load<SpriteFont>("Fonts/Title");
        public static readonly SpriteFont FontSubtitle = Globals.content.Load<SpriteFont>("Fonts/Subtitle");

        public static readonly SpriteFont FontPrimary = Globals.content.Load<SpriteFont>("Fonts/Large");
        public static readonly SpriteFont FontSecondary = Globals.content.Load<SpriteFont>("Fonts/Medium");
        public static readonly SpriteFont FontTertiary = Globals.content.Load<SpriteFont>("Fonts/Small");

        public static Color BorderColorPrimary = Color.Black;
        public static Color BorderColorSecondary = Color.DarkGray;

        public static Color BorderHighlightPrimary = Color.Goldenrod;
        public static Color BorderHighlightSecondary = Color.Red;

        public static readonly int BorderTiny = 1;
        public static readonly int BorderSmall = 3;
        public static readonly int BorderMedium = 4;
        public static readonly int BorderLarge = 12;
        public static readonly int BorderExtraLarge = 20;

        public static readonly Color HealthLevelLow = Color.Red;
        public static readonly Color HealthLevelMedium = Color.Orange;
        public static readonly Color HealthLevelHigh = Color.Green;
    }
}