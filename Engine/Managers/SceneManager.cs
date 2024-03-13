using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    public class SceneManager
    {
        private List<Scene> _sceneStack;

        public Scene ActiveScene { get; private set; }
        public Scene SceneBelow { get; private set; }
        public SceneTransition Transition { get; private set; }

        // todo - remove parameters and SetScreenSize?
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
            if (Transition != null && Transition.Finished)
                Transition = null;

            if (Transition != null)
                Transition.Update(gameTime);

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

            // todo - update scene below if stack is > 1 here??
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
            else
                ActiveScene._Draw(gameTime);
        }


        // Load a scene using Type t and add it to top of stack
        private Scene LoadScene(Type t)
        {
            Scene existingScene = GetScene(t);
            if (existingScene == null)
            {
                Console.WriteLine($"LoadScene {t}");
                object sceneObj = Activator.CreateInstance(t);

                if (sceneObj is Scene scene)
                {
                    Console.WriteLine($"New instance of {sceneObj.GetType()} is loading");
                    scene.Init();
                    scene._LoadContent();
                    _sceneStack.Add(scene);
                    return scene;
                }
            }
            return existingScene;
        }

        // Exit the scene and remove it from the scene stack
        private void UnloadScene(Scene scene)
        {
            Console.WriteLine($"Unloading scene {scene}");
            int index = _sceneStack.IndexOf(scene);
            if (index != -1)
            {
                scene.UpdateSceneBelow = false;
                scene._OnExit();
                scene._UnloadContent();
                _sceneStack.RemoveAt(index);
                Console.WriteLine($"{scene} unloaded \n");
            }
        }

        // Move a scene to the top of the stack if it is not there already.
        // Return true if carried out successfully.
        private bool MoveSceneToTop(Scene scene)
        {
            Console.WriteLine($"Moving scene {scene} to top of stack");
            Console.WriteLine(string.Join(", ", _sceneStack));
            if (_sceneStack.Count > 1 && scene != null)
            {
                int index = _sceneStack.IndexOf(scene);

                // Check scene exists and is not already at top of stack
                if (index != -1 && index != _sceneStack.Count - 1)
                {
                    Scene temp = _sceneStack[index];
                    _sceneStack.RemoveAt(index);
                    _sceneStack.Add(temp);
                    return true;
                }
            }
            Console.WriteLine(string.Join(", ", _sceneStack));
            return false;
        }


        // Check if the given scene is the active scene
        public bool IsActiveScene(Scene scene)
        {
            return scene == ActiveScene;
        }


        // Check if the given scene is the active scene
        public bool IsActiveScene<TScene>()
        {
            return typeof(TScene) == ActiveScene.GetType();
        }

        // Check if the scene stack is empty or a transition is in progress
        public bool IsSceneStackEmpty()
        {
            return _sceneStack.Count == 0 && Transition == null;
        }


        // todo
        // Unload the scene and create a new instance in the same scene stack position
        public Scene ResetScene<TScene>() where TScene : new()
        {
            //Scene scene = CheckSceneExists(typeof(TScene));
            //if (scene != null)
            Console.WriteLine($"\nReset scene {typeof(TScene)}\n");

            int index = -1;
            for (int i = 0; i < _sceneStack.Count; i++)
            {
                if (_sceneStack[i].GetType() == typeof(TScene))
                    index = i;
            }

            //object scene = new TScene();
            //int index = _sceneStack.IndexOf((Scene)scene);

            if (index != -1)
            {
                Scene oldScene = _sceneStack[index];
                bool isActiveScene = false;
                bool isSceneBelow = false;

                if (ActiveScene.GetType() == typeof(TScene))
                    isActiveScene = true;
                else if (SceneBelow.GetType() == typeof(TScene))
                    isSceneBelow = true;

                //object newScene = new TScene();

                //if (ActiveScene == oldScene)
                //    ActiveScene = (Scene)newScene;
                //else if (SceneBelow == oldScene)
                //    SceneBelow = (Scene)newScene;

                UnloadScene(oldScene);
                //LoadScene((Scene)newScene);

                // Loading the scene will add it to top of stack
                Scene newScene = LoadScene(typeof(TScene));

                Console.WriteLine(string.Join(", ", _sceneStack));

                if (isActiveScene)
                {
                    Console.WriteLine("Resetting active scene");
                    SetActiveScene(false);
                }
                else if (isSceneBelow)
                {
                    Console.WriteLine("Resetting scene below");

                    // Swap the new scene on top of stack with scene below
                    if (_sceneStack.Count > 1)
                    {
                        //Scene activeScene = ActiveScene;
                        _sceneStack.Insert(_sceneStack.Count - 2, newScene);
                        Console.WriteLine(string.Join(", ", _sceneStack));
                        _sceneStack.RemoveAt(_sceneStack.Count - 1);
                        Console.WriteLine(string.Join(", ", _sceneStack));
                    }
                }
                else
                {
                    Console.WriteLine("Resetting other scene");

                    // Insert the new scene instance into the original position
                    _sceneStack.Insert(index, newScene);
                    Console.WriteLine(string.Join(", ", _sceneStack));
                    _sceneStack.RemoveAt(_sceneStack.Count - 1);
                    Console.WriteLine(string.Join(", ", _sceneStack));
                }

                return newScene;
            }

            return null;
        }

        // Exit all scenes from the top of the stack to the bottom
        public void UnloadAllScenes()
        {
            for (int i = _sceneStack.Count - 1; i >= 0; i--)
                UnloadScene(_sceneStack[i]);
        }

        // Return a scene if it already exists in the stack. Otherwise return null.
        public Scene GetScene<TScene>()
        {
            return GetScene(typeof(TScene));
        }

        // Return a scene if it already exists in the stack. Otherwise return null.
        public Scene GetScene(Type t)
        {
            Console.WriteLine($"\nChecking if scene {t} already exists - count {_sceneStack.Count}");

            foreach (Scene scene in _sceneStack)
            {
                Type sType = scene.GetType();
                Console.WriteLine($"- Compare scene {scene} with {t}");
                if (sType == t)
                {
                    Console.WriteLine($"Scene {t} already exists at index {_sceneStack.IndexOf(scene)}");
                    Console.WriteLine();
                    return scene;
                }
            }
            Console.WriteLine();

            return null;
        }

        // Set ActiveScene to the scene at the top of the scene stack
        // and SceneBelow if there is one. Otherwise set scene(s) to null.
        public void SetActiveScene(bool unloadCurrentScene)
        {
            if (unloadCurrentScene)
                UnloadScene(ActiveScene);

            if (_sceneStack.Count > 0)
            {
                ActiveScene = _sceneStack[^1];
                if (_sceneStack.Count > 1)
                    SceneBelow = _sceneStack[^2];
                else
                    SceneBelow = null;
                ActiveScene.OnEnter();
            }
            else
                ActiveScene = null;
            Console.WriteLine($"Active scene: {ActiveScene}\nScene below: {SceneBelow}");
        }

        // Change from the ActiveScene to the given scene with no transition.
        public void ChangeScene<TScene>(bool unloadCurrentScene = true)
        {
            Console.WriteLine($"Changing scene: {typeof(TScene)}");
            // Return if a scene transition is in progress
            if (Transition != null)
                return;

            // Load scene and ensure it is at the top of the stack
            Scene scene = LoadScene(typeof(TScene));
            if (scene != null)
            {
                MoveSceneToTop(scene);
                SetActiveScene(unloadCurrentScene);
            }
        }

        // Start a scene transition with the current ActiveScene and the given scene.
        public void ChangeScene<TTransition, TScene>(
            bool unloadCurrentScene = true) where TTransition : new()
        {
            Console.WriteLine($"Changing scene: {typeof(TTransition)}, {typeof(TScene)}");
            // Return if a scene transition is in progress
            if (Transition != null)
                return;

            object transition = new TTransition();
            if (transition is SceneTransition)
            {
                // Load scene and ensure it is at the top of the stack
                Scene scene = LoadScene(typeof(TScene));
                if (scene != null)
                    MoveSceneToTop(scene);
                else
                    return;

                // Start a new scene transition
                Console.WriteLine($"\nStarting scene transition");
                Transition = (SceneTransition)transition;
                ((SceneTransition)transition).StartTransition(unloadCurrentScene);
            }
        }

        // Start a scene transition with the current ActiveScene.
        // Load the next scene and scene below.
        public void ChangeScene<TTransition, TSceneBelow, TScene>(
            bool unloadCurrentScene = true) where TTransition : new()
        {
            Console.WriteLine($"Changing scene: {typeof(TTransition)}, {typeof(TSceneBelow)}, {typeof(TScene)}");
            // Return if a scene transition is in progress
            if (Transition != null)
                return;

            object transition = new TTransition();
            if (transition is SceneTransition)
            {
                // Load both scenes and ensure they are the top 2 in the stack
                Scene sceneBelow = LoadScene(typeof(TSceneBelow));
                if (sceneBelow != null)
                    MoveSceneToTop(sceneBelow);
                else
                    return;

                Scene scene = LoadScene(typeof(TScene));
                if (scene != null)
                    MoveSceneToTop(scene);
                else
                    return;

                // Start a new scene transition
                Console.WriteLine($"\nStarting scene transition");
                Transition = (SceneTransition)transition;
                ((SceneTransition)transition).StartTransition(unloadCurrentScene);
            }
        }

        // Change active scene to scene below if it exists
        public void ChangeToSceneBelow(bool unloadCurrentScene = true)
        {
            if (unloadCurrentScene)
                SetActiveScene(true);
            else if (_sceneStack.Count > 1)
            {
                // Move the scene below to top of stack
                if (MoveSceneToTop(SceneBelow))
                    SetActiveScene(false);
            }
        }

        // Change active scene to scene below if it exists with a transition
        public void ChangeToSceneBelow<TTransition>(bool unloadCurrentScene = true)
            where TTransition : new()
        {
            object transition = new TTransition();
            if (transition is SceneTransition)
            {
                // Move the scene below to top of stack
                if (_sceneStack.Count > 1)
                    MoveSceneToTop(SceneBelow);

                // Start a new scene transition
                Console.WriteLine($"\nStarting scene transition");
                Transition = (SceneTransition)transition;
                ((SceneTransition)transition).StartTransition(unloadCurrentScene);
            }
        }

        // Return SceneBelow if no scene is given. Otherwise search for the given scene
        // and return the scene below in the stack if there is one.
        // Return null if no scene below exists.
        public Scene GetSceneBelow(Scene scene = null)
        {
            if (scene == null)
                return SceneBelow;
            else
            {
                // Return the scene below the given scene if there is one
                int sceneIndex = _sceneStack.IndexOf(scene);
                if (sceneIndex > 0 && sceneIndex < _sceneStack.Count)
                    return _sceneStack[sceneIndex - 1];
                else
                    return null;
            }
        }

        // todo - not currently used. Delete?
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
