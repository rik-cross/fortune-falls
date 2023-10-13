using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended;

namespace AdventureGame.Engine
{
    
    public class FadeBSceneTransition : SceneTransition
    {
        public FadeBSceneTransition(Scene toScene, bool unloadCurrentScene = true)
            : base(toScene, unloadCurrentScene)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            Globals.graphicsDevice.SetRenderTarget(Globals.sceneRenderTarget);
            Globals.spriteBatch.End();
            if (EngineGlobals.sceneManager.ActiveScene != null)
                EngineGlobals.sceneManager.ActiveScene._Draw(gameTime);

            Globals.graphicsDevice.SetRenderTarget(Globals.sceneRenderTarget);
            Globals.spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        }

    }
}
