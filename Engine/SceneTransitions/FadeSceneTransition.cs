using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

 
            Globals.spriteBatch.End();
            if (!EngineGlobals.sceneManager.IsEmpty())
                EngineGlobals.sceneManager.GetTopScene()._Draw(gameTime);
            Globals.graphicsDevice.SetRenderTarget(Globals.sceneRenderTarget);
            Globals.spriteBatch.Begin(samplerState: SamplerState.PointClamp);


            float f;
            if (percentage < 50)
                f = (percentage / 50);
            else
                f = Math.Abs((100 - percentage) / 50);
            Globals.spriteBatch.FillRectangle(
                new Rectangle(0, 0, Globals.ScreenWidth, Globals.ScreenHeight),
                Color.Black * f
            );

        }

    }
}
