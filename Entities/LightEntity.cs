using Microsoft.Xna.Framework;

using MonoGame.Extended.Content;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Serialization;

using AdventureGame.Engine;

namespace AdventureGame
{
    public static class LightEntity
    {

        public static void lightOnCollisionEnter(Entity thisEntity, Entity otherEntity, float distance)
        {
            otherEntity.AddComponent(new Engine.TextComponent("Hello! Here is some text, hopefully split over a few lines!"));
        }
        public static void lightOnCollisionExit(Entity thisEntity, Entity otherEntity, float distance)
        {
            otherEntity.RemoveComponent<Engine.TextComponent>();
        }

        public static Engine.Entity Create(int x, int y)
        {
            // values based off X / Y and width / height
            int lightColliderX = x - (int)(50 / 2);
            int lightColliderY = y - (int)(50 / 2);

            //Entity lightSourceEntity = new Entity();
            Entity lightSourceEntity = EngineGlobals.entityManager.CreateEntity();

            lightSourceEntity.AddTag("light");

            lightSourceEntity.AddComponent(new Engine.TransformComponent(new Vector2(x, y), new Vector2(32, 32)));
            lightSourceEntity.AddComponent(new Engine.AnimationComponent(new AnimatedSprite(Globals.content.Load<SpriteSheet>("candleTest.sf", new JsonContentLoader()))));
            lightSourceEntity.AddComponent(new Engine.ColliderComponent(lightColliderX, lightColliderY, 50, 50));
            lightSourceEntity.AddComponent(new Engine.LightComponent(50));
            lightSourceEntity.AddComponent(new Engine.TriggerComponent(
                new Vector2(-20, -20), new Vector2(72, 72),
                lightOnCollisionEnter,
                null,
                lightOnCollisionExit
            ));
            return lightSourceEntity;
        }

    }
}
