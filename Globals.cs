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
        public static bool IsControllerSelected = false;

        // Character sprite sheets
        public static string characterDir = "Characters/Human/";
        public static string characterBaseStr = "base";
        public static string characterToolStr = "tools";
        public static string characterHandStr = "hand";
        public static string characterSkinStr = "skin";
        public static string characterBodyStr = "body";
        public static string[] allCharacters = new string[6] {
                "longhair", "curlyhair", "bowlhair", "mophair", "shorthair", "spikeyhair" };
        public static string[] characterNames = new string[6]
        {
            "One", "Two", "Three", "Four", "Five", "Six"
        };
        public static Color[] characterHues = new Color[6]
        {
            new Color(246, 226, 172), new Color(196, 139, 105), new Color(230,188,152),
            new Color(255, 203, 147), new Color(142, 100, 77), new Color(246, 226, 172)
        };

        // Player settings
        public static int playerIndex = 0;
        public static string playerStr = allCharacters[playerIndex];

        public static bool hasInteracted = false;
        public static bool hasUsedAxe = false;

        // Called during MenuScene Init() to set all of the menu controls
        public static void SetCustomUIControls()
        {
            // Set the basic menu controls
            Engine.UIInput.Set("up", new Engine.InputItem(key: Keys.W, button: Buttons.LeftThumbstickUp));
            Engine.UIInput.Set("down", new Engine.InputItem(key: Keys.S, button: Buttons.LeftThumbstickDown));
            Engine.UIInput.Set("left", new Engine.InputItem(key: Keys.A, button: Buttons.LeftThumbstickLeft));
            Engine.UIInput.Set("right", new Engine.InputItem(key: Keys.D, button: Buttons.LeftThumbstickRight));
            Engine.UIInput.Set("back", new Engine.InputItem(key: Keys.Escape, button: Buttons.Back));
            Engine.UIInput.Set("select", new Engine.InputItem(key: Keys.Enter, button: Buttons.A));

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
