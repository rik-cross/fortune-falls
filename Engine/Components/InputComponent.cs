using System;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    public class InputComponent : Component
    {
        public InputMethod Input { get; set; }
        public List<Action<Entity>> InputControllerStack { get; private set; }
        public string TopControllerLabel { get; set; }

        public InputComponent(InputMethod input, Action<Entity> inputController)
        {
            Input = input;
            InputControllerStack = new List<Action<Entity>>();
            InputControllerStack.Add(inputController);
            TopControllerLabel = "";
        }

        public void PushController(Action<Entity> inputController, string controllerLabel = "")
        {
            InputControllerStack.Add(inputController);
            TopControllerLabel = controllerLabel;
        }

        // Remove the controller at top of stack unless it's the last one and clears the label
        public Action<Entity> PopController()
        {
            if (InputControllerStack.Count > 1)
            {
                // Return the controller at the end of the list and remove it
                int lastIndex = InputControllerStack.Count - 1;
                Action<Entity> controller = InputControllerStack[lastIndex];
                InputControllerStack.RemoveAt(lastIndex);

                TopControllerLabel = "";

                return controller;
            }
            else
                return null;
        }

        // Return the controller on the top of the stack
        public Action<Entity> PeekController()
        {
            if (InputControllerStack.Count > 0)
                return InputControllerStack[^1];
            else
                return null;
        }

        // Removes all but the last controller and clears the label
        public void Clear()
        {
            if (InputControllerStack.Count > 1)
            {
                Action<Entity> firstController = InputControllerStack[0];
                InputControllerStack.Clear();
                InputControllerStack.Add(firstController);

                TopControllerLabel = "";
            }
        }

    }
}
