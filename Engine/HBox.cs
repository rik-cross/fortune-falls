using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Engine
{
    public class HBox
    {
        public Vector2 size;
        public Vector2 offset;
        public int frame;
        //public string prevState;
        public HBox(Vector2 size, Vector2 offset = default, int frame = -1)
        {
            this.size = size;
            this.offset = offset;
            this.frame = frame;
            //this.prevState = "";
        }
    }
}
