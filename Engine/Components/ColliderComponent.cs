using Microsoft.Xna.Framework;

namespace AdventureGame.Engine
{
    class ColliderComponent : Component
    {
        public Rectangle rectangle;
        public int xOffset;
        public int yOffset;
        public int collidedEntityID = -1;
        public Color color = Color.Yellow; // TESTING rectangle outline

        public ColliderComponent(int x, int y, int w, int h,
            int xOffset = 0, int yOffset = 0)
        {
            this.xOffset = xOffset;
            this.yOffset = yOffset;
            this.rectangle = new Rectangle(x + xOffset, y + yOffset, w, h);
        }

    }

}
