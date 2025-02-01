using AdventureGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using S = System.Diagnostics.Debug;
using System.Collections.Generic;
using System;

namespace Engine
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        private string _title;
        public Game(
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
            EngineGlobals.ScreenWidth = screenWidth;
            EngineGlobals.ScreenHeight = screenHeight;
            EngineGlobals.fullscreen = isFullScreen;
            EngineGlobals.borderless = isBorderless;

            EngineGlobals.graphics = new GraphicsDeviceManager(this);
            EngineGlobals.graphics.PreferredBackBufferWidth = EngineGlobals.ScreenWidth;
            EngineGlobals.graphics.PreferredBackBufferHeight = EngineGlobals.ScreenHeight;
            EngineGlobals.graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            IsMouseVisible = isMouseVisible;

            

        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            EngineGlobals.gameWindow = Window;
            EngineGlobals.gameWindow.Title = _title;

            EngineGlobals.content = Content;
            EngineGlobals.graphicsDevice = GraphicsDevice;
            EngineGlobals.spriteBatch = new SpriteBatch(GraphicsDevice);

            // Fullscreen
            EngineGlobals.graphics.IsFullScreen = EngineGlobals.fullscreen;
            EngineGlobals.gameWindow.IsBorderless = EngineGlobals.borderless;

            EngineGlobals.graphics.ApplyChanges();

            // Todo move??
            EngineGlobals.sceneRenderTarget = new RenderTarget2D(
                EngineGlobals.graphicsDevice,
                EngineGlobals.graphicsDevice.PresentationParameters.BackBufferWidth,
                EngineGlobals.graphicsDevice.PresentationParameters.BackBufferHeight,
                false,
                EngineGlobals.graphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24, 0, RenderTargetUsage.PreserveContents
            );

            // Todo move??
            EngineGlobals.lightRenderTarget = new RenderTarget2D(
                EngineGlobals.graphicsDevice,
                EngineGlobals.graphicsDevice.PresentationParameters.BackBufferWidth,
                EngineGlobals.graphicsDevice.PresentationParameters.BackBufferHeight,
                false,
                EngineGlobals.graphicsDevice.PresentationParameters.BackBufferFormat,
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