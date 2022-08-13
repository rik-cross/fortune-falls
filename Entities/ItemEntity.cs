using Microsoft.Xna.Framework;

using MonoGame.Extended.Content;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Serialization;

using System.Collections.Generic;

using AdventureGame.Engine;

namespace AdventureGame
{
    public static class ItemEntity
    {

        public static Engine.Entity Create(int x, int y)
        {

            Entity itemEntity = EngineGlobals.entityManager.CreateEntity();

            itemEntity.AddTag("item");

            itemEntity.AddComponent(new Engine.TransformComponent(new Vector2(x, y), new Vector2(65, 50)));
            
            itemEntity.AddComponent(new Engine.SpritesComponent("idle", new Engine.Sprite(Globals.enemySpriteSheet, new List<Vector2> {
                new Vector2(0,0)
            })));

            itemEntity.AddComponent(new Engine.ItemComponent(new Vector2(0, 0), new Vector2(65, 50)));

            return itemEntity;

        }

    }
}
