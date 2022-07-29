using System;
using System.Collections.Generic;
using System.Text;

using AdventureGame.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended;

namespace AdventureGame
{
    public class MenuScene : Engine.Scene
    {

        private Engine.Text title;

        public override void Init()
        {
            this.title = new Text(text: "Game Title!", position: new Vector2(Globals.WIDTH / 2, 200), a: anchor.middlecenter);
        }

        public override void LoadContent()
        {
            Init();
        }

        public override void Update(GameTime gameTime)
        {
            //if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            if (EngineGlobals.inputManager.IsPressed(Globals.backInput))
            {
                EngineGlobals.sceneManager.PopScene();
            }
            if (EngineGlobals.inputManager.IsPressed(Globals.forwardInput))
            {
                EngineGlobals.sceneManager.PushScene(new GameScene());
            }

        }

        public override void Draw(GameTime gameTime)
        {

            title.Draw();

        }

    }

}
