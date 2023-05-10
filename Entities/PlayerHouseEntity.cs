using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;

using AdventureGame.Engine;

namespace AdventureGame
{
    public static class PlayerHouseEntity
    {
        public static Engine.Entity Create(int x, int y)
        {
            Entity entity = EngineGlobals.entityManager.CreateEntity();
            entity.Tags.Id = "player_house";
            entity.Tags.AddTag("building");

            entity.AddComponent(new TransformComponent(new Vector2(x, y), new Vector2(66, 67)));

            Engine.SpriteSheet spriteSheet = new Engine.SpriteSheet("Buildings/house", new Vector2(66, 67));

            Engine.SpriteComponent spriteComponent = entity.AddComponent<Engine.SpriteComponent>(
                new Engine.SpriteComponent(
                    new Engine.Sprite(spriteSheet.GetSubTexture(0, 0))
                )
            );

            entity.AddComponent<Engine.ColliderComponent>(
                new Engine.ColliderComponent(
                    size: new Vector2(60, 37),
                    offset: new Vector2(3, 29)
                )
            );

            spriteComponent.AddSprite(
                "door_closed",
                new Engine.Sprite(spriteSheet.GetSubTexture(0, 0))
            );

            spriteComponent.AddSprite(
                "door_open",
                new Engine.Sprite(spriteSheet.GetSubTexture(1, 0))
            );

            entity.State = "door_closed";


            entity.AddComponent(new Engine.TriggerComponent(
                size: new Vector2(16, 10),
                offset: new Vector2(25, 60),
                onCollide: (Entity entity, Entity otherEntity, float distance) => {
                    if (otherEntity == EngineGlobals.entityManager.GetLocalPlayer())
                    {
                        EngineGlobals.sceneManager.SetActiveScene<HomeScene>();
                        EngineGlobals.sceneManager.SetPlayerScene<HomeScene>(new Vector2(50, 50));
                    }
                }
            ));


            return entity;

        }

    }
}
