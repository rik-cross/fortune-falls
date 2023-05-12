using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame.Engine
{
    /// <summary>
    /// Contains a dictionary of {State -> Sprite} pairings,
    /// along with the visibility and opacity of the entity.
    /// </summary>
    public class SpriteComponent : Component
    {
        public Dictionary<string, Sprite> SpriteDict { get; private set; }
        public bool visible;
        public float alpha = 1.0f;
        //public bool flipH = true;
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

        //public SpriteComponent(SpriteSheet spriteSheet, List<List<int>> subTextureValues,
        //    string key = "idle", bool visible = true)
        //{
        //    SpriteDict = new Dictionary<string, Sprite>();
        //    AddSprite(key, spriteSheet, subTextureValues);
        //    this.visible = visible;
        //    lastState = "idle";
        //}

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

        /// <summary>
        /// Gets the Sprite for a given state.
        /// </summary>
        /// <param name="state">The state associated with the Sprite to retrieve.</param>
        /// <returns>Sprite, or null if no state exists for the state provided.</returns>
        public Sprite GetSprite(string state = "idle")
        {
            return SpriteDict[state];
        }

        public Vector2 GetSpriteSize(string state = "idle")
        {
            return SpriteDict[state].size;
        }

        public void AddSprite(string key, Sprite sprite)
        {
            SpriteDict[key] = sprite;
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

        //
        // NEW METHODS
        //


        public void AddSprite(string key, int x, int y)
        {
            //Sprite sprite = spriteSheet.GetSubTexture(x, y);
            //SpriteDict[key] = sprite;
        }

        // Creates a new sprite sheet and slices sprites based on
        // total frames and number of rows
        public void AddSpriteSheet(string key, string filePath,
            int totalFrames, int rows = 1, int framesPerRow = -1)
        {
            // Load the sprite sheet texture
            SpriteSheet spriteSheet = new SpriteSheet(filePath);

            // Calculate frames per row if not given
            if (rows <= 1)
                framesPerRow = totalFrames;
            else if (rows > 1 && framesPerRow < 1)
                framesPerRow = totalFrames / rows;

            // Calculate the width and height of a sprite
            int spriteWidth = spriteSheet.texture.Width / framesPerRow;
            int spriteHeight = spriteSheet.texture.Height / rows;
            spriteSheet.spriteSize = new Vector2(spriteWidth, spriteHeight);

            // Slice each individual sprite texture
            List<Texture2D> subTextures = new List<Texture2D>();
            int x = 0;
            int y = 0;
            for (int i = 0; i < totalFrames; i++)
            {
                x = i % framesPerRow;
                y = i / framesPerRow;
                subTextures.Add(spriteSheet.GetSubTexture(x, y));
            }

            Sprite sprite = new Sprite(subTextures);
            AddSprite(key, sprite);
        }

        //public void AddSprite(string key, SpriteSheet spriteSheet,
        //    List<List<int>> subTextureValues)
        //{
        //    List<Texture2D> subTextures = new List<Texture2D>();

        //    foreach (List<int> sub in subTextureValues)
        //    {
        //        subTextures.Add(spriteSheet.GetSubTexture(sub[0], sub[1]));
        //    }

        //    Sprite sprite = new Sprite(subTextures);
        //    AddSprite(key, sprite);
        //}

        public void SetAnimationDelay(int delay)
        {
            foreach (Sprite sprite in SpriteDict.Values)
                sprite.animationDelay = delay;
        }

        public void ModifyAnimationDelay(float modifier)
        {
            foreach (Sprite sprite in SpriteDict.Values)
                sprite.animationDelay = (int)Math.Ceiling(sprite.animationDelay * modifier);
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
