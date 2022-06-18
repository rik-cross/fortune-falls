using System;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    public class Entity
    {
        public Guid guid;
        public int id;
        public ulong signature;

        public List<Component> components = new List<Component>(); // dictionary?
        public string state = "idle"; // should this be in a component / messaging / player system?

        public List<string> tags = new List<string>();

        public ComponentManager componentManager;

        public Entity(int id)
        {
            this.id = id;
            GenerateGuid();

            componentManager = EngineGlobals.componentManager;
        }

        public void AddTag(string tag)
        {
            if (tags.Contains(tag) == false)
                tags.Add(tag);
        }

        public bool HasTag(string tag)
        {
            return tags.Contains(tag);
        }

        // Generates a unique GUID for the entity
        public void GenerateGuid()
        {
            guid = Guid.NewGuid();
        }

        // Adds a component to the entity
        public void AddComponent(Component component)
        {
            componentManager.AddComponent(this, component);
        }

        // Removes a component from the entity
        public void RemoveComponent<T>() where T : Component
        {
            Component component = GetComponent<T>();
            if (component != null)
                componentManager.RemoveComponent(this, component);
        }

        // Returns each component object of the entity
        public T GetComponent<T>() where T : Component
        {
            foreach (Component c in components)
            {
                if (c.GetType().Equals(typeof(T)))
                {
                    return (T)c;
                }
            }
            return null;
        }
    }

}
