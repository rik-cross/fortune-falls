using System.Collections.Generic;
using System;

using S = System.Diagnostics.Debug;

namespace AdventureGame.Engine
{
    public class EntityManager
    {
        private List<Entity> entities;
        private Dictionary<int, int> entityMapper;
        private List<Component> components;

        private HashSet<int> disabled;
        private HashSet<int> deleted;

        private List<int> idPool; // change to ConcurrentBag<T>?
        private int nextAvailableId;

        public EntityManager()
        {
            entities = new List<Entity>();
            entityMapper = new Dictionary<int, int>();
            components = new List<Component>();

            disabled = new HashSet<int>();
            deleted = new HashSet<int>();

            idPool = new List<int>();
        }

        // Creates a new entity and gives it an id
        public Entity CreateEntity()
        {
            Entity e = new Entity(CheckOutId());
            AddEntity(e);
            return e;
        }

        // Adds the entity to the list and mapper
        public void AddEntity(Entity e)
        {
            if (e == null)
                return;

            entities.Add(e);
            entityMapper[e.id] = entities.Count - 1;
        }

        // Returns the entity using the entity id
        public Entity GetEntity(int entityId)
        {
            return entities[entityId];
        }

        // Returns the list of entities
        public List<Entity> GetAllEntities()
        {
            return entities;
        }

        // Returns an entity using their Name tag
        public Entity GetEntityByName(string name)
        {
            foreach(Entity e in entities)
            {
                if (e.Tags.Name == name)
                    return e;
            }
            return null;
        }

        // Returns a list of entities based on their Type tag
        public List<Entity> GetAllEntitiesByType(string type)
        {
            List<Entity> entitiesByType = new List<Entity>();
            foreach (Entity e in entities)
            {
                if (e.Tags.HasTag(type))
                    entitiesByType.Add(e);
            }
            return entitiesByType;
        }

        // Removes the entity from the disabled set
        public void EnableEntity(Entity e)
        {
            disabled.Remove(e.id);
        }

        // Adds the entity to the disabled set
        public void DisableEntity(Entity e)
        {
            disabled.Add(e.id);
        }

        // Adds the entity to the deleted set
        public void DeleteEntity(Entity e)
        {
            deleted.Add(e.id);
        }

        // Returns if the entity is active
        public bool IsActive(int entityId)
        {
            return entities[entityId] != null;
        }

        // Returns if the entity is enabled
        public bool IsEnabled(int entityId)
        {
            return !disabled.Contains(entityId);
        }

        // Deletes entities from the deleted set at the start of the game tick
        public void DeleteEntitiesFromSet()
        {
            foreach (int entityId in deleted)
            {
                // Get the entity from the id
                Entity e = GetEntity(entityId);

                // Remove the entity's components
                EngineGlobals.componentManager.RemoveAllComponents(e);

                // Testing
                Console.WriteLine($"Deleting entity {entityId}");
                Console.WriteLine($"Entity {entityId} has signature {e.signature}");

                // Replace the deleted entity with the last entity in the list
                // and update the mapper
                if (entityMapper.ContainsKey(e.id))
                {
                    // Get the index of the current entity
                    int index = entityMapper[e.id];

                    // Get the last entity at the end of the list
                    Entity lastEntity = entities[^1];

                    // Replace the current entity with the last entity
                    entities[index] = lastEntity;

                    // Update the mapper with the new index value
                    entityMapper[lastEntity.id] = index;

                    // Remove the last entity from the list
                    entities.RemoveAt(entities.Count - 1);

                    // Remove the current entity from the mapper
                    entityMapper.Remove(e.id);
                }

                // Allow the entity id to be reused
                CheckInId(entityId);

                // Testing
                /*
                Console.WriteLine("Delete entity:");
                Console.WriteLine(String.Join(", ", entities));
                foreach (KeyValuePair<int, int> kv in entityMapper)
                    Console.WriteLine($"Key:{kv.Key} Value:{kv.Value}");
                */
            }

            // Clear the deleted set
            deleted.Clear();
        }

        // Handles creating and reusing entity ids
        public int CheckOutId()
        {
            int count = idPool.Count;
            if (count > 0)
            {
                int lastId = idPool[count - 1];
                idPool.RemoveAt(count - 1);
                return lastId;
            }
            return nextAvailableId++;
        }

        // Adds an entity id to be reused
        public void CheckInId(int id)
        {
            idPool.Add(id);
        }
    }

}
