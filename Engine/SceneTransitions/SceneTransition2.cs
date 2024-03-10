using System;
using S = System.Diagnostics.Debug;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame.Engine
{
    public abstract class SceneTransition2
    {
        private SceneManager _sceneManager;

        //protected Scene[] ToScenes { get; set; }
        //protected List<Scene> ToScenes { get; set; }
        protected bool UnloadCurrentScene { get; set; }
        //protected int NumberOfScenesToUnload { get; set; }
        protected float Percentage { get; set; }
        protected float Increment { get; set; }

        private bool _hasSceneChanged = false;
        public bool Finished { get; set; }

        public SceneTransition2()
        {
            _sceneManager = EngineGlobals.sceneManager;

            //ToScenes = toScenes;
            //NumberOfScenesToUnload = numScenesToUnload;
            //UnloadCurrentScene = unloadCurrentScene;
            Percentage = 0;
            Increment = 1.0f;
            Finished = false;
        }

        public void StartTransition(bool unloadCurrentScene = true)
        {
            //ToScenes = toScenes;
            UnloadCurrentScene = unloadCurrentScene;
            //NumberOfScenesToUnload = numScenesToUnload;
        }

        public void Update(GameTime gameTime)
        {
            //Console.WriteLine($"Scene transition update");

            Percentage = Math.Min(Percentage + Increment, 100);

            if (Percentage >= 50 && _hasSceneChanged == false)
            {
                Console.WriteLine($"Transitioning to next scene");

                //_sceneManager.ChangeScene(ToScene, UnloadCurrentScene);

                _hasSceneChanged = true;

                _sceneManager.SetActiveScene(UnloadCurrentScene);

                /*
                if (UnloadCurrentScene)
                    _sceneManager.UnloadScene(_sceneManager.ActiveScene);

                if (_sceneManager._sceneStack.Count > 0)
                {
                    _sceneManager.ActiveScene = _sceneManager._sceneStack[^1];
                    //_sceneManager.ActiveScene.Init();
                    //_sceneManager.ActiveScene.LoadContent();
                    _sceneManager.ActiveScene.OnEnter();
                }
                else
                {
                    _sceneManager.ActiveScene = null;
                }
                */

                //bool scenesAdded = false;
                //foreach (Scene s in ToScenes)
                //{
                //    if (_sceneManager.ActiveScene != null)
                //        _sceneManager.ActiveScene.OnExit();
                //    _sceneManager._sceneStack.Add(s);
                //    _sceneManager.ActiveScene = s;
                //    //s.Init();
                //    s.LoadContent();
                //    s.OnEnter();
                //    scenesAdded = true;
                //    S.WriteLine(_sceneManager._sceneStack.Count);
                //}

                ////if (_sceneManager.ActiveScene != null && scenesAdded)
                ////    _sceneManager.ActiveScene.OnExit();

                ////if (_sceneManager._sceneStack.Count > 0 && scenesAdded)
                ////{
                ////    _sceneManager.ActiveScene = _sceneManager._sceneStack[^1];
                //    //_sceneManager.ActiveScene.Init();
                //    //_sceneManager.ActiveScene.LoadContent();
                //    //_sceneManager.ActiveScene.OnEnter();
                ////}

                //// remove scenes

                //if (NumberOfScenesToUnload > 0)
                //{
                    
                //    int lastIndex = _sceneManager._sceneStack.Count - ToScenes.Count - 1;
                //    int firstIndex = Math.Max(0, lastIndex - NumberOfScenesToUnload + 1);
                //    S.WriteLine(lastIndex + " : " + firstIndex);
                //    for (int i = lastIndex; i >= firstIndex; i--)
                //    {
                //        S.WriteLine("removing " + i);
                //        if (i == lastIndex)
                //            _sceneManager._sceneStack[i].OnExit();
                //        S.WriteLine(_sceneManager._sceneStack[i].ToString());
                //        _sceneManager._sceneStack.RemoveAt(i);

                //        if (_sceneManager._sceneStack.Count > 0)
                //        {
                //            _sceneManager.ActiveScene = _sceneManager._sceneStack[^1];
                //            //_sceneManager.ActiveScene.Init();
                //            //_sceneManager.ActiveScene.LoadContent();
                //            _sceneManager.ActiveScene.OnEnter();
                //        } else
                //        {
                //            _sceneManager.ActiveScene = null;
                //        }

                //    }

                //    //_sceneManager.ActiveScene = _sceneManager._sceneStack[^1];
                //    //_sceneManager.ActiveScene.Init();
                //    //_sceneManager.ActiveScene.LoadContent();
                //    //_sceneManager.ActiveScene.OnEnter();

                //}

            }

            if (Percentage == 100)
            {
                Finished = true;
            }

            //if (_sceneManager.ActiveScene != null)
            //    _sceneManager.ActiveScene._Update(gameTime);
        }

        public void _Draw(GameTime gameTime)
        {
            Globals.graphicsDevice.SetRenderTarget(Globals.sceneRenderTarget);
            Globals.spriteBatch.Begin();
            Draw(gameTime);
            Globals.spriteBatch.End();

            Globals.graphicsDevice.SetRenderTarget(null);
            Globals.spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            Globals.spriteBatch.Draw(Globals.sceneRenderTarget, Globals.sceneRenderTarget.Bounds, Color.White);
            Globals.spriteBatch.End();

        }
        public abstract void Draw(GameTime gameTime);

    }
}
