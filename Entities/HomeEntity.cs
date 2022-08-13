using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended.Content;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Serialization;

using System.Collections.Generic;

using AdventureGame.Engine;

namespace AdventureGame
{
    public static class HomeEntity
    {

        public static void houseOnCollisionEnter(Entity thisEntity, Entity otherEntity, float distance)
        {
            if (otherEntity.HasTag("player"))
            {
                EngineGlobals.sceneManager.PopScene();
                EngineGlobals.entityManager.GetEntityByTag("player").GetComponent<Engine.TransformComponent>().position = new Vector2(150, 90);
                EngineGlobals.sceneManager.PushScene(new HomeScene());
            }
        }

        public static Engine.Entity Create(int x, int y)
        {
            Entity entity = EngineGlobals.entityManager.CreateEntity();

            entity.AddTag("home");

            entity.AddComponent(new TransformComponent(new Vector2(x, y), new Vector2(88, 89)));
            entity.AddComponent(new Engine.SpritesComponent("idle", new Engine.Sprite(new Engine.SpriteSheet(Globals.content.Load<Texture2D>("homeImage"), new Vector2(86, 88)), new List<Vector2> { new Vector2(0, 0) })));
            entity.AddComponent(new ColliderComponent(new Vector2(5,68),new Vector2(80,20)));
            entity.AddComponent(new TriggerComponent(
                new Vector2(35,88),
                new Vector2(20,3),
                onCollisionEnter: houseOnCollisionEnter
            ));

            return entity;
        }
    }
}
