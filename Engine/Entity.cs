using System;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    public class Entity
    {
        public Guid guid { get; set; }
        public int id { get; set; }
        public ulong signature { get; set; }

        public ComponentManager componentManager;

        public List<Component> components = new List<Component>(); // dictionary?
        public string state = "idle"; // should this be in a component / messaging / player system?

        public Entity(int id)
        {
            this.id = id;
            GenerateGuid();

            componentManager = EngineGlobals.componentManager;
        }

        public void GenerateGuid()
        {
            guid = Guid.NewGuid();
        }

        public void AddComponent(Component component)
        {
            components.Add(component);
            component.entity = this;

            string componentName = componentManager.GetComponentName(component);
            signature = componentManager.AddToSignature(signature, componentName);
            // Testing
            Console.WriteLine($"\nEntity {id}");
            Console.WriteLine(componentName);
            Console.WriteLine($"Entity signature: {signature}");
            Console.WriteLine(Convert.ToString((long)signature, 2));
        }

        public void RemoveComponent<T>() where T : Component
        {
            Component c = GetComponent<T>();
            components.Remove(c);

            // Needs testing
            string componentName = componentManager.GetComponentName(c);
            signature = componentManager.RemoveFromSignature(signature, componentName);
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
