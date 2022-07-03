using Microsoft.Xna.Framework;
using MonoGame.Extended.Sprites;
using S = System.Diagnostics.Debug;
using MonoGame.Extended.Tiled;

namespace AdventureGame.Engine
{
    public class MapCollisionSystem : System
    {

        public MapCollisionSystem()
        {
            RequiredComponent<ColliderComponent>();
            RequiredComponent<TransformComponent>();
        }

        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {

            ColliderComponent colliderComponent = entity.GetComponent<ColliderComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            Rectangle entityRect = new Rectangle(
                (int)transformComponent.position.X + colliderComponent.xOffset,
                (int)transformComponent.position.Y + colliderComponent.yOffset,
                20, 20
            );

            //START

            foreach (TiledMapTileLayer layer in scene.map.Layers)
            {
                if (layer.Properties.ContainsValue("collision"))
                {
                    for (int x = 0; x < layer.Width; x++)
                    {
                        for (int y = 0; y < layer.Height; y++)
                        {
                            TiledMapTile? t;
                            bool z = layer.TryGetTile((ushort)x, (ushort)y, out t);
                            if (z)
                            {
                                if (!layer.GetTile((ushort)x, (ushort)y).IsBlank)
                                {

                                    Rectangle tileRect = new Rectangle(
                                        x * 16, y * 16, 16, 16
                                    );

                                    if (entityRect.Intersects(tileRect))
                                    {
                                        
                                        // create a map collision object?

                                    }

                                }
                            }

                        }
                    }
                }
            }

            //END


        }
    }
}
