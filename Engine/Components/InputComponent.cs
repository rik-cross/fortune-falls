using System;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Input;

namespace AdventureGame.Engine
{
    public class InputComponent : Component
    {

        // input
        public InputMethod input;
        // input controller
        public Action<Entity> inputControllerPointer;

        // STACK! public Action<Entity> inputControllerPointerStack;

        public InputComponent(
                    InputMethod input,
                    Action<Entity> inputControllerPointer
                )
        {
            this.input = input;
            this.inputControllerPointer = inputControllerPointer;
        }

    }
}
