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

        public bool ContainsItem(string itemId)
        {
            foreach (Item item in InventoryItems)
                if (item.ItemId == itemId)
                    return true;
            return false;
        }
    }

}
