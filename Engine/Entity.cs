using System;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    public class Entity
    {
        public Guid Guid { get; set; }
        public int Id { get; set; }

        //public Game1 game1;
        public EntityManager entityManager;

        public List<Component> components = new List<Component>(); // dictionary?
        public string state = "idle"; // should this be in a component / messaging system?

        public Entity(int id) // parameters for game1 and id?
        {
            // set game1, id, entityManager?
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
        }

        public void RemoveComponent<T>() where T : Component
        {
            Component c = GetComponent<T>();
            components.Remove(c);
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
