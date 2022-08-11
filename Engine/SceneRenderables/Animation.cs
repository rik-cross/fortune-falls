using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using S = System.Diagnostics.Debug;
namespace AdventureGame.Engine
{
    class Animation : SceneRenderable
    {

        private List<Texture2D> textureList;
        public int animationDelay;
        private int textureTimer;
        private int textureIndex;
        public Color tint;
        public bool loop;
        public bool play;
        public bool reverse;

        public Animation(List<Texture2D> textureList, Vector2 size = default, int animationDelay = 8, Color tint = default, bool loop = true, bool play = true, bool reverse = false, Vector2 position = default, Anchor anchor = Anchor.topleft, float alpha = 1.0f, bool visible = true) : base(position, anchor, alpha, visible)
        {
            this.textureList = textureList;

            if (!reverse)
                this.textureIndex = 0;
            else
                this.textureIndex = textureList.Count - 1;

            this.animationDelay = animationDelay;
            this.textureTimer = 0;

            if (size != default)
                this.size = size;
            else
            {
                this.size.X = textureList[0].Width;
                this.size.Y = textureList[0].Height;
            }

            if (tint == default(Color))
                this.tint = Color.White;
            else
                this.tint = tint;

            this.loop = loop;
            this.play = play;
            this.reverse = reverse;

            CalculateAnchors();
        }

        public void Play() { play = true; }
        public void Pause() { play = false; }
        public void Stop()
        {
            play = false;
            Reset();
        }


        public void Reset()
        {
            textureTimer = 0;
            if (!reverse)
                textureIndex = 0;
            else
                textureIndex = textureList.Count - 1;
        }

        public override void Update()
        {

            if (!play)
                return;

            textureTimer += 1;

            if (!reverse)
            {
                if (textureIndex == textureList.Count - 1 && !loop)
                {
                    //textureTimer = 0;
                    play = false;
                }
                else
                {
                    if (textureTimer >= animationDelay)
                    {
                        textureTimer = 0;
                        textureIndex += 1;
                        if (textureIndex > textureList.Count - 1)
                            textureIndex = 0;
                    }
                }
            }
            else
            {
                if (textureIndex == 0 && !loop)
                {
                    //textureTimer = 0;
                    play = false;
                }
                else
                {
                    if (textureTimer >= animationDelay)
                    {
                        textureTimer = 0;
                        textureIndex -= 1;
                        if (textureIndex < 0)
                            textureIndex = textureList.Count - 1;
                    }
                }
            }
             
        }

        public override void Draw()
        {
            if (!visible)
                return;

            Globals.spriteBatch.Draw(
                textureList[textureIndex],
                new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y),
                tint * alpha
            );
        }
    }
}
