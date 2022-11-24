using Microsoft.Xna.Framework;
using System;

namespace AdventureGame.Engine
{
    public class ItemCollectionSystem : System
    {
        public ItemCollectionSystem()
        {
            RequiredComponent<ItemComponent>();
            RequiredComponent<CollectableComponent>();
        }

        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            ItemComponent itemComponent = entity.GetComponent<ItemComponent>();
            CollectableComponent collectableComponent = entity.GetComponent<CollectableComponent>();
            ColliderComponent colliderComponent = entity.GetComponent<ColliderComponent>();
            //TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            // Check if the item is active and has not been collected
            if (collectableComponent.IsActive && !collectableComponent.HasBeenCollected)
            {
                // Respond to a collision between the item and another entity
                foreach (Entity otherEntity in colliderComponent.CollidedEntities)
                {
                    // Check if the item can be collected by the other entity
                    if (collectableComponent.CanCollect(otherEntity.Tags))
                    {
                        // Test - delete or move the item entity
                        // Should this be broadcast as a message instead to be picked up by InventorySystem?
                        // E.g. broadcast("itemPickup / itemCollected", entity, otherEntity) scene,time?

                        // MOVE component declaration to the top?
                        // Check that the other entity has an inventory to add the item to
                        InventoryComponent inventoryComponent = otherEntity.GetComponent<InventoryComponent>();
                        if (inventoryComponent != null)
                        {
                            int origQuantity = itemComponent.Item.Quantity; // Testing

                            Item item = EngineGlobals.inventoryManager.AddItem(
                            inventoryComponent.InventoryItems, itemComponent.Item);
                            //Console.WriteLine($"\nOriginal item: {item.ItemId} Quantity{item.Quantity} Stack{item.StackSize} Durability{item.Durability}");
                            
                            // Try to add the item to the other entity's inventory
                            if (item == null)
                            {
                                //Console.WriteLine("Item collected!");
                                //entity.Destroy();
                                entity.GetComponent<TransformComponent>().position.X += 50;
                                itemComponent.Item.Quantity = origQuantity;

                                if (itemComponent.Item.HasItemHealth())
                                {
                                    Random random = new Random();
                                    itemComponent.Item.ItemHealth = random.Next(0, itemComponent.Item.MaxHealth);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Inventory full!");
                                Console.WriteLine($"Item {item.ItemId} has {item.Quantity} remaining");
                            }
                        }
                    }
                }
            }
        }


    }
}
