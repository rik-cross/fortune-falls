using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using AdventureGame.Engine;

using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

namespace AdventureGame
{
    public class Game1 : Game
    {
        public EntityManager entityManager; // REMOVE? reference from EngineGlobals
        public SceneManager sceneManager;

        public static void doorOnCollisionEnter(Entity thisEntity, Entity otherEntity, float distance)
        {
            if (otherEntity.HasTag("player"))
            {
                EngineGlobals.sceneManager.PopScene();
                EngineGlobals.entityManager.GetEntityByTag("player").GetComponent<Engine.TransformComponent>().position = new Vector2(525, 900);
                EngineGlobals.sceneManager.PushScene(new BeachScene());
                EngineGlobals.sceneManager.GetTopScene().GetCameraByName("main").SetWorldPosition(new Vector2(525, 900), instant: true);
            }
        }

        public static void beachOnCollisionEnter(Entity thisEntity, Entity otherEntity, float distance)
        {
            if (otherEntity.HasTag("player"))
            {
                EngineGlobals.sceneManager.PopScene();
                EngineGlobals.entityManager.GetEntityByTag("player").GetComponent<Engine.TransformComponent>().position = new Vector2(260, 60);
                EngineGlobals.sceneManager.PushScene(new GameScene());
                EngineGlobals.sceneManager.GetTopScene().GetCameraByName("main").SetWorldPosition(new Vector2(260, 60), instant: true);
            }
        }

        public static void homeOnCollisionEnter(Entity thisEntity, Entity otherEntity, float distance)
        {
            if (otherEntity.HasTag("player"))
            {
                EngineGlobals.sceneManager.PopScene();
                EngineGlobals.entityManager.GetEntityByTag("player").GetComponent<Engine.TransformComponent>().position = new Vector2(85, 90);
                EngineGlobals.sceneManager.PushScene(new GameScene());
                EngineGlobals.sceneManager.GetTopScene().GetCameraByName("main").SetWorldPosition(new Vector2(85, 90), instant: true);
            }
        }

        public Game1()
        {
            Globals.graphics = new GraphicsDeviceManager(this);
            
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            Globals.graphics.PreferredBackBufferWidth = Globals.WIDTH;
            Globals.graphics.PreferredBackBufferHeight = Globals.HEIGHT;
            Globals.graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Globals.content = this.Content;
            Globals.graphicsDevice = GraphicsDevice;
            
            Globals.spriteBatch = new SpriteBatch(GraphicsDevice);

            // load the fonts
            Globals.font = Content.Load<SpriteFont>("File");
            Globals.fontSmall = Content.Load<SpriteFont>("small");

            Globals.sceneRenderTarget = new RenderTarget2D(
                Globals.graphicsDevice,
                Globals.graphicsDevice.PresentationParameters.BackBufferWidth,
                Globals.graphicsDevice.PresentationParameters.BackBufferHeight,
                false,
                Globals.graphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24, 0, RenderTargetUsage.PreserveContents
            );

            Globals.lightRenderTarget = new RenderTarget2D(
                Globals.graphicsDevice,
                Globals.graphicsDevice.PresentationParameters.BackBufferWidth,
                Globals.graphicsDevice.PresentationParameters.BackBufferHeight,
                false,
                Globals.graphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24, 0, RenderTargetUsage.PreserveContents
            );

            Globals.playerSpriteSheet = new Engine.SpriteSheet(Globals.content.Load<Texture2D>("playerSpriteSheet"), new Vector2(26, 36));
            Globals.candleSpriteSheet = new Engine.SpriteSheet(Globals.content.Load<Texture2D>("candleTest"), new Vector2(32, 32));
            Globals.enemySpriteSheet = new Engine.SpriteSheet(Globals.content.Load<Texture2D>("spriteEnemy"), new Vector2(65, 50));

            // Instantiate the managers
            EngineGlobals.inputManager = new InputManager();
            EngineGlobals.componentManager = new ComponentManager();
            EngineGlobals.systemManager = new SystemManager();
            EngineGlobals.entityManager = new EntityManager();
            EngineGlobals.sceneManager = new SceneManager();

            //
            // create entities
            //

            // player entity
            Engine.Entity playerEntity = PlayerEntity.Create(100, 100); // opposites (180, 350)
            // home entity
            Engine.Entity homeEntity = HomeEntity.Create(50, 20);
            // home light entity
            Engine.Entity homeLightEntity = HomeLightEntity.Create(150,75);
            // light switch entity
            Engine.Entity lightSwitchEntity = LightSwitchEntity.Create(120, 135);

            // enemy entity
            Engine.Entity enemyEntity = EnemyEntity.Create(300, 480); // opposites (300, 483)
            // light source entity
            Engine.Entity lightSourceEntity = LightEntity.Create(300, 300);


            // Test player movement
            Engine.IntentionComponent pIntentionComponent = playerEntity.GetComponent<Engine.IntentionComponent>();
            //pIntentionComponent.up = true;
            //pIntentionComponent.left = true;

            // Test enemy movement
            Engine.IntentionComponent eIntentionComponent = enemyEntity.GetComponent<Engine.IntentionComponent>();
            //eIntentionComponent.up = true;
            //eIntentionComponent.right = true;
            //enemyEntity.state = "walkSouth";


            // Map trigger entity
            Engine.Entity m = EngineGlobals.entityManager.CreateEntity();
            m.AddTag("m");
            m.AddComponent(new Engine.TransformComponent(225, 0));
            m.AddComponent(new Engine.TriggerComponent(
                new Vector2(0, 0), new Vector2(75, 30),
                doorOnCollisionEnter,
                null,
                null
            ));

            // Beach trigger entity
            Engine.Entity b = EngineGlobals.entityManager.CreateEntity();
            b.AddTag("b");
            b.AddComponent(new Engine.TransformComponent(475, 1000));
            b.AddComponent(new Engine.TriggerComponent(
                new Vector2(0, 0), new Vector2(75, 30),
                null,
                beachOnCollisionEnter,
                null
            ));

            // Home trigger entity
            Engine.Entity h = EngineGlobals.entityManager.CreateEntity();
            h.AddTag("h");
            h.AddComponent(new Engine.TransformComponent(155, 135));
            h.AddComponent(new Engine.TriggerComponent(
                new Vector2(0, 0), new Vector2(20, 10),
                null,
                homeOnCollisionEnter,
                null
            ));

            //Globals.content.Load<TiledMap>("startZone");

            MenuScene menuScene = new MenuScene();
            EngineGlobals.sceneManager.PushScene(menuScene);

        }

        protected override void Update(GameTime gameTime)
        {
            if (EngineGlobals.sceneManager.isEmpty())
                Exit();
            EngineGlobals.inputManager.Update(gameTime);
            EngineGlobals.sceneManager.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            EngineGlobals.sceneManager.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
