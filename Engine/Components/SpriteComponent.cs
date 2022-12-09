//using MonoGame.Extended.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;
using System.Linq;

namespace AdventureGame.Engine
{
    public class SpriteComponent : Component
    {

        public Dictionary<string, Sprite> SpriteDict { get; private set; }
        public bool visible;
        public string lastState;

        public SpriteComponent()
        {
            SpriteDict = new Dictionary<string, Sprite>();
            lastState = "idle";
        }

        public SpriteComponent(Sprite sprite, string key = "idle", bool visible = true)
        {
            SpriteDict = new Dictionary<string, Sprite>();
            AddSprite(key, sprite);
            this.visible = visible;
            lastState = "idle";
        }

        public SpriteComponent(string filePath, string key = "idle", bool visible = true)
        {
            SpriteDict = new Dictionary<string, Sprite>();
            Sprite sprite = new Sprite(Globals.content.Load<Texture2D>(filePath));
            AddSprite(key, sprite);
            this.visible = visible;
            lastState = "idle";
        }

        public SpriteComponent(Texture2D texture, string key = "idle", bool visible = true)
        {
            SpriteDict = new Dictionary<string, Sprite>();
            Sprite sprite = new Sprite(texture);
            AddSprite(key, sprite);
            this.visible = visible;
            lastState = "idle";
        }

        public SpriteComponent(SpriteSheet spriteSheet, string key = "idle",
            bool visible = true)
        {
            SpriteDict = new Dictionary<string, Sprite>();
            Sprite sprite = new Sprite(spriteSheet.texture);
            AddSprite(key, sprite);
            this.visible = visible;
            lastState = "idle";
        }

        // Change x, y to row, column to match AddSprite methods below?
        public SpriteComponent(SpriteSheet spriteSheet, int x, int y, string key = "idle",
            bool visible = true)
        {
            SpriteDict = new Dictionary<string, Sprite>();
            Sprite sprite = new Sprite(spriteSheet.GetSubTexture(x, y));
            AddSprite(key, sprite);
            this.visible = visible;
            lastState = "idle";
        }

        public SpriteComponent(SpriteSheet spriteSheet, List<List<int>> subTextureValues,
            string key = "idle", bool visible = true)
        {
            SpriteDict = new Dictionary<string, Sprite>();
            AddSprite(key, spriteSheet, subTextureValues);
            this.visible = visible;
            lastState = "idle";
        }

        public SpriteComponent(string filePath, int width, int height,
            int rowIndex, int startColumn, int endColumn,
            string key = "idle", bool visible = true)
        {
            SpriteSheet spriteSheet = new SpriteSheet(filePath, width, height);
            SpriteDict = new Dictionary<string, Sprite>();
            AddSprite(key, spriteSheet, rowIndex, startColumn, endColumn);
            this.visible = visible;
            lastState = "idle";
        }

        public Sprite GetSprite(string state = "idle")
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

        // Indices start from 0. Use neutral to repeat a texture at the end of the loop
        public void AddSprite(string key, SpriteSheet spriteSheet, int rowIndex,
            int startColumn, int endColumn, bool repeatNeutral = false, int neutralIndex = -1)
        {
            List<Texture2D> subTextures = new List<Texture2D>();

            for (int i = startColumn; i <= endColumn; i++)
                subTextures.Add(spriteSheet.GetSubTexture(i, rowIndex));

            // Repeat the neutral sprite so the spritesheet loops nicely
            if (repeatNeutral && neutralIndex >= 0 && neutralIndex <= endColumn)
                subTextures.Add(spriteSheet.GetSubTexture(neutralIndex, rowIndex));

            Sprite sprite = new Sprite(subTextures);
            AddSprite(key, sprite);
        }

        public void SetAnimationDelay(int delay)
        {
            foreach (Sprite sprite in SpriteDict.Values)
                sprite.animationDelay = delay;
        }

        public void ModifyAnimationDelay(float modifier)
        {
            foreach (Sprite sprite in SpriteDict.Values)
                sprite.animationDelay = (int)(sprite.animationDelay * modifier);
        }

        // Assumes the row contains all the sprites for the animation
        /*public void AddSprite(string key, SpriteSheet spriteSheet, int rowIndex,
            int startColumn, int endColumn)
        {
            List<Texture2D> subTextures = new List<Texture2D>();

            for (int i = startColumn; i <= endColumn; i++)
            {
                subTextures.Add(spriteSheet.GetSubTexture(i, rowIndex));
            }

            Sprite sprite = new Sprite(subTextures);
            AddSprite(key, sprite);
        }*/

    }
}
