using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    class ColliderComponent : Component
    {
        public Rectangle BoundingBox; // is this and rect both needed?
        public Vector2 Size { get; set; }
        public Vector2 Offset { get; set; }
        public Rectangle Rect { get; set; }

        // CHANGE to dictionaries with the direction as the value (both?)
        public HashSet<Entity> CollidedEntities { get; set; }
        public HashSet<Entity> CollidedEntitiesEnded { get; set; }
        public bool IsSolid { get; set; }
        //public bool IsActive { get; set; } // Remove?

        public Color color = Color.Yellow; // Testing: rectangle outline

        public ColliderComponent(Vector2 size, Vector2 offset = default,
            bool isActive = true, bool isSolid = true)
        {
            Size = size;
            Offset = offset;
            //IsActive = isActive;
            IsSolid = isSolid;
            CollidedEntities = new HashSet<Entity>();
            CollidedEntitiesEnded = new HashSet<Entity>();
        }

        public ColliderComponent(int x, int y,
            int offsetX = default, int offsetY = default,
            bool isActive = true, bool isSolid = true)
        {
            Size = new Vector2(x, y);
            Offset = new Vector2(offsetX, offsetY);
            //IsActive = isActive;
            IsSolid = isSolid;
        }

    }

}
