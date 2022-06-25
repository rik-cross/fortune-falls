using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame.Engine
{
    public class SpriteSheet
    {

        public Texture2D texture;
        public Vector2 spriteSize;

        public SpriteSheet(Texture2D texture, Vector2 spriteSize)
        {
            this.texture = texture;
            this.spriteSize = spriteSize;
        }

    }

}
