﻿using Microsoft.Xna.Framework;
using System;
using AdventureGame;

namespace Engine
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
            CanCollectComponent canCollectComponent = entity.GetComponent<CanCollectComponent>();
            CollisionHandlerComponent collisionHandlerComponent = entity.GetComponent<CollisionHandlerComponent>();
            InventoryComponent inventoryComponent = entity.GetComponent<InventoryComponent>();

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
                EngineGlobals.soundManager.PlaySoundEffect(SoundEffects.itemPickupSound);

                // Check if the item is active and has not been collected
                if (collectableComponent.IsCollectableBy(entity.Tags)
                    && collectableComponent.IsActive
                    && !collectableComponent.HasBeenCollected)
                {
                    // Check if the item is a key item
                    if (itemComponent.Item.ItemTags.HasType("key_item"))
                    {
                        KeyItemsComponent keyItems = entity.GetComponent<KeyItemsComponent>();
                        if (keyItems != null)
                        {
                            keyItems.AddItem(itemComponent.Item);
                            if (collectableComponent.DestroyOnCollect)
                                otherEntity.Destroy();
                        }
                    }

                    // Try to add the item to the other entity's inventory
                    else
                    {
                        string itemText = itemComponent.Item.ItemId;
                        int amount = itemComponent.Item.Quantity;

                        Item item = EngineGlobals.inventoryManager.AddAndStackItem(
                            inventoryComponent.InventoryItems, itemComponent.Item);

                        if (item == null)
                            otherEntity.Destroy();
                        else
                            amount -= item.Quantity;

                        if (amount > 0)
                            itemText += $" x {amount}";

                        EngineGlobals.log.Add($"Collected: [{itemText}]");
                    }
                }
            }
        }


    }
}
