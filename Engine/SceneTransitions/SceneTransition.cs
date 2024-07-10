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
        protected float TimeToCompleteTransition { get; set; }
        protected float TimeToChangeScene { get; set; }
        protected float TimeElapsed { get; set; }
        protected float FadeOutDuration { get; set; }
        protected float FadeInDuration { get; set; }

        public bool HasSceneChanged { get; private set; }
        public bool Finished { get; private set; }


        public SceneTransition()
        {
            _sceneManager = EngineGlobals.sceneManager;
            Percentage = 0;
            Increment = 1.0f;

            TimeToCompleteTransition = 2.0f;  // default: 2 second transition
            TimeToChangeScene = 1.0f;         // default: 1 second to change scene
        }

        public void Init()
        {
            TimeElapsed = 0.0f;
            FadeOutDuration = TimeToChangeScene;
            FadeInDuration = TimeToCompleteTransition - FadeOutDuration;

            HasSceneChanged = false;
            Finished = false;
        }

        public void StartTransition(Type scene = null, Type sceneBelow = null, bool unloadCurrentScene = true)
        {
            NextScene = scene;
            NextSceneBelow = sceneBelow;
            UnloadCurrentScene = unloadCurrentScene;
            Init();
        }

        public void Update(GameTime gameTime)
        {
            //Console.WriteLine($"Scene transition update");

            //Percentage += Increment * deltaTime;
            Percentage = Math.Min(Percentage + Increment, 100);

            //if (Percentage >= 50 && HasSceneChanged == false)
            //if (TimeToCompleteTransition <= TimeToChangeScene && HasSceneChanged == false)
            if (TimeElapsed >= TimeToChangeScene && HasSceneChanged == false)
            //if (FadeOutDuration <= 0 && HasSceneChanged == false)
            {
                Console.WriteLine($"Transitioning to next scene");
                Console.WriteLine($"Time elapsed (fade out) {TimeElapsed}");

                HasSceneChanged = true;

                //_sceneManager.SetActiveScene(UnloadCurrentScene);

                //if (NextScene == null)
                //    _sceneManager.SetActiveScene(UnloadCurrentScene);
                //else
                //    _sceneManager.SetSceneDuringTransition(NextScene, NextSceneBelow, UnloadCurrentScene);

                _sceneManager.SetSceneDuringTransition(NextScene, NextSceneBelow, UnloadCurrentScene);
            }

            //if (Percentage >= 100)
            //if (TimeToCompleteTransition <= 0)
            if (TimeElapsed >= TimeToCompleteTransition)
            //if (FadeInDuration <= 0)
            {
                Console.WriteLine($"Time elapsed (fade in) {TimeElapsed - TimeToChangeScene}");
                Finished = true;
            }

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            //TimeToCompleteTransition -= deltaTime;
            TimeElapsed += deltaTime;

            //if (FadeOutDuration >= 0)
            //    FadeOutDuration -= deltaTime;
            //else
            //    FadeInDuration -= deltaTime;

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
