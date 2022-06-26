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
            
        }

        public override void LoadContent()
        {
            Init();
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

            Color bg = new Color(Color.Black, 150);
            Globals.spriteBatch.FillRectangle(
                new Rectangle((Globals.WIDTH/2)-200, (Globals.HEIGHT/2)-40, 400, 80), bg
            );

            Globals.spriteBatch.DrawString(Globals.font, "Paused", new Vector2((Globals.WIDTH/2)-70, Globals.HEIGHT/2-20), Color.White);
        }

    }

}
