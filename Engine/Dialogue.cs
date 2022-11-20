using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame.Engine
{
    public class Dialogue
    {
        public string text;
        public Entity entity;
        public Texture2D texture;
        public Dialogue(string text = null, Entity entity = null, Texture2D texture = null)
        {
            this.text = text;
            this.entity = entity;
            this.texture = texture;
        }
    }
}
