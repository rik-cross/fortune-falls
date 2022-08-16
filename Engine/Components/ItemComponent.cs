using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame.Engine
{

    public class ItemComponent : Component
    {
        public Texture2D texture;
        public Vector2 offset;
        public Vector2 size;
        public Rectangle rect;

        public bool isSolid;
        public bool isActive;

        //public ItemComponent(Vector2 offset, Vector2 size, bool isActive = true, bool isSolid = true)
        public ItemComponent(Vector2 offset, Vector2 size, Texture2D texture, bool isActive = true, bool isSolid = true)
        {
            this.texture = texture;
            this.offset = offset;
            this.size = size;

            this.isActive = isActive;
            this.isSolid = isSolid;
        }
    }

}
