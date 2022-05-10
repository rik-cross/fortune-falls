using System;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    public class Entity
    {
        public Guid guid; // delete?
        public int id;
        public ulong signature;

        public ComponentManager componentManager;
        private SystemManager systemManager;

        public List<Component> components = new List<Component>(); // dictionary?
        public string state = "idle"; // should this be in a component / messaging / player system?

        public Entity(int id)
        {
            this.id = id;
            GenerateGuid();

            componentManager = EngineGlobals.componentManager;
            systemManager = EngineGlobals.systemManager;
        }

        // Generates a unique GUID
        public void GenerateGuid()
        {
            guid = Guid.NewGuid();
        }

        // Adds a component to the entity and updates the systems
        // Warning: does not check that the component exists
        public void AddComponent(Component component)
        {
            // Add component object to the list and entity to the component
            components.Add(component);
            component.entity = this;

            // Add component to signature
            string componentName = componentManager.GetComponentName(component);
            signature = componentManager.AddToSignature(signature, componentName);

            // Update all the system's lists of entities
            systemManager.ComponentAdded(this);

            // Testing
            Console.WriteLine($"\nEntity {id} added component {componentName}");
            Console.WriteLine($"Entity signature: {signature}");
            Console.WriteLine(Convert.ToString((long)signature, 2));
        }

        // Removes a component from the entity and updates the systems
        public void RemoveComponent<T>() where T : Component
        {
            Component c = GetComponent<T>();
            components.Remove(c);

            // Remove component from signature
            string componentName = componentManager.GetComponentName(c);
            signature = componentManager.RemoveFromSignature(signature, componentName);

            // Update all the system's lists of entities
            systemManager.ComponentRemoved(this);

            // Testing
            Console.WriteLine($"\nEntity {id} removed component {componentName}");
            Console.WriteLine($"Entity signature: {signature}");
            Console.WriteLine(Convert.ToString((long)signature, 2));
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
