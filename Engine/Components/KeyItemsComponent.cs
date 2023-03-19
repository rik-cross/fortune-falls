using System;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    public class KeyItemsComponent : Component
    {
        public Dictionary<string, Item> KeyItems { get; private set; }

        public KeyItemsComponent(Dictionary<string, Item> keyItems = null)
        {
            if (keyItems == null)
                KeyItems = new Dictionary<string, Item>();
            else
                KeyItems = keyItems;
        }

        public void AddItem(Item item)
        {
            KeyItems[item.ItemId] = item;
            Console.WriteLine($"Added key item: {item.ItemId} {item.Filename}");
        }

        public bool RemoveItem(Item item)
        {
            return KeyItems.Remove(item.ItemId);
        }

        public Item RemoveItem(string itemId)
        {
            Item item = null;
            if (ContainsItem(itemId))
            {
                item = KeyItems[itemId];
                KeyItems.Remove(item.ItemId);
            }
            return item;
        }

        public bool ContainsItem(Item item)
        {
            return KeyItems.ContainsKey(item.ItemId);
        }

        public bool ContainsItem(string itemId)
        {
            return KeyItems.ContainsKey(itemId);
        }
    }

}
