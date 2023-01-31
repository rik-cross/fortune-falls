using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using AdventureGame.Engine;

using S = System.Diagnostics.Debug;

namespace AdventureGame
{
    public static class ChestEntity {

        public static Engine.Entity Create(int x, int y, int inventorySize = 20)
        {
            Vector2 chestSize = new Vector2(36, 42);

            Engine.Entity entity;
            entity = Engine.EngineGlobals.entityManager.CreateEntity();
            entity.Tags.AddTag("chest");
            
            entity.AddComponent(new Engine.TransformComponent(
                new Vector2(x,y),
                chestSize
            ));

            entity.AddComponent(new Engine.IntentionComponent());
            entity.AddComponent(new Engine.PhysicsComponent());

            Engine.SpriteSheet spriteSheet = new Engine.SpriteSheet("Objects/chest", (int)chestSize.X, (int)chestSize.Y);
            Engine.SpriteComponent spriteComponent = (Engine.SpriteComponent)entity.AddComponent(new Engine.SpriteComponent(new Engine.Sprite(spriteSheet.GetSubTexture(0,0))));
            spriteComponent.AddSprite("open", spriteSheet, 0, 0, 4, repeatNeutral: false);
            // TODO -- add to constrctor
            spriteComponent.GetSprite("open").loop = false;

            entity.AddComponent(new Engine.ColliderComponent(
                size: new Vector2(30, 30),
                offset: new Vector2(0, 30)
            ));

            entity.AddComponent(new Engine.TriggerComponent(
                size: new Vector2(chestSize.X + 10, chestSize.Y + 10),
                offset: new Vector2(-5, -5),
                onCollisionEnter: SwitchToOpenState
            ));

            entity.AddComponent(new Engine.InventoryComponent(inventorySize));

            spriteComponent.GetSprite("open").OnComplete = DropLoot;

            return entity;
        }

        public static void SwitchToOpenState(Entity entity, Entity otherEntity, float distance)
        {
            entity.State = "open";
        }

        public static void DropLoot(Entity entity)
        {
            InventoryComponent inventoryComponent = entity.GetComponent<Engine.InventoryComponent>();
            if (inventoryComponent != null)
            {
                EngineGlobals.inventoryManager.DropAllItems(
                    inventoryComponent.InventoryItems,
                    entity
                );
            }
        }

    }
}
