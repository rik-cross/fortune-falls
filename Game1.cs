using AdventureGame.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using S = System.Diagnostics.Debug;
using System.Collections.Generic;

namespace AdventureGame
{
    public class Game1 : Game
    {
        private string _title;
        public Game1(
            string title = "[Engine Name]",
            int screenWidth = 800,
            int screenHeight = 480,
            bool isFullScreen = false,
            bool isBorderless = false,
            bool isMouseVisible = true
        )
        
        {

            // set variables
            _title = title;
            Globals.ScreenWidth = screenWidth;
            Globals.ScreenHeight = screenHeight;
            EngineGlobals.fullscreen = isFullScreen;
            EngineGlobals.borderless = isBorderless;

            Globals.graphics = new GraphicsDeviceManager(this);
            Globals.graphics.PreferredBackBufferWidth = Globals.ScreenWidth;
            Globals.graphics.PreferredBackBufferHeight = Globals.ScreenHeight;
            Globals.graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            IsMouseVisible = isMouseVisible; // todo
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Globals.gameWindow = Window;
            Globals.gameWindow.Title = _title;

            Globals.content = Content;
            Globals.graphicsDevice = GraphicsDevice;
            Globals.spriteBatch = new SpriteBatch(GraphicsDevice);

            // Fullscreen
            Globals.graphics.IsFullScreen = EngineGlobals.fullscreen;
            Globals.gameWindow.IsBorderless = EngineGlobals.borderless;

            Globals.graphics.ApplyChanges();

            // Todo move??
            Globals.sceneRenderTarget = new RenderTarget2D(
                Globals.graphicsDevice,
                Globals.graphicsDevice.PresentationParameters.BackBufferWidth,
                Globals.graphicsDevice.PresentationParameters.BackBufferHeight,
                false,
                Globals.graphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24, 0, RenderTargetUsage.PreserveContents
            );

            // Todo move??
            Globals.lightRenderTarget = new RenderTarget2D(
                Globals.graphicsDevice,
                Globals.graphicsDevice.PresentationParameters.BackBufferWidth,
                Globals.graphicsDevice.PresentationParameters.BackBufferHeight,
                false,
                Globals.graphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24, 0, RenderTargetUsage.PreserveContents
            );

            // Instantiate the managers
            EngineGlobals.inputManager = new InputManager();
            EngineGlobals.componentManager = new ComponentManager();
            EngineGlobals.systemManager = new SystemManager();
            EngineGlobals.entityManager = new EntityManager();
            EngineGlobals.sceneManager = new SceneManager();
            EngineGlobals.playerManager = new PlayerManager();
            EngineGlobals.inventoryManager = new InventoryManager();
            EngineGlobals.soundManager = new SoundManager();
            EngineGlobals.achievementManager = new AchievementManager();

            EngineGlobals.soundManager.Volume = 0.25f;
            EngineGlobals.soundManager.SFXVolume = 1.0f;

            EngineGlobals.log = new Log();

            Globals.dialogueTickSound = Utils.LoadSoundEffect("Sounds/blip.wav");

            if (EngineGlobals.inputManager.IsControllerConnected())
            {
                Globals.IsControllerConnected = true;
                Globals.IsControllerSelected = true;
            }

            // Create and add MenuScene to the scene stack
            if (Globals.TEST)
            {
            }
            else
            {
                EngineGlobals.sceneManager.PreloadScene<MenuScene>();
                //EngineGlobals.sceneManager.ChangeScene<FadeSceneTransition, MenuScene>();
                EngineGlobals.sceneManager.ChangeScene<FadeSceneTransition, SplashScene>();
            }
                

        }

        protected override void Update(GameTime gameTime)
        {
            // exit if the scene manage is empty
            if (EngineGlobals.sceneManager.IsSceneStackEmpty())
                Exit();

            // update the various manager classes
            EngineGlobals.inputManager.Update(gameTime);
            EngineGlobals.sceneManager.Input(gameTime);
            EngineGlobals.sceneManager.Update(gameTime);
            EngineGlobals.playerManager.Update(gameTime);
            EngineGlobals.soundManager.Update(gameTime);
            EngineGlobals.achievementManager.Update(gameTime);
            EngineGlobals.log.Update(gameTime);
            base.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {
            EngineGlobals.sceneManager.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}