using Microsoft.Xna.Framework;

namespace AdventureGame.Engine
{
    class HitboxComponent : Component
    {
        public Rectangle rectangle;
        public int xOffset;
        public int yOffset;

        public int lifetime; // here or in a timer / lifetime system / component?
        public bool active;
        public Color color = Color.Blue; // TESTING rectangle outline

        public HitboxComponent(int x, int y, int w, int h,
            int xOffset = 0, int yOffset = 0,
            int lifetime = 0, bool active = true)
        {
            this.xOffset = xOffset;
            this.yOffset = yOffset;
            this.rectangle = new Rectangle(x + xOffset, y + yOffset, w, h);

            this.lifetime = lifetime;
            this.active = active;
        }
    }

}
