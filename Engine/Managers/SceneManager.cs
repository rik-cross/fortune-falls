using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    public class SceneManager
    {
        public List<Scene> SceneList { get; private set; }
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
            SceneList = new List<Scene>();
            _sceneStack = new List<Scene>();

            if (screenWidth > 0 && screenHeight > 0)
                SetScreenSize(screenWidth, screenHeight);
        }

        public void Update(GameTime gameTime)
        {
            if (Transition != null)
            {
                Transition.Update(gameTime);
            }
            else
            {
                if (ActiveScene != null)
                {
                    ActiveScene._Update(gameTime);

                    // Needed??
                    foreach (Entity e in ActiveScene.EntitiesToDelete)
                    {
                        ActiveScene.EntityList.Remove(e);
                    }
                    ActiveScene.ClearEntitiesToDelete();
                }

                // CHECK update scene below if stack is > 1 here??
                // Repeat EntitiesToDelete code too??
            }
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

        // Creates an instance of a scene and adds it to the scene list
        public void LoadScene(Scene scene)
        {
            if (!SceneList.Contains(scene))
            {
                scene.Init();
                scene._LoadContent();
                SceneList.Add(scene);
            }
        }

        // Removes the scene from the scene list and stack
        public void UnloadScene(Scene scene)
        {
            if (SceneList.Contains(scene))
            {
                scene._UnloadContent();
                SceneList.Remove(scene);
                // Or add it to a ScenesToRemove list and remove next tick?

                if (_sceneStack.Contains(scene))
                    _sceneStack.Remove(scene); // Likewise ^^
            }
        }

        // Adds a scene to the top of the scene stack
        public void PushSceneToStack(Scene scene)
        {
            _sceneStack.Add(scene);
        }

        // Removes the scene at the top of the scene stack
        public Scene PopSceneFromStack()
        {
            Scene scene = null;

            if (_sceneStack.Count > 0)
            {
                scene = _sceneStack[^1];
                _sceneStack.RemoveAt(_sceneStack.Count - 1);
            }

            return scene;
        }

        // Returns the scene at the top of the scene stack
        public Scene PeekSceneOnStack()
        {
            Scene scene = null;

            if (_sceneStack.Count > 0)
            {
                scene = _sceneStack[^1];
            }

            return scene;
        }

        // Removes a non-active scene with the option to transition and unload
        public void RemoveScene(Scene scene, bool applyTransition = false,
            bool unloadScene = true)
        {
            if (Transition != null && scene == ActiveScene)
                return;

            if (scene == ActiveScene)
            {
                // Check if there are more scenes on the stack
                if (_sceneStack.Count > 1)
                {
                    PopSceneFromStack();
                    Scene nextScene = PeekSceneOnStack();

                    if (applyTransition)
                        StartTransition(nextScene, true, unloadScene);
                    else
                        ChangeScene(nextScene, true, unloadScene);
                }
                // Otherwise remove the final scene on the stack
                else
                {
                    // Exit game logic here?


                    ChangeScene(null, true, true);
                }
            }
            else
            {
                scene._OnExit(); // Should it be called if not the ActiveScene?
                UnloadScene(scene);
            }
        }

        // Checks whether the scene already exists in the scene list
        public Scene CheckSceneExists<T>()
        {
            Console.WriteLine($"Checking if scene {typeof(T)} already exists");
            Console.WriteLine($"Scenes count {SceneList.Count}");

            Scene scene = null;

            foreach (var s in SceneList)
            {
                Console.WriteLine($"Compare scene {s} with {typeof(T)}");
                if (s is T)
                {
                    Console.WriteLine($"Scene {typeof(T)} already exists at index {SceneList.IndexOf(s)}");
                    scene = s;
                }
            }
            Console.WriteLine();

            return scene;
        }

        // Used to change the active scene during the game
        public void SetActiveScene<T>(bool applyTransition = true,
            bool removeCurrentSceneFromStack = true,
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
                StartTransition((Scene)scene, removeCurrentSceneFromStack, unloadCurrentScene);
            else
                ChangeScene((Scene)scene, removeCurrentSceneFromStack, unloadCurrentScene);
        }

        // Begins a scene transition from the active scene to another scene
        public void StartTransition(Scene nextScene, bool removeCurrentSceneFromStack = true,
            bool unloadCurrentScene = true)
        {
            if (Transition == null)
            {
                Transition = new FadeSceneTransition(nextScene, removeCurrentSceneFromStack,
                    unloadCurrentScene);
            }
        }

        // Called once a scene transition has finished
        public void EndTransition()
        {
            Transition = null;
        }

        // Changes the active scene with the option of retaining the current active scene
        public void ChangeScene(Scene nextScene, bool removeCurrentSceneFromStack = true,
            bool unloadCurrentScene = true)
        {
            // Handle a null next scene
            if (nextScene == null)
            {
                // Check if there are any more scenes on the scene stack
                if (_sceneStack.Count > 1)
                {
                    // Get the second to last scene
                    nextScene = _sceneStack[^2];
                }
                // Otherwise there are no more scenes
                else if (ActiveScene != null)
                {
                    ActiveScene._OnExit(); // Here or in UnloadScene()?
                    UnloadScene(ActiveScene);
                    return;
                }
            }

            // Check if the next scene has already been loaded
            bool isLoaded = false;
            foreach (Scene s in SceneList)
                if (s == nextScene)
                    isLoaded = true;

            // Otherwise load the next scene
            if (!isLoaded)
                LoadScene(nextScene);

            // Check if the active scene needs to be removed from the scene stack or unloaded
            if (ActiveScene != null)
            {
                ActiveScene._OnExit();

                if (removeCurrentSceneFromStack)
                {
                    PopSceneFromStack();
                    if (unloadCurrentScene)
                        UnloadScene(ActiveScene);
                    //Console.WriteLine($"Unload scene {ActiveScene} is {unloadCurrentScene}");
                }
            }

            // Set the next scene as active
            nextScene._OnEnter();
            _sceneStack.Add(nextScene);
            ActiveScene = nextScene;

            Console.WriteLine($"Scene list size {SceneList.Count}");
            Console.WriteLine($"Scene stack size {_sceneStack.Count}");

            // Used to delay changing the player scene during a transition
            if (_playerNextScene != null)
            {
                ChangePlayerScene(_playerNextScene, _playerNextScenePosition,
                    _playerNextSceneEntity);
            }

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
                PlayerScene.GetCameraByName("minimap").trackedEntity = null;
                PlayerScene.RemoveEntity(player);
            }

            // Add the player to the next scene
            TransformComponent transformComponent = player.GetComponent<Engine.TransformComponent>();
            transformComponent.position = playerPosition;
            nextScene.GetCameraByName("main").SetWorldPosition(transformComponent.GetCenter(), instant: true);
            nextScene.GetCameraByName("minimap").SetWorldPosition(transformComponent.GetCenter(), instant: true);
            nextScene.AddEntity(player);
            nextScene.GetCameraByName("main").trackedEntity = player;
            nextScene.GetCameraByName("minimap").trackedEntity = player;
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

        // Gets the scene below if there are any more scenes on the stack.
        // Returns null if no scene below exists.
        public Scene GetSceneBelow(Scene scene)
        {
            if (_sceneStack.Count <= 1)
                return null;

            int sceneIndex = _sceneStack.IndexOf(scene);
            if (sceneIndex < 1)
                return null;

            return _sceneStack[sceneIndex - 1];
        }

        public bool IsSceneListEmpty()
        {
            return SceneList.Count == 0 && Transition == null;
        }

        public void SetScreenSize(int width, int height)
        {
            Globals.graphics.PreferredBackBufferWidth = width;
            Globals.graphics.PreferredBackBufferHeight = height;
            Globals.graphics.ApplyChanges();

            Globals.ScreenWidth = Globals.graphics.PreferredBackBufferWidth;
            Globals.ScreenHeight = Globals.graphics.PreferredBackBufferHeight;
        }
    }

}
