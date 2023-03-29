using AdventureGame.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended;

using System;
using System.Collections.Generic;
using S = System.Diagnostics.Debug;


namespace AdventureGame
{
    public class MenuScene : Engine.Scene
    {
        private Engine.Text _title;
        private Engine.Image keyboardImage;
        private Engine.Image controllerImage;
        private Engine.Image inputImage;
        private Engine.Text inputText;

        private Engine.Camera camera;
        private Engine.Entity mainMenuPlayer;

        public void LoadGameScene()
        {
            //Vector2 playerPosition = new Vector2(20, 760);
            Vector2 playerPosition = new Vector2(220, 170);

            // Add the MenuScene to the scene stack
            EngineGlobals.sceneManager.SetActiveScene<GameScene>(
                removeCurrentSceneFromStack: false, unloadCurrentScene: false);

            EngineGlobals.sceneManager.SetPlayerScene<GameScene>(playerPosition);
        }
        public void LoadOptionsScene()
        {
            EngineGlobals.sceneManager.SetActiveScene<OptionsScene>(applyTransition: true, removeCurrentSceneFromStack: false, unloadCurrentScene: false);
        }
        public void LoadCreditsScene()
        {
            EngineGlobals.sceneManager.SetActiveScene<CreditsScene>(applyTransition: true, removeCurrentSceneFromStack: false, unloadCurrentScene: false);
        }
        public void UnloadMenuScene()
        {
            EngineGlobals.sceneManager.RemoveScene(this, applyTransition: true);
        }

        public MenuScene()
        {

            UIButton.drawMethod = UICustomisations.DrawButton;

            AddMap("Maps/Map_MainMenu");
            
            camera = new Engine.Camera(
                    name: "main",
                    size: new Vector2(Globals.ScreenWidth, Globals.ScreenHeight),
                    //worldPosition: new Vector2(-1200, -870),
                    zoom: 4.0f,
                    backgroundColour: Color.DarkSlateBlue
                );


            camera.SetWorldPosition(new Vector2(1300, 830), instant: true);
            //camera.zoomIncrement = 0.005f;
            //camera.SetZoom(3.0f);
            //AddCamera(n);
            CameraList.Add(camera);
            
            LightLevel = 1.0f;


            mainMenuPlayer = EngineGlobals.entityManager.CreateEntity();

            mainMenuPlayer.AddComponent(new Engine.TransformComponent(new Vector2(1140, 820), new Vector2(96,64)));

            Engine.SpriteSheet playerSpriteSheet = new Engine.SpriteSheet("Characters/Players/spr_waiting_strip9", new Vector2(96,64));
            Engine.SpriteComponent spriteComponent = mainMenuPlayer.AddComponent<SpriteComponent>(new Engine.SpriteComponent(playerSpriteSheet, 0, 0));
            spriteComponent.AddSprite("fishing", playerSpriteSheet, 0, 0, 3);

            mainMenuPlayer.State = "fishing";

            AddEntity(mainMenuPlayer);

            // title text
            _title = new Engine.Text(
                caption: "Adventure Game",
                font: Theme.FontTitle,
                colour: Color.White,
                anchor: Anchor.TopCenter,
                padding: new Padding(top: 100),
                outline: true,
                outlineColour: Color.Black,
                outlineThickness: 8
            );

            // title text
            inputText = new Engine.Text(
                caption: "Keyboard controls",
                font: Theme.FontSecondary,
                colour: Color.White,
                anchor: Anchor.BottomLeft,
                padding: new Padding(bottom: 0, left: 15),
                outline: true,
                outlineColour: Color.Black,
                outlineThickness: 4
            );

            UIMenu.AddUIElement(
                new UIButton(
                    position: new Vector2((Globals.ScreenWidth / 2) - 60, Globals.ScreenHeight - 300),
                    size: new Vector2(120, 45),
                    text: "Start",
                    textColour: Color.White,
                    outlineColour: Color.White,
                    outlineThickness: 2,
                    backgroundColour: Color.DarkSlateGray,
                    func: LoadGameScene
                )
            );

            UIMenu.AddUIElement(
                new UIButton(
                    position: new Vector2((Globals.ScreenWidth / 2) - 60, Globals.ScreenHeight - 250),
                    size: new Vector2(120, 45),
                    text: "Test",
                    textColour: Color.White,
                    outlineColour: Color.White,
                    outlineThickness: 2,
                    backgroundColour: Color.DarkSlateGray,
                    func: null
                )
            );

            UIMenu.AddUIElement(
                new UIButton(
                    position: new Vector2((Globals.ScreenWidth / 2) - 60, Globals.ScreenHeight - 200),
                    size: new Vector2(120, 45),
                    text: "Options",
                    textColour: Color.White,
                    outlineColour: Color.White,
                    outlineThickness: 2,
                    backgroundColour: Color.DarkSlateGray,
                    func: LoadOptionsScene
                )
            );

            UIMenu.AddUIElement(
                new UIButton(
                    position: new Vector2((Globals.ScreenWidth / 2) - 60, Globals.ScreenHeight - 150),
                    size: new Vector2(120, 45),
                    text: "Credits",
                    textColour: Color.White,
                    outlineColour: Color.White,
                    outlineThickness: 2,
                    backgroundColour: Color.DarkSlateGray,
                    func: LoadCreditsScene
                )
            );

            UIMenu.AddUIElement(
                new UIButton(
                    position: new Vector2((Globals.ScreenWidth / 2) - 60, Globals.ScreenHeight - 100),
                    size: new Vector2(120, 45),
                    text: "Quit",
                    textColour: Color.White,
                    outlineColour: Color.White,
                    outlineThickness: 2,
                    backgroundColour: Color.DarkSlateGray,
                    func: UnloadMenuScene
                )
            );


            // control images
            controllerImage = new Engine.Image(
                Globals.content.Load<Texture2D>("X360"),
                size: new Vector2(118, 76),
                anchor: Anchor.BottomLeft,
                padding: new Padding(bottom: 30, left: 30)
            );
            keyboardImage = new Engine.Image(
                Globals.content.Load<Texture2D>("Keyboard"),
                size: new Vector2(198, 63),
                anchor: Anchor.BottomLeft,
                padding: new Padding(bottom: 30, left: 30)
            );

        }

