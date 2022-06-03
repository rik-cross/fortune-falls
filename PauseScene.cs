using System;
using System.Collections.Generic;
using System.Text;

using AdventureGame.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
            if (EngineGlobals.inputManager.IsPressed(Keys.Escape))
            {
                EngineGlobals.sceneManager.PopScene();
            }
        }

        public override void Draw(GameTime gameTime)
        {

            Color bg = new Color(Color.Black, 150);
            Globals.spriteBatch.FillRectangle(
                new Rectangle(200, 200, 400, 80), bg
            );

            Globals.spriteBatch.DrawString(Globals.font, "Paused [esc]", new Vector2(300, 220), Color.White);
        }

    }

}
