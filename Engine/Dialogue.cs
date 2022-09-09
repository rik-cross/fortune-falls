using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace AdventureGame.Engine
{
    public class Dialogue
    {
        public List<string> text = new List<string>();
        public Entity entity;
        public Dialogue(string text, Entity entity = null)
        {
            this.text.Add(text);
            this.entity = entity;
        }
        public void DrawImage(int x, int y)
        {

        }
        public void DrawText(int x, int y)
        {

        }
    }
}
