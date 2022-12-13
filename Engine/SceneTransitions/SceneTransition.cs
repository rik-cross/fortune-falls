using System;
using System.Collections.Generic;
using System.Text;
using S = System.Diagnostics.Debug;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame.Engine
{
    public abstract class SceneTransition
    {
        private SceneManager _sceneManager;

        protected Scene ToScene { get; set; }
        protected bool RemoveCurrentSceneFromStack { get; set; }
        protected bool UnloadCurrentScene { get; set; }
        protected float Percentage { get; set; }
        protected float Increment { get; set; }

        public SceneTransition(Scene toScene, bool removeCurrentSceneFromStack = true,
            bool unloadCurrentScene = true)
        {
            _sceneManager = EngineGlobals.sceneManager;

            ToScene = toScene;
            RemoveCurrentSceneFromStack = removeCurrentSceneFromStack;
            UnloadCurrentScene = unloadCurrentScene;
            Increment = 1.0f;
        }

        public void Update(GameTime gameTime)
        {
            Percentage = Math.Min(Percentage + Increment, 100);

            if (Percentage == 50)
            {
                // moving up the sceneList
                if (ToScene != null)
                {
                    _sceneManager.ChangeScene(ToScene, RemoveCurrentSceneFromStack, UnloadCurrentScene);
                }
                // moving down
                else
                {
                    //_sceneManager.PopScene();
                }

            }

            if (Percentage == 100)
                _sceneManager.Transition = null;

            if (_sceneManager.ActiveScene != null)
                _sceneManager.ActiveScene._Update(gameTime);
        }
        /*
        public void Update(GameTime gameTime)
        {
            Percentage = Math.Min(Percentage + Increment, 100);

            if (Percentage == 50)
            {
                // moving up the sceneList
                if (ToScene != null)
                {
                    if (ReplaceScene)
                    {
                        //Scene sceneToPop = _sceneManager.GetTopScene();
                        //_sceneManager.SceneList.RemoveAt(_sceneManager.SceneList.Count - 1);
                        //sceneToPop._OnExit();
                        _sceneManager.RemoveSceneAtPosition(_sceneManager.SceneList.Count - 1);
                    }

                    ToScene._OnEnter();
                    _sceneManager.SceneList.Add(ToScene);

                }
                // moving down
                else
                {
                    _sceneManager.PopScene();
                }

            }

            if (Percentage == 100)
                _sceneManager.Transition = null;

            if (_sceneManager.SceneList.Count > 0)
                _sceneManager.GetTopScene()._Update(gameTime);
        }
        */
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
