using Microsoft.Xna.Framework;

namespace AdventureGame.Engine
{
    class TransformComponent : Component
    {
        public Vector2 Position;
        public Vector2 PreviousPosition;
        public Vector2 Size;

        // todo: change to DrawOrder to DrawY / DrawYOffset

        // Properties for draw order
        public int DrawOrder { get; private set; }
        public int DrawOrderOffset { get; private set; }
        public bool HasDrawOrderChanged { get; set; }

        // Properties to get and set the size and position
        public float Width
        {
            get { return Size.X; }
            set { Size.X = value; }
        }
        public float Height
        {
            get { return Size.Y; }
            set { Size.Y = value; }
        }
        public float X
        {
            get { return Position.X; }
            set { Position.X = value; }
        }
        public float Y
        {
            get { return Position.Y; }
            set { Position.Y = value; }
        }
        public float Top
        {
            get { return Position.Y; }
            set { Position.Y = value; }
        }
        public float Middle
        {
            get { return Position.Y + (Size.Y / 2); }
            set { Position.Y = value - (Size.Y / 2); }
        }
        public float Bottom
        {
            get { return Position.Y + Size.Y; }
            set { Position.Y = value - Size.Y; }
        }
        public float Left
        {
            get { return Position.X; }
            set { Position.X = value; }
        }
        public float Center
        {
            get { return Position.X + (Size.X / 2); }
            set { Position.X = value - (Size.X / 2); }
        }
        public float Right
        {
            get { return Position.X + Size.X; }
            set { Position.X = value - Size.X; }
        }

        public TransformComponent(Vector2 position, Vector2 size)
        {
            Position = position;
            PreviousPosition = position;
            Size = size;
            InitDrawOrder();
        }

        public TransformComponent(int x, int y, int w, int h)
        {
            Position = new Vector2(x, y);
            PreviousPosition = Position;
            this.Size = new Vector2(w, h);
            InitDrawOrder();
        }

        public TransformComponent(Rectangle rect)
        {
            Position = new Vector2(rect.X, rect.Y);
            PreviousPosition = Position;
            Size = new Vector2(rect.Width, rect.Height);
            InitDrawOrder();
        }

        public void InitDrawOrder()
        {
            DrawOrder = (int)Bottom;
            DrawOrderOffset = 0;
            HasDrawOrderChanged = true;
        }

        public void UpdateDrawOrder()
        {
            DrawOrder = (int)Bottom + DrawOrderOffset;
        }

        public void SetDrawOrderOffset(int drawOrderOffset)
        {
            DrawOrderOffset = drawOrderOffset;
            UpdateDrawOrder();
        }

        public Vector2 GetCenter()
        {
            return new Vector2(
                Position.X + (Size.X / 2),
                Position.Y + (Size.Y / 2)
            );
        }

        public Rectangle GetRectangle()
        {
            return new Rectangle(
                (int)Position.X,
                (int)Position.Y,
                (int)Size.X,
                (int)Size.Y
            );
        }

        public bool HasMoved()
        {
            return Position != PreviousPosition;
        }

        public bool HasMovedX()
        {
            return Position.X != PreviousPosition.X;
        }

        public bool HasMovedY()
        {
            return Position.Y != PreviousPosition.Y;
        }

        public Vector2 DistanceMoved()
        {
            return Position - PreviousPosition;
        }

        public void ToPrevious()
        {
            Position = PreviousPosition;
        }

        public void ToPreviousX()
        {
            Position = new Vector2(PreviousPosition.X, Position.Y);
        }

        public void ToPreviousY()
        {
            Position = new Vector2(Position.X, PreviousPosition.Y);
        }
    }

}
