using System;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    public class Entity
    {
        public Guid guid;
        public int id;
        public ulong signature;
        public string state = "idle"; // Should this move to Sprite / SpritesComponent??

        public List<Component> components; // Dictionary/HashSet?
        public Tags Tags { get; set; }

        public ComponentManager componentManager;

        public Entity(int id)
        {
            this.id = id;
            GenerateGuid();

            components = new List<Component>();
            Tags = new Tags();

            componentManager = EngineGlobals.componentManager;
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

        // Removes a given component from the entity
        public void RemoveComponent<T>() where T : Component
        {
            Component component = GetComponent<T>();
            if (component != null)
                componentManager.RemoveComponent(this, component);
        }

        // Returns a given component from the entity
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
