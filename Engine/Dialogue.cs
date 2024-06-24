using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System;

namespace AdventureGame.Engine
{
    public class Dialogue
    {
        public int frame = 0;
        public string text;
        public Entity entity;
        public Texture2D texture;
        public int index = 0;
        public int timer = 0;
        public int tickDelay;
        public bool playTickSoundEffect;
        public SoundEffect tickSoundEffect = Utils.LoadSoundEffect("Sounds/click.wav");
        public DoubleAnimation imageAlpha = new DoubleAnimation(0, 0.02f);
        public DoubleAnimation textAlpha = new DoubleAnimation(0, 0.02f);
        public bool markForRemoval = false;
        //public Action<Entity, Entity, float> onDialogueComplete;
        public Action<Entity> onDialogueStart;
        public Action<Entity> onDialogueComplete;

        public Dialogue(string text = null, Entity entity = null, Texture2D texture = null,
                        int tickDelay = 3, bool playTickSoundEffect = true,
                        SoundEffect tickSoundEffect = default,
                        Action<Entity> onDialogueStart = null,
                        Action<Entity> onDialogueComplete = null)
        {
            this.text = text + " >";
            this.entity = entity;
            this.texture = texture;
            this.tickDelay = tickDelay;
            this.playTickSoundEffect = playTickSoundEffect;
            if (tickSoundEffect != default)
                this.tickSoundEffect = tickSoundEffect;
            this.onDialogueStart = onDialogueStart;
            this.onDialogueComplete = onDialogueComplete;
        }
        public void Update()
        {
            imageAlpha.Update();
            textAlpha.Update();

            // Don't display text until the dialogue is fully visible
            if (textAlpha.Value < 1)
                return;

            timer++;
            frame++;
            

            if (timer == tickDelay)
            {
                if (index < text.Length)
                {
                    timer = 0;
                    index +=1;
                    if (playTickSoundEffect)
                        EngineGlobals.soundManager.PlaySoundEffect(tickSoundEffect);
                }
            }
        }
    }
}
