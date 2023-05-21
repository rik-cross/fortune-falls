using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame.Engine
{
    public class Sprite
    {
        public List<Texture2D> textureList = new List<Texture2D>();
        public Vector2 size;  // frameSize / textureSize?
        public Vector2 offset;
        public bool flipH;
        public bool flipV;
        public int layerDepth;

        public bool play;
        public bool loop;
        public int currentPosition; // change to currentFrame
        public int animationDelay;
        public int timer;
        public bool completed;
        public Action<Entity> OnComplete;

        public Sprite(Texture2D texture, Vector2 size = default, Vector2 offset = default,
            bool flipH = false, bool flipV = false, int layerDepth = 0)
        {
            textureList.Add(texture);

            if (size != default)
                this.size = size;
            else
                this.size = new Vector2(texture.Width, texture.Height);

            if (offset == default)
                this.offset = new Vector2(0, 0);
            else
                this.offset = offset;

            this.flipH = flipH;
            this.flipV = flipV;
            this.layerDepth = layerDepth;

            play = false;
            loop = false;
            animationDelay = 0;
            completed = false;

            Reset();
        }

        public Sprite(List<Texture2D> textureList, Vector2 offset = default,
            bool flipH = false, bool flipV = false, int layerDepth = 0,
            bool play = true, bool loop = true, int delay = 6)
        {
            this.textureList = textureList;
            size = new Vector2(textureList[0].Width, textureList[0].Height);

            if (offset == default)
                this.offset = new Vector2(0, 0);
            else
                this.offset = offset;

            this.flipH = flipH;
            this.flipV = flipV;
            this.layerDepth = layerDepth;

            this.play = play;
            this.loop = loop;
            animationDelay = delay;
            //OnComplete = a;

            Reset();
        }

        public Texture2D GetCurrentTexture()
        {
            return textureList[currentPosition];
        }

        public Texture2D GetTexture(int frame)
        {
            if (frame >= 0 && frame < textureList.Count)
                return textureList[frame];
            else
                return null;
        }

        public void Reset()
        {
            currentPosition = 0;
            timer = 0;
            completed = false;
        }
    }
}