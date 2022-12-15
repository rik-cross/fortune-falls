using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    class ColliderComponent : Component
    {
        // Change to dictionaries with the direction as the value (both?)
        public HashSet<Entity> CollidedEntities { get; set; }
        public HashSet<Entity> CollidedEntitiesEnded { get; set; }

        public Rectangle BoundingBox; // is this and rect both needed?
        public Rectangle Rect { get; set; }
        public Vector2 Size { get; set; }
        public Vector2 Offset { get; set; }
        public bool IsSolid { get; set; }
        //public bool IsActive { get; set; } // Remove?
        public Color color = Color.Yellow; // Testing: rectangle outline

        public ColliderComponent(Vector2 size, Vector2 offset = default, bool isSolid = true)
            //bool isActive = true)
        {
            CollidedEntities = new HashSet<Entity>();
            CollidedEntitiesEnded = new HashSet<Entity>();

            Size = size;
            Offset = offset;
            IsSolid = isSolid;
            //IsActive = isActive;
        }

        public ColliderComponent(int width, int height, int offsetX = 0, int offsetY = 0,
            bool isSolid = true)//, bool isActive = true)
        {
            CollidedEntities = new HashSet<Entity>();
            CollidedEntitiesEnded = new HashSet<Entity>();

            Size = new Vector2(width, height);
            Offset = new Vector2(offsetX, offsetY);
            IsSolid = isSolid;
            //IsActive = isActive;
        }

    }

}
