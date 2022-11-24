using Microsoft.Xna.Framework;

namespace AdventureGame.Engine
{
    class HurtboxComponent : Component
    {
        public Vector2 Size { get; set; }
        public Vector2 Offset { get; set; }
        public Rectangle Rect { get; set; }
        public bool IsActive { get; set; }
        public Color BorderColor = Color.Red; // TESTING rectangle outline

        public HurtboxComponent(Vector2 size, Vector2 offset = default,
            bool isActive = true)
        {
            Size = size;
            Offset = offset;
            IsActive = isActive;
        }
    }

}
