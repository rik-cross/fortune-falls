using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace AdventureGame
{
    public class Globals
    {
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

        // Game options
        public static bool TEST = false;
        public static bool newGame = true;
        public static SoundEffect dialogueTickSound;
        public static bool IsControllerConnected = false; // todo? check controller is connected periodically after Game1 initialisation?

        // Character sprite sheets
        public static string characterDir = "Characters/Human/";
        public static string characterBaseStr = "base";
        public static string characterToolStr = "tools";
        public static string[] allCharacters = new string[6] {
                "longhair", "curlyhair", "bowlhair", "mophair", "shorthair", "spikeyhair" };
        public static string[] characterNames = new string[6]
        {
            "One", "Two", "Three", "Four", "Five", "Six"
        };
        public static Color[] characterHues = new Color[6]
        {
            Color.White, Color.White, Color.White, Color.White, Color.White, Color.White
        };

        // Player settings
        public static int playerIndex = 0;
        public static string playerStr = allCharacters[playerIndex];

        public static bool hasInteracted = false;


        // Called during MenuScene Init() to set all of the menu controls
        public static void SetCustomUIControls()
        {
            // Menu windows
            Engine.UIInput.Set("menuDev", new Engine.InputItem(key: Keys.T, button: Buttons.RightStick));
            Engine.UIInput.Set("menuPause", new Engine.InputItem(key: Keys.Escape, button: Buttons.Start));
            Engine.UIInput.Set("menuInventory", new Engine.InputItem(key: Keys.I, button: Buttons.DPadUp));

            // Inventory menu
            Engine.UIInput.Set("inventoryCancel", new Engine.InputItem(key: Keys.Escape, button: Buttons.B));
            Engine.UIInput.Set("inventorySplitStack", new Engine.InputItem(key: Keys.LeftShift, button: Buttons.LeftTrigger));
            Engine.UIInput.Set("inventoryPrimarySelect", new Engine.InputItem(mouseButton: Engine.MouseButtons.LeftMouseButton, button: Buttons.A));
            Engine.UIInput.Set("inventorySecondarySelect", new Engine.InputItem(mouseButton: Engine.MouseButtons.RightMouseButton, button: Buttons.RightTrigger));

        }
}

}
