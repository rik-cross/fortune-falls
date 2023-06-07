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
        //public bool visible { get; set; }
        public float Alpha { get; set; }
        public string LastState { get; set; }

        public SpriteComponent()
        {
            SpriteDict = new Dictionary<string, Sprite>();
            Alpha = 1.0f;
        }

        // Constructor for a single static sprite
        public SpriteComponent(string filePath, string key = "default")
        {
            SpriteDict = new Dictionary<string, Sprite>();
            Alpha = 1.0f;
            LastState = key;
            AddSprite(filePath, key);
        }

        // Add a sprite to the dictionary
        public void AddSprite(string key, Sprite sprite)
        {
            SpriteDict[key] = sprite;
        }

        // Add a static sprite from a single image
        public void AddSprite(string filePath, string key = "default",
            Vector2 offset = default, bool flipH = false, bool flipV = false)
        {
            Texture2D texture = Globals.content.Load<Texture2D>(filePath);
            Vector2 size = new Vector2(texture.Width, texture.Height);
            Sprite sprite = new Sprite(texture, size, offset, flipH, flipV);
            //Sprite sprite = new Sprite(Engine.Utils.LoadTexture(filePath + ".png"));
            SpriteDict[key] = sprite;
        }

        // Add a static sprite from a sprite sheet
        public void AddSprite(string filePath, string key,
            int frame, int endFrame, int totalRows = 1, int framesPerRow = -1,
            Vector2 offset = default, bool flipH = false, bool flipV = false)
        {
            // Load the sprite sheet
            Texture2D spriteSheet = Globals.content.Load<Texture2D>(filePath);

            // Assume only one row if frames per row is not given
            if (framesPerRow == -1)
                framesPerRow = endFrame + 1;

            // Calculate the width and height of a single frame
            int spriteWidth = spriteSheet.Width / framesPerRow;
            int spriteHeight = spriteSheet.Height / totalRows;
            Vector2 spriteSize = new Vector2(spriteWidth, spriteHeight);

            // Calculate the x and y index values
            int x = frame % framesPerRow;
            int y = frame / framesPerRow;

            // Add the sprite
            Texture2D texture = GetSubTexture(spriteSheet, x, y, spriteWidth, spriteHeight);
            Sprite sprite = new Sprite(texture, spriteSize, offset, flipH, flipV);
            AddSprite(key, new Sprite(texture));
        }

        // Add multiple static sprites from a single spritesheet
        public void AddMultipleStaticSprites(string filePath, List<string> keys,
            int totalRows = 1, int framesPerRow = -1,
            Vector2 offset = default, bool flipH = false, bool flipV = false)
        {
            // Load the sprite sheet
            Texture2D spriteSheet = Globals.content.Load<Texture2D>(filePath);

            // Assume only one row if sprites per row is not given
            if (framesPerRow == -1)
                framesPerRow = keys.Count / totalRows;

            // Calculate the width and height of a sprite
            int spriteWidth = spriteSheet.Width / framesPerRow;
            int spriteHeight = spriteSheet.Height / totalRows;
            Vector2 spriteSize = new Vector2(spriteWidth, spriteHeight);

            // Slice the sprite sheet using size of keys list
            int x, y;
            for (int i = 0; i < keys.Count; i++)
            {
                // Calculate the x and y index values
                x = i % framesPerRow;
                y = i / framesPerRow;

                // Add the sprite
                Texture2D texture = GetSubTexture(spriteSheet, x, y, spriteWidth, spriteHeight);
                Sprite sprite = new Sprite(texture, spriteSize, offset, flipH, flipV);
                AddSprite(keys[i], sprite);
            }
        }

        /// <summary>
        /// Gets the Sprite for a given state.
        /// </summary>
        /// <param name="state">The state associated with the Sprite to retrieve.</param>
        /// <returns>Sprite, or null if no state exists for the state provided.</returns>
        public Sprite GetSprite(string state = "default")
        {
            if (SpriteDict.ContainsKey(state))
                return SpriteDict[state];
            else
                return null;

            // Testing
            //return SpriteDict[state];
        }

        public Vector2 GetSpriteSize(string state = "default")
        {
            //if (SpriteDict.ContainsKey(state))
            //    return SpriteDict[state].size;
            //else
            //    return new Vector2(0, 0);

            // Testing
            return SpriteDict[state].Size;

            // Todo Account for offset?
            //return SpriteDict[state].size + SpriteDict[state].offset;
        }

        public Texture2D GetSubTexture(Texture2D texture, int x, int y, int width, int height)
        {
            // Create the new sub texture
            Rectangle rect = new Rectangle(x * width, y * height, width, height);
            Texture2D subTexture = new Texture2D(Globals.graphicsDevice, rect.Width, rect.Height);

            // Set the texture data
            Color[] data = new Color[rect.Width * rect.Height];
            texture.GetData(0, rect, data, 0, data.Length);
            subTexture.SetData(data);

            return subTexture;
        }

    }
}