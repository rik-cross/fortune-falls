using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

using MonoGame.Extended;

namespace AdventureGame.Engine
{
    
    public class NoSceneTransition2 : SceneTransition2
    {
        public NoSceneTransition2()
        {
            Increment = 100.0f;
            Percentage = 100.0f;
        }

        public override void Draw(GameTime gameTime)
        {
            Globals.graphicsDevice.SetRenderTarget(Globals.sceneRenderTarget);
            Globals.spriteBatch.End();
            
            if (EngineGlobals.sceneManager.ActiveScene != null)
                EngineGlobals.sceneManager.ActiveScene._Draw(gameTime);

            Globals.graphicsDevice.SetRenderTarget(Globals.sceneRenderTarget);
            Globals.spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            //float f;
            //if (Percentage < 50)
            //    f = (Percentage / 50);
            //else
            //    f = Math.Abs((100 - Percentage) / 50);

            //Globals.spriteBatch.FillRectangle(
            //    new Rectangle(0, 0, Globals.ScreenWidth, Globals.ScreenHeight),
            //    Color.Black * f
            //);
        }

    }
}
