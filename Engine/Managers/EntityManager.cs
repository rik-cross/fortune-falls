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

        // Create a new entity and give it an id
        public Entity CreateEntity()
        {
            Entity e = new Entity(CheckOutId());
            AddEntity(e);
            return e;
        }

        // Add the entity to the list and mapper
        public void AddEntity(Entity e)
        {
            if (e == null)
                return;

            entities.Add(e);
            entityMapper[e.Id] = entities.Count - 1;
        }

        // Return the entity using the entity id
        public Entity GetEntity(int entityId)
        {
            int index = entityMapper[entityId];
            return entities[index];
        }

        // Return the list of entities
        public List<Entity> GetAllEntities()
        {
            return entities;
        }

        // Return an entity using their id Tag
        public Entity GetEntityById(string id)
        {
            foreach(Entity e in entities)
            {
                if (e.Tags.Id == id)
                    return e;
            }
            return null;
        }

        // Return the local player entity
        public Entity GetLocalPlayer()
        {
            // Use PlayerManager instead??
            foreach (Entity e in entities)
            {
                if (e.Tags.Id == "localPlayer")
                    return e;
            }
            return null;
        }

        // Return if the entity is the local player
        public bool IsLocalPlayer(Entity e)
        {
            return e.Tags.Id == "localPlayer";
        }

        // Return a list of entities based on their type Tag
        public List<Entity> GetAllEntitiesByType(string type)
        {
            List<Entity> entitiesByType = new List<Entity>();
            foreach (Entity e in entities)
            {
                if (e.Tags.HasType(type))
                    entitiesByType.Add(e);
            }
            return entitiesByType;
        }

        // Return if the entity has a player type Tag
        public bool IsPlayerType(Entity e)
        {
            return e.Tags.HasType("player");
        }

        // Remove the entity from the disabled set
        public void EnableEntity(Entity e)
        {
            disabled.Remove(e.Id);
        }

        // Add the entity to the disabled set
        public void DisableEntity(Entity e)
        {
            disabled.Add(e.Id);
        }

        // Add the entity to the deleted set
        public void DestroyEntity(Entity e)
        {
            deleted.Add(e.Id);
        }

        // Return if the entity is active
        public bool IsActive(int entityId)
        {
            return entities[entityId] != null;
        }

        // Return if the entity is enabled
        public bool IsEnabled(int entityId)
        {
            return !disabled.Contains(entityId);
        }

        // Delete entities from the deleted set at the start of the game tick
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
                Console.WriteLine($"Entity {entityId} has signature {e.Signature}");

                // To keep the index values accurate in the mapper
                // and for fast removal of an entity from the list,
                // overwrite the current entity with the last entity
                // in the list and update the mapper.
                if (entityMapper.ContainsKey(entityId))
                {
                    // Get the index of the current entity
                    int index = entityMapper[entityId];

                    // Get the last entity at the end of the list
                    Entity lastEntity = entities[^1];

                    // Replace the current entity with the last entity
                    entities[index] = lastEntity;

                    // Update the mapper with the new index value
                    entityMapper[lastEntity.Id] = index;

                    // Remove the last entity from the list
                    entities.RemoveAt(entities.Count - 1);

                    // Remove the current entity from the mapper
                    entityMapper.Remove(entityId);

                    // Allow the entity id to be reused
                    CheckInId(entityId);
                }

                // Testing
                /*
                Console.WriteLine("Delete entity:");
                //Console.WriteLine(String.Join(", ", entities));
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

        // Add an entity id to the id pool be reused
        public void CheckInId(int id)
        {
            idPool.Add(id);
        }
    }

}
