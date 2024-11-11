﻿using AdventureGame.Engine;
using Microsoft.Xna.Framework;

using System;
using S = System.Diagnostics.Debug;

namespace AdventureGame
{
    public class MenuScene : Engine.Scene
    {
        public Engine.UIButton BtnContinue { get; private set; }

        private Engine.Text _title;
        private Engine.Image _keyboardImage;
        private Engine.Image _controllerImage;
        private Engine.Image _inputImage;
        private Engine.Text _inputText;
        private Engine.Text _versionText;

        private Engine.Camera _camera;
        private Engine.Entity _mainMenuPlayer;
        private int _nextCatch;
        private int _frameOdo;
        private Random _random;


        public override void Init()
        {
            UIButton.drawMethod = UICustomisations.DrawButton;
            LightLevel = 1.0f;

            // Load the map but do not create collision entities
            LoadMap("Maps/Map_Village", false);

            // Create the menu character entities
            CreateMainMenuPlayer();
            CreateOtherCharacters();

            // Add the camera
            _camera = new Engine.Camera(
                name: "main",
                size: new Vector2(Globals.ScreenWidth, Globals.ScreenHeight),
                zoom: 4.0f,
                backgroundColour: Color.Black
            );
            CameraList.Add(_camera);

            // Set the camera to the main menu player
            Vector2 mainMenuPlayerPos = _mainMenuPlayer.GetComponent<TransformComponent>().Position;
            _camera.SetWorldPosition(new Vector2(mainMenuPlayerPos.X + 100, mainMenuPlayerPos.Y + 30), instant: true);

            // Create the player entity
            PlayerEntity.Create(x: 176, y: 1190, 15, 20, idTag: "localPlayer");

            // Preload the Village and PlayerSelect scenes
            EngineGlobals.sceneManager.PreloadScene<VillageScene>();
            EngineGlobals.sceneManager.PreloadScene<PlayerSelectScene>();

            // Set the UI input controls
            Globals.SetCustomUIControls();

            // Game title text
            _title = new Engine.Text(
                caption: "Fortune Falls",
                font: Theme.FontTitle,
                colour: Color.White,
                anchor: Anchor.TopCenter,
                padding: new Padding(top: 100),
                outline: true,
                outlineColour: Color.Black,
                outlineThickness: 8
            );

            // Game version text
            _versionText = new Engine.Text(
                caption: "v0.1",
                font: Theme.FontSecondary,
                colour: Color.White,
                anchor: Anchor.BottomRight,
                padding: new Padding(bottom: 0, right: 15),
                outline: true,
                outlineColour: Color.Black,
                outlineThickness: 4
            );

            // Input method text
            _inputText = new Engine.Text(
                caption: "Keyboard controls",
                font: Theme.FontSecondary,
                colour: Color.White,
                anchor: Anchor.BottomLeft,
                padding: new Padding(bottom: 0, left: 15),
                outline: true,
                outlineColour: Color.Black,
                outlineThickness: 4
            );

            // New game button
            UIMenu.AddUIElement(
                new UIButton(
                    position: new Vector2((Globals.ScreenWidth / 2) - 70, Globals.ScreenHeight - 300),
                    size: new Vector2(140, 45),
                    text: "New Game",
                    textColour: Color.White,
                    outlineColour: Color.White,
                    outlineThickness: 2,
                    backgroundColour: Color.DarkSlateGray,
                    func: LoadNewGameScene
                )
            );

            // Continue game button
            BtnContinue = new UIButton(
                position: new Vector2((Globals.ScreenWidth / 2) - 70, Globals.ScreenHeight - 250),
                size: new Vector2(140, 45),
                text: "Continue",
                textColour: Color.White,
                outlineColour: Color.White,
                outlineThickness: 2,
                backgroundColour: Color.DarkSlateGray,
                func: LoadContinueGameScene,
                active: false
            );
            UIMenu.AddUIElement(BtnContinue);

            // Options button
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

            // Credits button
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

            // Quit button
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

            //// control images
            //_controllerImage = new Engine.Image(
            //    Utils.LoadTexture("UI/xbox360.png"),
            //    size: new Vector2(118, 76),
            //    anchor: Anchor.BottomLeft,
            //    padding: new Padding(bottom: 30, left: 30)
            //);
            //_keyboardImage = new Engine.Image(
            //    Utils.LoadTexture("UI/keyboard.png"),
            //    size: new Vector2(198, 63),
            //    anchor: Anchor.BottomLeft,
            //    padding: new Padding(bottom: 30, left: 30)
            //);

        }

