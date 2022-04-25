using Microsoft.Xna.Framework;

namespace AdventureGame.Engine
{
    class HurtboxComponent : Component
    {
        public Rectangle rectangle;
        public int xOffset;
        public int yOffset;
        public Color color = Color.Red; // TESTING rectangle outline

        public HurtboxComponent(int x, int y, int w, int h,
            int xOffset = 0, int yOffset = 0)
        {
            this.xOffset = xOffset;
            this.yOffset = yOffset;
            this.rectangle = new Rectangle(x + xOffset, y + yOffset, w, h);
        }
    }

}
