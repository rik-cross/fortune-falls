using System;
using System.Collections.Generic;

namespace AdventureGame.Engine
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
        public Item AddItem(Item[] inventoryItems, Item item)
        {
            // Try to stack the item with existing inventory items first
            if (item.IsStackable())
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

        // Delete an item object
        public Item DeleteItem(Item item)
        {
            item = null;
            return item;
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

        /*
        // NOT used
        // Add an item to the inventory at a specified position
        // Return the item if one already exists in that position
        public Item AddItemAtPosition(Item[] inventoryItems, Item item, int position)
        {
            Item currentItem = inventoryItems[position];

            // Check if the position is empty
            if (currentItem == null)
                inventoryItems[position] = item;
            else
            {
                // Check if the items are the same type and can stack
                if (item.ItemId == currentItem.ItemId)
                {
                    item = StackItems(inventoryItems, item, position);
                    if (item == null || item.Quantity == 0)
                        return null;
                }
                else //if (item.Quantity > 0)
                {
                    //Item temp = currentItem;
                    inventoryItems[position] = item;
                }
            }
            return currentItem;
        }
        */
    }
}
