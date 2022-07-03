using Microsoft.Xna.Framework;

namespace AdventureGame.Engine
{
    class HitboxComponent : Component
    {
        public int xOffset;
        public int yOffset;
        public int width;
        public int height;
        public Rectangle rect;

        public int lifetime; // here or in a timer / lifetime system / component?
        public bool active;
        public Color color = Color.Blue; // TESTING rectangle outline

        public HitboxComponent(int x, int y, int w, int h,
            int lifetime = 0, bool active = true)
        {
            this.xOffset = x;
            this.yOffset = y;
            this.width = w;
            this.height = h;

            this.lifetime = lifetime;
            this.active = active;
        }
    }

}
