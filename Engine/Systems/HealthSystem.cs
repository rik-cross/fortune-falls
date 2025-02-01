using Microsoft.Xna.Framework;

using System;

namespace Engine
{
    public class HealthSystem : System
    {
        public HealthSystem()
        {
            RequiredComponent<HealthComponent>();
        }

        public override void DrawEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            if (EngineGlobals.DEBUG == false)
                return;

            if (entity.GetComponent<Engine.HealthComponent>() == null)
                return;

            Engine.HealthComponent hc = entity.GetComponent<Engine.HealthComponent>();
            Engine.TransformComponent tc = entity.GetComponent<Engine.TransformComponent>();

            EngineGlobals.spriteBatch.DrawString(Theme.FontSecondary, hc.Health.ToString(), new Vector2(tc.Position.X, tc.Position.Y-20), Color.White);
        }

        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {/*
            HealthComponent healthComponent = entity.GetComponent<HealthComponent>();

            if (!healthComponent.HasHealth())
            {
                Console.WriteLine($"Entity {entity.Id} has no health");
                if (!entity.IsPlayerType())
                {
                    InventoryComponent inventoryComponent = entity.GetComponent<InventoryComponent>();
                    if (inventoryComponent != null && inventoryComponent.DropOnDestroy)
                    {
                        EngineGlobals.inventoryManager.DropAllItems(
                            inventoryComponent.InventoryItems, entity);
                    }
                    entity.Destroy();
                    // entity.OnDestroy()
                    // set entity to disabled
                    // play destroyed animation depending on entity type
                    // play destroyed sound depending on entity type
                    // then call entity.Destroy()
                }
                else
                {
                    // Player destroyed logic
                }
            }*/
        }

    }
}
