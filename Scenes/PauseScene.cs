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
                new Rectangle(0, 0, Globals.WIDTH, Globals.HEIGHT), Color.Black * 0.5f
            );

            Globals.spriteBatch.DrawString(Globals.font, "Paused", new Vector2((Globals.WIDTH/2)-70, Globals.HEIGHT/2-20), Color.White);
        }

    }

}
