using System.Collections.Generic;

namespace AdventureGame.Engine
{
    public class Tags
    {
        // An Id tag should be unique to an entity
        public string Id { get; set; } // could be private set and check if id already exists

        // A Type tag can be shared by multiple entities.
        // Entities can have multiple Type tags
        public List<string> Type { get; private set; } = new List<string>();


        public Tags() { }

        public Tags(string type, string id = default)
        {
            Id = id;
            Type.Add(type);
        }

        public Tags(List<string> type, string id = default)
        {
            Id = id;

            if (type == null)
                return;

            if (Type.Count > 0)
                Type.AddRange(type);
            else
                Type = type;
        }

        // Clear the Id tag
        public void ClearId()
        {
            Id = "";
        }

        // Clear the Type tags
        public void ClearType()
        {
            Type = new List<string>();
        }

        // Add a tag to the Type list
        public void AddTag(string type)
        {
            if (!string.IsNullOrEmpty(type))
                Type.Add(type);
        }

        // Add a list of tags to the Type list
        public void AddTags(List<string> type)
        {
            if (type == null)
                return;

            if (Type.Count > 0)
                Type.AddRange(type);
            else
                Type = type;
        }

        // Remove a tag from the Type list
        public void RemoveTag(string type)
        {
            if (Type != null)
                Type.Remove(type);
        }

        // Return whether a tag exists in the Type list
        public bool HasType(string type)
        {
            if (Type != null)
                return Type.Contains(type);
            return false;
        }

    }
}
