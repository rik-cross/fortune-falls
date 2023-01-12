using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    class ColliderComponent : Component
    {
        public Rectangle Box; // Bounding box
        public Rectangle Broadphase; // Broad-phase box
        public Vector2 Size { get; set; }
        public Vector2 Offset { get; set; }
        public bool IsSolid { get; set; }
        //public bool IsActive { get; set; } // Remove?
        public Color color = Color.Yellow; // Testing: rectangle outline

        // Properties to get the bounding box's relative position
        public int Top
        {
            get { return Box.Y; }
        }
        public int Middle
        {
            get { return Box.Y + (int)(Size.Y / 2); }
        }
        public int Bottom
        {
            get { return Box.Y + (int)Size.Y; }
        }
        public int Left
        {
            get { return Box.X; }
        }
        public int Center
        {
            get { return Box.X + (int)(Size.X / 2); }
        }
        public int Right
        {
            get { return Box.X + (int)Size.X; }
        }

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
        public Rectangle GetBoundingBox(int positionX, int positionY)
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
        public Rectangle GetBoundingBox(Vector2 position)
        {
            Box = GetBoundingBox((int)position.X, (int)position.Y);

            return Box;
        }

        // Create and return the broad-phase box based on the velocity
        public Rectangle GetBroadphaseBox(Vector2 velocity)
        {
            int x = Box.X;
            int y = Box.Y;
            float vX = velocity.X;
            float vY = velocity.Y;
            int width = (int)Math.Ceiling(Box.Width + Math.Abs(vX) * 2);
            int height = (int)Math.Ceiling(Box.Height + Math.Abs(vY) * 2);

            if (vX < 0)
                x += (int)Math.Floor(vX * 2); // Math.Ceiling

            if (vY < 0)
                y += (int)Math.Floor(vY * 2);

            Broadphase = new Rectangle(x, y, width, height);

            return Broadphase;
        }

    }
}
