using System.Collections.Generic;

namespace AdventureGame.Engine
{
    public class Entity
    {
        public int ID;
        public List<Component> components = new List<Component>();
        public string state = "idle";
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
