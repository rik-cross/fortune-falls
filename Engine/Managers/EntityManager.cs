using System;
using System.Collections.Generic;
using S = System.Diagnostics.Debug;

namespace AdventureGame.Engine
{
    public class EntityManager
    {
        private List<Entity> _entityList;
        private Dictionary<int, int> _entityMapper;
        private List<Component> _components;
        private Entity _localPlayer;

        public HashSet<Entity> Added { get; private set; }
        public HashSet<Entity> Disabled { get; private set; } // Change to Entity?
        public HashSet<Entity> Deleted { get; private set; }

        private List<int> _idPool; // change to ConcurrentBag<T>?
        private int _nextAvailableId;

        public EntityManager()
        {
            _entityList = new List<Entity>();
            _entityMapper = new Dictionary<int, int>();
            _components = new List<Component>();

            Added = new HashSet<Entity>();
            Disabled = new HashSet<Entity>();
            Deleted = new HashSet<Entity>();

            _idPool = new List<int>();
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

            //scene.AddEntity(e);

            _entityList.Add(e);
            _entityMapper[e.Id] = _entityList.Count - 1;
        }

        // Add entities from the added set at the start of the game tick
        public void AddEntitiesToGame()
        {
            //foreach (Entity e in Added)
            //    AddEntity(e);

            Added.Clear();
        }

        // Return the entity using the entity id
        public Entity GetEntity(int entityId)
        {
            if (_entityMapper.TryGetValue(entityId, out int indexValue))
                return _entityList[indexValue];
            else
                return null;

            //int index = entityMapper[entityId];
            //return entities[index];
        }

        // Return the list of entities
        public List<Entity> GetAllEntities()
        {
            return _entityList;
        }

        // Return an entity using their id Tag
        public Entity GetEntityByIdTag(string id)
        {
            foreach(Entity e in _entityList)
                if (e.Tags.Id == id)
                    return e;
            return null;
        }

        public int CountEntitiesByTag(string id)
        {
            int r = 0;
            foreach (Entity e in _entityList)
                if (e.Tags.Id == id)
                    r+=1;
            return r;
        }

        // Set the local player entity
        public void SetLocalPlayer(Entity e)
        {
            _localPlayer = e;
        }

        // Return the local player entity
        public Entity GetLocalPlayer()
        {
            return _localPlayer;
        }

        // Return if the entity is the local player
        public bool IsLocalPlayer(Entity e)
        {
            return e.Tags.Id == "localPlayer";
        }

        // Return if the entity has a player type Tag
        public bool IsPlayerType(Entity e)
        {
            return e.Tags.HasType("player");
        }

        // Return a list of entities based on their type Tag
        public List<Entity> GetAllEntitiesByType(string type)
        {
            List<Entity> entitiesByType = new List<Entity>();
            foreach (Entity e in _entityList)
            {
                if (e.Tags.HasType(type))
                    entitiesByType.Add(e);
            }
            return entitiesByType;
        }

        // Return a list of entities that contain a given component
        public List<Entity> GetAllEntitiesByComponent(string componentName)
        {
            Flags componentId = EngineGlobals.componentManager.GetComponentFlag(componentName);
            List<Entity> entitiesByComponent = new List<Entity>();
            foreach (Entity e in _entityList)
            {
                if (e.ComponentFlags.HasFlags(componentId))
                    entitiesByComponent.Add(e);
            }
            return entitiesByComponent;
        }

        // Remove the entity from the disabled set
        public void EnableEntity(Entity e)
        {
            Disabled.Remove(e);
        }

        // Add the entity to the disabled set
        public void DisableEntity(Entity e)
        {
            Disabled.Add(e);
        }

        // Add the entity to the deleted set
        public void DeleteEntity(Entity e)
        {
            Deleted.Add(e);
        }

        // Delete entities from the deleted set at the start of the game tick
        public void DeleteEntitiesFromGame()
        {
            foreach (Entity e in Deleted)
            {
                e.OnDestroy();
                Added.Remove(e);
                Disabled.Remove(e);

                // Remove the entity's components
                EngineGlobals.componentManager.RemoveAllComponents(e);

                // To keep the index values accurate in the mapper
                // and for fast removal of an entity from the list,
                // overwrite the current entity with the last entity
                // in the list and update the mapper.

                int entityId = e.Id;

                // Testing
                //Console.WriteLine($"Deleting entity {entityId}");
                //Console.WriteLine($"Entity {entityId} has signature {e.Signature}");

                if (_entityMapper.ContainsKey(entityId))
                {
                    // Get the index of the current entity
                    int index = _entityMapper[entityId];

                    // Get the last entity at the end of the list
                    Entity lastEntity = _entityList[^1];

                    // Replace the current entity with the last entity
                    _entityList[index] = lastEntity;

                    // Update the mapper with the new index value
                    _entityMapper[lastEntity.Id] = index;

                    // Remove the last entity from the list
                    _entityList.RemoveAt(_entityList.Count - 1);

                    // Remove the current entity from the mapper
                    _entityMapper.Remove(entityId);

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
            Deleted.Clear();
        }

        // Return if the entity is active
        public bool IsActive(Entity e)
        {
            return _entityList[e.Id] != null;
        }

        // Return if the entity is enabled
        public bool IsEnabled(Entity e)
        {
            return !Disabled.Contains(e);
        }

        // Handles creating and reusing entity ids
        public int CheckOutId()
        {
            int count = _idPool.Count;
            if (count > 0)
            {
                int lastId = _idPool[count - 1];
                _idPool.RemoveAt(count - 1);
                //Console.WriteLine($"Last id {lastId}");
                return lastId;
            }
            //Console.WriteLine($"Next id {nextAvailableId }");
            return _nextAvailableId++;
        }

        // Add an entity id to the id pool be reused
        public void CheckInId(int id)
        {
            _idPool.Add(id);
        }
    }

}
