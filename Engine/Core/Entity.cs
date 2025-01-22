/*
 * File: Entity.cs
 * Project: MonoGame ECS Engine
 * (c) 2025, Alex Parry, Mac Bowley and Rik Cross
 * This source is subject to the MIT licence
 */

using System;
using System.Collections.Generic;

namespace Engine
{
    public class Entity
    {

        // TODO - add 'enabled' and other properties? Stored here or entity manager?

        public readonly int Id;
        // TODO - remove GUID?
        public Guid Guid { get; private set; }
        public Flags ComponentFlags = new Flags();
        public Entity Owner { get; set; }
        public Tags Tags { get; set; }
        // state properties
        private string _state;
        public string State {
            get {
                return _state;
            }
            set {
                PreviousState = _state;
                _state = value;
            }
        }
        public string PreviousState { get; private set; }
        public string NextState { get; set; }

        // TODO - are components stored in the entity?
        // TODO - are components stored in a dictionary or equivalent?
        public List<Component> Components { get; set; } = new List<Component>();
        private readonly EntityManager _entityManager;
        private readonly ComponentManager _componentManager;

        public List<TimedAction> TimedActionList = new List<TimedAction>();

        public Entity(int id, string idTag="", string state="default")
        {
            Id = id;
            // TODO - can we remove the GUID?
            Guid = Guid.NewGuid();
            Owner = this;
            Tags = new Tags();
            // TODO - another way to create unique ID? name?
            Tags.Id = idTag;
            State = state;
            _entityManager = EngineGlobals.entityManager;
            _componentManager = EngineGlobals.componentManager;
        }

        // Return if the entity is the local player
        // TODO - can we just use a tag?
        public bool IsLocalPlayer()
        {
            return _entityManager.IsLocalPlayer(this);
        }

        // Return if the entity has a player type Tag
        // TODO: can we just use a tag?
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
            _componentManager.RemoveMultipleComponents(this, instant, componentsToKeep);
        }

        // Reset entity components
        public void ResetComponents()
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

        // TODO - other callbacks? onCreate?
        public virtual void OnDestroy() { }

        // TODO - after time, not no. of frames
        public void After(int frames, Action<Entity> f)
        {
            TimedActionList.Add(new TimedAction(this, frames, f));
        }
    }

}
