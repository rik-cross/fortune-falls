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

        // input
        public List<InputItem> upKeys = new List<InputItem>();
        public List<InputItem> downKeys = new List<InputItem>();
        public List<InputItem> leftKeys = new List<InputItem>();
        public List<InputItem> rightKeys = new List<InputItem>();
        public List<InputItem> button1Keys = new List<InputItem>();
        public List<InputItem> button2Keys = new List<InputItem>();
        public List<InputItem> button3Keys = new List<InputItem>();
        public List<InputItem> button4Keys = new List<InputItem>();
        public List<InputItem> button5Keys = new List<InputItem>();
        public List<InputItem> button6Keys = new List<InputItem>();
        public List<InputItem> button7Keys = new List<InputItem>();
        public List<InputItem> button8Keys = new List<InputItem>();

        public InputComponent(
                    List<InputItem> upKeys, List<InputItem> downKeys, List<InputItem> leftKeys, List<InputItem> rightKeys,
                    List<InputItem> button1Keys, List<InputItem> button2Keys,
                    List<InputItem> button3Keys, List<InputItem> button4Keys,
                    List<InputItem> button5Keys, List<InputItem> button6Keys,
                    List<InputItem> button7Keys, List<InputItem> button8Keys,
                    Action<Entity> inputControllerPointer
                )
        {
            this.upKeys = upKeys;
            this.downKeys = downKeys;
            this.leftKeys = leftKeys;
            this.rightKeys = rightKeys;

            this.button1Keys = button1Keys;
            this.button2Keys = button2Keys;
            this.button3Keys = button3Keys;
            this.button4Keys = button4Keys;
            this.button5Keys = button5Keys;
            this.button6Keys = button6Keys;
            this.button7Keys = button7Keys;
            this.button8Keys = button8Keys;

            this.inputControllerPointer = inputControllerPointer;
        }

    }
}
