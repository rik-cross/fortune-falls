﻿using System;
using System.Collections.Generic;
using AdventureGame;

namespace Engine
{
    public class InventoryManager
    {
        // Testing - stores all information about each item
        private Dictionary<string, Dictionary<string, string>> itemInformation;

        public InventoryManager()
        {
            TestingAddItemInfo();
        }

        public void TestingAddItemInfo()
        {
            itemInformation = new Dictionary<string, Dictionary<string, string>>();

            itemInformation["arrowsStandard"] = new Dictionary<string, string>()
            {{"name", "Arrows"}, {"description", "Basic arrows"}, {"stackSize", "large"}};

            itemInformation["swordWooden"] = new Dictionary<string, string>()
            {{"name", "Wooden sword"}, {"description", "Sword of wood"}, {"stackSize", "single"}};

            itemInformation["sticks"] = new Dictionary<string, string>()
            {{"name", "Sticks"}, {"description", "Nice sticks"}, {"stackSize", "medium"}};

            itemInformation["pebbles"] = new Dictionary<string, string>()
            {{"name", "Pebbles"}, {"description", "Small stones"}, {"stackSize", "large"}};

            // armourType, armourAmount, damageType, damageAmount, healingAmount, healingOverTime
            // asset/image, icon
        }

        public int GetStackSize(string itemId, int modifier = 0)
        {
            int stackSize = 1;

            switch (itemInformation[itemId]["stackSize"])
            {
                case "single":
                    break;
                case "small":
                    stackSize = 5;
                    break;
                case "medium":
                    stackSize = 10;
                    break;
                case "large":
                    stackSize = 20;
                    break;
            }

            // Increase the stack size of anything that holds multiple items
            if (stackSize > 1)
                stackSize += modifier;

            return stackSize;
        }

        // Add an item to the inventory
        // Return the item or a null item if there is no quantity remaining
        public Item AddAndStackItem(Item[] inventoryItems, Item item, bool stack = true)
        {
            // Try to stack the item with existing inventory items first
            if (item.IsStackable() && stack)
            {
                for (int i = 0; i < inventoryItems.Length; i++)
                {
                    item = StackItem(inventoryItems, i, item);
                    if (item == null)
                        return null;
                }
            }

            // Add the item to the next free inventory slot if there's space
            int nextSlot = FindNextFreeSlot(inventoryItems);
            if (nextSlot != -1)
            {
                // Make a copy of the item's properties and update the quantity
                inventoryItems[nextSlot] = new Item(item);
                inventoryItems[nextSlot].Quantity = item.Quantity;
                return DeleteItem(item);
            }

            return item;
        }

        // Try to add the item to the inventory, otherwise drop the item in-game
        public void AddOrDropItem(Item[] inventoryItems, Item item)
        {
            if (item != null)
            {
                // Try adding the item to the inventory
                Console.WriteLine("Stack unable to be returned - try adding to inventory");
                item = AddAndStackItem(inventoryItems, item);

                // Check if the inventory is full 
                if (item != null)
                    DropItem(inventoryItems, item);

                // CHANGE to reflect more parameters???
                // where to get this info from?
            }
        }

        // Remove an item from the inventory
        public bool RemoveItem(Item[] inventoryItems, int index)
        {
            bool itemRemoved = false;

            if (IsItemValid(inventoryItems, index))
            {
                inventoryItems[index] = null;
                itemRemoved = true;
            }

            return itemRemoved;
        }

        // Delete an item object
        public Item DeleteItem(Item item)
        {
            item = null;
            return item;
        }

        // Drop the item on the in-game ground using the entity's position
        public void DropItem(Item[] inventoryItems, Item item = null, int index = -1,
            Entity entity = null, bool dropMultiple = false, bool isCollectable = true,
            List<string> collectableByType = default, bool animation = false)
        {
            Console.WriteLine("Drop the item on the in-game ground");

            Entity player = EngineGlobals.entityManager.GetEntityByName("player");

            if (IsItemValid(inventoryItems, index))
                item = inventoryItems[index];

            if (item == null)
                return;

            // If no entity is given then drop the item by the local player
            if (entity == null)
                entity = player;

            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();
            Random random = new Random();
            int itemX, itemY, randomSign;
            int randomX = 0;
            int randomY = 0;

            // todo: allow X and Y to be preset and/or in relation to item
            // Initialise the item position to below the bottom center of the entity
            itemX = (int)transformComponent.Center - item.Texture.Width / 2;
            //itemY = (int)transformComponent.Bottom;
            itemY = (int)transformComponent.Bottom - item.Texture.Height - 5;

            // todo: distribute items evenly
            if (dropMultiple)
            {
                // Offset the item X and Y position by a pseudo-random amount
                //randomX = random.Next(0, 15);
                //randomY = random.Next(0, 4);

                if (index % 2 == 0)
                {
                    itemX -= item.Texture.Width / 3;
                }
                else
                {
                    itemX += item.Texture.Width / 3;
                }
                itemY -= item.Texture.Height / 2;
            }
            //}
            //else
            //{
            //    // Initialise the item position to the middle center of the entity
            //    itemX = (int)transformComponent.Center - item.Texture.Width / 2;
            //    itemY = (int)transformComponent.Middle - item.Texture.Height / 2;

            //    // Offset the item X and Y position by a pseudo-random amount
            //    if (dropMultiple)
            //    {
            //        randomX = random.Next(0, 16);
            //        randomY = random.Next(0, 16);

            //        // Randomise +- item Y offset amount
            //        randomSign = random.Next(0, 2);
            //        if (randomSign == 0)
            //            randomY *= -1;
            //    }
            //}

            // Randomise +- item X offset amount
            randomSign = random.Next(0, 2);
            if (randomSign == 0)
                randomX *= -1;

            itemX += randomX;
            itemY += randomY;

            // todo
            // Check that the item position is within the scene boundaries
            // Check that the item is not colliding with anything e.g. a building

            // Create the item and add it to the player scene
            Scene playerScene = player.GetComponent<SceneComponent>().Scene;
            if (playerScene != null)
            {
                playerScene.AddEntity(ItemEntity.Create(itemX, itemY, item,
                    isCollectable, collectableByType, animation));
            }
        }

