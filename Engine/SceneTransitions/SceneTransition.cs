using System;
using S = System.Diagnostics.Debug;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame.Engine
{
    public abstract class SceneTransition
    {
        private SceneManager _sceneManager;

        protected Scene ToScene { get; set; }
        protected bool UnloadCurrentScene { get; set; }
        protected float Percentage { get; set; }
        protected float Increment { get; set; }

        public SceneTransition(Scene toScene, bool unloadCurrentScene = true)
        {
            _sceneManager = EngineGlobals.sceneManager;

            ToScene = toScene;
            UnloadCurrentScene = unloadCurrentScene;
            Increment = 1.0f;
        }

        public void Update(GameTime gameTime)
        {
            Percentage = Math.Min(Percentage + Increment, 100);

            if (Percentage == 50)
            {
                // moving up the sceneList
                /*if (ToScene != null)
                {
                    _sceneManager.ChangeScene(ToScene, RemoveCurrentSceneFromStack, UnloadCurrentScene);
                }
                // moving down
                else
                {
                    //_sceneManager.PopScene();
                }*/

                _sceneManager.ChangeScene(ToScene, UnloadCurrentScene);
            }

            if (Percentage == 100)
                _sceneManager.EndTransition();

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
