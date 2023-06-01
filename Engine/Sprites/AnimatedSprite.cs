using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace AdventureGame.Engine
{
    public class AnimatedSprite
    {
        //public List<Texture2D> textureList = new List<Texture2D>();
        public List<Sprite> spriteList = new List<Sprite>();

        public Entity ChildEntity { get; set; }

        //public string defaultDirection; // left/right to automate flipH 

        public Vector2 size;  // frameSize / textureSize?
        public Vector2 offset;
        public bool flipH;
        public bool flipV;

        public bool play;
        public bool loop;
        public int currentPosition; // change to currentFrame
        public int animationDelay;
        public int timer;
        public bool completed;
        public Action<Entity> OnComplete;

        public AnimatedSprite(Sprite sprite, Vector2 size = default,
            Vector2 offset = default, bool flipH = false, bool flipV = false,
            bool play = true, bool loop = true, int delay = 6,
            Action<Entity> onComplete = null)
        {
            spriteList.Add(sprite);

            if (size != default)
                this.size = size;
            else
                this.size = sprite.size;

            if (offset == default)
                this.offset = new Vector2(0, 0);
            else
                this.offset = offset;

            this.flipH = flipH;
            this.flipV = flipV;

            this.play = play;
            this.loop = loop;
            animationDelay = delay;
            OnComplete = onComplete;

            Reset();
        }

        public AnimatedSprite(List<Sprite> spriteList, Vector2 offset = default,
            bool flipH = false, bool flipV = false,
            bool play = true, bool loop = true, int delay = 6,
            Action<Entity> onComplete = null)
        {
            this.spriteList = spriteList;
            size = spriteList[0].size;

            if (offset == default)
                this.offset = new Vector2(0, 0);
            else
                this.offset = offset;

            this.flipH = flipH;
            this.flipV = flipV;

            this.play = play;
            this.loop = loop;
            animationDelay = delay;
            OnComplete = onComplete;

            Reset();
        }

        //public Sprite GetCurrentSprite()
        //{
        //    return spriteList[currentPosition];
        //}

        //public Texture2D GetCurrentTexture()
        //{
        //    return textureList[currentPosition];
        //}

        //public Texture2D GetTexture(int frame)
        //{
        //    if (frame >= 0 && frame < textureList.Count)
        //        return textureList[frame];
        //    else
        //        return null;
        //}

        public void Reset()
        {
            currentPosition = 0;
            timer = 0;
            completed = false;
        }
    }
}