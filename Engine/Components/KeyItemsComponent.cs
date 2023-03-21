using System.Collections.Generic;

namespace AdventureGame.Engine
{
    public class KeyItemsComponent : Component
    {
        public List<Item> KeyItems { get; private set; }

        public KeyItemsComponent(List<Item> keyItems = null)
        {
            if (keyItems == null)
                KeyItems = new List<Item>();
            else
                KeyItems = keyItems;
        }

        public void AddItem(Item item)
        {
            KeyItems.Add(item);
        }

        public bool RemoveItem(Item item)
        {
            return KeyItems.Remove(item);
        }

        public bool RemoveItem(string itemId)
        {
            for (int i = 0; i < KeyItems.Count; i++)
            {
                if (KeyItems[i].ItemId == itemId)
                {
                    KeyItems.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        public bool ContainsItem(Item item)
        {
            return KeyItems.Contains(item);
        }

        public bool ContainsItem(string itemId)
        {
            foreach (Item item in KeyItems)
                if (item.ItemId == itemId)
                    return true;
            return false;
        }
    }

}
