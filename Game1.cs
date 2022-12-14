using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using AdventureGame.Engine;

namespace AdventureGame
{
    public class Game1 : Game
    {

        public Game1()
        {
            Globals.graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
            // Globals.graphics.HardwareModeSwitch = false; // Fix for fullscreen issues
            // https://community.monogame.net/t/monogame-full-screen-mouse-y-position-wrong/11387/3
        }

        protected override void Initialize()
        {
            Globals.graphics.PreferredBackBufferWidth = Globals.ScreenWidth;
            Globals.graphics.PreferredBackBufferHeight = Globals.ScreenHeight;
            Globals.graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Globals.content = Content;
            Globals.graphicsDevice = GraphicsDevice;
            Globals.spriteBatch = new SpriteBatch(GraphicsDevice);

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
            Engine.Entity playerEntity = PlayerEntity.Create(20, 760, playerId);

            // Create scenes
            //Globals.menuScene = new MenuScene();
            //Globals.gameScene = new GameScene();
            //Globals.homeScene = new HomeScene();
            //Globals.beachScene = new BeachScene();

            //EngineGlobals.sceneManager.Transition = new FadeSceneTransition(Globals.menuScene);
            //EngineGlobals.sceneManager.LoadScene<MenuScene>(false);
            EngineGlobals.sceneManager.SetActiveScene<MenuScene>();
        }

        protected override void Update(GameTime gameTime)
        {
            if (EngineGlobals.sceneManager.IsSceneListEmpty())
                Exit();

            EngineGlobals.inputManager.Update(gameTime);
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
