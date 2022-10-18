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

                            //Console.WriteLine($"\nOriginal item: {item.ItemId} Quantity{item.Quantity} Stack{item.StackSize} Durability{item.Durability}");

                            // Destroy the item if required
                            if (item == null)
                            {
                                // DOES NOT work??
                                Console.WriteLine("Item collected!");
                                entity.Destroy();
                                //entity.GetComponent<TransformComponent>().position.X += 50;
                            }

                            /*
                            Item item2 = itemComponent.Item;
                            EngineGlobals.inventoryManager.AddItem(inventoryComponent.InventoryItems,
                                itemComponent.Item);

                            Console.WriteLine($"\nOriginal item: {item2.ItemId} Quantity{item2.Quantity} Stack{item2.StackSize} Durability{item2.Durability}");

                            // Destroy the item if required
                            if (item2.Quantity == 0 && itemComponent.DestroyOnCollect)
                            {
                                // DOES NOT work??
                                Console.WriteLine("Item destroyed!");
                                entity.Destroy();
                                //entity.GetComponent<TransformComponent>().position.X += 50;
                            }
                            */
                        }
                    }
                }
            }
        }

    }
}
