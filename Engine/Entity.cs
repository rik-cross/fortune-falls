using System;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    public class Entity
    {
        public Guid guid; // delete?
        public int id;
        public ulong signature;

        public List<Component> components = new List<Component>(); // dictionary?
        public string state = "idle"; // should this be in a component / messaging / player system?

        public ComponentManager componentManager;

        public Entity(int id)
        {
            this.id = id;
            GenerateGuid();

            componentManager = EngineGlobals.componentManager;
        }

        // Generates a unique GUID
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
            componentManager.RemoveComponent(this, component);
        }

        // Fastest way to check if an entity has the components a system requires
        public bool CheckComponents(ulong entitySignature, ulong systemSignature)
        {
            //Console.WriteLine(entitySignature & systemSignature);
            return (entitySignature & systemSignature) == systemSignature;
        }

        // Fastest way to check if an entity has the components a system requires
        public bool CheckComponents(Entity e, ulong systemSignature)
        {
            ulong entitySignature = e.signature;
            return (entitySignature & systemSignature) == systemSignature;
        }

        // Get each component name of the entity?
        // public string[] GetComponentNames (Entity e)


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
