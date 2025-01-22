using Microsoft.Xna.Framework;
using Engine;
using System.Collections.Generic;

namespace AdventureGame
{
    public static class StreetLightEntity
    {

        public static Engine.Entity Create(int x, int y, string filename)
        {
            Engine.Entity entity = Engine.EngineGlobals.entityManager.CreateEntity();
            entity.Tags.AddTag("streetlight");

            // Add sprites
            string dir = "Objects/";
            Engine.SpriteComponent spriteComponent = entity.AddComponent<Engine.SpriteComponent>();

            List<string> spriteKeys = new List<string>() { "light_off", "light_on" };
            spriteComponent.AddMultipleStaticSprites(dir + filename, spriteKeys);

            // Set state
            entity.SetState("light_off");

            // Add other components
            Vector2 size = spriteComponent.GetSpriteSize(entity.State);
            entity.AddComponent(new Engine.TransformComponent(
                position: new Vector2(x, y),
                size: size
            ));

            entity.AddComponent(new Engine.ColliderComponent(
                size: new Vector2(20, 5),
                offset: new Vector2(0, 133)
            ));

            entity.AddComponent(new LightComponent(
                radius: 200,
                visible: false,
                offset: new Vector2(10, 75))
            );

            return entity;
        }

    }
}