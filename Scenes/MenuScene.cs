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
        private Engine.Entity mainMenuCharacter1;
        private int nextCatch;
        private int frameOdo;
        private Random r;

        public void LoadGameScene(UIButton button)
        {
            /// todo
            /// check player position is set from here
            ///     if so, change to a variable
            /// use generic methods for scene transition
            /// add error handling in case player scene not set e.g. DropItem
             


            //Vector2 playerPosition = new Vector2(680, 580);
            EngineGlobals.entityManager.GetLocalPlayer().GetComponent<TransformComponent>().Position = new Vector2(680, 580);


            /// todo:
            /// pass the transition class and a single scene class OR a list of scene classes
            /// check how to pass multiple classes of different types
            ///
            /// in the Scene Manager, could it loop through the list of scenes and instantiate each one?
            /// and only create the instance of the transition, then pass or create the scenes

            /*
            EngineGlobals.sceneManager.StartSceneTransition(new FadeSceneTransition(
                    new List<Scene>() { new VillageScene(), new PlayerSelectScene() }
                ));

            /*
            EngineGlobals.sceneManager.StartSceneTransition(
                typeof(FadeSceneTransition2),
                new List<Type>() { typeof(VillageScene), typeof(PlayerSelectScene) }
            );
            */


            //EngineGlobals.sceneManager.StartSceneTransition<FadeSceneTransition2>(
            //    false, typeof(VillageScene), typeof(PlayerSelectScene)
            //);

            // <Scene>
            //EngineGlobals.sceneManager.StartSceneTransition<PauseScene>();

            // <Transition, Scene>
            //EngineGlobals.sceneManager.StartSceneTransition<
            //    FadeSceneTransition2, OptionsScene>();

            // <Transition, Scene, SceneBelow>
            EngineGlobals.sceneManager.StartSceneTransition<
                FadeSceneTransition2, VillageScene, PlayerSelectScene>(false);


            // LoadScene
            // transition type (optional)
            // set active scene (optional)
            // set player scene (optional)

            // LoadScenes
            // ^^^
            // list of scenes for transition


            // Add the MenuScene to the scene stack
            //EngineGlobals.sceneManager.SetActiveScene<VillageScene>(applyTransition: true, unloadCurrentScene: false);
            //EngineGlobals.sceneManager.SetPlayerScene<VillageScene>(playerPosition);

        }

        public void LoadOptionsScene(UIButton button)
        {
            EngineGlobals.sceneManager.StartSceneTransition(new FadeSceneTransition(
                    new List<Scene>() { new OptionsScene() }
                ));
        }
        public void LoadCreditsScene(UIButton button)
        {
            EngineGlobals.sceneManager.StartSceneTransition(new FadeSceneTransition(
                    new List<Scene>() { new CreditsScene() }
                ));
        }
        public void UnloadMenuScene(UIButton button)
        {
            //EngineGlobals.sceneManager.RemoveScene(this, applyTransition: false);
            EngineGlobals.soundManager.Volume = 0;
            EngineGlobals.sceneManager.StartSceneTransition(new FadeSceneTransition(
                    new List<Scene>() {}, numScenesToUnload: 1
                ));
        }

        public MenuScene()
        {
            

            EngineGlobals.DEBUG = false;

            UIButton.drawMethod = UICustomisations.DrawButton;

            AddMap("Maps/Map_MainMenu");
            
            camera = new Engine.Camera(
                    name: "main",
                    size: new Vector2(Globals.ScreenWidth, Globals.ScreenHeight),
                    zoom: 4.0f,
                    backgroundColour: Color.Black
                );

            camera.SetWorldPosition(new Vector2(1280, 864), instant: true);
            //camera.zoomIncrement = 0.005f;
            //camera.SetZoom(3.0f);
            //AddCamera(n);
            CameraList.Add(camera);
            
            LightLevel = 1.0f;


            // Character sprites
            string dir = Globals.characterDir;
            string characterStr = Globals.playerStr;
            string baseStr = Globals.characterBaseStr;
            string toolStr = Globals.characterToolStr;
            string folder = "";
            string keyStr = "";
            Vector2 offset = new Vector2(-41, -21);

            //
            // Player fishing
            //
            mainMenuPlayer = EngineGlobals.entityManager.CreateEntity();
            mainMenuPlayer.AddComponent(new Engine.TransformComponent(new Vector2(1184, 870), new Vector2(15,20)));
            //mainMenuPlayer.AddComponent(new Engine.ColliderComponent(new Vector2(15, 20)));

            Engine.AnimatedSpriteComponent animatedComponent = mainMenuPlayer.AddComponent<AnimatedSpriteComponent>();
            CreatePlayerSprites();

            //string dir = "Characters/Players/long_hair/";
            //Vector2 offset = new Vector2(-41, -21);

            //Engine.SpriteComponent spriteComponent = mainMenuPlayer.AddComponent<Engine.SpriteComponent>();
            //spriteComponent.AddAnimatedSprite(dir + "spr_waiting_strip9", "waiting", 0, 8, offset: offset);
            //spriteComponent.AddAnimatedSprite(dir + "spr_casting_strip15", "casting", 0, 14, offset: offset);
            //spriteComponent.AddAnimatedSprite(dir + "spr_caught_strip10", "caught", 0, 9, offset: offset);

            //spriteComponent.GetSprite("casting").OnComplete = (Engine.Entity e) => e.State = "waiting";
            //spriteComponent.GetSprite("caught").OnComplete = (Engine.Entity e) => e.State = "casting";

            mainMenuPlayer.State = "casting";

            r = new Random();
            nextCatch = r.Next(1500, 5000);
            frameOdo = 0;

            AddEntity(mainMenuPlayer);


            //
            // Character swimming
            //
            mainMenuCharacter1 = EngineGlobals.entityManager.CreateEntity();
            mainMenuCharacter1.AddComponent<Engine.TransformComponent>(new Engine.TransformComponent(new Vector2(1400, 920), new Vector2(15, 20)));
            //mainMenuCharacter1.AddComponent(new Engine.ColliderComponent(new Vector2(15, 20)));

            Engine.AnimatedSpriteComponent animatedComponentC1 = mainMenuCharacter1.AddComponent<AnimatedSpriteComponent>();

            // Swimming
            folder = "SWIMMING/";
            keyStr = "_swimming_strip12.png";
            characterStr = "bowlhair";
            animatedComponentC1.AddAnimatedSprite(dir + folder + baseStr + keyStr,
                "swimming", 0, 11, offset: offset, flipH: true);
            animatedComponentC1.AddAnimatedSprite(dir + folder + characterStr + keyStr,
                "swimming", 0, 11, offset: offset, flipH: true);
            animatedComponentC1.AddAnimatedSprite(dir + folder + toolStr + keyStr,
                "swimming", 0, 11, offset: offset, flipH: true);

            //string dirPlayer2 = "Characters/Players/";
            //Vector2 offsetPlayer2 = new Vector2(-41, -21);

            //Engine.SpriteComponent spriteComponentP2 = mainMenuCharacter1.AddComponent<Engine.SpriteComponent>();
            //spriteComponentP2.AddAnimatedSprite(dirPlayer2 + "spr_swimming_strip12", "swimming", 0, 11, offset: offsetPlayer2, flipH: true);

            mainMenuCharacter1.State = "swimming";

            AddEntity(mainMenuCharacter1);


            // title text
            _title = new Engine.Text(
                caption: "Fortuna",
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
                    position: new Vector2((Globals.ScreenWidth / 2) - 70, Globals.ScreenHeight - 300),
                    size: new Vector2(140, 45),
                    text: "New Game",
                    textColour: Color.White,
                    outlineColour: Color.White,
                    outlineThickness: 2,
                    backgroundColour: Color.DarkSlateGray,
                    func: LoadGameScene
                )
            );

            UIMenu.AddUIElement(
                new UIButton(
                    position: new Vector2((Globals.ScreenWidth / 2) - 70, Globals.ScreenHeight - 250),
                    size: new Vector2(140, 45),
                    text: "Continue",
                    textColour: Color.White,
                    outlineColour: Color.White,
                    outlineThickness: 2,
                    backgroundColour: Color.DarkSlateGray,
                    func: null,
                    active: false
                )
            );

            UIMenu.AddUIElement(
                new UIButton(
                    position: new Vector2((Globals.ScreenWidth / 2) - 70, Globals.ScreenHeight - 200),
                    size: new Vector2(140, 45),
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
                    position: new Vector2((Globals.ScreenWidth / 2) - 70, Globals.ScreenHeight - 150),
                    size: new Vector2(140, 45),
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
                    position: new Vector2((Globals.ScreenWidth / 2) - 70, Globals.ScreenHeight - 100),
                    size: new Vector2(140, 45),
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
                Utils.LoadTexture("UI/xbox360.png"),
                size: new Vector2(118, 76),
                anchor: Anchor.BottomLeft,
                padding: new Padding(bottom: 30, left: 30)
            );
            keyboardImage = new Engine.Image(
                Utils.LoadTexture("UI/keyboard.png"),
                size: new Vector2(198, 63),
                anchor: Anchor.BottomLeft,
                padding: new Padding(bottom: 30, left: 30)
            );

        }

        public override void OnEnter()
        {
            EngineGlobals.DEBUG = false;
            Globals.newGame = true;
            
            EngineGlobals.soundManager.PlaySongFade(Utils.LoadSong("Music/citadel.ogg"));

            if (EngineGlobals.entityManager.GetLocalPlayer().GetComponent<InputComponent>().topControllerLabel == "dialogue")
            {
                EngineGlobals.entityManager.GetLocalPlayer().GetComponent<InputComponent>().Pop();
                EngineGlobals.entityManager.GetLocalPlayer().GetComponent<InputComponent>().topControllerLabel = "";
            }
            EngineGlobals.entityManager.GetLocalPlayer().Reset();
            EngineGlobals.entityManager.GetLocalPlayer().RemoveComponent<EmoteComponent>();
            EngineGlobals.entityManager.GetLocalPlayer().RemoveComponent<AnimatedEmoteComponent>();
            EngineGlobals.entityManager.GetLocalPlayer().GetComponent<DialogueComponent>().dialoguePages.Clear();
            EngineGlobals.entityManager.GetLocalPlayer().GetComponent<DialogueComponent>().alpha.Set(0.0);
            EngineGlobals.entityManager.GetLocalPlayer().State = "idle_right";

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
            //S.WriteLine(EngineGlobals.sceneManager._sceneStack.Count + "  " + EngineGlobals.sceneManager.SceneList.Count);
        }

        public override void Draw(GameTime gameTime)
        {
            _title.Draw();
            inputText.Draw();
            versionText.Draw();
        }

        // Used to change the player style and re-create the sprites
        public void CreatePlayerSprites()
        {
            AnimatedSpriteComponent animatedComponent = mainMenuPlayer.GetComponent<AnimatedSpriteComponent>();

            // Character sprites
            string dir = Globals.characterDir;
            string characterStr = Globals.playerStr;
            string baseStr = Globals.characterBaseStr;
            string toolStr = Globals.characterToolStr;
            string folder = "";
            string keyStr = "";
            Vector2 offset = new Vector2(-41, -21);

            // Testing
            //characterStr = "spikeyhair";

            // Waiting
            folder = "WAITING/";
            keyStr = "_waiting_strip9.png";
            animatedComponent.AddAnimatedSprite(dir + folder + baseStr + keyStr,
                "waiting", 0, 8, offset: offset);
            animatedComponent.AddAnimatedSprite(dir + folder + characterStr + keyStr,
                "waiting", 0, 8, offset: offset);
            animatedComponent.AddAnimatedSprite(dir + folder + toolStr + keyStr,
                "waiting", 0, 8, offset: offset);

            // Casting
            folder = "CASTING/";
            keyStr = "_casting_strip15.png";
            animatedComponent.AddAnimatedSprite(dir + folder + baseStr + keyStr,
                "casting", 0, 14, offset: offset);
            animatedComponent.AddAnimatedSprite(dir + folder + characterStr + keyStr,
                "casting", 0, 14, offset: offset);
            animatedComponent.AddAnimatedSprite(dir + folder + toolStr + keyStr,
                "casting", 0, 14, offset: offset);
            animatedComponent.GetAnimatedSprite("casting").OnComplete = (Engine.Entity e) => e.State = "waiting";

            // Caught
            folder = "CAUGHT/";
            keyStr = "_caught_strip10.png";
            animatedComponent.AddAnimatedSprite(dir + folder + baseStr + keyStr,
                "caught", 0, 9, offset: offset);
            animatedComponent.AddAnimatedSprite(dir + folder + characterStr + keyStr,
                "caught", 0, 9, offset: offset);
            animatedComponent.AddAnimatedSprite(dir + folder + toolStr + keyStr,
                "caught", 0, 9, offset: offset);
            animatedComponent.GetAnimatedSprite("caught").OnComplete = (Engine.Entity e) => e.State = "casting";
        }

    }

}
