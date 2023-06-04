using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using S = System.Diagnostics.Debug;

namespace AdventureGame.Engine
{
    public class AnimatedSprite
    {
        public List<Sprite> SpriteList { get; private set; }
        public Entity ChildEntity { get; set; } // Todo: could be a list

        //public string DefaultDirection; // left/right to automate flipH
        public Vector2 Size { get; private set; }
        public Vector2 Offset { get; private set; }
        public bool FlipH { get; private set; }
        public bool FlipV { get; private set; }
        public bool Play { get; set; }
        public bool Loop { get; private set; }
        //public int CurrentFrame { get; set; }
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

            if (size != default)
                Size = size;
            else
                Size = sprite.Size;

            if (offset == default)
                Offset = new Vector2(0, 0);
            else
                Offset = offset;

            FlipH = flipH;
            FlipV = flipV;

            Play = play;
            Loop = loop;
            AnimationDelay = delay;
            OnComplete = onComplete;

            Reset();
        }

        public AnimatedSprite(List<Sprite> spriteList, Vector2 offset = default,
            bool flipH = false, bool flipV = false,
            bool play = true, bool loop = true, int delay = 6,
            Action<Entity> onComplete = null)
        {
            SpriteList = spriteList;
            Size = spriteList[0].Size;

            if (offset == default)
                Offset = new Vector2(0, 0);
            else
                Offset = offset;

            FlipH = flipH;
            FlipV = flipV;

            Play = play;
            Loop = loop;
            AnimationDelay = delay;
            OnComplete = onComplete;

            Reset();
        }

        public void Reset()
        {
            Timer = 0;
            Completed = false;

            foreach (Sprite sprite in SpriteList)
                sprite.CurrentFrame = 0;
        }

        public void NextFrame()
        {
            foreach (Sprite sprite in SpriteList)
                sprite.CurrentFrame += 1;
        }

        public void SetFrame(int frame)
        {
            foreach (Sprite sprite in SpriteList)
                sprite.CurrentFrame = frame;
        }
    }
}