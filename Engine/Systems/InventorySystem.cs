using Microsoft.Xna.Framework;

namespace AdventureGame.Engine
{
    public class InventorySystem : System
    {
        public InventorySystem()
        {
            RequiredComponent<InventoryComponent>();
        }

        // Add an item to the inventory
        // Return the inserted position or -1 if there is not enough space
        public Item AddItem(Item[] inventoryItems, Item item) // newItem?
        {
            //int insertedPosition = -1;
            //int quantity = item.Quantity;

            // Check if the item doesn't hold the maximum quantity already
            if (item.Quantity < item.StackSize)
            {
                // Check if the item can be stacked with an existing inventory item
                for (int i = 0; i < inventoryItems.Length; i++)
                {
                    Item currentItem = inventoryItems[i];

                    if (currentItem != null)
                    {
                        if (currentItem.ItemId == item.ItemId
                            && currentItem.Quantity < currentItem.StackSize)
                        {
                            if (currentItem.Quantity + item.Quantity <= currentItem.StackSize)
                            {
                                // The quantity can be added to the current item
                                inventoryItems[i].IncreaseQuantity(item.Quantity);
                                //insertedPosition = i;
                                //break;

                                // DELETE the Item?? Does it exist anywhere in memory?
                                // Does this effectively delete Item??
                                item = null;
                                return item;
                            }
                            else
                            {
                                // Add as much as possible to the current item's quantity
                                int availableSpace = currentItem.StackSize - currentItem.Quantity;
                                inventoryItems[i].IncreaseQuantity(availableSpace);
                                item.Quantity -= availableSpace;

                                //int remainingQuantity = quantity;
                            }
                        }
                    }
                }
            }

            // Check if there is any quantity of the item remaining
            // Check if the item has already been added to existing inventory items
            if (item.Quantity > 0)
            {
                // Add the item to the next free inventory slot if there's space
                int nextSlot = FindNextFreeSlot(inventoryItems);
                if (nextSlot != -1)
                {
                    inventoryItems[nextSlot] = item;
                    //insertedPosition = nextSlot;
                }
            }
            else
            {
                // DELETE item??
                item = null;
                return item;
            }

            //return insertedPosition;
            return item;
        }

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

        //
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

        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            InventoryComponent inventoryComponent = entity.GetComponent<InventoryComponent>();

            // how to check if item has been added / picked up?

        }

        public override void DrawEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            //InventoryComponent inventoryComponent = entity.GetComponent<InventoryComponent>();

            // drawn inventory depends on type (e.g. playerBag, chest, hotbar?) and size

            //Globals.spriteBatch.Draw(inventoryComponent.texture, transformComponent.position);
        }

    }
}
