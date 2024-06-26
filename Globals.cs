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


        // todo - move to another static class for game / UI controls
        // Menu input controls
        public static Dictionary<string, Engine.InputItem> MenuInputs = new Dictionary<string, Engine.InputItem>();
        public static bool IsControllerConnected = false; // todo? check controller is connected periodically after Game1 initialisation?

        // Basic menu controls
        //public static List<Engine.InputItem> upInput = new List<Engine.InputItem>() { Engine.KeyboardInput.W, Engine.ControllerInput.LeftThumbUp };
        //public static List<Engine.InputItem> upInput = new List<Engine.InputItem>() { new Engine.InputItem(key: Keys.W, button: Buttons.LeftThumbstickUp) };
        public static Engine.InputItem upInput = new Engine.InputItem(key: Keys.W, button: Buttons.LeftThumbstickUp);

        public static List<Engine.InputItem> downInput = new List<Engine.InputItem>() { Engine.KeyboardInput.S, Engine.ControllerInput.LeftThumbDown };
        public static List<Engine.InputItem> leftInput = new List<Engine.InputItem>() { Engine.KeyboardInput.A, Engine.ControllerInput.LeftThumbLeft };
        public static List<Engine.InputItem> rightInput = new List<Engine.InputItem>() { Engine.KeyboardInput.D, Engine.ControllerInput.LeftThumbRight };
        public static List<Engine.InputItem> backInput = new List<Engine.InputItem>() { Engine.KeyboardInput.Escape, Engine.ControllerInput.Back };
        public static List<Engine.InputItem> selectInput = new List<Engine.InputItem>() { Engine.KeyboardInput.Enter, Engine.ControllerInput.A };

        // Menu windows
        public static List<Engine.InputItem> devToolsInput = new List<Engine.InputItem>() { Engine.KeyboardInput.T };
        public static List<Engine.InputItem> pauseInput = new List<Engine.InputItem>() { Engine.KeyboardInput.Escape, Engine.ControllerInput.Start };
        public static List<Engine.InputItem> inventoryInput = new List<Engine.InputItem>() { Engine.KeyboardInput.I, Engine.ControllerInput.DPadUp };

        // Inventory menu
        public static List<Engine.InputItem> cancelInput = new List<Engine.InputItem>() { Engine.KeyboardInput.Escape, Engine.ControllerInput.B };
        public static List<Engine.InputItem> splitStackInput = new List<Engine.InputItem>() { Engine.KeyboardInput.LeftShift, Engine.ControllerInput.LeftTrigger };
        public static List<Engine.InputItem> primaryCursorInput = new List<Engine.InputItem>() { Engine.MouseInput.LeftButton, Engine.ControllerInput.A };
        public static List<Engine.InputItem> secondaryCursorInput = new List<Engine.InputItem>() { Engine.MouseInput.RightButton, Engine.ControllerInput.RightTrigger };

        // Mouse left click = Interact / button1, Mouse right click = RightClick / buttonX


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

    }

}
