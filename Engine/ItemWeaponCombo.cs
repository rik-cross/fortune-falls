using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace AdventureGame.Engine
{
    public class Item2
    {
        public string ItemId { get; private set; }
        public string Filename { get; set; }
        public int Quantity { get; set; }
        public int StackSize { get; set; } // or lookup based on Id?
        public int ItemHealth { get; set; } // bool HasHealth?
        public int MaxHealth { get; set; }
        public Texture2D Texture { get; set; }
        public SoundEffect SuccessSound { get; set; }
        public SoundEffect FailSound { get; set; }
        public Tags ItemTags { get; set; }
        // public string ItemType { get; private set; }

        public Item2()
        {
            ItemTags = new Tags();
        }

        public Item2(string itemId, string filename = null,
            int quantity = 1, int stackSize = 1,
            int itemHealth = -1, int maxHealth = 100,
            Texture2D texture = null,
            //SoundEffect successSound = null, SoundEffect failSound = null,
            string successSoundFilepath = null, string failSoundFilepath = null,
            Tags itemTags = null)
        {
            ItemId = itemId;
            Filename = filename;

            if (texture == null && filename != null)
                Texture = Globals.content.Load<Texture2D>(filename);
            else
                Texture = texture;

            if (successSoundFilepath != null)
                SuccessSound = Globals.content.Load<SoundEffect>(successSoundFilepath);

            if (failSoundFilepath != null)
                FailSound = Globals.content.Load<SoundEffect>(failSoundFilepath);

            Quantity = quantity;
            StackSize = stackSize;
            ItemHealth = itemHealth;

            if (Quantity > StackSize)
                Quantity = StackSize;

            if (ItemHealth == -1)
                MaxHealth = -1;
            else
                MaxHealth = maxHealth;

            if (itemTags == null)
                ItemTags = new Tags(); // Tags("item")?
            else
                ItemTags = itemTags;
        }

        public Item2(Item item)
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
        public void IncreaseHealth(int amount = 1)
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

        // Return whether the item has a health
        public bool HasItemHealth() { return ItemHealth != -1; }

        // Return the amount of health remaining out of 100
        public int GetHealthPercentage()
        {
            if (ItemHealth == -1)
                return -1;

            return (int)((double)ItemHealth / MaxHealth * 100);
        }

    }
}