        public void LoadNewGameScene(UIButton button)
        {
            EngineGlobals.soundManager.PlaySongFade(Utils.LoadSong("Music/2_i_passed_the_final_exam_master.ogg"));

            // Check if a new game has already started
            if (Globals.newGame == false)
            {
                Globals.newGame = true;
                EngineGlobals.sceneManager.ResetScene<VillageScene>();

                // Reset the player components
                // todo - move to PlayerManager?
                PlayerEntity.RemoveComponents();
                PlayerEntity.AddComponents();
                //Console.WriteLine(string.Join(", ", EngineGlobals.entityManager.GetLocalPlayer().Components));

                // Reset the player character default sprite
                Globals.playerIndex = 0;
                Globals.playerStr = Globals.allCharacters[0];
                PlayerEntity.UpdateSprites();  // todo - move to PlayerScript

                //// todo - bug: player position needs to be set via transform component
                //// Create player entity
                //Vector2 playerPosition = new Vector2(175, 1190);
                //int playerX = 175;
                //int playerY = 1190;

                //Engine.Entity player = PlayerEntity.Create(x: playerX, y: playerY, 15, 20, idTag: "localPlayer");

                //EngineGlobals.entityManager.SetLocalPlayer(player);

                //player.GetComponent<TransformComponent>().Position = playerPosition;
                ////player.State = "idle_right";
            }

            //PlayerEntity.UpdateSprites();  // todo - move to PlayerScript

            // todo - here or somewhere else?
            // todo - bug: player position needs to be set via transform component
            // Create player entity
            Vector2 playerPosition = new Vector2(176, 1190);
            EngineGlobals.entityManager.GetLocalPlayer().GetComponent<TransformComponent>().Position = playerPosition;

            // Transition to the PlayerSelectScene and load the VillageScene below
            EngineGlobals.sceneManager.ChangeScene<
                NoSceneTransition, VillageScene, PlayerSelectScene>(unloadCurrentScene: false);
        }

        public void LoadContinueGameScene(UIButton button)
        {
            EngineGlobals.soundManager.PlaySongFade(Utils.LoadSong("Music/2_i_passed_the_final_exam_master.ogg"));
            //EngineGlobals.sceneManager.ChangeScene<FadeSceneTransition, VillageScene>(false);

            if (EngineGlobals.sceneManager.SceneBelow != null)
                EngineGlobals.sceneManager.ChangeToSceneBelow<FadeSceneTransition>(false);
            else
                EngineGlobals.sceneManager.ChangeScene<FadeSceneTransition, VillageScene>(false);
        }

        public void LoadOptionsScene(UIButton button)
        {
            EngineGlobals.sceneManager.ChangeScene<FadeSceneTransition, OptionsScene>(false);
        }

        public void LoadCreditsScene(UIButton button)
        {
            EngineGlobals.sceneManager.ChangeScene<FadeSceneTransition, CreditsScene>(false);
        }

        public void UnloadMenuScene(UIButton button)
        {
            EngineGlobals.soundManager.Volume = 0;
            EngineGlobals.soundManager.SFXVolume = 0;
            EngineGlobals.log.visible = false;
            EngineGlobals.sceneManager.ChangeScene<FadeSceneTransition, ExitScene>();
            //EngineGlobals.sceneManager.UnloadAllScenes();
        }

        public override void OnEnter()
        {
            EngineGlobals.soundManager.PlaySong(Utils.LoadSong("Music/1_new_life_master.ogg"));

            if (Globals.IsControllerConnected && Globals.IsControllerSelected)
                //inputImage = keyboardImage;
                _inputText.Caption = "Controller";
            else
                //inputImage = controllerImage;
                _inputText.Caption = "Keyboard (WASD)";

            if (Globals.newGame)
            {
                // Create player entity
                //PlayerEntity.Create(x: 176, y: 1190, 15, 20, idTag: "localPlayer");
            }
            else
            {
                // Re-create the player sprites in case player has changed
                //CreatePlayerSprites();
            }
        }

