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
            // Testing 5s transition (fade out for 3s, fade in for 2s)
            //TimeToCompleteTransition = 5.0f;
            //TimeToChangeScene = 3.0f;
        }

        public override void Draw(GameTime gameTime)
        {
            Globals.graphicsDevice.SetRenderTarget(Globals.sceneRenderTarget);
            Globals.spriteBatch.End();
            if (EngineGlobals.sceneManager.ActiveScene != null)
                EngineGlobals.sceneManager.ActiveScene._Draw(gameTime);

            Globals.graphicsDevice.SetRenderTarget(Globals.sceneRenderTarget);
            Globals.spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            // Calculate alpha percentage of fade overlay colour
            // Fading scene out will increase the fade alpha from 0 to 1
            // Fading scene in will decrease the fade alpha from 1 to 0
            float alpha = 0.0f;
            if (TimeElapsed <= FadeOutDuration)
                alpha = (TimeElapsed / FadeOutDuration);
            else if (TimeElapsed <= TimeToCompleteTransition)
                alpha = 1 - (TimeElapsed - FadeOutDuration) / FadeInDuration;

            Globals.spriteBatch.FillRectangle(
                new Rectangle(0, 0, Globals.ScreenWidth, Globals.ScreenHeight),
                Color.Black * alpha
            );
        }

    }
}
