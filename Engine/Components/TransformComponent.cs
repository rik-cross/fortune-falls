using Microsoft.Xna.Framework;

namespace AdventureGame.Engine
{
    class TransformComponent : Component
    {
        public Vector2 position;
        public Vector2 previousPosition;
        public Vector2 size;

        // Todo
        // Make above properites get & set
        // Add property for Z offset
        // Calculate Z when setting Y using Y and Z-Offset
        // Remove all layerDepth from Sprite?
        // Groups for static and non-static entities? No...
        // Check whether Z should be Y or Bottom

        // Properties to get and set the size and position
        public float Width
        {
            get { return size.X; }
            set { size.X = value; }
        }
        public float Height
        {
            get { return size.Y; }
            set { size.Y = value; }
        }
        public float X
        {
            get { return position.X; }
            set { position.X = value; }
        }
        public float Y
        {
            get { return position.Y; }
            set { position.Y = value; }
        }
        public float Top
        {
            get { return position.Y; }
            set { position.Y = value; }
        }
        public float Middle
        {
            get { return position.Y + (size.Y / 2); }
            set { position.Y = value - (size.Y / 2); }
        }
        public float Bottom
        {
            get { return position.Y + size.Y; }
            set { position.Y = value - size.Y; }
        }
        public float Left
        {
            get { return position.X; }
            set { position.X = value; }
        }
        public float Center
        {
            get { return position.X + (size.X / 2); }
            set { position.X = value - (size.X / 2); }
        }
        public float Right
        {
            get { return position.X + size.X; }
            set { position.X = value - size.X; }
        }

        public TransformComponent()
        {
            this.position = new Vector2(0, 0); // change to Vector2.Zero
            this.previousPosition = position;
            this.size = new Vector2(0, 0);
        }

        public TransformComponent(Vector2 position)
        {
            this.position = position;
            this.previousPosition = position;
            this.size = new Vector2(0, 0);
        }

        public TransformComponent(Vector2 position, Vector2 size)
        {
            this.position = position;
            this.previousPosition = position;
            this.size = size;
        }

        public TransformComponent(Rectangle rect)
        {
            position = new Vector2(rect.X, rect.Y);
            previousPosition = position;
            size = new Vector2(rect.Width, rect.Y);
        }

        public TransformComponent(int x, int y, int w = 0, int h = 0)
        {
            this.position = new Vector2(x, y);
            this.previousPosition = position;
            this.size = new Vector2(w, h);
        }

        public Vector2 GetCenter()
        {
            return new Vector2(
                position.X + (size.X / 2),
                position.Y + (size.Y / 2)
            );
        }

        public Rectangle GetRectangle()
        {
            return new Rectangle(
                (int)position.X,
                (int)position.Y,
                (int)size.X,
                (int)size.Y
            );
        }

        public bool HasMoved()
        {
            return position != previousPosition;
        }

        public bool HasMovedX()
        {
            return position.X != previousPosition.X;
        }

        public bool HasMovedY()
        {
            return position.Y != previousPosition.Y;
        }

        public Vector2 DistanceMoved()
        {
            return position - previousPosition;
        }
    }

}
