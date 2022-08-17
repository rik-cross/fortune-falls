using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using S = System.Diagnostics.Debug;

namespace AdventureGame.Engine
{
    public class InputManager
    {

        KeyboardState prevKeyboardState;
        KeyboardState curKeyboardState;

        public List<GamePadState> prevGamePadState = new List<GamePadState>() {GamePad.GetState(0), GamePad.GetState(1), GamePad.GetState(2), GamePad.GetState(3)};
        public List<GamePadState> curGamePadState = new List<GamePadState>() {GamePad.GetState(0), GamePad.GetState(1), GamePad.GetState(2), GamePad.GetState(3)};

        public void Update(GameTime gameTime)
        {

            prevKeyboardState = curKeyboardState;
            curKeyboardState = Keyboard.GetState();

            for (int i=0; i<=3; i++)
            {
                prevGamePadState[i] = curGamePadState[i];
                curGamePadState[i] = GamePad.GetState(i);
            }
            

        }

        public bool IsDown(InputItem item)
        {
            if (item != null && item.key != null)
                return curKeyboardState.IsKeyDown((Keys)item.key);
            if (item != null && item.button != null)
                return curGamePadState[0].IsButtonDown((Buttons)item.button);

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
            if (item != null && item.key != null)
            {
                if (curKeyboardState.IsKeyDown((Keys)item.key) && !prevKeyboardState.IsKeyDown((Keys)item.key))
                {
                    Console.WriteLine($"{item.key} is pressed");
                    return true;
                }
            }
                //return curKeyboardState.IsKeyDown((Keys)item.key) && !prevKeyboardState.IsKeyDown((Keys)item.key);
            if (item != null && item.button != null)
                return curGamePadState[0].IsButtonDown((Buttons)item.button) && !prevGamePadState[0].IsButtonDown((Buttons)item.button);
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
            if (item != null && item.key != null)
                return !curKeyboardState.IsKeyDown((Keys)item.key) && prevKeyboardState.IsKeyDown((Keys)item.key);
            if (item != null && item.button != null)
                return !curGamePadState[0].IsButtonDown((Buttons)item.button) && prevGamePadState[0].IsButtonDown((Buttons)item.button);
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

    }

}
