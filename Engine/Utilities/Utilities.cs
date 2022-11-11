using Microsoft.Xna.Framework;
using System;

namespace AdventureGame.Engine
{
    public static class Utilities
    {
        // Return the relative center position to a parent
        // Use the screen width and height as the parent by default
        public static Vector2 CenterVectorToContainer(int width, int height, Vector2 parent = default)
        {
            if (parent == default)
                parent = new Vector2(Globals.ScreenWidth, Globals.ScreenHeight);

            return new Vector2(parent.X / 2 - width / 2, parent.Y / 2 - height / 2);
        }

        public static Rectangle ConvertCenterToTopLeft(Rectangle rectangle)
        {
            rectangle.X -= rectangle.Width / 2;
            rectangle.Y -= rectangle.Height / 2;
            return rectangle;
        }

        public static Rectangle ConvertCenterToTopLeft(int x, int y, int width, int height)
        {
            return ConvertCenterToTopLeft(new Rectangle(x, y, width, height));
        }

        public static Rectangle ConvertTopLeftToCenter(Rectangle rectangle)
        {
            rectangle.X += rectangle.Width / 2;
            rectangle.Y += rectangle.Height / 2;
            return rectangle;
        }

        public static Rectangle ConvertTopLeftToCenter(int x, int y, int width, int height)
        {
            return ConvertTopLeftToCenter(new Rectangle(x, y, width, height));
        }
    }
}
