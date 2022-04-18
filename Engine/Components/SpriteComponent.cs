using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame.Engine
{
    public class SpriteComponent : Component
    {
        public Texture2D sprite;
        public SpriteComponent()
        {

        }
        public SpriteComponent(Texture2D sprite)
        {
            this.sprite = sprite;
        }
    }

}
