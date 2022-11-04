using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using S = System.Diagnostics.Debug;

namespace AdventureGame.Engine
{
    public class InputManager
    {
        private KeyboardState previousKeyboardState;
        private KeyboardState currentKeyboardState;

        private MouseState previousMouseState;
        private MouseState currentMouseState;

        // Should this be changed to one controller for now? E.g. GamePad.GetState(PlayerIndex.One)
        private List<GamePadState> previousGamePadState = new List<GamePadState>() {GamePad.GetState(0), GamePad.GetState(1), GamePad.GetState(2), GamePad.GetState(3)};
        private List<GamePadState> currentGamePadState = new List<GamePadState>() {GamePad.GetState(0), GamePad.GetState(1), GamePad.GetState(2), GamePad.GetState(3)};

        public Texture2D CursorTexture { get; private set; }
        public Vector2 CursorPosition { get; private set; }
        public bool IsCursorVisible { get; set; }

        public InputManager()
        {
            CursorTexture = Globals.content.Load<Texture2D>("cursor");
            IsCursorVisible = false;
        }

        public void Update(GameTime gameTime)
        {
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            for (int i = 0; i <= 3; i++)
            {
                previousGamePadState[i] = currentGamePadState[i];
                currentGamePadState[i] = GamePad.GetState(i);
            }

            // Handle the cursor position if it is visible
            if (IsCursorVisible)
                PositionCursor();
        }

        public bool IsDown(InputItem item)
        {
            if (item == null)
                return false;

            if (item.key != null)
                return currentKeyboardState.IsKeyDown((Keys)item.key);

            if (item.button != null)
                return currentGamePadState[0].IsButtonDown((Buttons)item.button);

            if (item.mouseButton != null)
            {
                if (item.mouseButton == MouseButtons.LeftMouseButton)
                    return currentMouseState.LeftButton == ButtonState.Pressed;
                else if (item.mouseButton == MouseButtons.RightMouseButton)
                    return currentMouseState.RightButton == ButtonState.Pressed;
                else if (item.mouseButton == MouseButtons.MiddleMouseButton)
                    return currentMouseState.MiddleButton == ButtonState.Pressed;
            }

            return false;
        }

        public bool IsDown(List<InputItem> items)
        {
            foreach (InputItem i in items)
            {
                if (IsDown(i))
                {
                    return true;
                }
            }
            return false;
        }
        
        public bool IsPressed(InputItem item)
        {
            if (item == null)
                return false;

            if (item.key != null)
                return currentKeyboardState.IsKeyDown((Keys)item.key) && !previousKeyboardState.IsKeyDown((Keys)item.key);

            if (item.button != null)
                return currentGamePadState[0].IsButtonDown((Buttons)item.button) && !previousGamePadState[0].IsButtonDown((Buttons)item.button);

            if (item.mouseButton != null)
            {
                if (item.mouseButton == MouseButtons.LeftMouseButton)
                    return currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton != ButtonState.Pressed;
                else if (item.mouseButton == MouseButtons.RightMouseButton)
                    return currentMouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton != ButtonState.Pressed;
                else if (item.mouseButton == MouseButtons.MiddleMouseButton)
                    return currentMouseState.MiddleButton == ButtonState.Pressed && previousMouseState.MiddleButton != ButtonState.Pressed;
            }

            return false;
        }

