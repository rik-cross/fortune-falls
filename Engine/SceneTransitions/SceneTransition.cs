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

        protected List<Scene> fromScenes;
        protected List<Scene> toScenes;
        protected bool replaceScenes;

        public SceneTransition(List<Scene> fromScenes, List<Scene> toScenes, bool replaceScenes = false)
        {
            this.fromScenes = fromScenes;
            this.toScenes = toScenes;
            this.replaceScenes = replaceScenes;
        }

        public void Update(GameTime gameTime)
        {
            percentage = Math.Min(percentage + increment, 100);
            //S.WriteLine(percentage);
            if (percentage == 50)
            {
                if (toScenes.Count == 0)
                {
                    foreach (Scene s in fromScenes)
                        EngineGlobals.sceneManager.PopScene();
                } else {
                    if (replaceScenes)
                    {
                        foreach (Scene s in fromScenes)
                            EngineGlobals.sceneManager.PopScene();
                    }
                    foreach (Scene s in toScenes)
                        EngineGlobals.sceneManager.PushScene(s);
                }
            }
            if (percentage == 100)
            {
                EngineGlobals.sceneManager.transition = null;
            }

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
