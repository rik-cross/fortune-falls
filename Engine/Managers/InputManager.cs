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
        // Keyboard states
        // Store an empty state for resetting on InputStack change?? e.g. private KeyboardState _resetKeyboardState;
        private KeyboardState _keyboardState;
        private KeyboardState _previousKeyboardState;
        private Dictionary<Keys, double> _keyboardStateDurations = new Dictionary<Keys, double>();

        // Mouse states
        private MouseState _mouseState;
        private MouseState _previousMouseState;

        // Gamepad states
        // Should this be changed to one controller for now? E.g. GamePad.GetState(PlayerIndex.One)
        private List<GamePadState> _gamePadState = new List<GamePadState>() { GamePad.GetState(0), GamePad.GetState(1), GamePad.GetState(2), GamePad.GetState(3) };
        private List<GamePadState> _previousGamePadState = new List<GamePadState>() {GamePad.GetState(0), GamePad.GetState(1), GamePad.GetState(2), GamePad.GetState(3)};
        private Dictionary<Buttons, double> _gamePadStateDurations = new Dictionary<Buttons, double>();

        // Long press
        private double gamePadLongPressAdjustment = 1.0;
        public int longPressDuration = 50;
        public bool longPressEaseOut = true;
        public int longPressEaseOutAmount = 3;

        // Cursor
        public Texture2D CursorTexture { get; private set; }
        public Vector2 CursorPosition { get; private set; }
        private Vector2 _previousCursorPosition;
        private bool _isCursorVisible;
        public bool HasCursorMoved { get; set; }

        public InputManager()
        {
            CursorTexture = Utils.LoadTexture("UI/cursor.png");
            _isCursorVisible = false;
        }

        public void Update(GameTime gameTime)
        {
            _previousKeyboardState = _keyboardState;
            _keyboardState = Keyboard.GetState();
            CalculateKeyboardLongPresses();

            _previousMouseState = _mouseState;
            _mouseState = Mouse.GetState();

            for (int i = 0; i <= 3; i++)
            {
                _previousGamePadState[i] = _gamePadState[i];
                _gamePadState[i] = GamePad.GetState(i);
            }
            CalculateGamePadLongPresses();

            if (_isCursorVisible)
            {
                // Handle the cursor position
                _previousCursorPosition = CursorPosition;
                PositionCursor();

                if (_previousCursorPosition != CursorPosition)
                    HasCursorMoved = true;
                else
                    HasCursorMoved = false;
            }
        }

        public bool IsControllerConnected()
        {
            return GamePad.GetState(0).IsConnected;
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

        public bool IsLongPressed(InputItem item)
        {
            if (item == null)
                return false;

            if (item.key != null)
                return _keyboardStateDurations.ContainsKey((Keys)item.key) && _keyboardStateDurations[(Keys)item.key] == 50;

            if (item.button != null)
                return _gamePadStateDurations.ContainsKey((Buttons)item.button) && _gamePadStateDurations[(Buttons)item.button] == 50;

            return false;
        }

        public double GetLongPressPercentage(InputItem item)
        {
            if (item == null)
                return 0;

            if (item.key != null)
            {
                if (!_keyboardStateDurations.ContainsKey((Keys)item.key))
                    return 0;
                return 100 / longPressDuration * _keyboardStateDurations[(Keys)item.key];
            }

            if (item.button != null)
            {
                if (!_gamePadStateDurations.ContainsKey((Buttons)item.button))
                    return 0;
                return 100 / longPressDuration * _gamePadStateDurations[(Buttons)item.button];
            }

            return 0;
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

        private void CalculateKeyboardLongPresses()
        {
            foreach (Keys key in KeyboardInput.KeyList)
            {
                //int newDuration;
                //if 
                //    = _keyboardStateDurations[key];


                if (_keyboardState.IsKeyDown(key) && _previousKeyboardState.IsKeyDown(key))
                    _keyboardStateDurations[key] = Math.Min(longPressDuration, _keyboardStateDurations[key] + 1.0);
                else
                {
                    if (longPressEaseOut && _keyboardStateDurations.ContainsKey(key))
                        _keyboardStateDurations[key] = Math.Max(0, _keyboardStateDurations[key] - longPressEaseOutAmount);
                    else
                        _keyboardStateDurations[key] = 0;
                }
            }
        }

        private void CalculateGamePadLongPresses()
        {
            foreach (Buttons button in ControllerInput.ButtonList)
            {
                if (_gamePadState[0].IsButtonDown(button) && _previousGamePadState[0].IsButtonDown(button))
                    _gamePadStateDurations[button] = Math.Min(longPressDuration, _gamePadStateDurations[button] + (1 / gamePadLongPressAdjustment));
                else
                {
                    if (longPressEaseOut && _gamePadStateDurations.ContainsKey(button))
                        _gamePadStateDurations[button] = Math.Max(0, _gamePadStateDurations[button] - (longPressEaseOutAmount / gamePadLongPressAdjustment));
                    else
                        _gamePadStateDurations[button] = 0;
                }
            }
        }

    }

}
