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

        public static Engine.Entity Create(int x, int y)
        {
            Entity lightSourceEntity = EngineGlobals.entityManager.CreateEntity();

            lightSourceEntity.Tags.Id = "light1"; // REMOVE
            lightSourceEntity.Tags.AddTag("light");

            string directory = "";
            string filename = "candleTest";
            string filePath = directory + filename;
            int width = 32;
            int height = 32;

            // By default, could each sub texture be calculate using
            // x = filewidth / spritewidth, y = 0??
            //int[,] subTextures = new int[4, 2] { { 0, 0 }, { 1, 0 }, { 2, 0 }, { 3, 0 } };
            List<List<int>> subTextureValues = new List<List<int>>();
            subTextureValues.Add(new List<int>() { 0, 0 });
            subTextureValues.Add(new List<int>() { 1, 0 });
            subTextureValues.Add(new List<int>() { 2, 0 });
            subTextureValues.Add(new List<int>() { 3, 0 });

            Engine.SpriteSheet lightSourceSpriteSheet = new Engine.SpriteSheet(filePath, width, height);
            lightSourceEntity.AddComponent(new Engine.SpriteComponent(lightSourceSpriteSheet, subTextureValues));

            lightSourceEntity.AddComponent(new Engine.TransformComponent(x, y, width, height));
            lightSourceEntity.AddComponent(new Engine.ColliderComponent(new Vector2(12, 6), new Vector2(10, 26)));
            lightSourceEntity.AddComponent(new Engine.LightComponent(100));
            lightSourceEntity.AddComponent(new Engine.TriggerComponent(
                size: new Vector2(132, 132),
                offset: new Vector2(-50, -50),
                onCollisionEnter: SceneTriggers.LightOnCollisionEnter,
                onCollisionExit: SceneTriggers.LightOnCollisionExit
            ));
            return lightSourceEntity;
        }

    }
}
