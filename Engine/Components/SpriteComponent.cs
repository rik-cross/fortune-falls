using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame.Engine
{
    /// <summary>
    /// Creates single static <see cref="Sprite"/>s from an image or sprite sheet.
    /// Contains a dictionary of {State -> Sprite} pairings,
    /// along with the <see cref="Alpha"/> value and previous state of the entity.
    /// </summary>
    public class SpriteComponent : Component
    {
        /// <value>
        /// A dictionary that stores the state as a key and a <see cref="Sprite"/> as the value.
        /// </value>
        public Dictionary<string, Sprite> SpriteDict { get; private set; }
        //public bool visible { get; set; }

        /// <value>
        /// Sets the alpha opacity of all the <see cref="Sprite"/>s.
        /// </value>
        public float Alpha { get; set; }

        /// <value>
        /// Stores the previous State of the entity.
        /// </value>
        public string LastState { get; set; }

        /// <summary>
        /// Constructor for a single static sprite.
        /// </summary>
        public SpriteComponent()
        {
            SpriteDict = new Dictionary<string, Sprite>();
            Alpha = 1.0f;
        }

        /// <summary>
        /// Constructor for a single static sprite that creates a new <see cref="Sprite"/>.
        /// </summary>
        /// <param name="filePath">The full path of the <see cref="Texture2D"/>.</param>
        /// <param name="key">A unique state of an entity that maps to a <see cref="Sprite"/>.</param>
        public SpriteComponent(string filePath, string key = "default")
        {
            SpriteDict = new Dictionary<string, Sprite>();
            Alpha = 1.0f;
            LastState = key;
            AddSprite(filePath, key);
        }

        /// <summary>
        /// Adds a sprite to the dictionary.
        /// </summary>
        /// <param name="key">A unique state of an entity that maps to a <see cref="Sprite"/>.</param>
        /// <param name="sprite">The <see cref="Sprite"/> to be added.</param>
        public void AddSprite(string key, Sprite sprite)
        {
            SpriteDict[key] = sprite;
        }

        /// <summary>
        /// Adds a static sprite from a single image.
        /// </summary>
        /// <param name="filePath">The full path of the <see cref="Texture2D"/>.</param>
        /// <param name="key">A unique state of an entity that maps to a <see cref="Sprite"/>.</param>
        /// <param name="offset">The amount to offset the draw area of the <see cref="Sprite"/>.</param>
        /// <param name="flipH">Flips the image horizontally.</param>
        /// <param name="flipV">Flips the image vertically.</param>
        public void AddSprite(string filePath, string key = "default",
            Vector2 offset = default, bool flipH = false, bool flipV = false)
        {
            Texture2D texture = Globals.content.Load<Texture2D>(filePath);
            Vector2 size = new Vector2(texture.Width, texture.Height);
            Sprite sprite = new Sprite(texture, size, offset, flipH, flipV);
            //Sprite sprite = new Sprite(Engine.Utils.LoadTexture(filePath + ".png"));
            SpriteDict[key] = sprite;
        }

        /// <summary>
        /// Adds a static sprite from a sprite sheet using the frame number starting from 0.
        /// </summary>
        /// <param name="filePath">The full path of the <see cref="Texture2D"/>.</param>
        /// <param name="key">A unique state of an entity that maps to a <see cref="Sprite"/>.</param>
        /// <param name="frame">The frame to get the <see cref="Sprite"/> from, starting from 0.</param>
        /// <param name="endFrame">The final frame in the sprite sheet, starting from 0.</param>
        /// <param name="totalRows">The total number of rows of sprites in the sprite sheet.</param>
        /// <param name="framesPerRow">The number of frames per row.</param>
        /// <param name="offset">The amount to offset the draw area of the <see cref="Sprite"/>.</param>
        /// <param name="flipH">Flips the image horizontally.</param>
        /// <param name="flipV">Flips the image vertically.</param>
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
            AddSprite(key, sprite);
            //AddSprite(key, new Sprite(texture));
        }

        /// <summary>
        /// Adds multiple static sprites from a single spritesheet using a list of keys.
        /// </summary>
        /// <param name="filePath">The full path of the <see cref="Texture2D"/>.</param>
        /// <param name="keys">A list of unique states of an entity that map to <see cref="Sprite"/>s.</param>
        /// <param name="totalRows">The total number of rows of sprites in the sprite sheet.</param>
        /// <param name="framesPerRow">The number of frames per row.</param>
        /// <param name="offset">The amount to offset the draw area of the <see cref="Sprite"/>.</param>
        /// <param name="flipH">Flips the image horizontally.</param>
        /// <param name="flipV">Flips the image vertically.</param>
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
        /// Gets the <see cref="Sprite"/> for a given state.
        /// </summary>
        /// <param name="state">The state associated with the <see cref="Sprite"/> to retrieve.</param>
        /// <returns><see cref="Sprite"/>, or null if no state exists for the state provided.</returns>
        public Sprite GetSprite(string state = "default")
        {
            if (SpriteDict.ContainsKey(state))
                return SpriteDict[state];
            else
                return null;

            // Testing
            //return SpriteDict[state];
        }

        /// <summary>
        /// Gets the size of a <see cref="Sprite"/> for a given state.
        /// </summary>
        /// <param name="state">The state associated with the <see cref="Sprite"/> to retrieve.</param>
        /// <returns><see cref="Vector2"/>. The values are 0, 0 if no state exists for the state provided.</returns>
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

        /// <summary>
        /// Slices a sprite sheet and returns a <see cref="Texture2D"/> for a single sprite.
        /// </summary>
        /// <param name="texture">The image or sprite sheet to slice.</param>
        /// <param name="row">The row index, starting from 0.</param>
        /// <param name="col">The column index, starting from 0.</param>
        /// <param name="width">The width of the sliced sprite.</param>
        /// <param name="height">The height of the sliced sprite.</param>
        /// <returns></returns>
        public Texture2D GetSubTexture(Texture2D texture, int row, int col, int width, int height)
        {
            // Create the new sub texture
            Rectangle rect = new Rectangle(row * width, col * height, width, height);
            Texture2D subTexture = new Texture2D(Globals.graphicsDevice, rect.Width, rect.Height);

            // Set the texture data
            Color[] data = new Color[rect.Width * rect.Height];
            texture.GetData(0, rect, data, 0, data.Length);
            subTexture.SetData(data);

            return subTexture;
        }

    }
}