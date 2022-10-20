using System;

namespace AdventureGame.Engine
{

    public class InventoryComponent : Component
    {
        //public string InventoryId { get; private set; }
        public int InventorySize { get; private set; }
        public Item[] InventoryItems { get; private set; }
        public Tags Tags { get; set; }

        public InventoryComponent(int inventorySize = 20, string type = "chest")
        {
            InventorySize = inventorySize;
            InventoryItems = new Item[inventorySize];
            Tags = new Tags(type);
        }
    }

}
