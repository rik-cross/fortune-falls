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

                Globals.homeScene.AddEntity(EngineGlobals.entityManager.GetEntityByTag("player"));
                Globals.homeScene.GetCameraByName("main").trackedEntity = EngineGlobals.entityManager.GetEntityByTag("player");

                Globals.gameScene.GetCameraByName("main").trackedEntity = null;
                Globals.gameScene.RemoveEntity(EngineGlobals.entityManager.GetEntityByTag("player"));

                EngineGlobals.entityManager.GetEntityByTag("player").GetComponent<Engine.TransformComponent>().position = new Vector2(150, 90);
                EngineGlobals.sceneManager.transition = new FadeSceneTransition(new List<Scene> { Globals.gameScene }, new List<Scene> { Globals.homeScene }, replaceScenes: true);
            }
        }

        public static Engine.Entity Create(int x, int y)
        {
            Entity entity = EngineGlobals.entityManager.CreateEntity();

            entity.AddTag("home");

            entity.AddComponent(new TransformComponent(new Vector2(x, y), new Vector2(88, 89)));
            entity.AddComponent(new Engine.SpritesComponent("idle", new Engine.Sprite(Globals.content.Load<Texture2D>("homeImage"))));
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
