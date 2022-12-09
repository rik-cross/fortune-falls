using Microsoft.Xna.Framework;
using System;

namespace AdventureGame.Engine
{
    public class MapCollisionSystem : System
    {

        public MapCollisionSystem()
        {
            RequiredComponent<ColliderComponent>();
        }

        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {

            ColliderComponent colliderComponent = entity.GetComponent<ColliderComponent>();

            foreach (Rectangle rect in scene.CollisionTiles)
            {
                if (rect.Intersects(colliderComponent.Rect))
                {
                    // create a map collision object?
                    Console.WriteLine($"Entity {entity.Id} collided with tile");
                }
            }

        }
    }
}
