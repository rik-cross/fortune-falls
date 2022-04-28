using Microsoft.Xna.Framework;

namespace AdventureGame.Engine
{
    class HurtboxComponent : Component
    {
        public Rectangle rectangle;
        public int xOffset;
        public int yOffset;

        public bool active;
        public Color color = Color.Red; // TESTING rectangle outline

        public HurtboxComponent(int x, int y, int w, int h,
            int xOffset = 0, int yOffset = 0,
            bool active = true)
        {
            this.xOffset = xOffset;
            this.yOffset = yOffset;
            rectangle = new Rectangle(x + xOffset, y + yOffset, w, h);

            this.active = active;
        }
    }

}
