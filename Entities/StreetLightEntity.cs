using Microsoft.Xna.Framework;
using AdventureGame.Engine;

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
            Engine.SpriteComponent spriteComponent = entity.AddComponent<SpriteComponent>(
                new SpriteComponent(dir + filename));

            // Add other components
            Vector2 size = spriteComponent.GetSpriteSize();
            entity.AddComponent(new Engine.TransformComponent(
                new Vector2(x, y),
                size
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