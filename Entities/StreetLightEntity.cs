using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using AdventureGame.Engine;

using S = System.Diagnostics.Debug;

namespace AdventureGame
{
    public static class StreetLightEntity
    {

        public static Engine.Entity Create(int x, int y)
        {
            Vector2 entitySize = new Vector2(20, 138);

            Engine.Entity entity;
            entity = Engine.EngineGlobals.entityManager.CreateEntity();

            entity.Tags.AddTag("streetlight");

            entity.AddComponent(new Engine.TransformComponent(
                new Vector2(x, y),
                entitySize
            ));

            Engine.SpriteSheet spriteSheet = new Engine.SpriteSheet("Objects/light", (int)entitySize.X, (int)entitySize.Y);
            Engine.SpriteComponent spriteComponent = (Engine.SpriteComponent)entity.AddComponent(new Engine.SpriteComponent(new Engine.Sprite(spriteSheet.GetSubTexture(0, 0))));
            spriteComponent.AddSprite("on", spriteSheet, 0, 1, 1);
            spriteComponent.GetSprite("on").loop = false;

            entity.AddComponent(new Engine.ColliderComponent(
                size: new Vector2(20, 5),
                offset: new Vector2(0, 133)
            ));

            entity.AddComponent(new LightComponent(
                radius: 200,
                visible: false,
                offset: new Vector2(10, 75))
            );

            return entity;
        }

        public static void SwitchToOpenState(Entity entity, Entity otherEntity, float distance)
        {
            Engine.InputComponent inputComponent = otherEntity.GetComponent<Engine.InputComponent>();
            if (inputComponent != null && EngineGlobals.inputManager.IsPressed(inputComponent.input.button1))
            {
                entity.State = "open";
                entity.RemoveComponent<Engine.TriggerComponent>();
            }
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
