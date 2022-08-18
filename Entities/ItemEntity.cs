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
    public static class ItemEntity
    {
        public static Engine.Entity Create(int x, int y, string assetName,
            bool animation = false, List<string> collectable = default)
        {

            Entity itemEntity = EngineGlobals.entityManager.CreateEntity();

            itemEntity.AddTag("item");

            //Vector2 imageSize = new Vector2(34, 34);

            // This should probably be an image or sprite component
            // How to add optional params e.g. size
            if (!animation)
            {
                /*
                itemImage = new Engine.Image(
                    Globals.content.Load<Texture2D>(assetName),
                    position: new Vector2(x, y)
                );
                */

                itemEntity.AddComponent(new Engine.SpritesComponent("idle",
                    new Engine.Sprite(Globals.content.Load<Texture2D>(assetName))
                    ));
            }

            Texture2D texture = itemEntity.GetComponent<SpritesComponent>().GetSprite("idle").textureList[0];
            Vector2 imageSize = new Vector2(texture.Width, texture.Height);
            Console.WriteLine($"Item image width {imageSize.X} height {imageSize.Y}");
            //Console.WriteLine($"Item image width {itemImage.Width} height {itemImage.Height}");

            itemEntity.AddComponent(new Engine.TransformComponent(new Vector2(x, y), imageSize));
            //itemEntity.AddComponent(new Engine.ItemComponent(new Vector2(0, 0), itemImage.Size, itemImage.Texture));
            itemEntity.AddComponent(new Engine.ColliderComponent(imageSize));

            if (collectable != default)
            {
                //itemEntity.GetComponent<ItemComponent>().Collectable = collectable;
                /*
                Component itemComponent = itemEntity.GetComponent<SpritesComponent>();

                foreach (string canCollect in collectable)
                {
                    itemComponent.Add/Get component by tag
                }*/
            }

            return itemEntity;

        }

    }
}