        public bool IsPressed(List<InputItem> items)
        {
            foreach (InputItem i in items)
            {
                if (IsPressed(i))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsReleased(InputItem item)
        {
            if (item == null)
                return false;

            if (item.key != null)
                return !currentKeyboardState.IsKeyDown((Keys)item.key) && previousKeyboardState.IsKeyDown((Keys)item.key);

            if (item.button != null)
                return !currentGamePadState[0].IsButtonDown((Buttons)item.button) && previousGamePadState[0].IsButtonDown((Buttons)item.button);

            if (item.mouseButton != null)
            {
                if (item.mouseButton == MouseButtons.LeftMouseButton)
                    return currentMouseState.LeftButton != ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Pressed;
                else if (item.mouseButton == MouseButtons.RightMouseButton)
                    return currentMouseState.RightButton != ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Pressed;
                else if (item.mouseButton == MouseButtons.MiddleMouseButton)
                    return currentMouseState.MiddleButton != ButtonState.Pressed && previousMouseState.MiddleButton == ButtonState.Pressed;
            }

            return false;
        }

        public bool IsReleased(List<InputItem> items)
        {
            foreach (InputItem i in items)
            {
                if (IsReleased(i))
                {
                    return true;
                }
            }
            return false;
        }

        // Toggle the visibility of the custom cursor
        public void ToggleCursorVisibility()
        {
            IsCursorVisible = !IsCursorVisible; // Toggle on/off

            //GamePadCapabilities capabilities = GamePad.GetCapabilities(PlayerIndex.One);
            //if (capabilities.IsConnected)

            if (!IsCursorVisible)
                return;
            // If a controller is being used, position the cursor in the screen center
            else if (IsCursorVisible && currentGamePadState[0].IsConnected)
            {
                CursorPosition = new Vector2(
                    Globals.ScreenWidth / 2 - CursorTexture.Width / 2,
                    Globals.ScreenHeight / 2 - CursorTexture.Height / 2);
            }

            PositionCursor();
        }

        // Position the cursor within the screen boundary using a controller or mouse
        private void PositionCursor()
        {
            float newX = CursorPosition.X;
            float newY = CursorPosition.Y;

            // Check if a controller is being used, otherwise assume a mouse is used
            if (currentGamePadState[0].IsConnected)
            {
                float cursorSpeed = 10.0f;

                //if (IsDown(Buttons.LeftThumbstickUp))
                //if (IsDown(Globals.upInput))

                if (GetThumbstickDirection() == "Up")
                    newY -= cursorSpeed;
                else if (GetThumbstickDirection() == "Down")
                    newY += cursorSpeed;
                else if (GetThumbstickDirection() == "Left")
                    newX -= cursorSpeed;
                else if (GetThumbstickDirection() == "Right")
                    newX += cursorSpeed;
            }
            else
            {
                newX = currentMouseState.X;
                newY = currentMouseState.Y;
            }

            // If the cursor position is outside of the screen boundaries,
            // set the position to the min or max valid X / Y position
            if (newX < 0)
                newX = 0;
            else if (newX > Globals.ScreenWidth - CursorTexture.Width)
                newX = Globals.ScreenWidth - CursorTexture.Width;

            if (newY < 0)
                newY = 0;
            else if (newY > Globals.ScreenHeight - CursorTexture.Height)
                newY = Globals.ScreenHeight - CursorTexture.Height;

            CursorPosition = new Vector2(newX, newY);
        }

        // CHECK tolerance etc.
        // Replace IsDown etc for thumbstick movement?
        private string GetThumbstickDirection()
        {
            float thumbStickTolerance = 0.35f;

            Vector2 direction = currentGamePadState[0].ThumbSticks.Left;

            float absX = Math.Abs(direction.X);
            float absY = Math.Abs(direction.Y);

            if (absX > absY && absX > thumbStickTolerance)
            {
                return (direction.X > 0) ? "Right" : "Left";
            }
            else if (absX < absY && absY > thumbStickTolerance)
            {
                return (direction.Y > 0) ? "Up" : "Down";
            }
            return "";
        }

        /* Original post: https://www.magicaltimebean.com/2012/01/getting-dpad-input-from-thumbsticks-in-xna-the-right-way/
        static Buttons GetThumbstickDirection(PlayerIndex player, bool leftStick)
        {
            float thumbstickTolerance = 0.35f;

            GamePadState gs = GamePad.GetState(player);
            Vector2 direction = (leftStick) ? 
                gs.ThumbSticks.Left : gs.ThumbSticks.Right;

            float absX = Math.Abs(direction.X);
            float absY = Math.Abs(direction.Y);

            if (absX > absY && absX > thumbstickTolerance)
            {
                return (direction.X > 0) ? Buttons.DPadRight : Buttons.DPadLeft;
            }
            else if (absX < absY && absY > thumbstickTolerance)
            {
                return (direction.Y > 0) ? Buttons.DPadUp : Buttons.DPadDown;
            }
            return (Buttons)0;
        }
        */

    }

}
