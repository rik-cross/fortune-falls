using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    class ColliderComponent : Component
    {
        public Rectangle boundingBox;

        //public int xOffset;
        //public int yOffset;

        //public int width;
        //public int height;

        public Vector2 offset;
        public Vector2 size;
        public Rectangle rect;

        // CHANGE to dictionaries with the direction as the value (both?)
        public HashSet<Entity> collidedEntities;
        public HashSet<Entity> collidedEntitiesEnded;
        //public HashSet<Entity> resolvedCollisions;
        //public int collidedEntityId = -1;

        public bool isSolid;
        //public string collidingDirection; // REMOVE?

        public bool isActive; // REMOVE?

        public Color color = Color.Yellow; // Testing: rectangle outline

        public ColliderComponent(Vector2 offset, Vector2 size, bool active = true, bool isSolid = true)
        {
            collidedEntities = new HashSet<Entity>();
            collidedEntitiesEnded = new HashSet<Entity>();
            //resolvedCollisions = new HashSet<Entity>();

            this.isActive = active;

            this.isSolid = isSolid;

            //this.xOffset = x;
            //this.yOffset = y;
            //this.width = w;
            //this.height = h;
            this.offset = offset;
            this.size = size;

        }

    }

}
