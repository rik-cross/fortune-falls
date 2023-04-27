using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace AdventureGame.Engine
{
    public class HBox
    {
        public Vector2 offset;
        public Vector2 size;
        public int frame;
        //public string prevState;
        public HBox(Vector2 offset, Vector2 size, int frame = -1)
        {
            this.offset = offset;
            this.size = size;
            this.frame = frame;
            //this.prevState = "";
        }
    }
}
