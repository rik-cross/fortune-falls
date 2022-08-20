using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using S = System.Diagnostics.Debug;

using MonoGame.Extended;

namespace AdventureGame.Engine
{
    
    public class FadeSceneTransition : SceneTransition
    {
        public FadeSceneTransition(Scene toScene, bool replaceScene = false) : base(toScene, replaceScene)
        {
        }
        public override void Draw(GameTime gameTime)
        {

            Globals.graphicsDevice.SetRenderTarget(Globals.sceneRenderTarget);
            //Globals.spriteBatch.Begin();

 
            Globals.spriteBatch.End();
            if (!EngineGlobals.sceneManager.isEmpty())
                EngineGlobals.sceneManager.GetTopScene()._Draw(gameTime);
            Globals.graphicsDevice.SetRenderTarget(Globals.sceneRenderTarget);
            Globals.spriteBatch.Begin(samplerState: SamplerState.PointClamp);


            float f;
            if (percentage < 50)
                f = (percentage / 50);
            else
                f = Math.Abs((100 - percentage) / 50);
            //S.WriteLine(f);
            Globals.spriteBatch.FillRectangle(
                new Rectangle(0, 0, Globals.WIDTH, Globals.HEIGHT),
                Color.Black * f
            );

            //Globals.spriteBatch.End();
            //Globals.graphicsDevice.SetRenderTarget(null);
            //Globals.spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            //Globals.spriteBatch.Draw(Globals.sceneRenderTarget, Globals.sceneRenderTarget.Bounds, Color.White);
            //Globals.spriteBatch.End();
        }

    }
}
