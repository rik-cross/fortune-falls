using Microsoft.Xna.Framework;

namespace AdventureGame.Engine
{
    class HitboxComponent : Component
    {
        public Vector2 Size { get; set; }
        public Vector2 Offset { get; set; }
        public Rectangle Rect { get; set; }
        public int Lifetime { get; private set; } // here or in a timer / lifetime system / component?
        public bool IsActive { get; set; }
        public Color BorderColor = Color.Blue; // TESTING rectangle outline

        public HitboxComponent(Vector2 size, Vector2 offset = default,
            int lifetime = 0, bool isActive = true)
        {
            Size = size;
            Offset = offset;
            Lifetime = lifetime;
            IsActive = isActive;
        }
    }

}
