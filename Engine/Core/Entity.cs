/*
 *  File: Entity.cs
 *  Project: MonoGame ECS Engine
 *  (c) 2025, Alex Parry, Mac Bowley and Rik Cross
 *  This source is subject to the MIT licence
 */

using System;
using System.Collections.Generic;

namespace Engine
{
    public class Entity
    {

        //
        // attributes
        //

        // unique ID
        public readonly int Id;

        // stored component information
        public List<Component> Components { get; set; } = new List<Component>();
        public Flags ComponentFlags = new Flags();

        // links to global managers
        private readonly EntityManager _entityManager;
        private readonly ComponentManager _componentManager;
        
        // unique name
        private string _name;
        public string Name {
            get {
                return _name;
            }
            set {
                if (value == null)
                    return;
                foreach(Entity e in _entityManager.GetAllEntities()) {
                    if (e.Name == value) {
                        return;
                    }
                }
                _name = value;
            } 
        }

        // systems only run on enabled entities
        public bool Enabled {
            get {
                return !_entityManager.Disabled.Contains(this);
            }
            set {
                if (value == true)
                    _entityManager.EnableEntity(this);
                else
                    _entityManager.DisableEntity(this);
            }
        }
        
        public Entity Owner { get; set; }
        public Tags Tags { get; set; }

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
        public bool HasStateChanged() {
            return !(PreviousState == State);
        }
        
        public List<TimedAction> TimedActionList = new List<TimedAction>();

        //
        // methods
        //

        public Entity(string name=null, string state="default", List<string> tags=default, Entity owner=null)
        {

            // link entity to global managers
            _entityManager = EngineGlobals.entityManager;
            _componentManager = EngineGlobals.componentManager;

            // generate new ID from the ID pool
            Id = _entityManager.CheckOutId();
            Name = name;
            State = state;

            // set the owner, default = this
            if (Owner == null)
                Owner = this;
            else
                Owner = owner;

            // add tags
            Tags = new Tags(tags: tags);

            _entityManager.AddEntity(this);
            
        }

        // Add a single component to the entity
        public void AddComponent(Component component, bool instant = false)
        {
            _componentManager.AddComponent(this, component, instant);
        }

        // Add one or more components to the entity
        public void AddComponent(params Component[] components)
        {
            foreach (Component c in components) {
                AddComponent(c);
            }
        }

        // Create, add and return a component from the entity
        public T AddComponent<T>(Component component, bool instant = false) where T : Component
        {
            _componentManager.AddComponent(this, component, instant);
            return (T)component;
        }

        // Create, add and return a component with an empty constructor, using reflection
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

        // Return the specified component from the entity
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

        // checks whether an entity has a component of the specified type
        public bool HasComponent<T>()  {
            foreach (Component c in Components) {
                if (c.GetType().Equals(typeof(T))) {
                    return true;
                }
            }
            return false;
        }

        // remove the component of a specified type from the entity (if it exists)
        public void RemoveComponent<T>(bool instant = false) where T : Component
        {
            Component component = GetComponent<T>();
            if (component != null)
                _componentManager.RemoveComponent(this, component, instant);
        }

        // remove all components (except those specified) from the entity
        public void RemoveAllComponents(List<Component> componentsToKeep = null,
            bool instant = false)
        {
            _componentManager.RemoveMultipleComponents(this, instant, componentsToKeep);
        }

        // reset all entity components
        public void ResetComponents()
        {
            foreach(Component c in Components)
            {
                // defer to each component Reset() method
                c.Reset();
            }
        }

        // delete the entity
        public void Destroy()
        {
            _entityManager.DeleteEntity(this);
        }

        // perform the specified action after an amount of time
        // TODO - this should be after time, not number of frames
        public void AddTimedAction(int frames, Action<Entity> f)
        {
            TimedActionList.Add(new TimedAction(this, frames, f));
        }

    }

}
