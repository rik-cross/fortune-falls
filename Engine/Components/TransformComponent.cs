using Microsoft.Xna.Framework;

namespace AdventureGame.Engine
{
    class TransformComponent : Component
    {
        public Vector2 position;
        public Vector2 previousPosition;
        public Vector2 size;

        public TransformComponent()
        {
            this.position = new Vector2(0, 0);
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
    }

}
