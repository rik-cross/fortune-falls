using AdventureGame.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

using System.Collections.Generic;

using System;
using MonoGame.Extended;
using S = System.Diagnostics.Debug;


namespace AdventureGame
{
    public class MenuScene : Engine.Scene
    {
        private Engine.Text _title;
        private Engine.Image _controllerImage;
        private Engine.Image _keyboardImage;
        private Engine.Animation _controllerButton;
        private Engine.Animation _keyboardButton;
        private Engine.Text inputHelpText;

        public MenuScene()
        {

            // title text
            _title = new Engine.Text(
                caption: "Adventure Game",
                font: Theme.FontPrimary,
                colour: Theme.TextColorTertiary,
                anchor: Anchor.TopCenter,
                padding: new Padding(top: 200)
            );

            // get alpha values based on current player input type
            float controllerAlpha = 0.2f;
            if (EngineGlobals.entityManager.GetLocalPlayer().GetComponent<InputComponent>().input == Engine.Inputs.controller)
                controllerAlpha = 1.0f;
            float keyboardAlpha = 0.2f;
            if (EngineGlobals.entityManager.GetLocalPlayer().GetComponent<InputComponent>().input == Engine.Inputs.keyboard)
                keyboardAlpha = 1.0f;

            // control images
            _controllerImage = new Engine.Image(
                Globals.content.Load<Texture2D>("X360"),
                anchor: Anchor.MiddleCenter,
                padding: new Padding(left: 100, top: 130),
                alpha: controllerAlpha
            );
            _keyboardImage = new Engine.Image(
                Globals.content.Load<Texture2D>("Keyboard"),
                anchor: Anchor.MiddleCenter,
                padding: new Padding(right: 100, top: 130),
                alpha: keyboardAlpha
            );

            // controller buttons
            Engine.SpriteSheet controllerSpritesheet = new Engine.SpriteSheet(Globals.content.Load<Texture2D>("xbox_buttons"), new Vector2(16,16));

            _controllerButton = new Engine.Animation(
                new List<Texture2D> {
                    controllerSpritesheet.GetSubTexture(0,1),
                    controllerSpritesheet.GetSubTexture(1,1),
                    controllerSpritesheet.GetSubTexture(2,1)
                },
                anchor: Anchor.BottomCenter,
                anchorParent: _controllerImage.Rectangle,
                padding: new Padding(bottom: -20),
                size: new Vector2(16 * 3, 16 * 3),
                animationDelay: 2,
                loop: false,
                play: false
            ); ;

            Engine.SpriteSheet enterKeySpritesheet = new Engine.SpriteSheet(Globals.content.Load<Texture2D>("enter_key"), new Vector2(16, 12));
            
            _keyboardButton = new Engine.Animation(
                new List<Texture2D> {
                    enterKeySpritesheet.GetSubTexture(0,0),
                    enterKeySpritesheet.GetSubTexture(1,0),
                    enterKeySpritesheet.GetSubTexture(1,0)
                },
                anchor: Anchor.BottomCenter,
                anchorParent: _keyboardImage.Rectangle,
                padding: new Padding(bottom: -20),
                size: new Vector2(16 * 3, 16 * 3),
                animationDelay: 2,
                loop: false,
                play: false
            );

            inputHelpText = new Engine.Text(
                "Hold to select",
                position: new Vector2(Globals.ScreenWidth / 2, 0),
                font: Theme.FontTertiary,
                colour: Color.White,
                //anchorParent: _keyboardButton.Rectangle,
                anchor: Anchor.TopCenter,
                padding: new Padding(top: 550)
            );

        }

