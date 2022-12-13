using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AdventureGame.Engine
{
    public class SceneManager
    {
        public List<Scene> Scenes { get; private set; }
        public List<Scene> _sceneStack;
        public Scene ActiveScene { get; private set; }
        public Scene PlayerScene { get; set; }
        public SceneTransition Transition { get; set; }

        // Stores the player data if a transition is in process
        public Scene _playerNextScene;
        public Entity _playerNextSceneEntity;
        public Vector2 _playerNextScenePosition;


        public SceneManager(int screenWidth = 0, int screenHeight = 0)
        {
            Scenes = new List<Scene>();
            _sceneStack = new List<Scene>();
            /*ActiveScene = null;
            PlayerScene = null;
            _playerNextScene = null;
            Transition = null;
            */
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

                    if (ActiveScene == null)
                        return;

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

        // NOT used
        public void LoadContent()
        {
            if (ActiveScene != null)
                ActiveScene._LoadContent();
        }

        // Needed? Or LoadScene<T>
        public void PreloadScene<T>() where T : Scene
        {

        }

        public void LoadScene(Scene scene)
        {
            if (!Scenes.Contains(scene))
            {
                scene.Init();
                scene._LoadContent();
                Scenes.Add(scene);
            }
        }

        public void UnloadScene(Scene scene)
        {
            if (Scenes.Contains(scene))
            {
                scene._UnloadContent();
                Scenes.Remove(scene);
                // Or add it to a ScenesToRemove list and remove next tick?
                if (_sceneStack.Contains(scene))
                    _sceneStack.Remove(scene); // Likewise ^^
            }
        }

        public void PushSceneToStack(Scene scene)
        {
            /*
            if (Transition != null)
                return;

            if (replaceScene && SceneList.Count > 0)
                GetTopScene()._OnExit();
            scene._OnEnter();
            SceneList.Add(scene);
            */

            _sceneStack.Add(scene);
        }

        public Scene PopSceneFromStack()
        {
            /*
            if (Transition != null)
                return;

            Scene sceneToPop = SceneList[^1];
            SceneList.RemoveAt(SceneList.Count - 1);
            sceneToPop._OnExit();

            if (SceneList.Count > 0)
                GetTopScene()._OnEnter();
            */
            Scene scene = null;

            if (_sceneStack.Count > 0)
            {
                scene = _sceneStack[_sceneStack.Count - 1];
                _sceneStack.RemoveAt(_sceneStack.Count - 1);
            }

            return scene;
        }

        public Scene PeekSceneOnStack()
        {
            Scene scene = null;

            if (_sceneStack.Count > 0)
            {
                scene = _sceneStack[_sceneStack.Count - 1];
            }

            return scene;
        }

        // FIX and change TransitionScene(null) calls
        // FIX PopScene (Pause, Inventory)
        // FIX? FadeTransition movedown pop
        // To do?
        // Add / Remove from stack (replace push and pop?)
        // Should LoadScene or SetActiveScene create the scene instance?
        // Should _onExit always be called even if the scene is unloaded and not ActiveScene?
        // Is Transition != null needed anywhere else??
        // Change parameter transition to transitionScene everywhere

        public void RemoveScene<T>() where T : Scene
        {
            // Check that T is Scene??
            //object scene = T;
            //UnloadScene((Scene)scene);
            //UnloadScene(T);
            /*
            // Check if there are any scenes on the scene stack
            if (_sceneStack.Count > 0)
                nextScene = _sceneStack[^1];
            // Otherwise there are no more scenes
            else if (ActiveScene != null)
            {
                ActiveScene._OnExit(); // Here or in UnloadScene()?
                UnloadScene(ActiveScene);
                return;
            }*/
        }

        public void RemoveScene(Scene scene, bool transition = false, bool unloadScene = true)
        {
            scene._OnExit(); // Or within condition?

            if (scene == ActiveScene)
            {
                //scene._OnExit(); // Or before condition?

                // Check if there are more scenes on the stack
                if (_sceneStack.Count > 1)
                {
                    PopSceneFromStack();
                    Scene nextScene = PeekSceneOnStack();

                    if (transition)
                        TransitionScene(nextScene, true, unloadScene);
                    else
                        ChangeScene(nextScene, true, unloadScene);
                }
                // Otherwise remove the final scene on the stack
                else
                {
                    // Exit game logic here?

                    //scene._OnExit(); // Or before condition?

                    // TO DO transition final scene on stack
                    UnloadScene(scene);
                    //Transition = new FadeSceneTransition(null);
                }
            }
            else
                UnloadScene(scene);
        }

        public void SetActiveScene<T>(bool transition = true,
            bool removeCurrentSceneFromStack = true,
            bool unloadCurrentScene = true) where T : new()
        {
            if (Transition != null)
                return;

            Console.WriteLine("\nSet active scene");

            object scene;
            scene = CheckSceneExists<T>();

            if (scene == null)
            {
                // Create a new instance of the given scene and load it
                scene = new T();
                if (scene is Scene)
                    LoadScene((Scene)scene);
            }

            // The scene failed to load
            if (scene == null)
                return;

            // TO DO transition without ADDing a new scene, use LoadScene?
            // TO DO keep the current scene on the _sceneStack

            if (transition)
                TransitionScene((Scene)scene, removeCurrentSceneFromStack, unloadCurrentScene);
            else
                ChangeScene((Scene)scene, removeCurrentSceneFromStack, unloadCurrentScene);
            /*
            if (transition)
                TransitionScene((Scene)scene, unloadCurrentScene);
            else
                PushScene((Scene)scene, unloadCurrentScene);*/
        }

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
                    nextScene = _sceneStack[_sceneStack.Count - 2];
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
            foreach (Scene s in Scenes)
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

            Console.WriteLine($"Scene list size {Scenes.Count}");
            Console.WriteLine($"Scene stack size {_sceneStack.Count}");

            // Used to change the player scene during a transition
            if (_playerNextScene != null)
            {
                ChangePlayerScene(_playerNextScene, _playerNextScenePosition,
                    _playerNextSceneEntity);
            }

        }

        public void TransitionScene(Scene nextScene, bool removeCurrentSceneFromStack = true,
            bool unloadCurrentScene = true)
        {
            if (Transition == null)
            {
                Transition = new FadeSceneTransition(nextScene, removeCurrentSceneFromStack,
                    unloadCurrentScene);
            }
        }

        public Scene CheckSceneExists<T>()
        {
            Console.WriteLine($"Checking if scene {typeof(T)} already exists");
            Console.WriteLine($"Scenes count {Scenes.Count}");
            //bool sceneExists = false;
            Scene scene = null;

            foreach (var s in Scenes)
            {
                Console.WriteLine($"Compare scene {s} with {typeof(T)}");
                if (s is T)
                {
                    Console.WriteLine($"Scene {typeof(T)} already exists at index {Scenes.IndexOf(s)}");
                    //sceneExists = true;
                    scene = s;
                }
            }
            Console.WriteLine();

            return scene;
        }

        public void ChangePlayerScene<T>(Vector2 playerPosition = default,
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

        // ADD parameter for minicamera?
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

        // Move the entity from one scene to another
        public void ChangeEntityScene(Entity entity, Scene currentScene, Scene newScene)
        {
            newScene.AddEntity(entity);
            currentScene.RemoveEntity(entity);
        }

        // Get the scene below the given scene.
        // Return null if no scene below exists.
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
            return Scenes.Count == 0 && Transition == null;
        }

        public void SetScreenSize(int width, int height)
        {
            Globals.graphics.PreferredBackBufferWidth = width;
            Globals.graphics.PreferredBackBufferHeight = height;
            Globals.graphics.ApplyChanges();

            Globals.ScreenWidth = Globals.graphics.PreferredBackBufferWidth;
            Globals.ScreenHeight = Globals.graphics.PreferredBackBufferHeight;
        }




        /*
        public Scene GetTopScene() // CHANGE to ActiveScene.
        {
            if (SceneList.Count == 0)
                return null;
            return SceneList[^1];
        }
        */

        /*
        public void PushScene(Scene scene, bool replaceScene)
        {
            if (Transition != null)
                return;

            if (replaceScene && SceneList.Count > 0)
                GetTopScene()._OnExit();
            scene._OnEnter();
            SceneList.Add(scene);
        }

        public void PopScene()
        {
            if (Transition != null)
                return;

            Scene sceneToPop = SceneList[^1];
            SceneList.RemoveAt(SceneList.Count - 1);
            sceneToPop._OnExit();

            if (SceneList.Count > 0)
                GetTopScene()._OnEnter();
        }
        */
        /*
        public void InsertSceneAtPosition(Scene scene, int index)
        {
            if (Transition != null)
                return;

            if (SceneList.Count > 0)
                GetTopScene()._OnExit();
            scene._OnEnter();

            if (index >= 0 && index < SceneList.Count)
                SceneList.Insert(index, scene);
            else
                SceneList.Add(scene);
        }*/
        /*
        // Get the scene below the given scene.
        // Return null if no scene below exists.
        public Scene GetSceneBelow(Scene scene)
        {
            if (SceneList.Count <= 1)
                return null;

            int sceneIndex = SceneList.IndexOf(scene);
            if (sceneIndex < 1)
                return null;

            return SceneList[sceneIndex - 1];
        }
        
        public void TransitionScene(Scene nextScene, bool replaceScene = true)
        {
            if (Transition == null)
                Transition = new FadeSceneTransition(nextScene, replaceScene);
        }
        */
        // PreloadScene()??
        /*
        public void UnloadScene(Scene scene, bool transition = true)
        {
            if (scene == null)
                return;

            // Handle unloading the only scene present
            // Quit game??

            // Change to use a for loop and RemoveAt()??
            foreach (Scene s in SceneList)
            {
                if (s == scene)
                {
                    // Remove all entities from each system.entityList

                    // DontDestroyOnSceneChangeTag
                    // https://github.com/Dreaming381/Latios-Framework/blob/v0.2.1/Documentation~/Core/Scene%20Management.md

                    SceneList.Remove(s);
                }
            }
        }
        */
        public void RemoveSceneAtPosition(int index)
        {
            //UnloadScene(index);
        }
        /*
        public void UnloadScene(int index)
        {
            if (index > 0 && index < SceneList.Count)
            {
                Scene sceneToRemove = SceneList[index];

                // Remove all entities from each system.entityList

                sceneToRemove._OnExit();

                if (SceneList.Count > 0)
                    GetTopScene()._OnEnter();

                SceneList.RemoveAt(index);
            }
            else
            {
                // Handle unloading the only scene present
                // Quit game??
            }
        }
        */
        /*
        // Change player here too?
        public void ChangeScene(Scene nextScene, bool deleteCurrentScene = true)
        {
            // Check if the scene has already been loaded
            bool preLoaded = false;
            foreach (Scene s in SceneList)
                if (s == nextScene)
                    preLoaded = true;

            if (!preLoaded)
                LoadScene(nextScene);

            // Change the player scene here??

            if (deleteCurrentScene)
                UnloadScene(GetSceneBelow(nextScene));
        }
        
        public void ChangeScene(string nextSceneId)
        {

        }*/
        /*
        // Add the entity to the new scene and remove it from the current scene
        public void ChangeEntityScene(Entity entity, Scene currentScene, Scene newScene)
        {
            newScene.AddEntity(entity);
            currentScene.RemoveEntity(entity);
        }

        //public void ChangeScene(string nextSceneId, Vector2 playerPosition)
        //public void ChangePlayerScene(Scene nextScene, Vector2 playerPosition = default)
        public void ChangePlayerScene(Scene nextScene, Vector2 playerPosition = default,
            Entity player = null, bool replaceScene = true, bool transition = true)
        {
            if (Transition != null)
                return;

            if (player == null)
                player = EngineGlobals.entityManager.GetLocalPlayer();

            // Remove the player from the current scene
            if (PlayerScene != null)
            {
                PlayerScene.GetCameraByName("main").trackedEntity = null;
                PlayerScene.GetCameraByName("minimap").trackedEntity = null;
                PlayerScene.RemoveEntity(player);
            }

            // Add the player to the new scene
            TransformComponent transformComponent = player.GetComponent<Engine.TransformComponent>();
            transformComponent.position = playerPosition;
            nextScene.GetCameraByName("main").SetWorldPosition(transformComponent.GetCenter(), instant: true);
            nextScene.GetCameraByName("minimap").SetWorldPosition(transformComponent.GetCenter(), instant: true);
            nextScene.AddEntity(player);
            nextScene.GetCameraByName("main").trackedEntity = player;
            nextScene.GetCameraByName("minimap").trackedEntity = player;
            PlayerScene = nextScene;

            if (transition)
                TransitionScene(nextScene, replaceScene);
            else
                PushScene(nextScene, replaceScene);
            
        }
        */

    }

}
