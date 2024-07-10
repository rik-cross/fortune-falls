using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using S = System.Diagnostics.Debug;

namespace AdventureGame.Engine
{
    public abstract class SceneTransition
    {
        private SceneManager _sceneManager;

        protected Type NextScene { get; set; }
        protected Type NextSceneBelow { get; set; }

        protected bool UnloadCurrentScene { get; set; }
        protected float Percentage { get; set; }
        protected float Increment { get; set; }

        public bool HasSceneChanged { get; private set; }
        public bool Finished { get; private set; }


        public SceneTransition()
        {
            _sceneManager = EngineGlobals.sceneManager;
            Percentage = 0;
            Increment = 1.0f;
            HasSceneChanged = false;
            Finished = false;
        }

        // Delete? Or set scenes to null
        public void StartTransition(bool unloadCurrentScene = true)
        {
            UnloadCurrentScene = unloadCurrentScene;
        }

        public void StartTransition<TScene>(bool unloadCurrentScene = true)
        {
            NextScene = typeof(TScene);
            NextSceneBelow = null;
            UnloadCurrentScene = unloadCurrentScene;
        }

        public void StartTransition<TScene, TSceneBelow>(bool unloadCurrentScene = true)
        {
            NextScene = typeof(TScene);
            NextSceneBelow = typeof(TSceneBelow);
            UnloadCurrentScene = unloadCurrentScene;
        }

        public void StartTransition(Type scene, Type sceneBelow = null, bool unloadCurrentScene = true)
        {
            NextScene = scene;
            NextSceneBelow = sceneBelow;
            UnloadCurrentScene = unloadCurrentScene;
        }

        public void Update(GameTime gameTime)
        {
            //Console.WriteLine($"Scene transition update");

            Percentage = Math.Min(Percentage + Increment, 100);

            if (Percentage >= 50 && HasSceneChanged == false)
            {
                Console.WriteLine($"Transitioning to next scene");

                HasSceneChanged = true;

                //_sceneManager.SetActiveScene(UnloadCurrentScene);
                if (NextScene == null)
                    _sceneManager.SetActiveScene(UnloadCurrentScene);
                else
                    _sceneManager.SetSceneDuringTransition(NextScene, NextSceneBelow, UnloadCurrentScene);
            }

            if (Percentage >= 100)
            {
                Finished = true;
            }

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
