using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;

using AdventureGame.Engine;

namespace AdventureGame
{
    public static class BuildingEntity
    {
        public static Engine.Entity Create(int x, int y, string filename,
            string tagId,
            List<string> triggers = default)
        {

            Entity buildingEntity = EngineGlobals.entityManager.CreateEntity();

            buildingEntity.Tags.AddTag("building");

            string directory = "Buildings/";

            // Seems weird to have to set idle...
            // Should this be optional and then re-order the arguments?
            buildingEntity.AddComponent(new Engine.SpriteComponent("idle",
                new Engine.Sprite(Globals.content.Load<Texture2D>(directory + filename))
                ));

            Texture2D texture = buildingEntity.GetComponent<SpriteComponent>().GetSprite("idle").textureList[0];
            Vector2 imageSize = new Vector2(texture.Width, texture.Height);
            //Console.WriteLine($"Item image width {imageSize.X} height {imageSize.Y}");

            buildingEntity.AddComponent(new Engine.TransformComponent(new Vector2(x, y), imageSize));
            buildingEntity.AddComponent(new Engine.ColliderComponent(x, y-30, 0, 30));

            // How to add triggers programmtically?

            return buildingEntity;

        }

    }
}
