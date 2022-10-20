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
            string itemId, int quantity = 1, int stackSize = 1, int durability = 100,
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
                itemEntity.AddComponent(new Engine.SpriteComponent(filePath));
            }

            Vector2 imageSize = itemEntity.GetComponent<SpriteComponent>().GetSpriteSize();
            //Console.WriteLine($"Item image width {imageSize.X} height {imageSize.Y}");

            itemEntity.AddComponent(new Engine.TransformComponent(new Vector2(x, y), imageSize));

            Item item = new Item(itemId, quantity, stackSize, durability);
            //Item item = new Item("itemTestId", 3, 10, 50);
            itemEntity.AddComponent(new Engine.ItemComponent(item, collectableByTag));

            itemEntity.AddComponent(new Engine.ColliderComponent(imageSize));

            return itemEntity;

        }

    }
}
