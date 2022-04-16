using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended;

using MonoGame.Extended.Content;
using MonoGame.Extended.ViewportAdapters;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Serialization;

using AdventureGame.Engine;

namespace AdventureGame
{
    public class Game1 : Game
    {
        public SceneManager sceneManager;

        public Game1()
        {
            Globals.graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            Globals.font = Content.Load<SpriteFont>("File");
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Globals.content = this.Content;
            Globals.graphicsDevice = GraphicsDevice;
            
            Globals.spriteBatch = new SpriteBatch(GraphicsDevice);

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

            EngineGlobals.systems.Add(new InputSystem());
            EngineGlobals.systems.Add(new SpriteSystem());
            EngineGlobals.systems.Add(new TransformSystem());
            EngineGlobals.systems.Add(new AnimationSystem());
            
            sceneManager = new SceneManager();
            sceneManager.PushScene(new GameScene());
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (sceneManager.isEmpty())
                Exit();

            sceneManager.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            sceneManager.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
