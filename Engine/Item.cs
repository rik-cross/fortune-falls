using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame.Engine
{
    public class Item
    {
        public string ItemId { get; private set; }
        public string Filename { get; set; }
        public Texture2D Texture { get; set; }
        public int Quantity { get; set; }
        public int StackSize { get; set; } // or lookup based on Id?
        public int ItemHealth { get; set; }
        public int MaxHealth { get; set; }
        public Tags ItemTags { get; set; }
        // public string ItemType { get; private set; }

        public Item()
        {
            ItemTags = new Tags();
        }

        public Item(string itemId, string filename = default, Texture2D texture = default,
            int quantity = 1, int stackSize = 1,
            int itemHealth = -1, int maxHealth = 100,
            Tags itemTags = default)
        {
            ItemId = itemId;
            Filename = filename;

            if (texture == default && filename != default)
                Texture = Globals.content.Load<Texture2D>(filename);
            else
                Texture = texture;

            Quantity = quantity;
            StackSize = stackSize;
            ItemHealth = itemHealth;

            if (Quantity > StackSize)
                Quantity = StackSize;

            if (ItemHealth == -1)
                MaxHealth = -1;
            else
                MaxHealth = maxHealth;

            if (itemTags == default)
                ItemTags = new Tags();
            else
                ItemTags = itemTags;
        }

        public Item(Item item)
        {
            ItemId = item.ItemId;
            Filename = item.Filename;
            Texture = item.Texture;
            Quantity = item.Quantity;
            StackSize = item.StackSize;
            ItemHealth = item.ItemHealth;
            MaxHealth = item.MaxHealth;
            ItemTags = item.ItemTags;
        }

        // Increase the quantity of the inventory item
        public void IncreaseQuantity(int amount = 1)
        {
            int newQuantity = Quantity + amount;

            if (newQuantity > StackSize)
                Quantity = StackSize;
            else
                Quantity += amount;
        }

        // Decrease the quantity of the inventory item
        public void DecreaseQuantity(int amount = 1)
        {
            int newQuantity = Quantity - amount;

            if (newQuantity < 0)
                Quantity = 0;
            else
                Quantity -= amount;
        }

        // Return whether the item is stackable
        public bool IsStackable() { return StackSize > 1; }

        // Return whether the item has any free space
        public bool HasFreeSpace() { return Quantity < StackSize; }

        // Increase the health of the inventory item
        public void IncreaseItemHealth(int amount = 1)
        {
            int newHealth = ItemHealth + amount;

            if (newHealth > MaxHealth)
                ItemHealth = MaxHealth;
            else
                ItemHealth += amount;
        }

        // Decrease the health of the inventory item
        public void DecreaseHealth(int amount = 1)
        {
            int newHealth = ItemHealth - amount;

            if (newHealth < 0)
                ItemHealth = 0;
            else
                ItemHealth -= amount;
        }

        // Return whether the item has health
        public bool HasHealth() { return ItemHealth != -1; }

        // Return the amount of health remaining out of 100
        public int GetHealthPercentage()
        {
            if (ItemHealth == -1)
                return -1;

            return (int)((double)ItemHealth / MaxHealth * 100);
        }

    }
}
