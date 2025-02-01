using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine
{
    public static class Theme
    {
        public static Color ColorPrimary = new Color(234, 212, 170);
        public static Color ColorSecondary = new Color(194, 133, 105);
        public static Color ColorTertiary = Color.AntiqueWhite;

        public static Color TextColorPrimary = ColorPrimary;
        public static Color TextColorSecondary = ColorSecondary;
        public static Color TextColorTertiary = ColorTertiary;

        public static readonly SpriteFont FontTitle = EngineGlobals.content.Load<SpriteFont>("Fonts/Title");
        public static readonly SpriteFont FontSubtitle = EngineGlobals.content.Load<SpriteFont>("Fonts/Subtitle");

        public static readonly SpriteFont FontPrimary = EngineGlobals.content.Load<SpriteFont>("Fonts/Large");
        public static readonly SpriteFont FontSecondary = EngineGlobals.content.Load<SpriteFont>("Fonts/Medium");
        public static readonly SpriteFont FontTertiary = EngineGlobals.content.Load<SpriteFont>("Fonts/Small");

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