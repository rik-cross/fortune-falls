using System;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    public class Entity
    {
        public int Id { get; set; }
        public Guid Guid { get; private set; }
        public ulong Signature { get; set; }
        //public Scene Scene { get; set; }
        public string State { get; set; }
        public Tags Tags { get; set; }

        public List<Component> Components { get; set; } // Dictionary/HashSet?
        private readonly EntityManager entityManager;
        private readonly ComponentManager componentManager;

        public Entity(int id)
        {
            Id = id;
            GenerateGuid();
            State = "idle";
            Tags = new Tags();

            Components = new List<Component>();
            entityManager = EngineGlobals.entityManager;
            componentManager = EngineGlobals.componentManager;
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
        public void AddComponent(Component component)
        {
            componentManager.AddComponent(this, component);
        }

        // Remove a given component from the entity
        public void RemoveComponent<T>() where T : Component
        {
            Component component = GetComponent<T>();
            if (component != null)
                componentManager.RemoveComponent(this, component);
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

        // Destroy the entity
        public void Destroy()
        {
            OnDestroy();
            entityManager.DeleteEntity(this);
        }

        public virtual void OnDestroy() { }
    }

}
