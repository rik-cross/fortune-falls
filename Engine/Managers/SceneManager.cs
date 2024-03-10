using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AdventureGame.Engine
{
    public class SceneManager
    {
        // todo - change to _sceneList
        public List<Scene> _sceneStack;
        public Scene ActiveScene { get; set; }
        // todo - move to PlayerManager?
        public Scene PlayerScene { get; private set; }
        public SceneTransition Transition { get; set; }
        public SceneTransition2 Transition2 { get; set; }

        // todo - move to PlayerManager or delete?
        // Stores the player data if a transition is in process
        public Scene _playerNextScene;
        public Entity _playerNextSceneEntity;
        public Vector2 _playerNextScenePosition;

        // todo - remove parameters? remove SetScreenSize?
        public SceneManager(int screenWidth = 0, int screenHeight = 0)
        {
            _sceneStack = new List<Scene>();

            if (screenWidth > 0 && screenHeight > 0)
                SetScreenSize(screenWidth, screenHeight);
        }

        public void Input(GameTime gameTime)
        {
            if (ActiveScene != null)
                ActiveScene._Input(gameTime);
        }

        public void Update(GameTime gameTime)
        {
            //if (Transition != null && Transition.Finished)
            //    Transition = null;

            //if (Transition != null)
            //    Transition.Update(gameTime);

            if (Transition != null && Transition.Finished) 
                Transition = null;

            if (Transition != null)
                Transition.Update(gameTime);

            if (Transition2 != null && Transition2.Finished)
                Transition2 = null;

            if (Transition2 != null)
                Transition2.Update(gameTime);

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
                Transition._Draw(gameTime);
            else if (Transition2 != null)
                Transition2._Draw(gameTime);
            else
                ActiveScene._Draw(gameTime);
        }


        public Scene LoadScene<TScene>() where TScene : new()
        {
            Scene existingScene = CheckSceneExists<TScene>();
            if (existingScene == null)
            {
                Console.WriteLine($"LoadScene {typeof(TScene)}");
                object sceneObj = new TScene();// Activator.CreateInstance(t);

                if (sceneObj is Scene scene)
                {
                    Console.WriteLine($"Scene {sceneObj.GetType()} is loading");
                    scene.Init();
                    scene._LoadContent();
                    _sceneStack.Add(scene);
                    return scene;
                }
            }
            return existingScene;
        }


        public Scene LoadScene(Type t)
        {
            Scene existingScene = CheckSceneExists(t);
            if (existingScene == null)
            {
                Console.WriteLine($"LoadScene {t}");
                object sceneObj = Activator.CreateInstance(t);

                if (sceneObj is Scene scene)
                {
                    Console.WriteLine($"Scene {sceneObj.GetType()} is loading");
                    scene.Init();
                    scene._LoadContent();
                    _sceneStack.Add(scene);
                    return scene;
                }
            }
            return existingScene;
        }

        // todo - delete??
        // Create an instance of a scene and add it to the top of the scene stack
        private void LoadScene(Scene scene)
        {
            if (!_sceneStack.Contains(scene))
            {
                scene.Init();
                scene._LoadContent();
                _sceneStack.Add(scene);
            }
        }

        // Exit the scene and remove it from the scene stack
        public void UnloadScene(Scene scene)
        {
            Console.WriteLine($"Unloading scene {scene}");
            int index = _sceneStack.IndexOf(scene);
            if (index != -1)
            {
                scene._OnExit();
                scene._UnloadContent();
                _sceneStack.RemoveAt(index);
                Console.WriteLine($"{scene} unloaded \n");
            }
        }

        // Exit all scenes from the top of the stack to the bottom
        public void UnloadAllScenes()
        {
            for (int i = _sceneStack.Count - 1; i >= 0; i--)
                UnloadScene(_sceneStack[i]);
        }

        // todo
        // Unload the scene and create a new instance in the same scene stack position
        public void ResetScene<TScene>() where TScene : new()
        {
            Scene scene = CheckSceneExists(typeof(TScene));
            //if (scene != null)
        }

        // Set the active scene to the scene at the top of the scene stack
        public void SetActiveScene(bool unloadCurrentScene)
        {
            if (unloadCurrentScene)
                UnloadScene(ActiveScene);

            if (_sceneStack.Count > 0)
            {
                ActiveScene = _sceneStack[^1];
                ActiveScene.OnEnter();
            }
            else
                ActiveScene = null;
        }

        //public void SetActiveScene()
        //{
        //    if (_sceneStack.Count > 0)
        //    {
        //        ActiveScene = _sceneStack[^1];
        //        //_sceneManager.ActiveScene.Init();
        //        //_sceneManager.ActiveScene.LoadContent();
        //        ActiveScene.OnEnter();
        //    }
        //    else
        //        ActiveScene = null;
        //}



        // todo - change to ChangeScene
        // Change from the ActiveScene to the given scene with no transition.
        public void StartSceneTransition<TScene>(bool unloadCurrentScene = true)
        {
            Console.WriteLine($"Changing scene: {typeof(TScene)}");
            // Return if a scene transition is in progress
            if (Transition2 != null)
                return;

            // Load scene and ensure it is at the top of the stack
            Scene scene = LoadScene(typeof(TScene));
            if (scene != null)
            {
                MoveSceneToTop(scene);
                SetActiveScene(unloadCurrentScene);
            }
        }

        // todo - change to ChangeScene
        // Start a scene transition with the ActiveScene and the given scene.
        public void StartSceneTransition<TTransition, TScene>(
            bool unloadCurrentScene = true) where TTransition : new()
        {
            Console.WriteLine($"Changing scene: {typeof(TTransition)}, {typeof(TScene)}");
            // Return if a scene transition is in progress
            if (Transition2 != null)
                return;

            object transition = new TTransition();
            if (transition is SceneTransition2)
            {
                // Load scene and ensure it is at the top of the stack
                Scene scene = LoadScene(typeof(TScene));
                if (scene != null)
                    MoveSceneToTop(scene);
                else
                    return;

                // Start a new scene transition
                Console.WriteLine($"\nStarting scene transition");
                Transition2 = (SceneTransition2)transition;
                ((SceneTransition2)transition).StartTransition(unloadCurrentScene);
            }
        }

        // todo - change to ChangeScene
        // Start a scene transition with the ActiveScene. Load the next scene and scene below.
        public void StartSceneTransition<TTransition, TScene, TSceneBelow>(
            bool unloadCurrentScene = true) where TTransition : new()
        {
            Console.WriteLine($"Changing scene: {typeof(TTransition)}, {typeof(TScene)}, {typeof(TSceneBelow)}");
            // Return if a scene transition is in progress
            if (Transition2 != null)
                return;

            object transition = new TTransition();
            if (transition is SceneTransition2)
            {
                // Load both scenes and ensure they are at the top of the stack
                Scene sceneBelow = LoadScene(typeof(TSceneBelow));
                if (sceneBelow != null)
                    MoveSceneToTop(sceneBelow);
                else
                    return;

                Scene scene = LoadScene(typeof(TScene));
                if (scene != null)
                    MoveSceneToTop(scene);
                else
                {
                    UnloadScene(sceneBelow); // Clean up in case scene is not valid
                    return;
                }

                // Start a new scene transition
                Console.WriteLine($"\nStarting scene transition");
                Transition2 = (SceneTransition2)transition;
                ((SceneTransition2)transition).StartTransition(unloadCurrentScene);
            }
        }



        // todo - public Scene GetSceneFromType(Type t) ??
        // todo - SetActiveScene<T> ??
        // todo - ChangeScene ??
        // todo - RemoveScene ??

        // todo - check if stack still works e.g. a new Scene is added but the previous already
        // exists, therefore the scene below in the "stack" is out of order



        // Check if the scene stack is empty or a transition is in progress
        public bool IsSceneStackEmpty()
        {
            return _sceneStack.Count == 0 && Transition == null;
        }

        // todo - is this needed or use _sceneStack.contains() ?
        // Check whether the scene already exists in the scene list
        public Scene CheckSceneExists(Type t)
        {
            Console.WriteLine($"\nChecking if scene {t} already exists - count {_sceneStack.Count}");

            foreach (Scene scene in _sceneStack)
            {
                Type sType = scene.GetType();
                Console.WriteLine($"- Compare scene {scene} with {t}");
                if (sType == t) // if (sType.Equals(t))
                {
                    Console.WriteLine($"Scene {t} already exists at index {_sceneStack.IndexOf(scene)}");
                    Console.WriteLine();
                    return scene;
                }
            }
            Console.WriteLine();

            return null;
        }

        // todo delete??
        // Check whether the scene already exists in the scene stack
        public Scene CheckSceneExists<TScene>()
        {
            //Console.WriteLine($"Checking if scene {typeof(T)} already exists - count {_sceneStack.Count}");

            foreach (Scene scene in _sceneStack)
            {
                //Console.WriteLine($"- Compare scene {s} with {typeof(T)}");
                if (scene is TScene)
                {
                    //Console.WriteLine($"Scene {typeof(T)} already exists at index {_sceneStack.IndexOf(s)}");
                    return scene;
                }
            }
            //Console.WriteLine();

            return null;
        }

        // Move a scene to the top of the stack if it is not there already
        public void MoveSceneToTop(Scene scene)
        {
            Console.WriteLine($"Moving scene {scene} to top of stack");
            Console.WriteLine(string.Join(", ", _sceneStack));
            if (_sceneStack.Count > 1)
            {
                int index = _sceneStack.IndexOf(scene);

                // Check scene exists and is not already at top of stack
                if (index != -1 && index != _sceneStack.Count - 1)
                {
                    Scene temp = _sceneStack[index];
                    _sceneStack.RemoveAt(index);
                    _sceneStack.Add(temp);
                }
            }
            Console.WriteLine(string.Join(", ", _sceneStack));
        }

        // Return the next scene in the stack if there is one, otherwise returns null
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



        public void StartSceneTransition(SceneTransition transition)
        {
            Transition = transition;

            //Transition = (SceneTransition2)Activator.CreateInstance(transition.GetType());

            //Transition = new FadeSceneTransition2();
            //Transition.StartTransition(transition.ToScenes);
        }

        
        // todo - more methods for ChangeScene (no transition, single scene...)
        // todo - change to ChangeScene
        public void StartSceneTransition<T>(bool unloadCurrentScene, params Type[] scenes)
            where T : new()
        {
            // check if scene transition is in progress
            // if so, exit OR add to queue

            object transition = new T();

            if (transition is SceneTransition2)
            {
                List<Scene> sceneObjects = new List<Scene>();

                foreach (Type s in scenes)
                {
                    Scene scene = LoadScene(s);
                    if (scene != null)
                        sceneObjects.Add(scene);
                }

                if (sceneObjects.Count > 0)
                {
                    Console.WriteLine($"\nStarting scene transition");
                    //UnloadScene(ActiveScene);
                    //Transition2 = (SceneTransition2)transition;
                    //((SceneTransition2)transition).StartTransition(sceneObjects, unloadCurrentScene);
                }
            }

            //Console.WriteLine($"SetActiveScene {scene.GetType().Name}, transition {applyTransition}");
        }






        public void StartSceneTransition(Type transition, List<Type> scenes)
        {
            object transitionObj = Activator.CreateInstance(transition);

            if (transitionObj is SceneTransition)
            {

            }

            // Create an instance of types[0] using the default constructor
            object newObject = Activator.CreateInstance(scenes[0]);

            //SceneTransition transition = new FadeSceneTransition(scenes);
            //Transition = transition;
        }


        //public void StartSceneTransition<T>(T transition, params Type[] scenes) where T : SceneTransition
        //{
        //    //if (typeof(T) is SceneTransition)
        //    //object transitionObj = Activator.CreateInstance(typeof(T));

        //    //Scene sceneArray = new Scene[scenes.Length];


        //    //// Get all public constructors for types[0]
        //    //var ctors = scenes[0].GetConstructors(BindingFlags.Public);

        //    //// Create a class of types[0] using the first constructor
        //    //var object = ctors[0].Invoke(new object[] { });

        //    //object newTransition = Activator.CreateInstance(transition);

        //    // Create an instance of types[0] using the default constructor
        //    //object newTransition = Activator.CreateInstance(transition,
        //    //    new List<Scene>() { new VillageScene(), new PlayerSelectScene() },
        //    //    2);

        //    //SceneTransition newTransition = new typeof()
        //    //Type myGeneric = typeof(SceneTransition<>);
        //    //Type constructedClass = myGeneric.MakeGenericType(T);
        //    //object newTransition = Activator.CreateInstance(transition);

        //    object transitionObj = Activator.CreateInstance(typeof(T));

        //    // Create an instance of types[0] using the default constructor
        //    object newObject = Activator.CreateInstance(scenes[0]);

        //    //SceneTransition transition = new FadeSceneTransition(scenes);

        //    Transition = (SceneTransition)transitionObj;
        //}

        //public void StartSceneTransition<T>(Scene[] scenes) where T : new()

        //public void StartSceneTransition<T>(List<T> scenes)
        //public void StartSceneTransition(params Type[] scenes, int num)
        //public void StartSceneTransition<T>(params Type[] scenes) where T : new()
        //public void StartSceneTransition(Type transition, params Type[] scenes)

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

            //Console.WriteLine($"SetActiveScene {scene.GetType().Name}, transition {applyTransition}");

            //// Otherwise change the scene
            //if (applyTransition)
            //{
            //    // BUG: MainMenu transition = true then applies to PlayerSelectScene
            //    // even though transition = false on Set and Remove scene
            //    StartTransition((Scene)scene, unloadCurrentScene);
            //}
            //else
            //    ChangeScene((Scene)scene, unloadCurrentScene);
        }

        
        // todo - move method to another place e.g. PlayerManager?

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
            transformComponent.Position = playerPosition;
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

        // todo - move to PlayerManager??
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
