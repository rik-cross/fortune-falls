using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame.Engine
{
    public class Sprite
    {

        public SpriteSheet spriteSheet;
        public List<Vector2> positions;
        public int currentPosition;
        public bool loop;
        public double animationDelay;
        public double timer;

        public Sprite(SpriteSheet spriteSheet, List<Vector2> positions, double animationDelay = 10, bool loop = true)
        {

            this.spriteSheet = spriteSheet;
            this.positions = positions;
            this.animationDelay = animationDelay;
            this.loop = loop;

        }

    }

}
