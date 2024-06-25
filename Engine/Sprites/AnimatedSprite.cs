using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using S = System.Diagnostics.Debug;

namespace AdventureGame.Engine
{
    public class AnimatedSprite
    {
        public List<Sprite> SpriteList { get; private set; }
        //public Entity ChildEntity { get; set; } // Todo: could be a list

        //public string DefaultDirection; // left/right to automate flipH
        public bool Play { get; set; }
        public bool Loop { get; private set; }
        //public int CurrentFrame { get; set; }
        public int AnimationDelay { get; private set; }
        public int Timer { get; set; }
        public bool Completed { get; set; }
        public Action<Entity> OnComplete { get; set; }
        //public Color Hue { get; set; }

        public AnimatedSprite(Sprite sprite,
            bool play = true, bool loop = true, int delay = 6,
            Action<Entity> onComplete = null)
        {
            SpriteList = new List<Sprite>() { sprite };
            Play = play;
            Loop = loop;
            AnimationDelay = delay;
            OnComplete = onComplete;
            //Hue = hue;

            Reset();
        }

        public AnimatedSprite(List<Sprite> spriteList,
            bool play = true, bool loop = true, int delay = 6,
            Action<Entity> onComplete = null)
        {
            SpriteList = spriteList;
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

        public Vector2 GetSize(int index = 0)
        {
            if (index < SpriteList.Count && index > 0)
                return SpriteList[index].Size;
            else
                return new Vector2(0, 0);
        }
    }
}