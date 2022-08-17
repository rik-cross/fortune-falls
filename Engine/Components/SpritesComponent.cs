using MonoGame.Extended.Sprites;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    public class SpritesComponent : Component
    {

        public Dictionary<string, Sprite> SpriteDict { get; private set; }
        public bool visible;
        public string lastState;

        public SpritesComponent(string key, Sprite sprite, bool visible = true)
        {
            this.SpriteDict = new Dictionary<string, Sprite>();
            AddSprite(key, sprite);
            this.visible = visible;
            this.lastState = "idle";
        }

        public Sprite GetSprite(string state)
        {
            return SpriteDict[state];
        }

        public void AddSprite(string key, Sprite sprite)
        {
            SpriteDict[key] = sprite;
        }

    }
}
