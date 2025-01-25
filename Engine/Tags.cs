/*
 *  File: Tags.cs
 *  Project: MonoGame ECS Engine
 *  (c) 2025, Alex Parry, Mac Bowley and Rik Cross
 *  This source is subject to the MIT licence
 */

using System.Collections.Generic;

namespace Engine
{
    public class Tags
    {

        public List<string> TagList {get; private set;} = new List<string>();

        // contructor, passing in 0 or more tags
        public Tags(params string[] tags)
        {
            AddTags(tags);
        }

        // constructor, passing in a list of tags
        public Tags(List<string> tags)
        {
            AddTags(tags);
        }

        // clear all tags
        public void ClearTags()
        {
            TagList.Clear();
        }

        // add 0 or more tags to the tag list
        public void AddTags(params string[] tags)
        {
            foreach (string t in tags)
                if (t != null)
                    TagList.Add(t.ToLower());
        }

        // add a list of tags to the tag list
        public void AddTags(List<string> tags)
        {
            if (tags != null)
                AddTags(tags.ToArray());
        }

        // remove 0 or more tags from the tag list
        public void RemoveTags(params string[] tags)
        {
            foreach (string t in tags) {
                TagList.Remove(t);
            }
        }
        
        public void RemoveTags(List<string> tags)
        {
            RemoveTags(tags.ToArray());
        }

        // return whether 0 or more tags exist in the tag list
        public bool HasTags(params string[] tags)
        {
            foreach (string t in tags) {
                if (!TagList.Contains(t.ToLower())) {
                    return false;
                }
            }
            return true;
        }

        public bool HasTags(List<string> tags) {
            return HasTags(tags.ToArray());
        }

    }
}
