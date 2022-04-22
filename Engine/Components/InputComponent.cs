using System;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Input;

namespace AdventureGame.Engine
{
    public class InputComponent : Component
    {

        // input controller
        public Action<Entity> inputControllerPointer;

        // keyboard input
        public List<Keys> upKeys = new List<Keys>();
        public List<Keys> downKeys = new List<Keys>();
        public List<Keys> leftKeys = new List<Keys>();
        public List<Keys> rightKeys = new List<Keys>();
        public List<Keys> button1Keys = new List<Keys>();
        public List<Keys> button2Keys = new List<Keys>();

        public InputComponent(
                    List<Keys> upKeys, List<Keys> downKeys, List<Keys> leftKeys, List<Keys> rightKeys,
                    List<Keys> button1Keys, List<Keys> button2Keys,
                    Action<Entity> inputControllerPointer
                )
        {
            this.upKeys = upKeys;
            this.downKeys = downKeys;
            this.leftKeys = leftKeys;
            this.rightKeys = rightKeys;

            this.button1Keys = button1Keys;
            this.button2Keys = button2Keys;

            this.inputControllerPointer = inputControllerPointer;
        }

        public bool IsKeyDown(List<Keys> option)
        {
            foreach(Keys k in option)
            {
                if (Keyboard.GetState().IsKeyDown(k))
                    return true;
            }
            return false;
        }

        public bool IsKeyUp(List<Keys> option)
        {
            foreach (Keys k in option)
            {
                if (Keyboard.GetState().IsKeyUp(k))
                    return true;
            }
            return false;
        }

    }
}