        // Drop all the items from an inventory on the in-game ground
        public void DropAllItems(Item[] inventoryItems, Entity entity = null)
            //bool isCollectable = true, List<string> collectableByType = default,
            //bool animation = false)
        {
            Console.WriteLine("Drop all inventory items on the in-game ground");

            bool multiple = false;
            for (int i = 0; i < inventoryItems.Length; i++)
            {
                Item item = inventoryItems[i];
                if (item != null)
                {
                    DropItem(inventoryItems, item, i, entity, dropMultiple: multiple);
                    multiple = true;
                }
            }
        }

        // Return true if an item is within range and not null
        public bool IsItemValid(Item[] inventoryItems, int index)
        {
            bool isValid = false;

            if (index >= 0 && index < inventoryItems.Length)
                isValid = true;

            if (isValid && inventoryItems[index] == null)
                isValid = false;

            return isValid;
        }

        // Find the next available position in the inventory list 
        public int FindNextFreeSlot(Item[] inventoryItems)
        {
            int availablePosition = -1;

            for (int i = 0; i < inventoryItems.Length; i++)
            {
                if (inventoryItems[i] == null)
                {
                    availablePosition = i;
                    break;
                }
            }

            return availablePosition;
        }

        // Add the quantity of another item to the index item if possible.
        // Return null if the other item is now empty.
        public Item StackItem(Item[] inventoryItems, int itemIndex, Item otherItem)
        {
            Item itemToAddTo = inventoryItems[itemIndex];

            if (itemToAddTo == null || otherItem == null)
                return otherItem;

            if (itemToAddTo.ItemId == otherItem.ItemId && itemToAddTo.HasFreeSpace())
            {
                int availableSpace = itemToAddTo.StackSize - itemToAddTo.Quantity;

                if (availableSpace >= otherItem.Quantity)
                {
                    // Add the entire quantity to the item
                    inventoryItems[itemIndex].IncreaseQuantity(otherItem.Quantity);
                    return DeleteItem(otherItem);
                }
                else
                {
                    // Add as much as possible to the item
                    inventoryItems[itemIndex].IncreaseQuantity(availableSpace);
                    otherItem.DecreaseQuantity(availableSpace);
                }
            }

            return otherItem;
        }

        // Add the quantity of another item to the index item if possible.
        // Update the original array of inventory items.
        public void StackItem(Item[] inventoryItems, int itemIndex, int otherItemIndex)
        {
            Item otherItem = inventoryItems[otherItemIndex];
            otherItem = StackItem(inventoryItems, itemIndex, otherItem);
            inventoryItems[otherItemIndex] = otherItem;
        }

        // Add an item to the inventory at a specified index.
        // Return the item if one already exists or it cannot be fully stacked
        public Item AddItemAtPosition(Item[] inventoryItems, Item item, int index)
        {
            Item itemAtIndex = inventoryItems[index];

            if (itemAtIndex == null)
            {
                inventoryItems[index] = new Item(item);
                return DeleteItem(item);
            }
            else if (item.ItemId == itemAtIndex.ItemId)
            {
                // Try to stack the items
                item = StackItem(inventoryItems, index, item);
                if (item == null)
                    return DeleteItem(item);
            }

            return item;
        }

        // Swap the positions of two items
        public void SwapItems(Item[] inventoryItems, int firstIndex, int secondIndex)
        {
            if (firstIndex < inventoryItems.Length && secondIndex < inventoryItems.Length
                && firstIndex >= 0 && secondIndex >= 0)
            {
                Item firstItem = inventoryItems[firstIndex];
                Item secondItem = inventoryItems[secondIndex];
                inventoryItems[firstIndex] = secondItem;
                inventoryItems[secondIndex] = firstItem;
            }
        }

        // Create a copy of an item and share the quantity between the two
        public Item SplitItemStack(Item[] inventoryItems, int itemIndex,
            int newItemQuantity = 0)
        {
            Item originalItem = inventoryItems[itemIndex];
            Item newItem = null;

            int quantity = originalItem.Quantity;
            if (quantity > 1)
            {
                // By default, split the stack in half
                if (newItemQuantity == 0)
                    quantity /= 2;
                else if (newItemQuantity > 0 && newItemQuantity <= quantity)
                    quantity -= newItemQuantity;
                else
                    return null;

                // Create a copy of the item and update both quantities
                newItem = new Item(originalItem);
                inventoryItems[itemIndex].Quantity = quantity;
                newItem.DecreaseQuantity(quantity);
            }

            return newItem;
        }

        // NOT used
        // Stack all inventory items as much as possible from the start
        // Could also bunch items together to fill empty spaces? SortItems() / DefragItems()
        public void StackAllItems(Item[] inventoryItems)
        {
            Item currentItem;

            // Go through each item from end to start + 1
            for (int i = inventoryItems.Length - 1; i > 0; i--)
            {
                currentItem = inventoryItems[i];
                if (currentItem.IsStackable())
                {
                    // Check every previous item
                    for (int j = i - 1; j >= 0; j--)
                    {
                        // CHANGE to use StackItems()
                        Item itemToStack = inventoryItems[j];
                        if (currentItem.ItemId == itemToStack.ItemId
                            && itemToStack.HasFreeSpace())
                        {
                            // STACK!
                        }
                    }
                }
            }
        }
    }
}
