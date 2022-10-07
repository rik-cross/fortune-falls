using System.Collections.Generic;

namespace AdventureGame.Engine
{
    public class Item
    {
        public string ItemId { get; set; }
        public int Quantity { get; set; }
        public int StackSize { get; set; }
        public int Durability { get; set; }
        // public string Filename { get; private set; }
        public Tags ItemTags { get; set; } // will this work? not an entity
        // public string ItemType { get; private set; }

        public Item()
        {
            ItemTags = new Tags();
        }

        public Item(string itemId, int quantity = 1, int stackSize = 1,
            int durability = 100, Tags itemTags = default)
        {
            ItemId = itemId;
            Quantity = quantity;
            StackSize = stackSize;
            Durability = durability;

            if (itemTags == default)
                ItemTags = new Tags();
            else
                ItemTags = itemTags;
        }

        // Increase the quantity of the inventory item
        public void IncreaseQuantity(int amount = 1) { Quantity += amount; }

        // Decrease the quantity of the inventory item
        public void DecreaseQuantity(int amount = 1) { Quantity -= amount; }

        // Increase the durability of the inventory item
        public void IncreaseDurability(int amount = 1) { Durability += amount; }

        // Decrease the durability of the inventory item
        public void DecreaseDurability(int amount = 1) { Durability -= amount; }

    }
}
