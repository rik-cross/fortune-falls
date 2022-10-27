using System.Collections.Generic;

namespace AdventureGame.Engine
{

    public class ItemComponent : Component  // CollectableComponent??
    {
        //public string ItemId { get; set; }
        public Item Item { get; set; } // Move to make it a CollectableComponent?
        public Tags CollectableByTag { get; set; }
        public bool HasBeenCollected { get; set; }
        public bool DestroyOnCollect { get; set; }
        public bool IsActive { get; set; } // needed?
        //public bool IsVisible { get; set; }

        public ItemComponent(Item item = default,
            string collectableByType = "player",
            bool hasBeenCollected = false, bool destroyOnCollect = true,
            bool isActive = true)
        {
            Item = item;
            CollectableByTag = new Tags(collectableByType);
            HasBeenCollected = hasBeenCollected;
            DestroyOnCollect = destroyOnCollect;
            IsActive = isActive;
        }

        public ItemComponent(Item item = default,
            List<string> collectableByTag = default,
            bool hasBeenCollected = false, bool destroyOnCollect = true,
            bool isActive = true)
        {
            Item = item;
            CollectableByTag = new Tags(collectableByTag);
            HasBeenCollected = hasBeenCollected;
            DestroyOnCollect = destroyOnCollect;
            IsActive = isActive;
        }

        // Check if an entity can collect the item
        public bool CanCollect(string tag)
        {
            return CollectableByTag.HasType(tag);
        }

        // Check if any given entities can collect the item
        public bool CanCollect(Tags tags)//List<string> tags)
        {
            return CanCollect(tags.Type);
        }

        // Check if any given entities can collect the item
        public bool CanCollect(List<string> tags)
        {
            foreach (string type in tags)
            {
                if (CollectableByTag.HasType(type))
                    return true;
            }
            return false;
        }
    }

}
