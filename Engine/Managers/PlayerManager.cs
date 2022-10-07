using System;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    public class PlayerManager
    {
        private Dictionary<Entity, string> playerByEntity;
        private Dictionary<string, HashSet<Entity>> entitiesByPlayer;

        public PlayerManager()
        {
            playerByEntity = new Dictionary<Entity, string>();
            entitiesByPlayer = new Dictionary<string, HashSet<Entity>>();
        }

        // Set the player that an entity belongs too
        public void SetPlayer(Entity e, string player)
        {
            // Remove the entity if it already belongs to another player
            RemoveFromPlayer(e);

            // Map the player to the entity
            playerByEntity[e] = player;

            // Map the entity to the player
            if (!entitiesByPlayer.ContainsKey(player))
            {
                HashSet<Entity> entities = new HashSet<Entity>();
                entities.Add(e);
                entitiesByPlayer.Add(player, entities);
            }
            else
                entitiesByPlayer[player].Add(e);
        }

        // Remove the entity from a player
        public void RemoveFromPlayer(Entity e)
        {
            if (playerByEntity.ContainsKey(e))
            {
                string player = playerByEntity[e];
                if (!String.IsNullOrEmpty(player))
                    entitiesByPlayer[player].Remove(e);
            }
        }

        // Return the player that an entity belongs to
        public string GetPlayerOfEntity(Entity e)
        {
            return playerByEntity[e];
        }

        // Return all of the entities that belong to a player
        public HashSet<Entity> GetEntitiesOfPlayer(string player)
        {
            if (entitiesByPlayer.ContainsKey(player) && entitiesByPlayer[player].Count > 0)
                return entitiesByPlayer[player];
            else
                return new HashSet<Entity>();
        }

        public bool EntityBelongsToPlayer(Entity e, string player)
        {
            if (playerByEntity.ContainsKey(e) && playerByEntity[e] == player)
                return true;
            return false;
        }
    }

}
