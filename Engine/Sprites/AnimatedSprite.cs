using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace AdventureGame.Engine
{
    public class AnimatedSprite
    {
        //public List<Texture2D> textureList = new List<Texture2D>();
        public List<Sprite> SpriteList { get; private set; }

        public Entity ChildEntity { get; set; }

        //public string defaultDirection; // left/right to automate flipH 

        public Vector2 Size { get; private set; }
        public Vector2 Offset { get; private set; }
        public bool FlipH { get; private set; }
        public bool FlipV { get; private set; }

        public bool Play { get; set; }
        public bool Loop { get; private set; }
        public int CurrentFrame { get; set; }
        public int AnimationDelay { get; private set; }
        public int Timer { get; set; }
        public bool Completed { get; set; }
        public Action<Entity> OnComplete { get; set; }

        public AnimatedSprite(Sprite sprite, Vector2 size = default,
            Vector2 offset = default, bool flipH = false, bool flipV = false,
            bool play = true, bool loop = true, int delay = 6,
            Action<Entity> onComplete = null)
        {
            SpriteList = new List<Sprite>() { sprite };
            //SpriteList.Add(sprite);

            if (size != default)
                this.Size = size;
            else
                this.Size = sprite.Size;

            if (offset == default)
                this.Offset = new Vector2(0, 0);
            else
                this.Offset = offset;

            this.FlipH = flipH;
            this.FlipV = flipV;

            this.Play = play;
            this.Loop = loop;
            AnimationDelay = delay;
            OnComplete = onComplete;

            Reset();
        }

        public AnimatedSprite(List<Sprite> spriteList, Vector2 offset = default,
            bool flipH = false, bool flipV = false,
            bool play = true, bool loop = true, int delay = 6,
            Action<Entity> onComplete = null)
        {
            this.SpriteList = spriteList;
            Size = spriteList[0].Size;

            if (offset == default)
                this.Offset = new Vector2(0, 0);
            else
                this.Offset = offset;

            this.FlipH = flipH;
            this.FlipV = flipV;

            this.Play = play;
            this.Loop = loop;
            AnimationDelay = delay;
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
            CurrentFrame = 0;
            Timer = 0;
            Completed = false;
        }
    }
}