//using MonoGame.Extended.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

namespace AdventureGame.Engine
{
    public class SpriteComponent : Component
    {

        public Dictionary<string, Sprite> SpriteDict { get; private set; }
        public bool visible;
        public string lastState;

        public SpriteComponent(string key, Sprite sprite, bool visible = true)
        {
            this.SpriteDict = new Dictionary<string, Sprite>();
            AddSprite(key, sprite);
            this.visible = visible;
            this.lastState = "idle";
        }

        public SpriteComponent(string filePath, string key = "idle", bool visible = true)
        {
            SpriteDict = new Dictionary<string, Sprite>();
            Sprite sprite = new Sprite(Globals.content.Load<Texture2D>(filePath));
            AddSprite(key, sprite);
            this.visible = visible;
            lastState = "idle";
        }

        public SpriteComponent(string key, Texture2D texture, bool visible = true)
        {
            SpriteDict = new Dictionary<string, Sprite>();
            Sprite sprite = new Sprite(texture);
            AddSprite(key, sprite);
            this.visible = visible;
            lastState = "idle";
        }

        public SpriteComponent(string key, SpriteSheet spriteSheet, int x, int y,
            bool visible = true)
        {
            SpriteDict = new Dictionary<string, Sprite>();
            Sprite sprite = new Sprite(spriteSheet.GetSubTexture(x, y));
            AddSprite(key, sprite);
            this.visible = visible;
            lastState = "idle";
        }

        public Sprite GetSprite(string state)
        {
            return SpriteDict[state];
        }

        public Vector2 GetSpriteSize(string state = "idle")
        {
            Sprite sprite = SpriteDict[state];
            Texture2D texture = sprite.textureList[0];
            Vector2 spriteSize = new Vector2(texture.Width, texture.Height);

            return spriteSize;
        }

        public void AddSprite(string key, Sprite sprite)
        {
            SpriteDict[key] = sprite;
        }

        public void AddSprite(string key, SpriteSheet spriteSheet,
            List<List<int>> subTextureValues)
        {
            List<Texture2D> subTextures = new List<Texture2D>();

            foreach (List<int> sub in subTextureValues)
            {
                subTextures.Add(spriteSheet.GetSubTexture(sub[0], sub[1]));
            }

            Sprite sprite = new Sprite(subTextures);
            AddSprite(key, sprite);
        }

    }
}
