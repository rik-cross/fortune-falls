using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

using MonoGame.Extended;

namespace AdventureGame.Engine
{
    
    public class FadeSceneTransition2 : SceneTransition2
    {
        public FadeSceneTransition2()
        {
        }

        //public FadeSceneTransition(List<Scene> toScenes, int numScenesToUnload = 0)
        //    : base(toScenes, numScenesToUnload)
        //{
        //}

        //public override void StartTransition(List<Scene> toScenes, int numScenesToUnload = 0)
        //{

        //}

        public override void Draw(GameTime gameTime)
        {
            Console.WriteLine("Draw fade transition");
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
