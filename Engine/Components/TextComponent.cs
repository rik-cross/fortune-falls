using Microsoft.Xna.Framework;

namespace AdventureGame.Engine
{
    public class TextComponent : Component
    {
        public string text;
        public Color colour;
        public TextComponent(string text)
        {
            this.text = text;
            this.colour = Color.White;
        }
    }
}
