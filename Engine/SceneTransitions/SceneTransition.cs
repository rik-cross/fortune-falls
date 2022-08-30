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

        public float percentage;
        public float increment = 1.0f;

        protected Scene toScene;
        protected bool replaceScene;

        public SceneTransition(Scene toScene, bool replaceScene = false)
        {
            this.toScene = toScene;
            this.replaceScene = replaceScene;
        }

        public void Update(GameTime gameTime)
        {
            percentage = Math.Min(percentage + increment, 100);

            if (percentage == 50)
            {

                // moving up the sceneList
                if (toScene != null)
                {
                    if (replaceScene)
                    {
                        
                        Scene sceneToPop = EngineGlobals.sceneManager.GetTopScene();
                        EngineGlobals.sceneManager.sceneList.RemoveAt(EngineGlobals.sceneManager.sceneList.Count - 1);

                        sceneToPop._OnExit();
                    }

                    toScene._OnEnter();
                    EngineGlobals.sceneManager.sceneList.Add(toScene);


                }
                // moving down
                else
                {
                    EngineGlobals.sceneManager.PopScene();


                }

            }

            if (percentage == 100)
                EngineGlobals.sceneManager.transition = null;

            if (EngineGlobals.sceneManager.sceneList.Count > 0)
                EngineGlobals.sceneManager.GetTopScene()._Update(gameTime);
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
