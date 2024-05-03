using System;
using System.Collections.Generic;
using S = System.Diagnostics.Debug;

namespace AdventureGame.Engine
{
    public class Entity
    {
        public int Id { get; set; }
        public Guid Guid { get; private set; }
        public Flags ComponentFlags;

        public Entity Owner { get; set; }
        public Tags Tags { get; set; }
        public string State { get; set; }
        public string PrevState { get; set; }
        public string NextState { get; set; }

        public List<Component> Components { get; set; } // Dictionary/HashSet?
        private readonly EntityManager _entityManager;
        private readonly ComponentManager _componentManager;

        public List<TimedAction> TimedActionList = new List<TimedAction>(); // delete?

        public Entity(int id, string idTag="")
        {
            Id = id;
            GenerateGuid();
            ComponentFlags = new Flags();
            Owner = this;
            Tags = new Tags();
            Tags.Id = idTag;
            State = "default";
            PrevState = State;

            Components = new List<Component>();
            _entityManager = EngineGlobals.entityManager;
            _componentManager = EngineGlobals.componentManager;
        }

        // Generate a unique GUID for the entity
        public void GenerateGuid()
        {
            Guid = Guid.NewGuid();
        }

        // Return if the entity is the local player
        public bool IsLocalPlayer()
        {
            return _entityManager.IsLocalPlayer(this);
        }

        // Return if the entity has a player type Tag
        public bool IsPlayerType()
        {
            return _entityManager.IsPlayerType(this);
        }

        // Add a component to the entity
        public void AddComponent(Component component, bool instant = false)
        {
            _componentManager.AddComponent(this, component, instant);
        }

        // Add and return a component from the entity
        public T AddComponent<T>(Component component, bool instant = false) where T : Component
        {
            _componentManager.AddComponent(this, component, instant);
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
                _componentManager.AddComponent(this, (Component)component, instant);
                return (T)component;
            }
            else
                return default;
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

        // Remove a given component from the entity
        public void RemoveComponent<T>(bool instant = false) where T : Component
        {
            Component component = GetComponent<T>();
            if (component != null)
                _componentManager.RemoveComponent(this, component, instant);
        }

        // Remove all components apart from TransformComponent and any given components
        public void RemoveAllComponents(List<Component> componentsToKeep = null,
            bool instant = false)
        {
            //Console.WriteLine($"Components count {Components.Count}");
            _componentManager.RemoveMultipleComponents(this, instant, componentsToKeep);
            //Console.WriteLine($"Components count {Components.Count}");
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
            _entityManager.DeleteEntity(this);
        }

        public virtual void OnDestroy() { }

        public void After(int frames, Action<Entity> f)
        {
            TimedActionList.Add(new TimedAction(this, frames, f));
        }
    }

}
