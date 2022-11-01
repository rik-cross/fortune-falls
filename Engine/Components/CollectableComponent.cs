using System.Collections.Generic;

namespace AdventureGame.Engine
{

    public class CollectableComponent : Component
    {
        public Tags CollectableByType { get; set; }
        public bool HasBeenCollected { get; set; }
        public bool DestroyOnCollect { get; set; }
        public bool IsActive { get; set; } // needed?
        //public bool IsVisible { get; set; }
        //public bool IsCollectable { get; set; }

        public CollectableComponent(string collectableByType = "player",
            bool hasBeenCollected = false, bool destroyOnCollect = true,
            bool isActive = true)
        {
            CollectableByType = new Tags(collectableByType);
            HasBeenCollected = hasBeenCollected;
            DestroyOnCollect = destroyOnCollect;
            IsActive = isActive;
        }

        public CollectableComponent(List<string> collectableByType = default,
            bool hasBeenCollected = false, bool destroyOnCollect = true,
            bool isActive = true)
        {
            if (collectableByType == default)
                CollectableByType = new Tags("player");
            else
                CollectableByType = new Tags(collectableByType);

            HasBeenCollected = hasBeenCollected;
            DestroyOnCollect = destroyOnCollect;
            IsActive = isActive;
        }

        // Return whether an entity can collect the item
        public bool CanCollect(string tag)
        {
            return CollectableByType.HasType(tag);
        }

        // Return whether any given entities can collect the item
        public bool CanCollect(Tags tags)//List<string> tags)
        {
            return CanCollect(tags.Type);
        }

        // Return whether any given entities can collect the item
        public bool CanCollect(List<string> tags)
        {
            foreach (string type in tags)
            {
                if (CollectableByType.HasType(type))
                    return true;
            }
            return false;
        }
    }

}
