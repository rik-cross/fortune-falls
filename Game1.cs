using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using AdventureGame.Engine;
using S = System.Diagnostics.Debug;

namespace AdventureGame
{
    public class Game1 : Game
    {

        public Game1()
        {
            Globals.graphics = new GraphicsDeviceManager(this);
            //Globals.gameWindow = GameWindow.Create(this, Globals.ScreenWidth, Globals.ScreenHeight);
            //Globals.gameWindow = this.Window;
               
            //this.Window.IsBorderless = false;

            Content.RootDirectory = "Content";
            IsMouseVisible = false;
            // Globals.graphics.HardwareModeSwitch = false; // Fix for fullscreen issues
            // https://community.monogame.net/t/monogame-full-screen-mouse-y-position-wrong/11387/3
        }

        protected override void Initialize()
        {
            //Globals.ScreenWidth = 1280;//GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            //Globals.ScreenHeight = 720;//GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            //S.WriteLine(System.IO.Directory.GetCurrentDirectory());

            Globals.graphics.PreferredBackBufferWidth = Globals.ScreenWidth;
            Globals.graphics.PreferredBackBufferHeight = Globals.ScreenHeight;
            
            Globals.graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Globals.gameWindow = Window;
            Globals.content = Content;
            Globals.graphicsDevice = GraphicsDevice;
            Globals.spriteBatch = new SpriteBatch(GraphicsDevice);

            // Fullscreen
            //Globals.graphics.IsFullScreen = true;

            // Borderless window
            //Globals.gameWindow.IsBorderless = true;
            Globals.graphics.ApplyChanges();

            // Move??
            Globals.sceneRenderTarget = new RenderTarget2D(
                Globals.graphicsDevice,
                Globals.graphicsDevice.PresentationParameters.BackBufferWidth,
                Globals.graphicsDevice.PresentationParameters.BackBufferHeight,
                false,
                Globals.graphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24, 0, RenderTargetUsage.PreserveContents
            );
            // Move??
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

            // Move to another accessible place for the menu and scenes?
            Globals.dialogueTickSound = Globals.content.Load<SoundEffect>("Sounds/blip");
            //Globals.playerSpriteSheet = new Engine.SpriteSheet("playerSpriteSheet", 26, 36);

            // Leave here or move to scene that checks if player already exists?
            // Create player entity
            string playerId = "localPlayer";
            //Engine.Entity playerEntity = PlayerEntity.Create(x: 0, y: 0, idTag: playerId);
            Engine.Entity playerEntity = PlayerEntity2.Create(x: 0, y: 0, idTag: playerId);

            // Create the menu and set as the active scene
            //EngineGlobals.sceneManager.SetActiveScene<MenuScene>();

            if (EngineGlobals.inputManager.IsControllerConnected())
                EngineGlobals.entityManager.GetLocalPlayer().GetComponent<InputComponent>().input = Engine.Inputs.controller;
            else
                EngineGlobals.entityManager.GetLocalPlayer().GetComponent<InputComponent>().input = Engine.Inputs.keyboard;

            Vector2 playerPosition = new Vector2(230, 30);

            // Add the MenuScene to the scene stack
            EngineGlobals.sceneManager.SetActiveScene<MenuScene>(unloadCurrentScene: false);

            // Todo - remove from here, load player on appropriate scene
            //
            //EngineGlobals.sceneManager.SetPlayerScene<GameScene>(playerPosition);


        }

        protected override void Update(GameTime gameTime)
        {
            if (EngineGlobals.sceneManager.IsSceneStackEmpty())
                Exit();

            EngineGlobals.inputManager.Update(gameTime);

            EngineGlobals.sceneManager.Input(gameTime);
            EngineGlobals.sceneManager.Update(gameTime);
            
            EngineGlobals.soundManager.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            EngineGlobals.sceneManager.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
