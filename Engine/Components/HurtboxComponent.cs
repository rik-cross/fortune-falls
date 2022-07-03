using Microsoft.Xna.Framework;

namespace AdventureGame.Engine
{
    class HurtboxComponent : Component
    {
        public Rectangle rectangle;
        public int xOffset;
        public int yOffset;
        public int width;
        public int height;

        public bool active;
        public Color color = Color.Red; // TESTING rectangle outline

        public HurtboxComponent(int x, int y, int w, int h,
            bool active = true)
        {
            this.xOffset = x;
            this.yOffset = y;
            this.width = w;
            this.height = h;

            this.active = active;
        }
    }

}
