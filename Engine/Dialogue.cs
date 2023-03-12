using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace AdventureGame.Engine
{
    public class Dialogue
    {
        public string text;
        public Entity entity;
        public Texture2D texture;
        public int index = 0;
        public int timer = 0;
        public int tickDelay;
        public bool playTickSoundEffect;
        public SoundEffect tickSoundEffect = Globals.content.Load<SoundEffect>("Sounds/blip");
        public DoubleAnimation alpha = new DoubleAnimation(0, 0.02f);
        public bool markForRemoval = false;
        public Dialogue(string text = null, Entity entity = null, Texture2D texture = null,
                        int tickDelay = 5, bool playTickSoundEffect = true,
                        SoundEffect tickSoundEffect = default)
        {
            this.text = text + " >";
            this.entity = entity;
            this.texture = texture;
            this.tickDelay = tickDelay;
            this.playTickSoundEffect = playTickSoundEffect;
            if (tickSoundEffect != default)
                this.tickSoundEffect = tickSoundEffect;
        }
        public void Update()
        {
            alpha.Update();
            // Don't display text until the dialogue is fully visible
            if (alpha.Value < 1)
                return;

            timer++;
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
