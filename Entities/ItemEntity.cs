using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;

using AdventureGame.Engine;

namespace AdventureGame
{
    public static class ItemEntity
    {
        public static Engine.Entity Create(int x, int y, string filename,
            List<string> collectableByTag = default,
            //int width = default, int height = default,
            bool animation = false)
        {

            Entity itemEntity = EngineGlobals.entityManager.CreateEntity();

            itemEntity.Tags.AddTag("item");

            string directory = "Items/";
            string filePath = directory + filename;

            // How to handle sprite sheets dynamically?
            // How to add optional params? e.g. size
            if (!animation)
            {
                /*itemEntity.AddComponent(new Engine.SpriteComponent("idle",
                    new Engine.Sprite(Globals.content.Load<Texture2D>(directory + filename))
                    ));*/
                itemEntity.AddComponent(new Engine.SpriteComponent(filePath));
            }

            Texture2D texture = itemEntity.GetComponent<SpriteComponent>().GetSprite("idle").textureList[0];
            Vector2 imageSize = new Vector2(texture.Width, texture.Height);
            //Console.WriteLine($"Item image width {imageSize.X} height {imageSize.Y}");

            itemEntity.AddComponent(new Engine.TransformComponent(new Vector2(x, y), imageSize));
            itemEntity.AddComponent(new Engine.ItemComponent(collectableByTag));
            itemEntity.AddComponent(new Engine.ColliderComponent(imageSize));

            return itemEntity;

        }

    }
}
