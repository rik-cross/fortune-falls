using AdventureGame.Engine;
using Microsoft.Xna.Framework;

namespace AdventureGame
{
    public static class PlayerHouseEntity
    {
        public static Engine.Entity Create(string filename, int x, int y, int width, int height)
        {
            Entity entity = EngineGlobals.entityManager.CreateEntity();
            entity.Tags.Id = "player_house";
            entity.Tags.AddTag("building");
            string dir = "Buildings/";
            Vector2 position = new Vector2(x, y);
            Vector2 size = new Vector2(width, height);

            // Sprites
            Engine.SpriteSheet spriteSheet = new Engine.SpriteSheet(dir + filename, size);

            Engine.SpriteComponent spriteComponent = entity.AddComponent<Engine.SpriteComponent>(
                new Engine.SpriteComponent(
                    new Engine.Sprite(spriteSheet.GetSubTexture(0, 0))
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

            // Components
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
                        otherEntity.State = "idle_" + otherEntity.State.Split("_")[1];
                        entity.State = "door_open";
                        EngineGlobals.sceneManager.SetActiveScene<HomeScene>();
                        EngineGlobals.sceneManager.SetPlayerScene<HomeScene>(new Vector2(114, 127));
                    }
                }
            ));

            return entity;
        }

    }
}