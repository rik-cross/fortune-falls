using AdventureGame.Engine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace AdventureGame
{
    public static class BuildingEntity3
    {
        public static Engine.Entity Create(int x, int y, string filename,
            List<string> spriteKeys = null, string defaultState = "default",
            string idTag = null)
        {
            Entity entity = EngineGlobals.entityManager.CreateEntity();

            // Tags
            if (idTag != null)
                entity.Tags.Id = idTag;
            entity.Tags.AddTag("building");
            entity.Tags.AddTag(filename);

            // Sprites
            string dir = "Buildings/";

            Engine.SpriteComponent spriteComponent = entity.AddComponent<Engine.SpriteComponent>(
                new Engine.SpriteComponent());

            if (spriteKeys == null)
                spriteComponent.AddSprite(dir + filename, defaultState);
            else
            {
                spriteComponent.AddMultipleStaticSprites(dir + filename, spriteKeys);
                // Set the state to the first key if none given
                if (!spriteKeys.Contains(defaultState))
                    defaultState = spriteKeys[0];
            }

            entity.State = defaultState;

            Vector2 position = new Vector2(x, y);
            Vector2 size = spriteComponent.GetSpriteSize(defaultState);

            // Components
            entity.AddComponent(new TransformComponent(position, size));

            float colliderHeightPercentage = 0.7f;
            entity.AddComponent<Engine.ColliderComponent>(
                new Engine.ColliderComponent(
                    size: new Vector2(size.X, size.Y * colliderHeightPercentage),
                    offset: new Vector2(0, size.Y * (1 - colliderHeightPercentage))
                )
            );

            return entity;
        }
    }
}