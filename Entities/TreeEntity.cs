using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace AdventureGame
{
    public static class TreeEntity
    {
        public static Engine.Entity Create(int x, int y, bool stump = false)
        {
            Engine.Entity entity = Engine.EngineGlobals.entityManager.CreateEntity();

            entity.Tags.AddTag("tree");

            entity.AddComponent(new Engine.TransformComponent(
                position: new Vector2(x, y),
                size: new Vector2(26, 31)
            ));

            Engine.SpriteSheet spriteSheet = new Engine.SpriteSheet("Objects/tree", new Vector2(26,33));
            Engine.SpriteComponent spriteComponent = entity.AddComponent<Engine.SpriteComponent>(
                new Engine.SpriteComponent(
                    new Engine.Sprite(spriteSheet.GetSubTexture(0, 0))
                )
            );


            spriteComponent.AddSprite(
                "tree",
                new Engine.Sprite(spriteSheet.GetSubTexture(0, 0))
            );

            spriteComponent.AddSprite(
                "tree_stump",
                new Engine.Sprite(spriteSheet.GetSubTexture(1, 0))
            );

            entity.AddComponent<Engine.ColliderComponent>(
                new Engine.ColliderComponent(
                    size: new Vector2(14, 10),
                    offset: new Vector2(6, 21)
                )    
            );

            if (stump)
                entity.State = "tree_stump";
            else
                entity.State = "tree";

            return entity;
        }
    }
}
