using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame.Engine
{
    /// <summary>
    /// Contains a dictionary of {State -> AnimatedSprite} pairings,
    /// along with the visibility and opacity of the entity.
    /// </summary>
    public class AnimatedSpriteComponent : Component
    {
        public Dictionary<string, AnimatedSprite> AnimatedSprites { get; private set; }
        //public bool visible { get; set; }
        public float alpha = 1.0f;
        public string lastState;

        public AnimatedSpriteComponent()
        {
            AnimatedSprites = new Dictionary<string, AnimatedSprite>();
        }

        // Add a sprite to the dictionary
        public void AddSprite(string key, Sprite sprite)
        {
            //AnimatedSprites[key] = sprite;
            AddToDictionary(key, sprite);
        }

        // Add animated sprites using frames which start at 0.
        // Width and height calculated using framesPerRow.
        // Todo change delay to speed (FPS)?
        public void AddAnimatedSprite(string filePath, string key,
            int startFrame, int endFrame, int totalRows = 1, int framesPerRow = -1,
            Vector2 offset = default, bool flipH = false, bool flipV = false,
            bool play = true, bool loop = true, int delay = 6,
            Action<Entity> onComplete = null)
        {
            // Load the sprite sheet
            Texture2D spriteSheet = Globals.content.Load<Texture2D>(filePath);

            // Assume only one row if frames per row is not given
            if (framesPerRow == -1)
                framesPerRow = (endFrame - startFrame + 1) / totalRows;

            // Calculate the width and height of a single frame
            int frameWidth = spriteSheet.Width / framesPerRow;
            int frameHeight = spriteSheet.Height / totalRows;

            // Slice the sprite sheet using start and end frame
            List<Texture2D> subTextures = new List<Texture2D>();
            int x, y;
            for (int i = startFrame; i <= endFrame; i++)
            {
                // Calculate the x and y index values
                x = i % framesPerRow;
                y = i / framesPerRow;
                subTextures.Add(GetSubTexture(spriteSheet, x, y, frameWidth, frameHeight));
            }

            Sprite sprite = new Sprite(subTextures, offset, flipH, flipV, play, loop, delay, onComplete);
            //AddSprite(key, sprite);
            AddToDictionary(key, sprite);
        }

        private void AddToDictionary(string key, Sprite sprite)
        {
            // Todo here or before?
            // SpriteLayerDepth (relative to all children AND world?)

            if (AnimatedSprites.ContainsKey(key))
                AnimatedSprites[key].SpriteList.Add(sprite);
            else
                AnimatedSprites.Add(key, new AnimatedSprite(
                    sprite, sprite.Size, sprite.Offset, sprite.FlipH, sprite.FlipV,
                    sprite.Play, sprite.Loop, sprite.AnimationDelay));
        }

        /// <summary>
        /// Gets the AnimatedSprite for a given state.
        /// </summary>
        /// <param name="state">The state associated with the AnimatedSprite to retrieve.</param>
        /// <returns>AnimatedSprite, or null if no state exists for the state provided.</returns>
        public AnimatedSprite GetAnimatedSprite(string state = "default")
        {
            //if (SpriteDict.ContainsKey(state))
            //    return SpriteDict[state];
            //else
            //    return null;

            // Testing
            return AnimatedSprites[state];
        }

        public Sprite GetSprite(int index = 0, string state = "default")
        {
            //if (SpriteDict.ContainsKey(state))
            //    return SpriteDict[state];
            //else
            //    return null;

            // Testing
            return AnimatedSprites[state].SpriteList[index];
        }

        public Vector2 GetSpriteSize(string state = "default")
        {
            //if (SpriteDict.ContainsKey(state))
            //    return SpriteDict[state].size;
            //else
            //    return new Vector2(0, 0);

            // Testing
            return AnimatedSprites[state].Size;

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

        //public void SetAnimationDelay(int delay)
        //{
        //    foreach (Sprite sprite in AnimatedSprites.Values)
        //        sprite.animationDelay = delay;
        //}

        //public void ModifyAnimationDelay(float modifier)
        //{
        //    foreach (Sprite sprite in AnimatedSprites.Values)
        //        sprite.animationDelay = (int)Math.Ceiling(sprite.animationDelay * modifier);
        //}

    }
}