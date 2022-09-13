using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame.Engine
{

    public class ItemComponent : Component  // CollectableComponent??
    {
        public bool HasBeenCollected { get; set; }
        public bool IsActive { get; set; } // or isCollectable ??

        public Tags CollectableByTag { get; set; }


        public ItemComponent(List<string> collectableByTag = default,
            bool hasBeenCollected = false, bool isActive = true)
        {
            CollectableByTag = new Tags(collectableByTag);

            HasBeenCollected = hasBeenCollected;
            IsActive = isActive;
        }

        // Check if an entity can collect the item
        public bool CanCollect(string tag)
        {
            return CollectableByTag.HasTag(tag);
        }

        // Check if an entity can collect the item
        public bool CanCollect(List<string> tags)
        {
            foreach (string tag in tags)
            {
                if (CollectableByTag.HasTag(tag))
                    return true;
            }
            return false;
        }
    }

}
