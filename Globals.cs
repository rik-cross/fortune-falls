using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace AdventureGame
{
    public class Globals
    {

        public static bool TEST = false;
        public static bool newGame = true;

        public static int ScreenWidth = 1280;
        public static int ScreenHeight = 720;
        //public static int MinScreenWidth = 1280;
        //public static int MinScreenHeight = 720;

        // todo? use GameServices e.g. https://pastebin.com/GyYE2bji
        public static GameWindow gameWindow;
        public static ContentManager content;
        public static SpriteBatch spriteBatch;
        public static GraphicsDeviceManager graphics;
        public static GraphicsDevice graphicsDevice;
        public static RenderTarget2D sceneRenderTarget;
        public static RenderTarget2D lightRenderTarget;

        // Should these be here on reference to directly in another static class?
        // e.g. Engine.Input.PauseInput() or Engine.Input.Up()

        // Movement
        public static List<Engine.InputItem> upInput = new List<Engine.InputItem>() { Engine.KeyboardInput.W, Engine.ControllerInput.LeftThumbUp };
        public static List<Engine.InputItem> downInput = new List<Engine.InputItem>() { Engine.KeyboardInput.S, Engine.ControllerInput.LeftThumbDown };
        public static List<Engine.InputItem> leftInput = new List<Engine.InputItem>() { Engine.KeyboardInput.A, Engine.ControllerInput.LeftThumbLeft };
        public static List<Engine.InputItem> rightInput = new List<Engine.InputItem>() { Engine.KeyboardInput.D, Engine.ControllerInput.LeftThumbRight };

        // todo remove player controls
        // Player controls
        public static List<Engine.InputItem> sprintInput = new List<Engine.InputItem>() { Engine.KeyboardInput.LeftShift, Engine.ControllerInput.LeftTrigger };
        public static List<Engine.InputItem> button2Input = new List<Engine.InputItem>() { Engine.KeyboardInput.LeftShift, Engine.ControllerInput.LeftTrigger };
        // todo - add other player controls here? e.g.
        public static List<Engine.InputItem> primaryActionInput = new List<Engine.InputItem>() { Engine.MouseInput.LeftButton, Engine.ControllerInput.A };

        // Menu interaction
        public static List<Engine.InputItem> devToolsInput = new List<Engine.InputItem>() { Engine.KeyboardInput.T };
        public static List<Engine.InputItem> pauseInput = new List<Engine.InputItem>() { Engine.KeyboardInput.Escape, Engine.ControllerInput.Start };
        public static List<Engine.InputItem> inventoryInput = new List<Engine.InputItem>() { Engine.KeyboardInput.I, Engine.ControllerInput.DPadUp };
        //public static List<Engine.InputItem> forwardInput = new List<Engine.InputItem>() { Engine.KeyboardInput.Enter, Engine.ControllerInput.Start };
        public static List<Engine.InputItem> backInput = new List<Engine.InputItem>() { Engine.KeyboardInput.Escape, Engine.ControllerInput.Back };
        public static List<Engine.InputItem> selectInput = new List<Engine.InputItem>() { Engine.KeyboardInput.Enter, Engine.ControllerInput.A };
        public static List<Engine.InputItem> cancelInput = new List<Engine.InputItem>() { Engine.KeyboardInput.Escape, Engine.ControllerInput.B };
        public static List<Engine.InputItem> splitStackInput = new List<Engine.InputItem>() { Engine.KeyboardInput.LeftShift, Engine.ControllerInput.LeftTrigger };
        public static List<Engine.InputItem> primaryCursorInput = new List<Engine.InputItem>() { Engine.MouseInput.LeftButton, Engine.ControllerInput.A };
        public static List<Engine.InputItem> secondaryCursorInput = new List<Engine.InputItem>() { Engine.MouseInput.RightButton, Engine.ControllerInput.RightTrigger };

        // Mouse left click = Interact / button1, Mouse right click = RightClick / buttonX


        public static float globalZoomLevel = 2.5f;

        public static SoundEffect dialogueTickSound;

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

        // Player settings
        public static int playerIndex = 0;
        public static string playerStr = allCharacters[playerIndex];
    }

}
