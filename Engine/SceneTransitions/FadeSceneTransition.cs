using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

using System;
using S = System.Diagnostics.Debug;

namespace AdventureGame.Engine
{
    public class FadeSceneTransition : SceneTransition
    {
        public FadeSceneTransition()
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

            float f;
            if (Percentage < 50)
                f = (Percentage / 50);
            else
                f = Math.Abs((100 - Percentage) / 50);

            Globals.spriteBatch.FillRectangle(
                new Rectangle(0, 0, Globals.ScreenWidth, Globals.ScreenHeight),
                Color.Black * f
            );
        }

    }
}
