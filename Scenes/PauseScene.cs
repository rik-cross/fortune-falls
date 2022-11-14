using System;
using System.Collections.Generic;
using System.Text;

using AdventureGame.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended;

namespace AdventureGame
{
    public class PauseScene : Engine.Scene
    {

        public override void Init()
        {
            drawSceneBelow = true;    
        }

        public override void Update(GameTime gameTime)
        {
            if (EngineGlobals.inputManager.IsPressed( Globals.pauseInput ))
            {
                EngineGlobals.sceneManager.PopScene();
            }

        }

        public override void Draw(GameTime gameTime)
        {

            Globals.spriteBatch.FillRectangle(
                new Rectangle(0, 0, Globals.ScreenWidth, Globals.ScreenHeight), Color.Black * 0.5f
            );

            Vector2 fontSize = Theme.FontPrimary.MeasureString("Paused");
            Globals.spriteBatch.DrawString(Theme.FontPrimary,
                "Paused",
                new Vector2(
                    Globals.ScreenWidth / 2 - fontSize.X / 2,
                    Globals.ScreenHeight / 2 - fontSize.Y / 2),
                Color.White);
            //Globals.spriteBatch.DrawString(Theme.primaryFont, "Paused", new Vector2((Globals.ScreenWidth/2)-70, Globals.ScreenHeight/2-20), Color.White);
        }

    }

}
