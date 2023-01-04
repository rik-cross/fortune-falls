using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    class ColliderComponent : Component
    {
        public Rectangle Box;
        public Rectangle Sweep;
        public Vector2 Size { get; set; }
        public Vector2 Offset { get; set; }
        public bool IsSolid { get; set; }
        //public bool IsActive { get; set; } // Remove?
        public Color color = Color.Yellow; // Testing: rectangle outline

        public ColliderComponent(Vector2 size, Vector2 offset = default, bool isSolid = true)
            //bool isActive = true)
        {
            Size = size;
            Offset = offset;
            IsSolid = isSolid;
            //IsActive = isActive;
        }

        public ColliderComponent(int width, int height, int offsetX = 0, int offsetY = 0,
            bool isSolid = true)//, bool isActive = true)
        {
            Size = new Vector2(width, height);
            Offset = new Vector2(offsetX, offsetY);
            IsSolid = isSolid;
            //IsActive = isActive;
        }

        // Create and return the bounding box based on the X and Y position
        public Rectangle CreateBoundingBox(int positionX, int positionY)
        {
            Box = new Rectangle(
                positionX + (int)Offset.X,
                positionY + (int)Offset.Y,
                (int)Size.X,
                (int)Size.Y
            );

            return Box;
        }

        // Create and return the bounding box based on the X and Y position
        public Rectangle CreateBoundingBox(Vector2 position)
        {
            Box = CreateBoundingBox((int)position.X, (int)position.Y);

            return Box;
        }

    }

}
