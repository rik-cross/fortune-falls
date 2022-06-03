using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AdventureGame.Engine
{
    public class InputManager
    {

        KeyboardState prevKeyboardState;
        KeyboardState curKeyboardState;

        public void Update(GameTime gameTime)
        {

            prevKeyboardState = curKeyboardState;
            curKeyboardState = Keyboard.GetState();

        }

        public bool IsDown(Keys key)
        {
            return curKeyboardState.IsKeyDown(key);
        }

        public bool IsDown(List<Keys> keys)
        {
            foreach (Keys k in keys)
            {
                if (IsDown(k))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsPressed(Keys key)
        {
            return curKeyboardState.IsKeyDown(key) && !prevKeyboardState.IsKeyDown(key);
        }

        public bool IsPressed(List<Keys> keys)
        {
            foreach (Keys k in keys)
            {
                if (IsPressed(k))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsReleased(Keys key)
        {
            return !curKeyboardState.IsKeyDown(key) && prevKeyboardState.IsKeyDown(key);
        }

        public bool IsReleased(List<Keys> keys)
        {
            foreach (Keys k in keys)
            {
                if (IsReleased(k))
                {
                    return true;
                }
            }
            return false;
        }

    }

}
