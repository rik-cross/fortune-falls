using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using S = System.Diagnostics.Debug;

namespace Engine
{
    public class AnimatedSprite
    {
        // todo: does it need to be a List?
        public List<Sprite> SpriteList { get; private set; }
        //public Entity ChildEntity { get; set; } // Todo: could be a list

        //public string DefaultDirection; // left/right to automate flipH
        public bool Play { get; set; }
        public bool Loop { get; private set; }
        //public int CurrentFrame { get; set; }
        public int AnimationDelay { get; set; }
        public int Timer { get; set; }
        public bool Completed { get; set; } // todo: change to Ended??
        public Action<Entity> OnComplete { get; set; }
        public Action<Entity> OnLoop { get; set; } // todo: change to OnReset (invoke) / OnStart
        //public Color Hue { get; set; }

        public float TimeElapsed { get; set; } // change to Timer
        public float FrameDuration { get; set; }
        public float LoopDelay { get; set; }

        public AnimatedSprite(Sprite sprite,
            bool play = true, bool loop = true, int delay = 6,
            Action<Entity> onComplete = null,
            //Action<Entity> onLoop = null,
            float frameDuration = 0.1f,
            float loopDelay = 0.0f)
        {
            SpriteList = new List<Sprite>() { sprite };
            Play = play;
            Loop = loop;
            AnimationDelay = delay;
            OnComplete = onComplete;
            //OnLoop = onLoop;
            //Hue = hue;

            TimeElapsed = 0.0f;
            FrameDuration = frameDuration;
            LoopDelay = loopDelay;

            Reset();
        }

        // todo: delete?? not being used
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

        public void Reset() // optional parameter to set frame? (of particular sprite?)
        {
            Timer = 0;
            Completed = false; // todo: needed?

            //TimeElapsed = 0.0f;

            foreach (Sprite sprite in SpriteList)
                sprite.CurrentFrame = 0;

            // todo: invoke OnReset
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