using MonoGame.Extended.Sprites;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    public class SpritesComponent : Component
    {
        public Sprite sprite;
        public string lastState = "idle";
        public Dictionary<string, Sprite> spriteDict = new Dictionary<string, Sprite>();
        public SpritesComponent(string key, Sprite sprite)
        {
            AddSprite(key, sprite);
        }

        public void AddSprite(string key, Sprite sprite)
        {
            this.spriteDict[key] = sprite;
        }

    }
}
