using AdventureGame.Engine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace AdventureGame
{
    public static class BuildingEntity2
    {
        public static Engine.Entity Create(string filename, int x, int y, List<string> keys = null)
        //public static Engine.Entity Create(string filename, int x, int y, int width, int height,
        //    Dictionary<string, Vector2> spriteData = null)
        {
            Entity entity = EngineGlobals.entityManager.CreateEntity();
            entity.Tags.Id = "player_house";
            entity.Tags.AddTag("building");
            string dir = "Buildings/";
            Vector2 position = new Vector2(x, y);
            //Vector2 size = new Vector2(width, height); // Todo

            // Sprites
            //Engine.SpriteSheet spriteSheet = new Engine.SpriteSheet(dir + filename, size);
            Engine.SpriteSheet spriteSheet = new Engine.SpriteSheet(dir + filename);

            Engine.SpriteComponent spriteComponent = entity.AddComponent<Engine.SpriteComponent>(
                new Engine.SpriteComponent(
                    new Engine.Sprite(spriteSheet.GetSubTexture(0, 0))
                )
            );
            /*
            spriteComponent.AddSprite(
                "door_closed",
                new Engine.Sprite(spriteSheet.GetSubTexture(0, 0))
            );

            spriteComponent.AddSprite(
                "door_open",
                new Engine.Sprite(spriteSheet.GetSubTexture(1, 0))
            );
            */

            // Testing

            // Create a sprite sheet from the file path (without state="idle" ?)
            //Engine.SpriteComponent spriteComponent = entity.AddComponent<Engine.SpriteComponent>(
            //    dir + filename
            //);

            // OR
            // Create a sprite sheet from the file path with the default sprite
            // which may not be "idle" (defaultState property?)

            //// Assign a key to each sprite subtexture
            //foreach (KeyValuePair<string, Vector2> sprite in spriteData)
            //{
            //    spriteComponent.AddSprite(sprite.Key, (int)sprite.Value.X, (int)sprite.Value.Y);
            //}

            // Does a single sprite always need a key?
            if (keys == null || keys.Count == 0)
            {
                //spriteComponent.AddSprite("idle", 0, 0);  // Todo
                spriteComponent.AddSprite(
                    "door_closed",  // idle?
                    new Engine.Sprite(spriteSheet.GetSubTexture(0, 0)));
            }
            else
            {
                // Hacky AF - todo!
                spriteSheet.spriteSize = new Vector2(
                    spriteSheet.spriteSize.X / keys.Count, spriteSheet.spriteSize.Y);

                // Will only work for sprite sheets on a single row
                for (int i = 0; i < keys.Count; i++)
                {

                    spriteComponent.AddSprite(
                        keys[i],
                        new Engine.Sprite(spriteSheet.GetSubTexture(i, 0)));
                }
            }

            entity.State = "door_closed";

            Vector2 size = spriteComponent.GetSpriteSize();

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