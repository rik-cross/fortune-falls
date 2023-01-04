using Microsoft.Xna.Framework;
using System;

namespace AdventureGame.Engine
{
    public class ItemCollectionSystem : System
    {
        public ItemCollectionSystem()
        {
            //RequiredComponent<ItemComponent>();
            //RequiredComponent<CollectableComponent>();
            RequiredComponent<CanCollectComponent>();
            RequiredComponent<CollisionHandlerComponent>();
            RequiredComponent<InventoryComponent>();
        }

        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            //ItemComponent itemComponent = entity.GetComponent<ItemComponent>();
            //CollectableComponent collectableComponent = entity.GetComponent<CollectableComponent>();
            CanCollectComponent canCollectComponent = entity.GetComponent<CanCollectComponent>();
            CollisionHandlerComponent collisionHandlerComponent = entity.GetComponent<CollisionHandlerComponent>();
            InventoryComponent inventoryComponent = entity.GetComponent<InventoryComponent>();
            //ColliderComponent colliderComponent = entity.GetComponent<ColliderComponent>();
            //TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            if (!canCollectComponent.IsActive)
                return;

            // Respond to a collision between the item and another entity
            foreach (Entity otherEntity in collisionHandlerComponent.CollidedEntities)
            {
                CollectableComponent collectableComponent = otherEntity.GetComponent<CollectableComponent>();
                ItemComponent itemComponent = otherEntity.GetComponent<ItemComponent>();

                if (collectableComponent == null || itemComponent == null)
                    continue;

                Console.WriteLine($"Item collection: {entity.Id} and {otherEntity.Id}");

                // Check if the item is active and has not been collected
                if (collectableComponent.IsCollectableBy(entity.Tags)
                    && collectableComponent.IsActive
                    && !collectableComponent.HasBeenCollected)
                {
                    // Test - delete or move the item entity
                    // Should this be broadcast as a message instead to be picked up by InventorySystem?
                    // E.g. broadcast("itemPickup / itemCollected", entity, otherEntity) scene,time?

                    Console.WriteLine($"\nOriginal item: {itemComponent.Item.ItemId} Quantity{itemComponent.Item.Quantity} Stack{itemComponent.Item.StackSize} Health{itemComponent.Item.ItemHealth}");

                    // Try to add the item to the other entity's inventory
                    Item item = EngineGlobals.inventoryManager.AddItem(
                        inventoryComponent.InventoryItems, itemComponent.Item);

                    if (item == null)
                    {
                        //Console.WriteLine("Item collected!");
                        otherEntity.Destroy();

                        /* // Testing
                        otherEntity.GetComponent<TransformComponent>().position.X += 50;
                        itemComponent.Item.Quantity = origQuantity;

                        if (itemComponent.Item.HasItemHealth())
                        {
                            Random random = new Random();
                            itemComponent.Item.ItemHealth = random.Next(0, itemComponent.Item.MaxHealth);
                        }
                        */
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
