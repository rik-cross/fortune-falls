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
        public static int ScreenWidth = 1280;
        public static int ScreenHeight = 720;

        public static ContentManager content;
        public static SpriteBatch spriteBatch;
        public static GraphicsDeviceManager graphics;
        public static GraphicsDevice graphicsDevice;
        public static RenderTarget2D sceneRenderTarget;
        public static RenderTarget2D lightRenderTarget;

        // Should these be here on reference to directly in another static class?
        // e.g. Engine.Input.PauseInput() or Engine.Input.Up()
        public static List<Engine.InputItem> pauseInput = new List<Engine.InputItem>() { Engine.KeyboardInput.P, Engine.ControllerInput.Start };
        public static List<Engine.InputItem> inventoryInput = new List<Engine.InputItem>() { Engine.KeyboardInput.I, Engine.ControllerInput.DPadUp };
        public static List<Engine.InputItem> forwardInput = new List<Engine.InputItem>() { Engine.KeyboardInput.Enter, Engine.ControllerInput.Start };
        public static List<Engine.InputItem> backInput = new List<Engine.InputItem>() { Engine.KeyboardInput.Escape, Engine.ControllerInput.Back };
        public static List<Engine.InputItem> interactInput = new List<Engine.InputItem>() { Engine.KeyboardInput.Enter, Engine.ControllerInput.A };
        public static List<Engine.InputItem> button2Input = new List<Engine.InputItem>() { Engine.KeyboardInput.LeftShift, Engine.ControllerInput.LeftTrigger };
        //public static List<Engine.InputItem> sprintInput = new List<Engine.InputItem>() { Engine.KeyboardInput.LeftShift, Engine.ControllerInput.LeftShoulder };
        public static List<Engine.InputItem> primaryCursorInput = new List<Engine.InputItem>() { Engine.MouseInput.LeftButton, Engine.ControllerInput.A };
        public static List<Engine.InputItem> secondaryCursorInput = new List<Engine.InputItem>() { Engine.MouseInput.RightButton, Engine.ControllerInput.RightTrigger };
        // How to map a list of buttons? e.g. Escape and Tab, B and Start
        public static List<Engine.InputItem> cancelInput = new List<Engine.InputItem>() { Engine.KeyboardInput.Escape, Engine.ControllerInput.B };

        // Mouse left click = Interact / button1, Mouse right click = RightClick / buttonX

        public static List<Engine.InputItem> upInput = new List<Engine.InputItem>() { Engine.KeyboardInput.W, Engine.ControllerInput.LeftThumbUp };
        public static List<Engine.InputItem> downInput = new List<Engine.InputItem>() { Engine.KeyboardInput.S, Engine.ControllerInput.LeftThumbDown };
        public static List<Engine.InputItem> leftInput = new List<Engine.InputItem>() { Engine.KeyboardInput.A, Engine.ControllerInput.LeftThumbLeft };
        public static List<Engine.InputItem> rightInput = new List<Engine.InputItem>() { Engine.KeyboardInput.D, Engine.ControllerInput.LeftThumbRight };

        public static Engine.SpriteSheet playerSpriteSheet; // Move to MenuScene?

        public static Engine.Scene menuScene;
        public static Engine.Scene gameScene;
        public static Engine.Scene homeScene;
        public static Engine.Scene beachScene;

        public static float globalZoomLevel = 3.0f;

        public static SoundEffect dialogueTickSound;

    }

}
