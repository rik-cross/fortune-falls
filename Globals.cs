using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace AdventureGame
{
    public class Globals
    {
        public static int WIDTH = 1280;
        public static int HEIGHT = 720;

        public static ContentManager content;
        public static SpriteBatch spriteBatch;
        public static GraphicsDeviceManager graphics;
        public static GraphicsDevice graphicsDevice;
        public static SpriteFont font;
        public static SpriteFont fontSmall;
        public static RenderTarget2D sceneRenderTarget;
        public static RenderTarget2D lightRenderTarget;

        public static List<Engine.InputItem> pauseInput = new List<Engine.InputItem>() { Engine.KeyboardInput.P, Engine.ControllerInput.Start };
        public static List<Engine.InputItem> forwardInput = new List<Engine.InputItem>() { Engine.KeyboardInput.Enter, Engine.ControllerInput.Start };
        public static List<Engine.InputItem> backInput = new List<Engine.InputItem>() { Engine.KeyboardInput.Escape, Engine.ControllerInput.Back };

        public static Engine.SpriteSheet playerSpriteSheet;
        public static Engine.SpriteSheet candleSpriteSheet;
        public static Engine.SpriteSheet enemySpriteSheet;

        public static Engine.Scene menuScene;
        public static Engine.Scene gameScene;
        public static Engine.Scene homeScene;
        public static Engine.Scene beachScene;

        public static float globalZoomLevel = 3.0f;

    }

}
