using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    class ColliderComponent : Component
    {
        public Rectangle boundingBox;

        public Vector2 size;
        public Vector2 offset;
        public Rectangle rect;

        // CHANGE to dictionaries with the direction as the value (both?)
        public HashSet<Entity> collidedEntities;
        public HashSet<Entity> collidedEntitiesEnded;
        //public HashSet<Entity> resolvedCollisions;
        //public int collidedEntityId = -1;
        //public string collidingDirection; // REMOVE?
        //public string / dict{otherEntityId, "direction"} previousCollisionDirection

        public bool isSolid;
        public bool isActive; // REMOVE?

        public Color color = Color.Yellow; // Testing: rectangle outline

        public ColliderComponent(Vector2 size, Vector2 offset = default,
            bool isActive = true, bool isSolid = true)
        {
            this.size = size;
            this.offset = offset;
            this.isActive = isActive;
            this.isSolid = isSolid;
            collidedEntities = new HashSet<Entity>();
            collidedEntitiesEnded = new HashSet<Entity>();
        }

        public ColliderComponent(int x, int y,
            int offsetX = default, int offsetY = default,
            bool isActive = true, bool isSolid = true)
        {
            size = new Vector2(x, y);
            offset = new Vector2(offsetX, offsetY);
            this.isActive = isActive;
            this.isSolid = isSolid;
        }

    }

}
