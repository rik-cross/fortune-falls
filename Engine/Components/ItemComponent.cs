using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace AdventureGame.Engine
{

    public class ItemComponent : Component
    {

        public Vector2 offset;
        public Vector2 size;
        public Rectangle rect;

        public bool isSolid;
        public bool isActive;

        public ItemComponent(Vector2 offset, Vector2 size, bool isActive = true, bool isSolid = true)
        {
            this.offset = offset;
            this.size = size;

            this.isActive = isActive;
            this.isSolid = isSolid;
        }
    }

}