        public override void OnEnter()
        {
            EngineGlobals.soundManager.PlaySongFade(Globals.content.Load<Song>("Music/citadel"));

            if (EngineGlobals.entityManager.GetLocalPlayer().GetComponent<InputComponent>().topControllerLabel == "dialogue")
            {
                EngineGlobals.entityManager.GetLocalPlayer().GetComponent<InputComponent>().Pop();
                EngineGlobals.entityManager.GetLocalPlayer().GetComponent<InputComponent>().topControllerLabel = "";
            }
            EngineGlobals.entityManager.GetLocalPlayer().Reset();
            EngineGlobals.entityManager.GetLocalPlayer().RemoveComponent<EmoteComponent>();
            EngineGlobals.entityManager.GetLocalPlayer().GetComponent<DialogueComponent>().dialoguePages.Clear();
            EngineGlobals.entityManager.GetLocalPlayer().GetComponent<DialogueComponent>().alpha.Set(0.0);

            if (EngineGlobals.entityManager.GetLocalPlayer().GetComponent<InputComponent>().input == Engine.Inputs.keyboard)
                //inputImage = keyboardImage;
                inputText.Caption = "Keyboard";
            else
                //inputImage = controllerImage;
                inputText.Caption = "Controller";

        }
        public override void OnExit()
        {

        }
        public override void Input(GameTime gameTime)
        {
            if (EngineGlobals.inputManager.IsPressed(Globals.backInput))
            {
                EngineGlobals.sceneManager.RemoveScene(this, applyTransition: true);

                // Handle exit game logic here?
            }




        }
        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(GameTime gameTime)
        {
            //titleImage.Draw();
            _title.Draw();
            //inputImage.Draw();
            inputText.Draw();
        }

    }

}
