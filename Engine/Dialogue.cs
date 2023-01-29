using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using S = System.Diagnostics.Debug;

namespace AdventureGame.Engine
{
    public class Dialogue
    {
        public string text;
        public Entity entity;
        public Texture2D texture;
        public int index = 0;
        public int timer = 0;
        public int tickDelay = 5;
        public int initialDelay = 40;
        public DoubleAnimation alpha = new DoubleAnimation(0, 0.02f);
        public bool markForRemoval = false;
        public Dialogue(string text = null, Entity entity = null, Texture2D texture = null)
        {
            this.text = text + " >";
            this.entity = entity;
            this.texture = texture;
        }
        // todo -- 
        public void Update()
        {
            initialDelay = Math.Max(0, initialDelay-1);
            alpha.Update();

            if (initialDelay > 0)
                return;

            timer++;
            if (timer == tickDelay)
            {
                if (index < text.Length)
                {
                    timer = 0;
                    index +=1;
                    // if (component.playTickSoundEffect)
                    EngineGlobals.soundManager.PlaySoundEffect(Globals.dialogueTickSound);
                }
            }
            //S.WriteLine(index);
        }
    }
}
