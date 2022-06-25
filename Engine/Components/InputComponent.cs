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
        public List<InputItem> upKeys = new List<InputItem>();
        public List<InputItem> downKeys = new List<InputItem>();
        public List<InputItem> leftKeys = new List<InputItem>();
        public List<InputItem> rightKeys = new List<InputItem>();
        public List<InputItem> button1Keys = new List<InputItem>();
        public List<InputItem> button2Keys = new List<InputItem>();

        // controller input
              

        public InputComponent(
                    List<InputItem> upKeys, List<InputItem> downKeys, List<InputItem> leftKeys, List<InputItem> rightKeys,
                    List<InputItem> button1Keys, List<InputItem> button2Keys,
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

    }
}
