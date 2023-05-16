using System;
using System.Collections.Generic;
using S = System.Diagnostics.Debug;

namespace AdventureGame.Engine
{
    public class Entity
    {
        public int Id { get; set; }
        public Guid Guid { get; private set; }
        public ulong Signature { get; set; }
        //public Scene Scene { get; set; }
        public string State { get; set; }
        public string PrevState { get; set; }
        public Tags Tags { get; set; }

        public List<Component> Components { get; set; } // Dictionary/HashSet?
        private readonly EntityManager entityManager;
        private readonly ComponentManager componentManager;

        public List<TimedAction> timedActionList = new List<TimedAction>();

        public Entity(int id)
        {
            Id = id;
            GenerateGuid();
            State = "idle";
            Tags = new Tags();

            Components = new List<Component>();
            entityManager = EngineGlobals.entityManager;
            componentManager = EngineGlobals.componentManager;

            PrevState = State;
        }

        // Generate a unique GUID for the entity
        public void GenerateGuid()
        {
            Guid = Guid.NewGuid();
        }

        // Return if the entity is the local player
        public bool IsLocalPlayer()
        {
            return entityManager.IsLocalPlayer(this);
        }

        // Return if the entity has a player type Tag
        public bool IsPlayerType()
        {
            return entityManager.IsPlayerType(this);
        }

        // Add a component to the entity
        public void AddComponent(Component component, bool instant = false)
        {
            componentManager.AddComponent(this, component, instant);
        }

        // Add and return a component from the entity
        public T AddComponent<T>(Component component, bool instant = false) where T : Component
        {
            componentManager.AddComponent(this, component, instant);
            return (T)component;
        }

        // Add a component with an empty constructor using reflection
        public T AddComponent<T>(bool instant = false) where T : new()
        {
            object component;

            // Create a new instance of the given component
            component = new T();
            if (component is Component)
            {
                componentManager.AddComponent(this, (Component)component, instant);
                return (T)component;
            }
            else
                return default;
        }

        // Remove a given component from the entity
        public void RemoveComponent<T>(bool instant = false) where T : Component
        {
            Component component = GetComponent<T>();
            if (component != null)
                componentManager.RemoveComponent(this, component, instant);
        }

        // Return a given component from the entity
        public T GetComponent<T>() where T : Component
        {
            foreach (Component c in Components)
            {
                if (c.GetType().Equals(typeof(T)))
                {
                    return (T)c;
                }
            }
            return null;
        }

        // Reset entity components
        public void Reset()
        {
            foreach(Component c in Components)
            {
                c.Reset();
            }
        }

        // Destroy the entity
        public void Destroy()
        {
            OnDestroy();
            entityManager.DeleteEntity(this);
        }

        public virtual void OnDestroy() { }

        public void After(int frames, Action<Entity> f)
        {
            timedActionList.Add(new TimedAction(this, frames, f));
        }
    }

}
