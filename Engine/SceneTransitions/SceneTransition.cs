using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using S = System.Diagnostics.Debug;

namespace AdventureGame.Engine
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

                HasSceneChanged = true;
                EngineGlobals.sceneManager.SetSceneDuringTransition(NextScene, NextSceneBelow, UnloadCurrentScene);
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
