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
        // Return the item or a null Item if there is no quantity remaining
        public Item AddItem(Item[] inventoryItems, Item item) // newItem? InventoryComponent?
        {
            // Note: should an item with >1 stack size and different durability / shelf-life
            // be combined or kept separate? (depending on item type?)

            int quantity = item.Quantity;
            //Console.WriteLine($"\nCollect item: {item.ItemId} Quantity{item.Quantity} Stack{item.StackSize} Durability{item.Durability}");

            // Check if the item doesn't hold the maximum quantity already
            if (quantity < item.StackSize)
            {
                // Check if the item can be stacked with an existing inventory item
                for (int i = 0; i < inventoryItems.Length; i++)
                {
                    Item currentItem = inventoryItems[i];
                    //Console.WriteLine($"Checking inventory slot {i}");

                    if (currentItem != null)
                    {
                        if (currentItem.ItemId == item.ItemId && currentItem.HasFreeSpace())
                        {
                            if (currentItem.Quantity + quantity <= currentItem.StackSize)
                            {
                                // The quantity can be added to the current item
                                inventoryItems[i].IncreaseQuantity(quantity);
                                //Console.WriteLine($"Adding ALL {quantity} to slot {i}");
                                //Console.WriteLine($"Slot {i} quantity is now {inventoryItems[i].Quantity}");

                                return DeleteItem(item);
                            }
                            else
                            {
                                // Add as much as possible to the current item's quantity
                                int availableSpace = currentItem.StackSize - currentItem.Quantity;
                                inventoryItems[i].IncreaseQuantity(availableSpace);
                                quantity -= availableSpace;

                                // Reduce the quantity of the original Item
                                item.Quantity -= availableSpace;

                                //Console.WriteLine($"Adding {availableSpace} to slot {i}");
                                //Console.WriteLine($"Slot {i} quantity is now {inventoryItems[i].Quantity}");
                                //Console.WriteLine($"Quantity remaining is {quantity}");
                            }
                        }
                    }
                }
            }

            // Check if the item has already been added to existing inventory items
            if (quantity > 0)
            {
                // Add the item to the next free inventory slot if there's space
                int nextSlot = FindNextFreeSlot(inventoryItems);
                if (nextSlot != -1)
                {
                    // Make a copy of the item's properties and update the quantity
                    inventoryItems[nextSlot] = new Item(item);
                    inventoryItems[nextSlot].Quantity = quantity; // Is this bad?? :P
                    //Console.WriteLine($"Adding item with quantity {quantity} to slot {nextSlot}");

                    return DeleteItem(item);
                }
            }
            else
                return DeleteItem(item);

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

        // Delete an Item object
        public Item DeleteItem(Item item)
        {
            item = null;
            return item;
        }

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

        // NOT used
        public Item StackItems(Item[] inventoryItems, Item item, int position)
        {
            Item existingItem = inventoryItems[position];

            if (existingItem == null)
                return null;

            // Check if there is space to stack more
            if (existingItem.ItemId == item.ItemId
                && existingItem.Quantity < existingItem.StackSize)
            {
                if (existingItem.Quantity + item.Quantity <= existingItem.StackSize)
                {
                    // The quantity can be added to the current item
                    inventoryItems[position].IncreaseQuantity(item.Quantity);
                    return null;
                }
                else
                {
                    // Add as much as possible to the current item's quantity
                    int availableSpace = existingItem.StackSize - existingItem.Quantity;
                    inventoryItems[position].IncreaseQuantity(availableSpace);
                    item.Quantity -= availableSpace;
                    // quantityDifference
                    //int remainingQuantity = quantity;
                }
            }
            return item;
        }
    }
}
