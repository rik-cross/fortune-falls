using System;
using System.Collections.Generic;
using System.Text;

namespace AdventureGame.Engine
{
    public class Entity
    {
        public int ID;
        public string state = "idle";
        public List<Component> components = new List<Component>();
        public void AddComponent(Component component)
        {
            components.Add(component);
            component.entity = this;
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
