using System;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    public class Tags
    {
        // A Name tag should be unique to an entity
        public string Name { get; set; }

        // A Type tag can be shared by multiple entities.
        // Entities can have multiple Type tags
        public List<string> Type { get; } = new List<string>();


        public Tags() { }

        public Tags(string type, string name = default)
        {
            Name = name;
            Type.Add(type);
        }

        public Tags(List<string> type, string name = default)
        {
            Name = name;
            Type = type;
        }

        // Removes the Name tag
        public void RemoveName()
        {
            Name = "";
        }

        // Adds a tag to the Type hashset
        public void AddTag(string type)
        {
            Type.Add(type);
        }

        // Removes a tag from the Type hashset
        public void RemoveTag(string type)
        {
            if (Type != null)
                Type.Remove(type);
        }

        // Returns whether a tag exists in the Type hashset
        public bool HasTag(string type)
        {
            if (Type != null)
                return Type.Contains(type);
            return false;
        }

    }
}
