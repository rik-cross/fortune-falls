using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

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

        // Reduced the transparency of any building entities overlapping a player entity
        public static void SetBuildingAlpha(List<Entity> entityList)
        {
            foreach (Entity e in entityList)
            {
                // only reduce alpha for entities overlapping players
                if (e.IsPlayerType())
                {
                    foreach (Entity o in entityList)
                    {
                        // only reduce the alpha of buildings
                        if (e != o && o.Tags.HasType("building"))
                        {
                            // ensure required components are present
                            if (e.GetComponent<TransformComponent>() != null && o.GetComponent<TransformComponent>() != null 
                                && e.GetComponent<SpriteComponent>() != null && o.GetComponent<SpriteComponent>() != null
                                && e.GetComponent<ColliderComponent>() != null && o.GetComponent<ColliderComponent>() != null)
                            {
                                SpriteComponent sco = o.GetComponent<SpriteComponent>();
                                TransformComponent tce = e.GetComponent<TransformComponent>();
                                TransformComponent tco = o.GetComponent<TransformComponent>();

                                ColliderComponent cce = e.GetComponent<ColliderComponent>();
                                ColliderComponent cco = o.GetComponent<ColliderComponent>();

                                // reduce alpha if there's an overlap
                                if (tce.GetRectangle().Intersects(tco.GetRectangle()) &&
                                    tce.position.Y + cce.Offset.Y + cce.Size.Y - 5 < tco.position.Y + cco.Offset.Y )
                                {
                                    sco.alpha = 0.5f;
                                }
                                else
                                {
                                    sco.alpha = 1.0f;
                                }
                            }
                        }
                    }
                }
            }
        }

    }
}
