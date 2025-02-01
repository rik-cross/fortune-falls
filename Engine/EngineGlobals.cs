using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace Engine
{
    public static class EngineGlobals
    {
        public static InputManager inputManager;
        public static ComponentManager componentManager;
        public static SystemManager systemManager;
        public static EntityManager entityManager;
        public static SceneManager sceneManager;
        public static PlayerManager playerManager;
        public static InventoryManager inventoryManager;
        public static SoundManager soundManager;
        public static AchievementManager achievementManager;
        public static Log log;
        //public static List<System> systems = new List<System>();
        public static bool DEBUG = false;
        public static bool fullscreen;
        public static bool borderless;

        // XNA
        public static GameWindow gameWindow;
        public static ContentManager content;
        public static SpriteBatch spriteBatch;
        public static GraphicsDeviceManager graphics;
        public static GraphicsDevice graphicsDevice;
        public static RenderTarget2D sceneRenderTarget;
        public static RenderTarget2D lightRenderTarget;

        // Display options
        public static int ScreenWidth = 1280;
        public static int ScreenHeight = 720;
        //public static int MinScreenWidth = 1280;
        //public static int MinScreenHeight = 720;
        public static float globalZoomLevel = 2.5f;

        
    }

}
