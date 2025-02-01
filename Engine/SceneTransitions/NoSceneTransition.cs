using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using S = System.Diagnostics.Debug;

namespace Engine
{
    public class NoSceneTransition : SceneTransition
    {
        public NoSceneTransition()
        {
            // Instantly complete the scene transition with no fade
            TimeToCompleteTransition = 0.0f;
            TimeToChangeScene = 0.0f;
        }

        public override void Draw(GameTime gameTime)
        {
            EngineGlobals.graphicsDevice.SetRenderTarget(EngineGlobals.sceneRenderTarget);
            EngineGlobals.spriteBatch.End();
            
            if (EngineGlobals.sceneManager.ActiveScene != null)
                EngineGlobals.sceneManager.ActiveScene._Draw(gameTime);

            EngineGlobals.graphicsDevice.SetRenderTarget(EngineGlobals.sceneRenderTarget);
            EngineGlobals.spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        }

    }
}
