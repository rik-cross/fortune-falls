using Engine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace AdventureGame
{
    public static class PlayerHouseEntity
    {
        public static Engine.Entity Create(int x, int y, string filename,
            List<string> spriteKeys = null, string defaultState = "default")
        {
            Entity entity = EngineGlobals.entityManager.CreateEntity();

            // Add tags
            entity.Tags.Id = "player_house";
            entity.Tags.AddTag("building");
            entity.Tags.AddTag(filename);

            // Add sprites
            string dir = "Buildings/";
            Engine.SpriteComponent spriteComponent = entity.AddComponent<Engine.SpriteComponent>();

            if (spriteKeys == null)
                spriteComponent.AddSprite(dir + filename, defaultState);
            else
            {
                spriteComponent.AddMultipleStaticSprites(dir + filename, spriteKeys);
                // Set the state to the first key if none given
                if (!spriteKeys.Contains(defaultState))
                    defaultState = spriteKeys[0];
            }

            // Set state
            entity.SetState(defaultState);

            // Add other components
            Vector2 position = new Vector2(x, y);
            Vector2 size = spriteComponent.GetSpriteSize(defaultState);
            entity.AddComponent(new TransformComponent(position, size));

            entity.AddComponent<Engine.ColliderComponent>(
                new Engine.ColliderComponent(
                    size: new Vector2(60, 37),
                    offset: new Vector2(3, 29)
                )
            );

            entity.AddComponent(new Engine.TriggerComponent(
                size: new Vector2(16, 10),
                offset: new Vector2(25, 60),
                onCollide: (Entity entity, Entity otherEntity, float distance) => {
                    if (otherEntity.IsPlayerType())
                    {
                        otherEntity.SetState("idle_" + otherEntity.State.Split("_")[1]);
                        entity.SetState("door_open");
                        //EngineGlobals.sceneManager.SetActiveScene<HomeScene>();
                        //EngineGlobals.sceneManager.SetPlayerScene<HomeScene>(new Vector2(114, 127));
                    }
                }
            ));

            return entity;
        }

    }
}