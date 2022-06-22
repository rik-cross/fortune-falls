using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    class ColliderComponent : Component
    {
        public Rectangle boundingBox;
        public int xOffset;
        public int yOffset;

        // CHANGE to dictionaries with the direction as the value (both?)
        public HashSet<Entity> collidedEntities;
        public HashSet<Entity> collidedEntitiesEnded;
        //public HashSet<Entity> resolvedCollisions;
        //public int collidedEntityId = -1;

        public bool isSolid;
        //public string collidingDirection; // REMOVE?

        public bool active; // REMOVE?

        public Color color = Color.Yellow; // Testing: rectangle outline

        public ColliderComponent(int x, int y, int w, int h,
            int xOffset = 0, int yOffset = 0,
            bool active = true, bool isSolid = true)
        {
            collidedEntities = new HashSet<Entity>();
            collidedEntitiesEnded = new HashSet<Entity>();
            //resolvedCollisions = new HashSet<Entity>();

            this.active = active;

            this.isSolid = isSolid;

            this.xOffset = xOffset;
            this.yOffset = yOffset;
            this.boundingBox = new Rectangle(x + xOffset, y + yOffset, w, h);
        }

    }

}
