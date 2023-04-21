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
        private Engine.Text versionText;

        private Engine.Camera camera;
        private Engine.Entity mainMenuPlayer;
        private Engine.Entity mainMenuPlayer2;
        private int nextCatch;
        private int frameOdo;
        private Random r;
        public void LoadGameScene(UIButton button)
        {
            //Vector2 playerPosition = new Vector2(20, 760);
            Vector2 playerPosition = new Vector2(220, 170);

            // Add the MenuScene to the scene stack
            EngineGlobals.sceneManager.SetActiveScene<GameScene>(
                removeCurrentSceneFromStack: false, unloadCurrentScene: false);

            EngineGlobals.sceneManager.SetPlayerScene<GameScene>(playerPosition);

        }
        public void LoadOptionsScene(UIButton button)
        {
            EngineGlobals.sceneManager.SetActiveScene<OptionsScene>(applyTransition: true, removeCurrentSceneFromStack: false, unloadCurrentScene: false);
        }
        public void LoadCreditsScene(UIButton button)
        {
            EngineGlobals.sceneManager.SetActiveScene<CreditsScene>(applyTransition: true, removeCurrentSceneFromStack: false, unloadCurrentScene: false);
        }
        public void UnloadMenuScene(UIButton button)
        {
            EngineGlobals.sceneManager.RemoveScene(this, applyTransition: true);
        }

        public void SwitchToWaiting(Engine.Entity entity)
        {
            entity.State = "waiting";
        }
        public void SwitchToCasting(Engine.Entity entity)
        {
            entity.State = "casting";
        }

        public MenuScene()
        {

            EngineGlobals.DEBUG = false;

            UIButton.drawMethod = UICustomisations.DrawButton;

            AddMap("Maps/Map_MainMenu");
            
            camera = new Engine.Camera(
                    name: "main",
                    size: new Vector2(Globals.ScreenWidth, Globals.ScreenHeight),
                    //worldPosition: new Vector2(-1200, -870),
                    zoom: 4.0f,
                    backgroundColour: Color.DarkSlateBlue
                );


            camera.SetWorldPosition(new Vector2(1280, 864), instant: true);
            //camera.zoomIncrement = 0.005f;
            //camera.SetZoom(3.0f);
            //AddCamera(n);
            CameraList.Add(camera);
            
            LightLevel = 1.0f;


            mainMenuPlayer = EngineGlobals.entityManager.CreateEntity();
            mainMenuPlayer.AddComponent(new Engine.TransformComponent(new Vector2(1184, 870), new Vector2(15,20)));
            mainMenuPlayer.AddComponent(new Engine.ColliderComponent(new Vector2(15,20)));

            Engine.SpriteSheet waitingSpriteSheet = new Engine.SpriteSheet("Characters/Players/long_hair/spr_waiting_strip9", new Vector2(96,64));
            Engine.SpriteSheet castingSpriteSheet = new Engine.SpriteSheet("Characters/Players/long_hair/spr_casting_strip15", new Vector2(96, 64));
            Engine.SpriteSheet caughtSpriteSheet = new Engine.SpriteSheet("Characters/Players/long_hair/spr_caught_strip10", new Vector2(96, 64));
            Engine.SpriteSheet swimSpriteSheet = new Engine.SpriteSheet("Characters/Players/spr_swimming_strip12", new Vector2(96, 64));

            Engine.SpriteComponent spriteComponent = mainMenuPlayer.AddComponent<Engine.SpriteComponent>(new Engine.SpriteComponent(waitingSpriteSheet, 0, 0));
            spriteComponent.AddSprite("waiting", waitingSpriteSheet, 0, 0, 8);
            spriteComponent.GetSprite("waiting").offset = new Vector2(-41, -21);
            //spriteComponent.GetSprite("waiting").flipH = true;
            //spriteComponent.GetSprite("waiting").flipV = true;
            //spriteComponent.GetSprite("waiting").size = new Vector2(20,20);

            //Engine.SpriteComponent spriteComponent = (Engine.SpriteComponent)mainMenuPlayer.AddComponent(new Engine.SpriteComponent(waitingSpriteSheet, 0, 0));
            spriteComponent.AddSprite("casting", castingSpriteSheet, 0, 0, 14);
            spriteComponent.GetSprite("casting").offset = new Vector2(-41, -21);
            //spriteComponent.GetSprite("casting").loop = false;

            spriteComponent.GetSprite("casting").OnComplete = SwitchToWaiting;

            spriteComponent.AddSprite("caught", caughtSpriteSheet, 0, 0, 9);
            spriteComponent.GetSprite("caught").offset = new Vector2(-41, -21);
            //spriteComponent.GetSprite("caught").loop = false;
            spriteComponent.GetSprite("caught").OnComplete = SwitchToCasting;

            spriteComponent.AddSprite("swimming", swimSpriteSheet, 0, 0, 11);

            mainMenuPlayer.State = "casting";

            r = new Random();
            nextCatch = (int)r.Next(1500, 5000);
            frameOdo = 0;

            AddEntity(mainMenuPlayer);




            mainMenuPlayer2 = EngineGlobals.entityManager.CreateEntity();

            mainMenuPlayer2.AddComponent<Engine.TransformComponent>(new Engine.TransformComponent(new Vector2(1400, 920), new Vector2(15, 20)));
            mainMenuPlayer2.AddComponent(new Engine.ColliderComponent(new Vector2(15, 20)));

            Engine.SpriteSheet waitingSpriteSheet2 = new Engine.SpriteSheet("Characters/Players/long_hair/spr_waiting_strip9", new Vector2(96, 64));
            Engine.SpriteSheet castingSpriteSheet2 = new Engine.SpriteSheet("Characters/Players/spr_casting_strip15", new Vector2(96, 64));
            Engine.SpriteSheet caughtSpriteSheet2 = new Engine.SpriteSheet("Characters/Players/spr_caught_strip10", new Vector2(96, 64));
            Engine.SpriteSheet swimSpriteSheet2 = new Engine.SpriteSheet("Characters/Players/spr_swimming_strip12", new Vector2(96, 64));

            Engine.SpriteComponent spriteComponent2 = mainMenuPlayer2.AddComponent<Engine.SpriteComponent>(new Engine.SpriteComponent(waitingSpriteSheet, 0, 0));
            spriteComponent2.AddSprite("swimming", swimSpriteSheet2, 0, 0, 11);
            spriteComponent2.GetSprite("swimming").offset = new Vector2(-41, -21);
            spriteComponent2.GetSprite("swimming").flipH = true;
            mainMenuPlayer2.State = "swimming";

            AddEntity(mainMenuPlayer2);





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

            // title text
            versionText = new Engine.Text(
                caption: "v0.0",
                font: Theme.FontSecondary,
                colour: Color.White,
                anchor: Anchor.BottomRight,
                padding: new Padding(bottom: 0, right: 15),
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
            frameOdo++;

            //S.WriteLine(frameOdo + " " + nextCatch);
            if (frameOdo == nextCatch)
            {
                frameOdo = 0;
                nextCatch = (int)r.Next(1500, 5000);
                mainMenuPlayer.State = "caught";
            }
        }

        public override void Draw(GameTime gameTime)
        {
            //titleImage.Draw();
            _title.Draw();
            //inputImage.Draw();
            inputText.Draw();
            versionText.Draw();
        }

    }

}
