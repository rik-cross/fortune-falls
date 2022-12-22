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
        // input controller stack
        public Stack<Action<Entity>> inputControllerStack = new Stack<Action<Entity>>();

        public InputComponent(
                    InputMethod input,
                    Action<Entity> inputController
                )
        {
            this.input = input;
            this.inputControllerStack.Push(inputController);
        }

    }
}
