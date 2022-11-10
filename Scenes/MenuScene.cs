using AdventureGame.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

namespace AdventureGame
{
    public class MenuScene : Engine.Scene
    {

        private Engine.Text title;
        private Engine.Image controllerImage;
        private Engine.Image keyboardImage;
        private Engine.Animation controllerButton;
        private Engine.Animation keyboardButton;
        private Engine.Animation testAnimation;

        public MenuScene()
        {
            // title text
            title = new Engine.Text(
                caption: "Game Title!",
                //position: new Vector2(Globals.ScreenWidth / 2, 200),
                font: Theme.primaryFont,
                colour: Theme.tertiaryText,
                //anchor: Anchor.middlecenter
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
            controllerImage = new Engine.Image(
                Globals.content.Load<Texture2D>("X360"),
                //position: new Vector2((Globals.ScreenWidth / 2) + 100, Globals.ScreenHeight - 150),
                anchor: Anchor.MiddleCenter,
                padding: new Padding(left: 100, top: 130),
                alpha: controllerAlpha
            );
            keyboardImage = new Engine.Image(
                Globals.content.Load<Texture2D>("Keyboard"),
                //position: new Vector2((Globals.ScreenWidth / 2) - 100, Globals.ScreenHeight - 150),
                anchor: Anchor.MiddleCenter,
                padding: new Padding(right: 100, top: 130),
                alpha: keyboardAlpha
            );

            // controller buttons
            Engine.SpriteSheet controllerSpritesheet = new Engine.SpriteSheet(Globals.content.Load<Texture2D>("xbox_buttons"), new Vector2(16,16));
            /*controllerButton = new Engine.Animation(
                new List<Texture2D> {
                    controllerSpritesheet.GetSubTexture(0,1),
                    controllerSpritesheet.GetSubTexture(1,1),
                    controllerSpritesheet.GetSubTexture(2,1)
                },
                position: new Vector2(controllerImage.Center, controllerImage.Bottom),
                anchor: Anchor.middlecenter,
                size: new Vector2(16*3,16*3),
                animationDelay: 2,
                loop: false,
                play: false
            );*/
            controllerButton = new Engine.Animation(
                new List<Texture2D> {
                    controllerSpritesheet.GetSubTexture(0,1),
                    controllerSpritesheet.GetSubTexture(1,1),
                    controllerSpritesheet.GetSubTexture(2,1)
                },
                //position: new Vector2(controllerImage.Center, controllerImage.Bottom),
                anchor: Anchor.BottomCenter,
                anchorParent: controllerImage.Rectangle,
                padding: new Padding(bottom: -20),
                size: new Vector2(16 * 3, 16 * 3),
                animationDelay: 2,
                loop: false,
                play: false
            ); ;
            //Console.WriteLine($"Controller rectangle: {controllerImage.Rectangle}");
            Engine.SpriteSheet enterKeySpritesheet = new Engine.SpriteSheet(Globals.content.Load<Texture2D>("enter_key"), new Vector2(16, 12));
            /*keyboardButton = new Engine.Animation(
                new List<Texture2D> {
                    enterKeySpritesheet.GetSubTexture(0,0),
                    enterKeySpritesheet.GetSubTexture(1,0),
                    enterKeySpritesheet.GetSubTexture(1,0)
                },
                position: new Vector2(keyboardImage.Center, keyboardImage.Bottom),
                anchor: Anchor.middlecenter,
                size: new Vector2(16 * 3, 16 * 3),
                animationDelay: 2,
                loop: false,
                play: false
            );*/
            keyboardButton = new Engine.Animation(
                new List<Texture2D> {
                    enterKeySpritesheet.GetSubTexture(0,0),
                    enterKeySpritesheet.GetSubTexture(1,0),
                    enterKeySpritesheet.GetSubTexture(1,0)
                },
                //position: new Vector2(keyboardImage.Center, keyboardImage.Bottom),
                anchor: Anchor.BottomCenter,
                anchorParent: keyboardImage.Rectangle,
                padding: new Padding(bottom: -20),
                size: new Vector2(16 * 3, 16 * 3),
                animationDelay: 2,
                loop: false,
                play: false
            );

            // test animation
            testAnimation = new Engine.Animation(
                new List<Texture2D> {
                    Globals.playerSpriteSheet.GetSubTexture(6,4),
                    Globals.playerSpriteSheet.GetSubTexture(7,4),
                    Globals.playerSpriteSheet.GetSubTexture(8,4),
                    Globals.playerSpriteSheet.GetSubTexture(7,4)
                },
                //position: new Vector2(Globals.ScreenWidth / 2, Globals.ScreenHeight - 350),
                size: new Vector2(26*4,36*4),
                anchor: Anchor.MiddleCenter,
                animationDelay: 12
            );

        }

        public override void OnEnter()
        {

            Engine.Entity playerEntity = EngineGlobals.entityManager.GetLocalPlayer();
            Globals.gameScene.AddEntity(playerEntity);
            playerEntity.GetComponent<TransformComponent>().position = new Vector2(100, 100);
            Globals.gameScene.GetCameraByName("main").SetWorldPosition(new Vector2(100, 100), instant: true);
            Globals.gameScene.GetCameraByName("minimap").SetWorldPosition(new Vector2(100, 100), instant: true);
            Globals.gameScene.GetCameraByName("main").trackedEntity = playerEntity;
            Globals.gameScene.GetCameraByName("minimap").trackedEntity = playerEntity;

            controllerButton.Stop();
            keyboardButton.Stop();
        }
        public override void OnExit()
        {
            controllerButton.Stop();
            keyboardButton.Stop();
        }
        public override void Update(GameTime gameTime)
        {
            if (EngineGlobals.inputManager.IsPressed(Globals.backInput) && EngineGlobals.sceneManager.transition == null)
            {
                EngineGlobals.sceneManager.transition = new FadeSceneTransition(null);
            }

            InputComponent inputComponent = EngineGlobals.entityManager.GetLocalPlayer().GetComponent<InputComponent>();
            if (inputComponent != null)
            {
                InputMethod inputMethod = inputComponent.input;
                if (inputMethod != null)
                {
                    InputItem inputItem = inputMethod.button1;
                    if (inputItem != null) {
                        if (EngineGlobals.inputManager.IsPressed(inputMethod.button1) && EngineGlobals.sceneManager.transition == null)
                            EngineGlobals.sceneManager.transition = new FadeSceneTransition(Globals.gameScene);
                    }
                }
            }

            if (EngineGlobals.inputManager.IsPressed(KeyboardInput.Enter))
            {
                keyboardImage.Alpha = 1.0f;
                controllerImage.Alpha = 0.2f;
                EngineGlobals.entityManager.GetLocalPlayer().GetComponent<InputComponent>().input = Engine.Inputs.keyboard;
            }

            if (EngineGlobals.inputManager.IsPressed(ControllerInput.A))
            {
                keyboardImage.Alpha = 0.2f;
                controllerImage.Alpha = 1.0f;
                EngineGlobals.entityManager.GetLocalPlayer().GetComponent<InputComponent>().input = Engine.Inputs.controller;
            }

            if (EngineGlobals.inputManager.IsPressed(ControllerInput.A))
            {
                //controllerButton.reverse = false;
                controllerButton.Reverse(false);
                controllerButton.Reset();
                controllerButton.Play();
            }
            if (EngineGlobals.inputManager.IsReleased(ControllerInput.A))
            {
                //controllerButton.reverse = true;
                controllerButton.Reverse(true);
                controllerButton.Reset();
                controllerButton.Play();
            }
            if (EngineGlobals.inputManager.IsPressed(KeyboardInput.Enter))
            {
                //keyboardButton.reverse = false;
                keyboardButton.Reverse(false);
                keyboardButton.Reset();
                keyboardButton.Play();
            }
            if (EngineGlobals.inputManager.IsReleased(KeyboardInput.Enter))
            {
                //keyboardButton.reverse = true;
                keyboardButton.Reverse(true);
                keyboardButton.Reset();
                keyboardButton.Play();
            }

            testAnimation.Update();
            controllerButton.Update();
            keyboardButton.Update();

        }

        public override void Draw(GameTime gameTime)
        {
            title.Draw();
            controllerImage.Draw();
            keyboardImage.Draw();
            controllerButton.Draw();
            keyboardButton.Draw();
            testAnimation.Draw();
        }

    }

}
