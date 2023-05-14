using AdventureGame.Engine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace AdventureGame
{
    public static class BuildingEntity3
    {
        public static Engine.Entity Create(string filename, int x, int y,
            int width = 0, int height = 0, string defaultState = "default")
        //public static Engine.Entity Create(int x, int y, string filename = "")//, List<string> keys = null)
        //public static Engine.Entity Create(string filename, int x, int y, int width, int height,
        //    Dictionary<string, Vector2> spriteData = null)
        {
            Entity entity = EngineGlobals.entityManager.CreateEntity();
            //entity.Tags.Id = "player_house";
            entity.Tags.AddTag("building");
            entity.Tags.AddTag(filename);
            string dir = "Buildings/";
            Vector2 position = new Vector2(x, y);
            //Vector2 size = new Vector2(width, height);

            // Sprites
            Engine.SpriteComponent spriteComponent = entity.AddComponent<Engine.SpriteComponent>(
                new Engine.SpriteComponent(dir + filename, defaultState));

            entity.State = defaultState;
            //entity.State = "door_closed";

            Vector2 size = spriteComponent.GetSpriteSize(defaultState);

            // Components
            entity.AddComponent(new TransformComponent(position, size));

            entity.AddComponent<Engine.ColliderComponent>(
                new Engine.ColliderComponent(
                    size: new Vector2(60, 37),
                    offset: new Vector2(3, 29)
                )
            );

            //entity.AddComponent(new Engine.TriggerComponent(
            //    size: new Vector2(16, 10),
            //    offset: new Vector2(25, 60),
            //    onCollide: (Entity entity, Entity otherEntity, float distance) => {
            //        if (otherEntity == EngineGlobals.entityManager.GetLocalPlayer())
            //        {
            //            EngineGlobals.sceneManager.SetActiveScene<HomeScene>();
            //            EngineGlobals.sceneManager.SetPlayerScene<HomeScene>(new Vector2(50, 50));
            //        }
            //    }
            //));

            return entity;
        }

    }
}