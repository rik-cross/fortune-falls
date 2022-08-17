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
        //private Engine.Image itemImage;
        //private Vector2 imageSize;
        //private Engine.Animation itemAnimation;

        public static Engine.Entity Create(int x, int y, string assetName, bool animation = false)
        {

            Entity itemEntity = EngineGlobals.entityManager.CreateEntity();

            itemEntity.AddTag("item");

            Vector2 imageSize = new Vector2(34, 34);

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
                    new Engine.Sprite(
                        new Engine.SpriteSheet(Globals.content.Load<Texture2D>(assetName),
                        new Vector2(34, 34)),
                        new List<Vector2> { new Vector2(0, 0) })));

            }

            //Console.WriteLine($"Item image width {itemImage.Width} height {itemImage.Height}");

            /*
            itemEntity.AddComponent(new Engine.SpritesComponent("idle", new Engine.Sprite(Globals.enemySpriteSheet, new List<Vector2> {
                new Vector2(0,0)
            })));
            Texture2D sprite = itemEntity.GetComponent<SpriteComponent>().sprite;
            */

            itemEntity.AddComponent(new Engine.TransformComponent(new Vector2(x, y), imageSize));//itemImage.Size));
            //itemEntity.AddComponent(new Engine.ItemComponent(new Vector2(0, 0), itemImage.Size));
            //itemEntity.AddComponent(new Engine.ItemComponent(new Vector2(0, 0), itemImage.Size, itemImage.Texture));
            //itemEntity.AddComponent(new Engine.ItemComponent(new Vector2(0, 0), imageSize, itemImage.Texture));

            return itemEntity;

        }

    }
}
