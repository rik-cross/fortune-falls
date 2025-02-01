using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using S = System.Diagnostics.Debug;

namespace Engine
{
    public static class Utilities
    {
        /// <summary>
        /// Splits a textures into a 2D list of textures
        /// </summary>
        /// <param name="originalTexture">The texture to split</param>
        /// <param name="subTextureSize">The size of each sub-texture</param>
        /// <param name="subTextureClippingRect">Additional clipping of each sub-texture</param>
        /// <returns>A 2D list of textures</returns>
        public static List<List<Texture2D>> SplitTexture(
            Texture2D originalTexture,
            Vector2 subTextureSize,
            Rectangle subTextureClippingRect = default
        )
        {
            List<List<Texture2D>> subTextureList = new List<List<Texture2D>>();

            for (int row = 0; row < originalTexture.Height; row += (int)subTextureSize.Y)
            {
                //S.WriteLine("Row " + row);
                List<Texture2D> textureRow = new List<Texture2D>();
                for (int col = 0; col < originalTexture.Width; col += (int)subTextureSize.X)
                {
                    int x = col;
                    int y = row;
                    int w = (int)subTextureSize.X; 
                    int h = (int)subTextureSize.Y;

                    if (subTextureClippingRect != default)
                    {
                        x += subTextureClippingRect.X;
                        y += subTextureClippingRect.Y;
                        w = subTextureClippingRect.Width;
                        h = subTextureClippingRect.Height;
                    }
                    //S.WriteLine(x + " " + y + " " + w + " " + h);
                    Texture2D t = GetSubTexture(originalTexture, x, y, w, h);
                    textureRow.Add(t);
                }
                subTextureList.Add(textureRow);
            }
            return subTextureList;
        }
        public static Texture2D GetSubTexture(Texture2D texture, int x, int y, int width, int height)
        {
            // Create the new sub texture
            Rectangle rect = new Rectangle(x, y, width, height);
            Texture2D subTexture = new Texture2D(EngineGlobals.graphicsDevice, rect.Width, rect.Height);

            // Set the texture data
            Color[] data = new Color[rect.Width * rect.Height];
            texture.GetData(0, rect, data, 0, data.Length);
            subTexture.SetData(data);

            return subTexture;
        }
        public static List<Texture2D> flatten2DList(List<List<Texture2D>> inputList)
        {
            List<Texture2D> returnList = new List<Texture2D>();
            return returnList;
        }

        //public static Texture2D CropTexture(Texture2D texture, Rectangle cropArea)
        //{
        //    Texture2D newTexture = texture;

        //    if (newTexture.Bounds)

        //    return newTexture;
        //}


        // Return the relative center position to a parent
        // Use the screen width and height as the parent by default
        public static Vector2 CenterVectorToContainer(int width, int height, Vector2 parent = default)
        {
            if (parent == default)
                parent = new Vector2(EngineGlobals.ScreenWidth, EngineGlobals.ScreenHeight);

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

        // todo turn into BuildingSystem / PlayerSystem / LightingSystem
        // Reduced the transparency of any building entities overlapping a player entity
        public static void SetBuildingAlpha(List<Entity> entityList)
        {
            foreach (Entity e in entityList)
            {
                // only reduce alpha for entities overlapping players
                if (e.Name == "player")
                {
                    foreach (Entity o in entityList)
                    {
                        // only reduce the alpha of buildings
                        if (e != o && (o.Tags.HasTags("building") || o.Tags.HasTags("tree")))
                        {
                            // ensure required components are present
                            if (e.GetComponent<TransformComponent>() != null && o.GetComponent<TransformComponent>() != null 
                                && e.GetComponent<AnimatedSpriteComponent>() != null && o.GetComponent<SpriteComponent>() != null
                                && e.GetComponent<ColliderComponent>() != null && o.GetComponent<ColliderComponent>() != null)
                            {
                                SpriteComponent sco = o.GetComponent<SpriteComponent>();
                                TransformComponent tce = e.GetComponent<TransformComponent>();
                                TransformComponent tco = o.GetComponent<TransformComponent>();

                                ColliderComponent cce = e.GetComponent<ColliderComponent>();
                                ColliderComponent cco = o.GetComponent<ColliderComponent>();

                                // reduce alpha if there's an overlap
                                if (tce.GetRectangle().Intersects(tco.GetRectangle()) &&
                                    tce.Position.Y + cce.Offset.Y + cce.Size.Y - 5 < tco.Position.Y + cco.Offset.Y )
                                {
                                    sco.Alpha = 0.5f;
                                }
                                else
                                {
                                    sco.Alpha = 1.0f;
                                }
                            }
                        }
                    }
                }
            }
        }

    }
}
