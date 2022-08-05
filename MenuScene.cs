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
        private Engine.Image controllerImage;
        private Engine.Image keyboardImage;

        public override void Init()
        {
            this.title = new Engine.Text(caption: "Game Title!", position: new Vector2(Globals.WIDTH / 2, 200), font: Globals.font, a: anchor.middlecenter);

            // control images
            this.controllerImage = new Engine.Image(
                Globals.content.Load<Texture2D>("X360"),
                position: new Vector2((Globals.WIDTH / 2) + 100, Globals.HEIGHT - 150),
                a: anchor.middlecenter, alpha: 0.2f
            );
            this.keyboardImage = new Engine.Image(
                Globals.content.Load<Texture2D>("Keyboard"),
                position: new Vector2((Globals.WIDTH / 2) - 100, Globals.HEIGHT - 150),
                a: anchor.middlecenter, alpha: 0.2f
            );
        }

        public override void LoadContent()
        {
            Init();
        }

        public override void Update(GameTime gameTime)
        {

            if (EngineGlobals.inputManager.IsPressed(Globals.backInput))
            {
                EngineGlobals.sceneManager.PopScene();
            }

            InputComponent inputComponent = EngineGlobals.entityManager.GetEntityByTag("player").GetComponent<InputComponent>();
            if (inputComponent != null)
            {
                InputMethod inputMethod = inputComponent.input;
                if (inputMethod != null)
                {
                    InputItem inputItem = inputMethod.button1;
                    if (inputItem != null) {
                        if (EngineGlobals.inputManager.IsPressed(inputMethod.button1))
                            EngineGlobals.sceneManager.PushScene(new GameScene());
                    }
                }
            }

            if (EngineGlobals.inputManager.IsPressed(KeyboardInput.Enter))
            {
                keyboardImage.alpha = 1.0f;
                controllerImage.alpha = 0.2f;
                EngineGlobals.entityManager.GetEntityByTag("player").GetComponent<InputComponent>().input = Engine.Inputs.keyboard;
            }

            if (EngineGlobals.inputManager.IsPressed(ControllerInput.A))
            {
                keyboardImage.alpha = 0.2f;
                controllerImage.alpha = 1.0f;
                EngineGlobals.entityManager.GetEntityByTag("player").GetComponent<InputComponent>().input = Engine.Inputs.controller;
            }


        }

        public override void Draw(GameTime gameTime)
        {
            title.Draw();
            controllerImage.Draw();
            keyboardImage.Draw();
        }

    }

}
