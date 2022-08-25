using Microsoft.Xna.Framework;

namespace AdventureGame.Engine
{
    class HurtboxComponent : Component
    {
        public Vector2 size;
        public Vector2 offset;
        public Rectangle rect;

        public bool active;
        public Color color = Color.Red;

        public HurtboxComponent(Vector2 size, Vector2 offset = default,
            bool active = true)
        {
            this.size = size;
            this.offset = offset;

            this.active = active;
        }
    }

}
