using Microsoft.Xna.Framework;
using System;

namespace AdventureGame.Engine
{
    public class ItemSystem : System
    {
        public ItemSystem()
        {
            RequiredComponent<ItemComponent>();
        }

        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            ItemComponent itemComponent = entity.GetComponent<ItemComponent>();
            ColliderComponent colliderComponent = entity.GetComponent<ColliderComponent>();
            //TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            // Check if the item is active and has not been collected
            if (itemComponent.IsActive && !itemComponent.HasBeenCollected)
            {
                // Respond to a collision between the item and another entity
                foreach (Entity otherEntity in colliderComponent.collidedEntities)
                {
                    // Check if the item can be collected by the other entity
                    if (itemComponent.CanCollect(otherEntity.Tags))
                    {
                        // Test - delete or hide the item entity
                        //ColliderComponent otherColliderComponent = otherEntity.GetComponent<ColliderComponent>();
                        //otherColliderComponent.collidedEntities.Remove(entity);

                        //EngineGlobals.entityManager.DestroyEntity(entity);

                        //EngineGlobals.inventoryManager.AddItem(itemComponent, otherEntity);
                        //EngineGlobals.inventoryManager.AddItem(itemComponent.Item, otherEntity);
                        //EngineGlobals.inventoryManager.AddItem(entity, otherEntity);

                        // Should this be broadcast as a message instead to be picked up by InventorySystem?
                        // E.g. broadcast("itemPickup / itemCollected", entity, otherEntity) scene,time?
                        InventoryComponent inventoryComponent = otherEntity.GetComponent<InventoryComponent>();
                        if (inventoryComponent != null)
                        {
                            /*
                            // Add the item to the other entity's inventory component if it exists
                            EngineGlobals.inventoryManager.AddItem(inventoryComponent.InventoryItems,
                                itemComponent.Item);

                            // Destroy the item if required
                            if (itemComponent.HasBeenCollected && itemComponent.DestroyOnCollect)
                            {
                                Console.WriteLine("Item collected!");
                                entity.Destroy();
                            }
                            */
                            Item item = EngineGlobals.inventoryManager.AddItem(
                            inventoryComponent.InventoryItems, itemComponent.Item);

                            // Destroy the item if required
                            if (item == null)
                            {
                                Console.WriteLine("Item collected!");
                                entity.Destroy();
                            }
                        }
                    }
                }
            }
        }

    }
}
