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
        public bool visible { get; set; }
        public float alpha = 1.0f;
        //public bool flipH = true;
        public string lastState;


        public SpriteComponent()
        {
            SpriteDict = new Dictionary<string, Sprite>();
        }

        // Constructor for a single static sprite
        public SpriteComponent(string filePath, string key = "idle")
        {
            SpriteDict = new Dictionary<string, Sprite>();
            //Sprite sprite = new Sprite(Globals.content.Load<Texture2D>(filePath));
            AddSprite(filePath, key);
            lastState = key;
        }

        // Constructor for a single static sprite within a sprite sheet
        // Todo: X and Y are co-ordinate positions
        // elsewhere, X and Y are index values
        public SpriteComponent(string filePath, int x, int y,
            int width, int height, string key = "idle")
        {
            SpriteDict = new Dictionary<string, Sprite>();
            //Sprite sprite = new Sprite(Globals.content.Load<Texture2D>(filePath));
            AddSprite(filePath, key);
            lastState = key;
        }

        // Add a static sprite 
        public void AddSprite(string filePath, string key = "idle")
        {
            Sprite sprite = new Sprite(Globals.content.Load<Texture2D>(filePath));
            SpriteDict[key] = sprite;
        }

        // Add animated sprites using frames which start at 0.
        // Width and height calculated using framesPerRow.
        // Change delay to speed (FPS)??
        // Could be split into AnimatedSprite class and Sprite class
        public void AddAnimatedSprite(string filePath, string key,
            int startFrame, int endFrame, int totalRows = 1, int framesPerRow = -1,
            Vector2 offset = default, bool flipH = false, bool flipV = false,
            bool loop = true, int delay = 6)
        {
            // Load the entire image
            Texture2D spriteSheet = Globals.content.Load<Texture2D>(filePath);

            // Calculate the frames per row if not given
            if (framesPerRow == -1)
                framesPerRow = (endFrame - startFrame + 1) / totalRows;

            // Calculate the width and height of a single frame
            int frameWidth = spriteSheet.Width / framesPerRow;
            int frameHeight = spriteSheet.Height / totalRows;

            // Slice the sprite sheet using start and end frame
            List<Texture2D> subTextures = new List<Texture2D>();
            int x, y;
            for (int i = startFrame; i < endFrame; i++)
            {
                // Calculate the x and y index values
                x = i % framesPerRow;
                y = i / framesPerRow;
                subTextures.Add(GetSubTexture(spriteSheet, x, y, frameWidth, frameHeight));
            }

            Sprite sprite = new Sprite(subTextures, offset, flipH, flipV, loop, delay);
            AddSprite(key, sprite);
        }

        // Or pass the frame size
        // Calculate the start and end frame using the frame size if none are given
        public void AddAnimatedSprite(string filePath, string key, Vector2 frameSize,
            int startFrame = 0, int endFrame = -1,
            Vector2 offset = default, bool flipH = false, bool flipV = false,
            bool loop = true, int delay = 6)
        {
            // Load the entire image
            Texture2D spriteSheet = Globals.content.Load<Texture2D>(filePath);

            // Calculate the frames per row
            int frameWidth = (int)frameSize.X;
            int frameHeight = (int)frameSize.Y;
            int framesPerRow = spriteSheet.Width / frameWidth;

            // Set the end frame to the last frame if none is given
            if (endFrame == -1)
                endFrame = framesPerRow - 1;

            // Slice the sprite sheet using start and end frame
            List<Texture2D> subTextures = new List<Texture2D>();
            int x, y;
            for (int i = startFrame; i < endFrame; i++)
            {
                // Calculate the x and y index values
                x = i % framesPerRow;
                y = i / framesPerRow;
                subTextures.Add(GetSubTexture(spriteSheet, x, y, frameWidth, frameHeight));
            }

            Sprite sprite = new Sprite(subTextures, offset, flipH, flipV, loop, delay);
            AddSprite(key, sprite);
        }


        // Todo check which methods are still needed

        public SpriteComponent(Sprite sprite, string key = "idle")
        {
            SpriteDict = new Dictionary<string, Sprite>();
            AddSprite(key, sprite);
            lastState = key;
        }

        // Change x, y to row, column to match AddSprite methods below?
        public SpriteComponent(SpriteSheet spriteSheet, int x, int y, string key = "idle")
        {
            SpriteDict = new Dictionary<string, Sprite>();
            Sprite sprite = new Sprite(spriteSheet.GetSubTexture(x, y));
            AddSprite(key, sprite);
            lastState = key;
        }

        //public SpriteComponent(string filePath, int width, int height,
        //    int rowIndex, int startColumn, int endColumn,
        //    string key = "idle", bool visible = true)
        //{
        //    SpriteSheet spriteSheet = new SpriteSheet(filePath, width, height);
        //    SpriteDict = new Dictionary<string, Sprite>();
        //    AddSprite(key, spriteSheet, rowIndex, startColumn, endColumn);
        //    this.visible = visible;
        //    lastState = "idle";
        //}

        //public SpriteComponent(SpriteSheet spriteSheet, List<List<int>> subTextureValues,
        //    string key = "idle", bool visible = true)
        //{
        //    SpriteDict = new Dictionary<string, Sprite>();
        //    AddSprite(key, spriteSheet, subTextureValues);
        //    this.visible = visible;
        //    lastState = "idle";
        //}

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

        // Todo swap parameter order?
        public void AddSprite(string key, Sprite sprite)
        {
            SpriteDict[key] = sprite;
        }

        // Indices start from 0. Use neutral to repeat a texture at the end of the loop
        public void AddSprite(string key, SpriteSheet spriteSheet, int rowIndex,
            int startColumn, int endColumn)
        {
            List<Texture2D> subTextures = new List<Texture2D>();

            for (int i = startColumn; i <= endColumn; i++)
                subTextures.Add(spriteSheet.GetSubTexture(i, rowIndex));

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

            // Todo - sprite size calculated in the method above based on the full size

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

    }
}
