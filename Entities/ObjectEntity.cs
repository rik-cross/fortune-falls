using AdventureGame.Engine;
using Microsoft.Xna.Framework;

namespace AdventureGame
{
    public static class ObjectEntity
    {
        public static Engine.Entity Create(int x, int y, string filename,
            int drawOrderOffset = 0, bool isSolid = true, bool canWalkBehind = false,
            Vector2 colliderSize = default, Vector2 colliderOffset = default,
            string idTag = null) // , bool isInteractive = false)
        {
            Entity objectEntity = EngineGlobals.entityManager.CreateEntity();
            objectEntity.Tags.Id = idTag;
            objectEntity.Tags.AddTag("object");

            // Add sprites
            string dir = "Objects/";
            objectEntity.AddComponent(new Engine.SpriteComponent(dir + filename));

            // Add other components
            Vector2 imageSize = objectEntity.GetComponent<SpriteComponent>().GetSpriteSize();
            objectEntity.AddComponent(new Engine.TransformComponent(new Vector2(x, y), imageSize));

            // Draw order offset
            if (drawOrderOffset != 0)
                objectEntity.GetComponent<TransformComponent>().SetDrawOrderOffset(drawOrderOffset);

            // Add a collider component if required
            if (isSolid)
            {
                if (colliderSize == default)
                {
                    if (canWalkBehind)
                    {
                        // Make the collider component a third of the object's height
                        float colliderHeight = imageSize.Y / 3;
                        colliderSize = new Vector2(imageSize.X, colliderHeight);

                        if (colliderOffset.Y == 0)
                        {
                            colliderOffset = new Vector2(colliderOffset.X,
                                imageSize.Y - colliderHeight);
                        }
                    }
                    else
                        colliderSize = imageSize;
                }
                objectEntity.AddComponent(new Engine.ColliderComponent(colliderSize, colliderOffset));
            }

            return objectEntity;
        }
    }
}