﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;

using Engine;

namespace AdventureGame
{
    public static class ItemEntity
    {
        public static Engine.Entity Create(int x, int y, Item item,
            bool isCollectable = true, List<string> collectableByType = default,
            //int width = default, int height = default, // Tags tag, 
            bool animation = false)
        {
            // Should isCollectable and collectableByType by database lookups?
            // Should this be stored in the Item?

            Entity itemEntity = EngineGlobals.entityManager.CreateEntity();
            itemEntity.Tags.AddTag("item");

            // How to handle sprite sheets dynamically?
            // How to add optional params? e.g. size
            if (!animation)
            {
                itemEntity.AddComponent(new Engine.SpriteComponent(item.Filename));
            }

            Vector2 imageSize = itemEntity.GetComponent<SpriteComponent>().GetSpriteSize();
            //Console.WriteLine($"Item image width {imageSize.X} height {imageSize.Y}");

            itemEntity.AddComponent(new Engine.TransformComponent(new Vector2(x, y), imageSize));
            itemEntity.AddComponent(new Engine.ColliderComponent(imageSize, isSolid: false));
            itemEntity.AddComponent(new Engine.ItemComponent(item));

            if (isCollectable)
                itemEntity.AddComponent(new Engine.CollectableComponent(collectableByType));

            return itemEntity;

        }

    }
}
