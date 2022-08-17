using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended.Content;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Serialization;

using System.Collections.Generic;

using AdventureGame.Engine;

namespace AdventureGame
{
    public static class LightEntity
    {

        public static void lightOnCollisionEnter(Entity thisEntity, Entity otherEntity, float distance)
        {
            EngineGlobals.sceneManager.GetTopScene().GetCameraByName("main").trackedEntity = thisEntity;
            EngineGlobals.sceneManager.GetTopScene().GetCameraByName("main").SetZoom(4.0f, 0.02f);
            //otherEntity.AddComponent(new Engine.TextComponent("Hello! Here is some text, hopefully split over a few lines!"));
        }
        public static void lightOnCollisionExit(Entity thisEntity, Entity otherEntity, float distance)
        {
            EngineGlobals.sceneManager.GetTopScene().GetCameraByName("main").trackedEntity = EngineGlobals.entityManager.GetEntityByTag("player");
            EngineGlobals.sceneManager.GetTopScene().GetCameraByName("main").SetZoom(3.0f, 0.01f);
        }

        public static Engine.Entity Create(int x, int y)
        {
            Entity lightSourceEntity = EngineGlobals.entityManager.CreateEntity();

            lightSourceEntity.AddTag("light");

            lightSourceEntity.AddComponent(new Engine.TransformComponent(new Vector2(x, y), new Vector2(32, 32)));

            lightSourceEntity.AddComponent(new Engine.SpritesComponent("idle", new Engine.Sprite(new List<Texture2D> {
                Globals.candleSpriteSheet.GetSubTexture(0, 0),
                Globals.candleSpriteSheet.GetSubTexture(1, 0),
                Globals.candleSpriteSheet.GetSubTexture(2, 0),
                Globals.candleSpriteSheet.GetSubTexture(3, 0),
            })));

            lightSourceEntity.AddComponent(new Engine.ColliderComponent(new Vector2(10, 26), new Vector2(12, 6)));
            lightSourceEntity.AddComponent(new Engine.LightComponent(100));
            lightSourceEntity.AddComponent(new Engine.TriggerComponent(
                new Vector2(-50, -50), new Vector2(132, 132),
                lightOnCollisionEnter,
                null,
                lightOnCollisionExit
            ));
            return lightSourceEntity;
        }

    }
}
