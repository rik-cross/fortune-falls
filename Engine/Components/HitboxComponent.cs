using Microsoft.Xna.Framework;

namespace AdventureGame.Engine
{
    class HitboxComponent : Component
    {
        public Vector2 size;
        public Vector2 offset;
        public Rectangle rect;

        public int lifetime; // here or in a timer / lifetime system / component?
        public bool active;
        public Color color = Color.Blue; // TESTING rectangle outline

        public HitboxComponent(Vector2 size, Vector2 offset = default,
            int lifetime = 0, bool active = true)
        {
            this.size = size;
            this.offset = offset;

            this.lifetime = lifetime;
            this.active = active;
        }
    }

}