        public override void OnEnter()
        {
            //EngineGlobals.soundManager.PlaySongFade(Globals.content.Load<Song>("Music/citadel"));

            _controllerButton.Stop();
            _controllerButton.Reverse(false);
            _controllerButton.Reset();

            _keyboardButton.Stop();
            _keyboardButton.Reverse(false);
            _keyboardButton.Reset();
        }
        public override void OnExit()
        {
            _controllerButton.Stop();
            _keyboardButton.Stop();
        }
        public override void Update(GameTime gameTime)
        {

            if (EngineGlobals.inputManager.IsPressed(Globals.backInput))
            {
                EngineGlobals.sceneManager.RemoveScene(this, applyTransition: true);

                // Handle exit game logic here?
            }

            if (EngineGlobals.inputManager.IsDown(KeyboardInput.Enter))
                _keyboardImage.Alpha = 1.0f;
            else
                _keyboardImage.Alpha = 0.2f;

            if (EngineGlobals.inputManager.IsLongPressed(KeyboardInput.Enter))
            {
                EngineGlobals.entityManager.GetLocalPlayer().GetComponent<InputComponent>().input = Engine.Inputs.keyboard;
                Vector2 playerPosition = new Vector2(20, 760);
                //playerPosition = new Vector2(220, 700); // X left
                //playerPosition = new Vector2(500, 700); // X right 
                //playerPosition = new Vector2(410, 1080); // Y down
                //playerPosition = new Vector2(410, 480); // Y up

                //playerPosition = new Vector2(500, 300); // X left same  // 100
                //playerPosition = new Vector2(1200, 300); // X right same
                //playerPosition = new Vector2(1240, 400); // Y down same  // 100
                //playerPosition = new Vector2(1240, 500); // Y up same  // 900

                //playerPosition = new Vector2(950, 200); // perpendicular

                playerPosition = new Vector2(1113, 330); // opposite SW, X
                //playerPosition = new Vector2(1120, 330); // opposite SW, Y

                //playerPosition = new Vector2(1747, 945); // Collision tiles testing
                //playerPosition = new Vector2(1558, 1073); // Collision tiles testing

                // Add the MenuScene to the scene stack
                EngineGlobals.sceneManager.SetActiveScene<GameScene>(
                    removeCurrentSceneFromStack: false, unloadCurrentScene: false);

                EngineGlobals.sceneManager.SetPlayerScene<GameScene>(playerPosition);
            }

            if (EngineGlobals.inputManager.IsDown(ControllerInput.A))
                _controllerImage.Alpha = 1.0f;
            else
                _controllerImage.Alpha = 0.2f;

            if (EngineGlobals.inputManager.IsLongPressed(ControllerInput.A))
            {
                EngineGlobals.entityManager.GetLocalPlayer().GetComponent<InputComponent>().input = Engine.Inputs.controller;
                Vector2 playerPosition = new Vector2(20, 760);

                // Add the MenuScene to the scene stack
                EngineGlobals.sceneManager.SetActiveScene<GameScene>(
                    removeCurrentSceneFromStack: false, unloadCurrentScene: false);

                EngineGlobals.sceneManager.SetPlayerScene<GameScene>(playerPosition);
            }

            if (EngineGlobals.inputManager.IsPressed(ControllerInput.A))
            {
                //controllerButton.reverse = false;
                _controllerButton.Reverse(false);
                _controllerButton.Reset();
                _controllerButton.Play();
            }
            if (EngineGlobals.inputManager.IsReleased(ControllerInput.A))
            {
                //controllerButton.reverse = true;
                _controllerButton.Reverse(true);
                _controllerButton.Reset();
                _controllerButton.Play();
            }
            if (EngineGlobals.inputManager.IsPressed(KeyboardInput.Enter))
            {
                //keyboardButton.reverse = false;
                _keyboardButton.Reverse(false);
                _keyboardButton.Reset();
                _keyboardButton.Play();
            }
            if (EngineGlobals.inputManager.IsReleased(KeyboardInput.Enter))
            {
                //keyboardButton.reverse = true;
                _keyboardButton.Reverse(true);
                _keyboardButton.Reset();
                _keyboardButton.Play();
            }

            //_playerAnimation.Update();
            _controllerButton.Update();
            _keyboardButton.Update();

        }

        public override void Draw(GameTime gameTime)
        {
            _title.Draw();
            _controllerImage.Draw();
            _keyboardImage.Draw();

            //Globals.spriteBatch.DrawCircle(new CircleF(new Vector2(_keyboardButton.Center, _keyboardButton.Middle),25),360, Color.LightGray);
            //Globals.spriteBatch.DrawCircle(new CircleF(new Vector2(_controllerButton.Center, _controllerButton.Middle), 25), 360, Color.LightGray);

            for (double i = 0; i < 100; i+=0.5)
            {
                if (i >= EngineGlobals.inputManager.GetLongPressPercentage(KeyboardInput.Enter))
                    break;
                double rad = (Math.PI / 180) * i * 3.6 ;
                float x = (float)(25 * Math.Cos(rad));
                float y = (float)(25 * Math.Sin(rad));
                Globals.spriteBatch.DrawLine(new Vector2(_keyboardButton.Center, _keyboardButton.Middle), new Vector2(_keyboardButton.Center+x,_keyboardButton.Middle+y), Color.White);
            }
            for (double i = 0; i < 100; i += 0.5)
            {
                if (i >= EngineGlobals.inputManager.GetLongPressPercentage(ControllerInput.A))
                    break;
                double rad = (Math.PI / 180) * i * 3.6;
                float x = (float)(25 * Math.Cos(rad));
                float y = (float)(25 * Math.Sin(rad));
                Globals.spriteBatch.DrawLine(new Vector2(_controllerButton.Center, _controllerButton.Middle), new Vector2(_controllerButton.Center + x, _controllerButton.Middle + y), Color.White);
            }

            _controllerButton.Draw();
            _keyboardButton.Draw();
            inputHelpText.Draw();


        }

    }

}
