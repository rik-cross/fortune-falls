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
        // Store an empty state for resetting on InputStack change?? e.g.
        // private KeyboardState _resetKeyboardState;
        private KeyboardState _keyboardState;
        private KeyboardState _previousKeyboardState;

        private MouseState _mouseState;
        private MouseState _previousMouseState;

        // Should this be changed to one controller for now? E.g. GamePad.GetState(PlayerIndex.One)
        private List<GamePadState> _gamePadState = new List<GamePadState>() { GamePad.GetState(0), GamePad.GetState(1), GamePad.GetState(2), GamePad.GetState(3) };
        private List<GamePadState> _previousGamePadState = new List<GamePadState>() {GamePad.GetState(0), GamePad.GetState(1), GamePad.GetState(2), GamePad.GetState(3)};

        private Dictionary<InputItem, Timer> delayDictionary;

        // Cursor
        public Texture2D CursorTexture { get; private set; }
        public Vector2 CursorPosition { get; private set; }
        private Vector2 _previousCursorPosition;
        private bool _isCursorVisible { get; set; }
        public bool HasCursorMoved { get; set; }

        public InputManager()
        {
            delayDictionary = new Dictionary<InputItem, Timer>();
            CursorTexture = Globals.content.Load<Texture2D>("cursor");
            _isCursorVisible = false;
        }

        public void Update(GameTime gameTime)
        {
            _previousKeyboardState = _keyboardState;
            _keyboardState = Keyboard.GetState();

            _previousMouseState = _mouseState;
            _mouseState = Mouse.GetState();

            for (int i = 0; i <= 3; i++)
            {
                _previousGamePadState[i] = _gamePadState[i];
                _gamePadState[i] = GamePad.GetState(i);
            }

            // Handle the cursor position if it is visible
            if (_isCursorVisible)
            {
                _previousCursorPosition = CursorPosition;
                PositionCursor();

                if (_previousCursorPosition != CursorPosition)
                    HasCursorMoved = true;
                else
                    HasCursorMoved = false;
            }

            // ADD Check if there are any keys in the delayIsDown dictionary
            // If so, iterate over them

            // Check if the key is still down
            // Check if the elapsed time has occured
            // If so, IsDown(item)

            // If key not still down, remove the timer and dictionary item
            foreach (KeyValuePair<InputItem, Timer> kvp in delayDictionary)
            {
                kvp.Value.Update(gameTime);
            }
        }

        public bool IsDown(InputItem item)
        {
            if (item == null)
                return false;

            if (item.key != null)
                return _keyboardState.IsKeyDown((Keys)item.key);

            if (item.button != null)
                return _gamePadState[0].IsButtonDown((Buttons)item.button);

            if (item.mouseButton != null)
            {
                if (item.mouseButton == MouseButtons.LeftMouseButton)
                    return _mouseState.LeftButton == ButtonState.Pressed;
                else if (item.mouseButton == MouseButtons.RightMouseButton)
                    return _mouseState.RightButton == ButtonState.Pressed;
                else if (item.mouseButton == MouseButtons.MiddleMouseButton)
                    return _mouseState.MiddleButton == ButtonState.Pressed;
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

        public bool IsDownDelay(InputItem item, float delay = 1f, bool repeat = true)
            // invokeImmediately = true, initialDelay = default / 0f
        {
            // If the item wasn't down and now is down
            // Create a new timer with the delay
            //Timer timer = new Timer();
            if (IsDown(item) && !delayDictionary.ContainsKey(item))
            {
                delayDictionary.Add(item, new Timer(delay, repeat));
                //delayDown.Add(item, currentTime);
                return true;
            }

            // If enough time has passed
            //IsDown(item);

            // Otherwise
            return false;
        }

        public bool IsDownDelay(List<InputItem> items, float delay = 1f, bool repeat = true)
        {
            foreach (InputItem i in items)
            {
                if (IsDownDelay(i))
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
                return _keyboardState.IsKeyDown((Keys)item.key) && !_previousKeyboardState.IsKeyDown((Keys)item.key);

            if (item.button != null)
                return _gamePadState[0].IsButtonDown((Buttons)item.button) && !_previousGamePadState[0].IsButtonDown((Buttons)item.button);

            if (item.mouseButton != null)
            {
                if (item.mouseButton == MouseButtons.LeftMouseButton)
                    return _mouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton != ButtonState.Pressed;
                else if (item.mouseButton == MouseButtons.RightMouseButton)
                    return _mouseState.RightButton == ButtonState.Pressed && _previousMouseState.RightButton != ButtonState.Pressed;
                else if (item.mouseButton == MouseButtons.MiddleMouseButton)
                    return _mouseState.MiddleButton == ButtonState.Pressed && _previousMouseState.MiddleButton != ButtonState.Pressed;
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
                return !_keyboardState.IsKeyDown((Keys)item.key) && _previousKeyboardState.IsKeyDown((Keys)item.key);

            if (item.button != null)
                return !_gamePadState[0].IsButtonDown((Buttons)item.button) && _previousGamePadState[0].IsButtonDown((Buttons)item.button);

            if (item.mouseButton != null)
            {
                if (item.mouseButton == MouseButtons.LeftMouseButton)
                    return _mouseState.LeftButton != ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Pressed;
                else if (item.mouseButton == MouseButtons.RightMouseButton)
                    return _mouseState.RightButton != ButtonState.Pressed && _previousMouseState.RightButton == ButtonState.Pressed;
                else if (item.mouseButton == MouseButtons.MiddleMouseButton)
                    return _mouseState.MiddleButton != ButtonState.Pressed && _previousMouseState.MiddleButton == ButtonState.Pressed;
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

        public void ShowCursor()
        {
            _isCursorVisible = true;

            // For controllers, set the initial position to the screen's center
            if (_gamePadState[0].IsConnected)
            {
                CursorPosition = Utilities.CenterVectorToContainer(CursorTexture.Width, CursorTexture.Height);
            }

            PositionCursor();
            _previousCursorPosition = CursorPosition;
        }

        public void HideCursor()
        {
            _isCursorVisible = false;
        }

        // Position the cursor within the screen boundary using a controller or mouse
        private void PositionCursor()
        {
            float newX = CursorPosition.X;
            float newY = CursorPosition.Y;

            // Check if a controller is being used, otherwise assume a mouse is used
            if (_gamePadState[0].IsConnected)
            {
                float cursorSpeed = 10.0f;

                //if (IsDown(Buttons.LeftThumbstickUp))
                //if (IsDown(Globals.upInput))

                // CHANGE to normal controls i.e. not constrained to one "DPad" direction only

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
                newX = _mouseState.X;
                newY = _mouseState.Y;
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

        public bool IsMouseInsideWindow()
        {
            /* 
             * Issue - OnRelease sets state position to 0 when outside the window
            MouseState exactMouseState = Mouse.GetState();
            Point position = new Point(exactMouseState.X, exactMouseState.Y);
            Console.WriteLine(position);
            return Globals.graphicsDevice.Viewport.Bounds.Contains(position);
            */
            return Globals.graphicsDevice.Viewport.Bounds.Contains(_previousMouseState.X, _previousMouseState.Y);
        }

        // CHECK tolerance etc.
        // Replace IsDown etc for thumbstick movement?
        private string GetThumbstickDirection()
        {
            float thumbStickTolerance = 0.35f;

            Vector2 direction = _gamePadState[0].ThumbSticks.Left;

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
