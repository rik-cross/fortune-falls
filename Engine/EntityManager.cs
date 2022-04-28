using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace AdventureGame.Engine
{
    public class EntityManager
    {
        private Dictionary<int, Entity> entities;
        private Dictionary<int, Entity> disabled;

        private List<int> idPool; // change to ConcurrentBag<T>?
        private int nextAvailableId;

        public EntityManager() // change to private internal class IdentifierPool?
        {
            entities = new Dictionary<int, Entity>();
            disabled = new Dictionary<int, Entity>();
            idPool = new List<int>();
        }

        public Entity CreateEntity() // protected?
        {
            //Entity e = new Entity();
            //Entity e = new Entity(game1, CheckOutId());
            Entity e = new Entity(CheckOutId());
            return e;
        }

        public void AddEntity(Entity e)
        {
            entities.Add(e.Id, e);
        }

        public void EnableEntity(Entity e)
        {
            disabled.Remove(e.Id);
        }

        public void DisableEntity(Entity e)
        {
            disabled.Add(e.Id, e);
        }

        public void DeleteEntity(Entity e)
        {
            entities[e.Id] = null;
            disabled.Remove(e.Id);
            CheckInId(e.Id);
        }

        public bool IsActive(int entityId)
        {
            return entities[entityId] != null;
        }

        public bool IsEnabled(int entityId)
        {
            return !disabled.ContainsKey(entityId);
        }

        public Entity GetEntity(int entityId) // protected?
        {
            return entities[entityId];
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

        // Add an entity id to be reused
        public void CheckInId(int id)
        {
            idPool.Add(id);
        }
    }

}
