using Microsoft.Xna.Framework;

using System;

namespace AdventureGame.Engine
{
    public class HealthSystem : System
    {
        public HealthSystem()
        {
            RequiredComponent<HealthComponent>();
        }

        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
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
                }
                else
                {
                    // Player destroyed logic
                }
            }
        }

    }
}
