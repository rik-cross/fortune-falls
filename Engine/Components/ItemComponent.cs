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

        //public HashSet<string> collectableByTag;
        public Tags CollectableByTag { get; set; }

        //public ItemComponent(Vector2 offset, Vector2 size, bool isActive = true, bool isSolid = true)
        public ItemComponent(HashSet<string> collectableByTag = default,
            bool hasBeenCollected = false, bool isActive = true)
        {
            //CollectableByTag = collectableByTag;
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
        public bool CanCollect(HashSet<string> tags)
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
