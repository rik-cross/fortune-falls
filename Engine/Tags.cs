using System;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    public class Tags
    {
        public string Name { get; set; }
        public HashSet<string> Type { get; }

        public Tags()
        {
            Type = new HashSet<string>();
        }

        public Tags(string type, string name = default)
        {
            Name = name;
            Type.Add(type);
        }

        public Tags(HashSet<string> type, string name = default)
        {
            Name = name;
            Type = type;
        }

        public Tags(List<string> type, string name = default)
        {
            Name = name;
            Type = new HashSet<string>(type);
        }

        // Removes the name
        public void RemoveName()
        {
            Name = "";
        }

        // Adds a tag to the type hashset
        public void AddTag(string type)
        {
            Type.Add(type);
        }

        // Removes a tag from the type hashset
        public void RemoveTag(string type)
        {
            Type.Remove(type);
        }

        // Returns whether a tag exists in the type hashset
        public bool HasTag(string type)
        {
            return Type.Contains(type);
        }

    }
}
