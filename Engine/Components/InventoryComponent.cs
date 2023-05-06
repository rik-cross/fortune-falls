namespace AdventureGame.Engine
{

    public class InventoryComponent : Component
    {
        public Item[] InventoryItems { get; private set; }
        //public string InventoryId { get; private set; }
        public int InventorySize { get; private set; }
        public bool DropOnDestroy { get; set; }
        public Tags Tags { get; set; }

        public InventoryComponent(int inventorySize = 20, string type = "chest",
            bool dropOnDestroy = true)
        {
            InventorySize = inventorySize;
            InventoryItems = new Item[inventorySize];
            DropOnDestroy = dropOnDestroy;
            Tags = new Tags(type);
        }

        // Adds a new item to the first available position
        // Returns True if there is space to add the item
        public bool AddItem(Item newItem)
        {
            for (int i = 0; i < InventoryItems.Length; i++)
            {
                if (InventoryItems[i] == null)
                {
                    InventoryItems[i] = newItem;
                    return true;
                }
            }
            return false;
        }

        public bool ContainsItem(string itemId)
        {
            foreach (Item item in InventoryItems)
                if (item != null && item.ItemId == itemId)
                    return true;
            return false;
        }
    }

}
