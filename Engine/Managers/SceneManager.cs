using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    public class SceneManager
    {
        public List<Scene> _sceneStack;
        public Scene ActiveScene { get; private set; }
        public Scene PlayerScene { get; private set; }
        public SceneTransition Transition { get; set; }

        // Stores the player data if a transition is in process
        public Scene _playerNextScene;
        public Entity _playerNextSceneEntity;
        public Vector2 _playerNextScenePosition;


        public SceneManager(int screenWidth = 0, int screenHeight = 0)
        {
            _sceneStack = new List<Scene>();

            if (screenWidth > 0 && screenHeight > 0)
                SetScreenSize(screenWidth, screenHeight);
        }

        public void Input(GameTime gameTime)
        {
            if (ActiveScene != null)
            {
                ActiveScene._Input(gameTime);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (Transition != null)
            {
                Transition.Update(gameTime);
            }

            if (ActiveScene != null)
            {
                ActiveScene._Update(gameTime);

                // Needed??
                foreach (Entity e in ActiveScene.EntitiesToRemove)
                {
                    int index = ActiveScene.EntityList.IndexOf(e);
                    if (index != -1)
                        ActiveScene.EntityList.RemoveAt(index);
                }
                ActiveScene.ClearEntitiesToRemove();

                //foreach (Entity e in ActiveScene.EntityList)
                //    e.PrevState = e.State;

            }

            // CHECK update scene below if stack is > 1 here??
            // Repeat EntitiesToDelete code too??
        }

        public void Draw(GameTime gameTime)
        {
            if (ActiveScene == null)
                return;

            Globals.graphicsDevice.SetRenderTarget(Globals.sceneRenderTarget);
            Globals.graphicsDevice.Clear(Color.Black);

            if (Transition != null)
            {
                Transition._Draw(gameTime);
            }
            else
            {
                ActiveScene._Draw(gameTime);
                // CHECK draw scene below if stack is > 1 here??
            }
        }

        // Creates an instance of a scene and adds it to the scene stack
        private void LoadScene(Scene scene)
        {
            if (!_sceneStack.Contains(scene))
            {
                scene.Init();
                scene._LoadContent();
                _sceneStack.Add(scene);
            }
        }

        // Removes the scene from the scene stack
        private void UnloadScene(Scene scene)
        {
            //Console.WriteLine($"Unload scene {scene} - count {_sceneStack.Count}");
            //Console.WriteLine(string.Join(", ", _sceneStack));

            int index = _sceneStack.IndexOf(scene);
            if (index != -1)
            {
                scene._OnExit();
                scene._UnloadContent();
                _sceneStack.RemoveAt(index);
                // Or add it to a ScenesToRemove list and remove next tick?
                //Console.WriteLine($"Scene unloaded successfully - count {_sceneStack.Count}");
                //Console.WriteLine(string.Join(", ", _sceneStack));
            }
        }

        // Used to change the active scene during the game
        public void SetActiveScene<T>(bool applyTransition = true,
            bool unloadCurrentScene = true) where T : new()
        {
            if (Transition != null)
                return;

            object scene;
            scene = CheckSceneExists<T>();

            if (scene == null)
            {
                // Create a new instance of the given scene and load it
                scene = new T();
                if (scene is Scene)
                    LoadScene((Scene)scene);
            }

            // Return if the scene failed to load
            if (scene == null)
                return;

            // Otherwise change the scene
            if (applyTransition)
                StartTransition((Scene)scene, unloadCurrentScene);
            else
                ChangeScene((Scene)scene, unloadCurrentScene);
        }

        // Checks whether the scene already exists in the scene list
        public Scene CheckSceneExists<T>()
        {
            Console.WriteLine($"Checking if scene {typeof(T)} already exists - count {_sceneStack.Count}");

            Scene scene = null;

            foreach (Scene s in _sceneStack)
            {
                Console.WriteLine($"- Compare scene {s} with {typeof(T)}");
                if (s is T)
                {
                    //Console.WriteLine($"Scene {typeof(T)} already exists at index {_sceneStack.IndexOf(s)}");
                    scene = s;
                    break;
                }
            }
            //Console.WriteLine();

            return scene;
        }

        // Changes the active scene with the option of retaining the current active scene
        public void ChangeScene(Scene nextScene, bool unloadCurrentScene = true)
        {
            // Check if the active scene needs to be unloaded
            if (ActiveScene != null && unloadCurrentScene)
            {
                UnloadScene(ActiveScene);
            }

            // Set the next scene as active
            nextScene._OnEnter();
            ActiveScene = nextScene;

            //Console.WriteLine($"Scene stack size {_sceneStack.Count}");
            //Console.WriteLine($"Active scene {ActiveScene}");
            //Console.WriteLine($"Total number of entities {EngineGlobals.entityManager.GetAllEntities().Count}\n");

            // Used to delay changing the player scene during a transition
            if (_playerNextScene != null)
            {
                ChangePlayerScene(_playerNextScene, _playerNextScenePosition,
                    _playerNextSceneEntity);
            }

        }

        // Removes and unloads a scene from the stack with the option to transition
        public void RemoveScene(Scene scene, bool applyTransition = true)
        {
            if (Transition != null && scene == ActiveScene)
                return;

            if (scene == ActiveScene)
            {
                // Check if there are more scenes on the stack
                if (_sceneStack.Count > 1)
                {
                    Scene nextScene = GetNextScene();
                    if (applyTransition)
                        StartTransition(nextScene, true);
                    else
                        ChangeScene(nextScene, true);
                }
                // Otherwise remove the final scene on the stack
                // Exit game logic here?
                else
                    UnloadScene(scene);
            }
            else
                UnloadScene(scene);
        }

        // Returns the next scene in the stack if there is one, otherwise returns null
        public Scene GetNextScene()
        {
            if (_sceneStack.Count > 1)
                return _sceneStack[^2];
            else
                return null;
        }

        // Returns the scene below if there are any more scenes on the stack.
        // Returns null if no scene below exists.
        public Scene GetSceneBelow(Scene scene = null)
        {
            // Return the next scene in the stack
            if (scene == null)
            {
                return GetNextScene();
            }
            // Return the scene below the given scene if there is one
            else
            {
                int sceneIndex = _sceneStack.IndexOf(scene);
                if (sceneIndex > 0 && sceneIndex < _sceneStack.Count)
                    return _sceneStack[sceneIndex - 1];
                else
                    return null;
            }
        }

        public bool IsSceneStackEmpty()
        {
            return _sceneStack.Count == 0 && Transition == null;
        }

        // Begins a scene transition from the active scene to another scene
        public void StartTransition(Scene nextScene, bool unloadCurrentScene = true)
        {
            if (Transition == null)
                Transition = new FadeSceneTransition(nextScene, unloadCurrentScene);
        }

        // Called once a scene transition has finished
        public void EndTransition()
        {
            Transition = null;
        }

        // Sets the scene that the player belongs to
        public void SetPlayerScene<T>(Vector2 playerPosition = default,
            Entity player = null) where T : Scene
        {
            // If the scene doesn't exist the player won't change scene
            Scene scene = CheckSceneExists<T>();
            if (scene == null)
                return;

            // Store the player data if a transition is in process
            if (Transition != null)
            {
                _playerNextScene = scene;
                _playerNextScenePosition = playerPosition;
                _playerNextSceneEntity = player;
            }
            else
            {
                ChangePlayerScene(scene, playerPosition, player);
            }
        }

        // Add a parameter for displaying the minicamera?
        // Change the player entity and cameras from one scene to another
        public void ChangePlayerScene(Scene nextScene, Vector2 playerPosition = default,
            Entity player = null)
        {
            if (player == null)
                player = EngineGlobals.entityManager.GetLocalPlayer();

            // Remove the player from the current scene
            if (PlayerScene != null)
            {
                PlayerScene.GetCameraByName("main").trackedEntity = null;
                //PlayerScene.GetCameraByName("minimap").trackedEntity = null;
                PlayerScene.RemoveEntity(player);
            }

            // Add the player to the next scene
            TransformComponent transformComponent = player.GetComponent<Engine.TransformComponent>();
            transformComponent.position = playerPosition;
            nextScene.GetCameraByName("main").SetWorldPosition(transformComponent.GetCenter(), instant: true);
            //nextScene.GetCameraByName("minimap").SetWorldPosition(transformComponent.GetCenter(), instant: true);
            nextScene.AddEntity(player);
            nextScene.GetCameraByName("main").trackedEntity = player;
            //nextScene.GetCameraByName("minimap").trackedEntity = player;
            PlayerScene = nextScene;

            // Reset the player next scene fields
            _playerNextScene = null;
            _playerNextScenePosition = Vector2.Zero;
            _playerNextSceneEntity = null;

        }

        // Moves an entity from one scene to another
        public void ChangeEntityScene(Entity entity, Scene currentScene, Scene newScene)
        {
            newScene.AddEntity(entity);
            currentScene.RemoveEntity(entity);
        }

        public void SetScreenSize(int width, int height)
        {
            /*
            if (width < Globals.MinScreenWidth)
                width = Globals.MinScreenWidth;
            if (height < Globals.MinScreenHeight)
                height = Globals.MinScreenHeight;
            */
            Globals.graphics.PreferredBackBufferWidth = width;
            Globals.graphics.PreferredBackBufferHeight = height;
            Globals.graphics.ApplyChanges();

            Globals.ScreenWidth = Globals.graphics.PreferredBackBufferWidth;
            Globals.ScreenHeight = Globals.graphics.PreferredBackBufferHeight;

            // Set the cameras to focus on the player again
            /*
            Entity player = EngineGlobals.entityManager.GetLocalPlayer();
            TransformComponent transformComponent = player.GetComponent<Engine.TransformComponent>();
            ActiveScene.GetCameraByName("main").SetWorldPosition(transformComponent.GetCenter(), instant: true);
            ActiveScene.GetCameraByName("minimap").SetWorldPosition(transformComponent.GetCenter(), instant: true);
            ActiveScene.GetCameraByName("main").trackedEntity = player;
            ActiveScene.GetCameraByName("minimap").trackedEntity = player;
            */
        }
    }

}
