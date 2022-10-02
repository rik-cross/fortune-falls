using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended.Content;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Serialization;

using System;
using System.Collections.Generic;

using AdventureGame.Engine;

namespace AdventureGame
{
    public static class HomeEntity
    {

        public static void houseOnCollisionEnter(Entity thisEntity, Entity otherEntity, float distance)
        {
            if (otherEntity.Tags.HasTag("player"))
            {

                Globals.gameScene.GetCameraByName("main").trackedEntity = null;
                Globals.gameScene.GetCameraByName("minimap").trackedEntity = null;
                Globals.gameScene.RemoveEntity(otherEntity);

                otherEntity.GetComponent<Engine.TransformComponent>().position = new Vector2(150, 90);
                TransformComponent thisTransform = thisEntity.GetComponent<Engine.TransformComponent>();
                //float newX = thisTransform.GetCenter().X;
                float newX = thisTransform.size.X;
                Console.WriteLine($"Building X:{thisTransform.position.X}  New Player X:{newX}");

                Globals.homeScene.GetCameraByName("main").SetWorldPosition(otherEntity.GetComponent<Engine.TransformComponent>().GetCenter(), instant: true);
                Globals.homeScene.GetCameraByName("minimap").SetWorldPosition(otherEntity.GetComponent<Engine.TransformComponent>().GetCenter(), instant: true);
                Globals.homeScene.GetCameraByName("main").trackedEntity = otherEntity;
                Globals.homeScene.GetCameraByName("minimap").trackedEntity = otherEntity;
                Globals.homeScene.AddEntity(otherEntity);

                EngineGlobals.sceneManager.transition = new FadeSceneTransition(Globals.homeScene, replaceScene: true);

            }
        }

        public static Engine.Entity Create(int x, int y)
        {
            Entity entity = EngineGlobals.entityManager.CreateEntity();

            entity.Tags.Name = "home";
            entity.Tags.AddTag("building"); // home or building?

            entity.AddComponent(new TransformComponent(new Vector2(x, y), new Vector2(88, 89)));
            entity.AddComponent(new Engine.SpriteComponent("homeImage"));
            entity.AddComponent(new ColliderComponent(new Vector2(80,20), new Vector2(5, 68)));
            entity.AddComponent(new TriggerComponent(
                new Vector2(20,3),
                new Vector2(35, 88),
                onCollisionEnter: houseOnCollisionEnter
            ));

            return entity;
        }
    }
}
