using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    class ColliderComponent : Component
    {
        public Rectangle rectangle;
        public int xOffset;
        public int yOffset;

        public List<Entity> collidedEntities; // either list or id?
        public int collidedEntityId = -1;
        public Guid collidedEntityGuid = Guid.Empty;

        public bool active;

        public Color color = Color.Yellow; // TESTING rectangle outline

        public ColliderComponent(int x, int y, int w, int h,
            int xOffset = 0, int yOffset = 0,
            bool active = true)
        {
            collidedEntities = new List<Entity>();
            this.active = active;

            this.xOffset = xOffset;
            this.yOffset = yOffset;
            this.rectangle = new Rectangle(x + xOffset, y + yOffset, w, h);
        }

    }

}
