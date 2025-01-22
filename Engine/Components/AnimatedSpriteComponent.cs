using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine
{
    /// <summary>
    /// Contains a dictionary of {State -> AnimatedSprite} pairings,
    /// along with the visibility and opacity of the entity.
    /// </summary>
    public class AnimatedSpriteComponent : Component
    {
        public Dictionary<string, AnimatedSprite> AnimatedSprites { get; set; }
        //public bool visible { get; set; }
        public bool IsVisible { get; set; }
        public float Alpha { get; set; }
        public string LastState { get; set; }

        public AnimatedSpriteComponent()
        {
            AnimatedSprites = new Dictionary<string, AnimatedSprite>();
            IsVisible = true;
            Alpha = 1.0f;
        }

        public void RemoveAllAnimatedSprites()
        {
            AnimatedSprites.Clear();
        }

        public void RemoveAnimatedSprite(string key)
        {
            AnimatedSprites.Remove(key);
        }

        // Add animated sprites using frames which start at 0.
        // Width and height calculated using framesPerRow.
        // Todo:
        //  remove delay
        //  remove startFrame and change endFrame to totalFrames
        //  add startFrame = 0
        //  add endFrame = -1, default is totalFrames - 1
        // OR
        // startFrame should set the current frame of animation
        public void AddAnimatedSprite(string filePath, string key,
            int startFrame, int endFrame, int totalRows = 1, int framesPerRow = -1,
            Vector2 offset = default, bool flipH = false, bool flipV = false,
            bool play = true, bool loop = true, int delay = 6,
            Action<Entity> onComplete = null, Color spriteHue = default,
            float frameDuration = 0.1f,
            float loopDelay = 0.0f)
        {

            if (spriteHue == default)
                spriteHue = Color.White;

            // Load the sprite sheet
            Texture2D spriteSheet = Utils.LoadTexture(filePath);

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

            Sprite sprite = new Sprite(subTextures, offset, flipH, flipV, spriteHue: spriteHue);
            //AddToDictionary(key, sprite);

            if (AnimatedSprites.ContainsKey(key))
                AnimatedSprites[key].SpriteList.Add(sprite);
            else
                AnimatedSprites.Add(key, new AnimatedSprite(
                    sprite, play, loop, delay, onComplete,
                    frameDuration, loopDelay));
        }

        // todo: delete?
        public void AddAnimatedSprite(string key, Sprite sprite,
            bool play = true, bool loop = true, int delay = 6,
            Action<Entity> onComplete = null, Color spriteHue = default)
        {

            if (spriteHue == default)
                spriteHue = Color.White;

            if (AnimatedSprites.ContainsKey(key))
                AnimatedSprites[key].SpriteList.Add(sprite);
            else
                AnimatedSprites.Add(key, new AnimatedSprite(
                    sprite, play, loop, delay, onComplete));
        }

        /// <summary>
        /// Gets the AnimatedSprite for a given state.
        /// </summary>
        /// <param name="state">The state associated with the AnimatedSprite to retrieve.</param>
        /// <returns>AnimatedSprite, or null if no state exists for the state provided.</returns>
        public AnimatedSprite GetAnimatedSprite(string state = "default")
        {
            //if (AnimatedSprites.ContainsKey(state))
            //    return AnimatedSprites[state];
            //else
            //    return null;

            // Testing
            return AnimatedSprites[state];
        }

        public Vector2 GetAnimatedSpriteSize(string state = "default")
        {
            //if (SpriteDict.ContainsKey(state))
            //    return SpriteDict[state].size;
            //else
            //    return new Vector2(0, 0);

            // Testing
            return AnimatedSprites[state].GetSize();

            // Todo Account for offset?
            //return SpriteDict[state].size + SpriteDict[state].offset;
        }

        public void SetAnimatedSpriteFrame(int frame, string state)
        {
            //if (string.IsNullOrEmpty(state))
            //    state = LastState;
            if (AnimatedSprites.ContainsKey(state))
                AnimatedSprites[state].SetFrame(frame);
        }

        public Sprite CloneSprite(Sprite sprite)
        {
            return new Sprite(sprite.TextureList, sprite.Offset, sprite.FlipH, sprite.FlipV);
        }

        public bool HasSpriteForState(string state)
        {
            if (AnimatedSprites.ContainsKey(state))
                return true;
            return false;
        }

        public Sprite GetSprite(string state = "default", int index = 0)
        {
            //if (AnimatedSprites.ContainsKey(state))
            //    return AnimatedSprites[state];
            //else
            //    return null;

            // Testing

            if (AnimatedSprites.ContainsKey(state))
                return AnimatedSprites[state].SpriteList[index];
            else
                return null;
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