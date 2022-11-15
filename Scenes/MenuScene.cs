using AdventureGame.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

namespace AdventureGame
{
    public class MenuScene : Engine.Scene
    {
        private Engine.Text _title;
        private Engine.Image _controllerImage;
        private Engine.Image _keyboardImage;
        private Engine.Animation _controllerButton;
        private Engine.Animation _keyboardButton;
        private Engine.Animation _playerAnimation;

        public MenuScene()
        {
            // title text
            _title = new Engine.Text(
                caption: "Game Title!",
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

            // player animation
            _playerAnimation = new Engine.Animation(
                new List<Texture2D> {
                    Globals.playerSpriteSheet.GetSubTexture(6,4),
                    Globals.playerSpriteSheet.GetSubTexture(7,4),
                    Globals.playerSpriteSheet.GetSubTexture(8,4),
                    Globals.playerSpriteSheet.GetSubTexture(7,4)
                },
                size: new Vector2(26*4,36*4),
                anchor: Anchor.MiddleCenter,
                animationDelay: 12
            );
        }

        public override void OnEnter()
        {
            /*
            Engine.Entity playerEntity = EngineGlobals.entityManager.GetLocalPlayer();
            Globals.gameScene.AddEntity(playerEntity);
            playerEntity.GetComponent<TransformComponent>().position = new Vector2(100, 100);
            Globals.gameScene.GetCameraByName("main").SetWorldPosition(new Vector2(100, 100), instant: true);
            Globals.gameScene.GetCameraByName("minimap").SetWorldPosition(new Vector2(100, 100), instant: true);
            Globals.gameScene.GetCameraByName("main").trackedEntity = playerEntity;
            Globals.gameScene.GetCameraByName("minimap").trackedEntity = playerEntity;
            */

            _controllerButton.Stop();
            _keyboardButton.Stop();
        }
        public override void OnExit()
        {
            _controllerButton.Stop();
            _keyboardButton.Stop();
        }
        public override void Update(GameTime gameTime)
        {
            if (EngineGlobals.inputManager.IsPressed(Globals.backInput) && EngineGlobals.sceneManager.Transition == null)
            {
                EngineGlobals.sceneManager.Transition = new FadeSceneTransition(null);
            }

            InputComponent inputComponent = EngineGlobals.entityManager.GetLocalPlayer().GetComponent<InputComponent>();
            if (inputComponent != null)
            {
                InputMethod inputMethod = inputComponent.input;
                if (inputMethod != null)
                {
                    InputItem inputItem = inputMethod.button1;
                    if (inputItem != null)
                    {
                        if (EngineGlobals.inputManager.IsPressed(inputMethod.button1)
                            && EngineGlobals.sceneManager.Transition == null)
                        {
                            //EngineGlobals.sceneManager.Transition = new FadeSceneTransition(Globals.gameScene);
                            Vector2 playerPosition = new Vector2(100, 100);
                            EngineGlobals.sceneManager.ChangePlayerScene(Globals.gameScene, playerPosition);
                        }
                    }
                }
            }

            if (EngineGlobals.inputManager.IsPressed(KeyboardInput.Enter))
            {
                _keyboardImage.Alpha = 1.0f;
                _controllerImage.Alpha = 0.2f;
                EngineGlobals.entityManager.GetLocalPlayer().GetComponent<InputComponent>().input = Engine.Inputs.keyboard;
            }

            if (EngineGlobals.inputManager.IsPressed(ControllerInput.A))
            {
                _keyboardImage.Alpha = 0.2f;
                _controllerImage.Alpha = 1.0f;
                EngineGlobals.entityManager.GetLocalPlayer().GetComponent<InputComponent>().input = Engine.Inputs.controller;
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

            _playerAnimation.Update();
            _controllerButton.Update();
            _keyboardButton.Update();

        }

        public override void Draw(GameTime gameTime)
        {
            _title.Draw();
            _controllerImage.Draw();
            _keyboardImage.Draw();
            _controllerButton.Draw();
            _keyboardButton.Draw();
            _playerAnimation.Draw();
        }

    }

}
