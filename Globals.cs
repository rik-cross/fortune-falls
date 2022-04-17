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
        public static ContentManager content;
        public static SpriteBatch spriteBatch;
        public static GraphicsDeviceManager graphics;
        public static GraphicsDevice graphicsDevice;
        public static SpriteFont font;
        public static SpriteFont fontSmall;
        public static RenderTarget2D sceneRenderTarget;
        public static RenderTarget2D lightRenderTarget;
    }

}
