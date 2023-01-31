using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame.Engine
{
    public class Sprite
    {

        public List<Texture2D> textureList;
        //public Vector2 size;
        public bool loop;
        public int currentPosition;
        public int animationDelay;
        public int timer;
        public Action<Entity> OnComplete;
        public bool completed = false;

        public Sprite(Texture2D texture/*, Vector2 size = default*/)
        {
            this.textureList = new List<Texture2D> { texture };

            //if (size != default)
            //    this.size = size;
            //else
            //    this.size = new Vector2(texture.Width, texture.Height);

            this.loop = false;
            this.animationDelay = 0;
            Reset();
        }

        public Sprite(List<Texture2D> textureList, bool loop = true, int animationDelay = 10, Action<Entity> a = null)
        {
            this.textureList = textureList;

            //if (size != default)
            //    this.size = size;
            //else
            //    this.size = new Vector2(textureList[0].Width, textureList[0].Height);

            this.loop = loop;
            this.animationDelay = animationDelay;
            OnComplete = a;
            Reset();
        }
        public Texture2D GetCurrentTexture()
        {
            return textureList[currentPosition];
        }
        public void Reset()
        {
            this.currentPosition = 0;
            this.timer = 0;
            this.completed = false;
        }

    }

}
