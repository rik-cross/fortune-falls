using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    public class PlayerManager
    {
        // Stores the player data if a scene transition is in process
        private bool _isSceneChange;
        private Scene _playerNextScene;
        public Vector2 _playerNextScenePosition;

        private Dictionary<Entity, string> _playerByEntity;
        private Dictionary<string, HashSet<Entity>> _entitiesByPlayer;

        public PlayerManager()
        {
            _isSceneChange = false;
            _playerByEntity = new Dictionary<Entity, string>();
            _entitiesByPlayer = new Dictionary<string, HashSet<Entity>>();
        }

        public void Update(GameTime gameTime)
        {
            if (_isSceneChange)
            {
                if (EngineGlobals.sceneManager.Transition != null &&
                    EngineGlobals.sceneManager.Transition.HasSceneChanged)
                {
                    if (_playerNextScene == null)
                        _playerNextScene = EngineGlobals.sceneManager.ActiveScene;

                    ChangeScene();
                }
            }
        }

        public void ClearPlayerSceneProperties()
        {
            _isSceneChange = false;
            _playerNextScene = null;
            _playerNextScenePosition = Vector2.Zero;
        }

        // Attempt to change the player scene unless a transition is in progress.
        public void ChangePlayerScene(Vector2 playerPosition = default, Scene nextScene = null)
        {
            Console.WriteLine($"Player scene attempting to change");
            if (EngineGlobals.sceneManager.Transition != null)
            {
                _playerNextScenePosition = playerPosition;
                _playerNextScene = nextScene;
                _isSceneChange = true;
            }
            else
                ChangeScene();
        }

        // Change the player entity and cameras from one scene to another.
        private void ChangeScene()
        {
            Console.WriteLine($"Player scene change to {_playerNextScene}");
            Entity player = EngineGlobals.entityManager.GetLocalPlayer();

            SceneComponent sceneComponent = player.GetComponent<SceneComponent>();
            if (sceneComponent == null)
                return;

            Scene playerScene;
            if (player != null)
                playerScene = sceneComponent.Scene;
            else
                return;

            // Remove the player from the current scene
            if (playerScene != null)
            {
                playerScene.GetCameraByName("main").trackedEntity = null;
                //PlayerScene.GetCameraByName("minimap").trackedEntity = null;
                playerScene.RemoveEntity(player);
            }

            // Add the player to the next scene
            TransformComponent transformComponent = player.GetComponent<Engine.TransformComponent>();
            transformComponent.Position = _playerNextScenePosition;

            _playerNextScene.GetCameraByName("main").SetWorldPosition(transformComponent.GetCenter(), instant: true);
            //_playerNextScene.GetCameraByName("minimap").SetWorldPosition(transformComponent.GetCenter(), instant: true);
            _playerNextScene.AddEntity(player);
            _playerNextScene.GetCameraByName("main").trackedEntity = player;
            //_playerNextScene.GetCameraByName("minimap").trackedEntity = player;
            player.GetComponent<SceneComponent>().Scene = _playerNextScene;

            ClearPlayerSceneProperties();
        }

        // Set the player that an entity belongs too
        public void SetPlayer(Entity e, string player)
        {
            // Remove the entity if it already belongs to another player
            RemoveFromPlayer(e);

            // Map the player to the entity
            _playerByEntity[e] = player;

            // Map the entity to the player
            if (!_entitiesByPlayer.ContainsKey(player))
            {
                HashSet<Entity> entities = new HashSet<Entity>();
                entities.Add(e);
                _entitiesByPlayer.Add(player, entities);
            }
            else
                _entitiesByPlayer[player].Add(e);
        }

        // Remove the entity from a player
        public void RemoveFromPlayer(Entity e)
        {
            if (_playerByEntity.ContainsKey(e))
            {
                string player = _playerByEntity[e];
                if (!String.IsNullOrEmpty(player))
                    _entitiesByPlayer[player].Remove(e);
            }
        }

        // Return the player that an entity belongs to
        public string GetPlayerOfEntity(Entity e)
        {
            return _playerByEntity[e];
        }

        // Return all of the entities that belong to a player
        public HashSet<Entity> GetEntitiesOfPlayer(string player)
        {
            if (_entitiesByPlayer.ContainsKey(player) && _entitiesByPlayer[player].Count > 0)
                return _entitiesByPlayer[player];
            else
                return new HashSet<Entity>();
        }

        public bool EntityBelongsToPlayer(Entity e, string player)
        {
            if (_playerByEntity.ContainsKey(e) && _playerByEntity[e] == player)
                return true;
            return false;
        }
    }

}
