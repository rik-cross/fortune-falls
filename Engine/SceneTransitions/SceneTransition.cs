using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using S = System.Diagnostics.Debug;

namespace Engine
{
    public abstract class SceneTransition
    {
        protected Type NextScene { get; set; }
        protected Type NextSceneBelow { get; set; }
        protected bool UnloadCurrentScene { get; set; }

        protected float TimeToCompleteTransition { get; set; }
        protected float TimeToChangeScene { get; set; }
        protected float TimeElapsed { get; private set; }
        protected float FadeOutDuration { get; private set; }
        protected float FadeInDuration { get; private set; }

        public bool HasSceneChanged { get; private set; }
        public bool Finished { get; private set; }


        public SceneTransition()
        {
            // Default: 2 second transition, 1 second to change scene
            TimeToCompleteTransition = 2.0f;
            TimeToChangeScene = 1.0f;
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
            if (TimeElapsed >= TimeToChangeScene && HasSceneChanged == false)
            {
                Console.WriteLine($"Transitioning to next scene: fade out time {TimeElapsed}");
                Console.WriteLine($"Running at {1.0f / (float)gameTime.ElapsedGameTime.TotalSeconds} FPS");

                HasSceneChanged = true;
                EngineGlobals.sceneManager.SetSceneDuringTransition(NextScene, NextSceneBelow, UnloadCurrentScene);

                Console.WriteLine($"Running at {1.0f / (float)gameTime.ElapsedGameTime.TotalSeconds} FPS");
            }

            if (TimeElapsed >= TimeToCompleteTransition)
            {
                Console.WriteLine($"Transition ended: fade in time {TimeElapsed - TimeToChangeScene}");
                Finished = true;
            }

            // Update elapsed time
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TimeElapsed += deltaTime;
        }

        public void _Draw(GameTime gameTime)
        {
            EngineGlobals.graphicsDevice.SetRenderTarget(EngineGlobals.sceneRenderTarget);
            EngineGlobals.spriteBatch.Begin();
            Draw(gameTime);
            EngineGlobals.spriteBatch.End();

            EngineGlobals.graphicsDevice.SetRenderTarget(null);
            EngineGlobals.spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            EngineGlobals.spriteBatch.Draw(EngineGlobals.sceneRenderTarget, EngineGlobals.sceneRenderTarget.Bounds, Color.White);
            EngineGlobals.spriteBatch.End();

        }

        public abstract void Draw(GameTime gameTime);

    }
}