        public override void OnExit()
        {

        }
        public override void Input(GameTime gameTime)
        {
        }

        public override void Update(GameTime gameTime)
        {
            _frameOdo++;

            //S.WriteLine(frameOdo + " " + nextCatch);
            if (_frameOdo == _nextCatch)
            {
                _frameOdo = 0;
                _nextCatch = _random.Next(1500, 5000);
                _mainMenuPlayer.SetState("caught");
            }
        }

        public override void Draw(GameTime gameTime)
        {
            _title.Draw();
            _inputText.Draw();
            _versionText.Draw();
        }

        // Used to change the player style and re-create the sprites
        public void CreateMainMenuPlayer()
        {
            // Character sprites
            string dir = Globals.characterDir;
            string characterStr = Globals.playerStr;
            string baseStr = Globals.characterBaseStr;
            string toolStr = Globals.characterToolStr;
            string folder = "";
            string keyStr = "";
            Vector2 offset = new Vector2(-41, -21);
            //characterStr = "spikeyhair"; // Testing

            // Create the main menu player entity
            _mainMenuPlayer = EngineGlobals.entityManager.CreateEntity();
            _mainMenuPlayer.AddComponent(new Engine.TransformComponent(
                new Vector2(176, 1190), new Vector2(15, 20)));
            AnimatedSpriteComponent animatedComponent = _mainMenuPlayer.AddComponent<AnimatedSpriteComponent>();
            _mainMenuPlayer.SetState("casting");

            // Randomise the catch time
            _random = new Random();
            _nextCatch = _random.Next(1500, 5000);
            _frameOdo = 0;

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
            animatedComponent.GetAnimatedSprite("casting").OnComplete = (Engine.Entity e) => e.SetState("waiting");

            // Caught
            folder = "CAUGHT/";
            keyStr = "_caught_strip10.png";
            animatedComponent.AddAnimatedSprite(dir + folder + baseStr + keyStr,
                "caught", 0, 9, offset: offset);
            animatedComponent.AddAnimatedSprite(dir + folder + characterStr + keyStr,
                "caught", 0, 9, offset: offset);
            animatedComponent.AddAnimatedSprite(dir + folder + toolStr + keyStr,
                "caught", 0, 9, offset: offset);
            animatedComponent.GetAnimatedSprite("caught").OnComplete = (Engine.Entity e) => e.SetState("casting");

            // Add entity
            AddEntity(_mainMenuPlayer);
        }

        // Create the other characters for the main menu
        public void CreateOtherCharacters()
        {
            // Character sprites
            string dir = Globals.characterDir;
            string characterStr = Globals.playerStr;
            string baseStr = Globals.characterBaseStr;
            string toolStr = Globals.characterToolStr;
            string folder = "";
            string keyStr = "";
            Vector2 offset = new Vector2(-41, -21);

            //
            // Character swimming
            //
            Engine.Entity characterSwimming = EngineGlobals.entityManager.CreateEntity();
            characterSwimming.AddComponent<Engine.TransformComponent>(new Engine.TransformComponent(
                new Vector2(380, 1240),
                new Vector2(15, 20)));

            Engine.AnimatedSpriteComponent animatedSwimming = characterSwimming.AddComponent<AnimatedSpriteComponent>();

            // Swimming
            folder = "SWIMMING/";
            keyStr = "_swimming_strip12.png";
            characterStr = "bowlhair";
            animatedSwimming.AddAnimatedSprite(dir + folder + baseStr + keyStr,
                "swimming", 0, 11, offset: offset, flipH: true);
            animatedSwimming.AddAnimatedSprite(dir + folder + characterStr + keyStr,
                "swimming", 0, 11, offset: offset, flipH: true);
            animatedSwimming.AddAnimatedSprite(dir + folder + toolStr + keyStr,
                "swimming", 0, 11, offset: offset, flipH: true);

            characterSwimming.SetState("swimming");
            AddEntity(characterSwimming);
        }

    }

}
