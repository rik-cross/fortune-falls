using System;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    public class Entity
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }
        public ulong Signature { get; set; }

        public EntityManager entityManager;
        public ComponentManager componentManager = EngineGlobals.componentManager;

        public List<Component> components = new List<Component>(); // dictionary?
        public string state = "idle"; // should this be in a component / messaging system?

        public Entity(int id)
        {
            this.Id = id;
            GenerateGuid();
        }

        public void GenerateGuid()
        {
            Guid = Guid.NewGuid();
        }

        public void AddComponent(Component component)
        {
            components.Add(component);
            component.entity = this;

            string componentName = componentManager.GetComponentName(component);
            Signature = componentManager.AddToSignature(Signature, componentName);
            Console.WriteLine(componentName);
            Console.WriteLine(Signature);
            Console.WriteLine(Convert.ToString((long)Signature, 2));
        }

        public void RemoveComponent<T>() where T : Component
        {
            Component c = GetComponent<T>();
            components.Remove(c);

            // needs testing
            string componentName = componentManager.GetComponentName(c);
            Signature = componentManager.RemoveFromSignature(Signature, componentName);
        }

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
