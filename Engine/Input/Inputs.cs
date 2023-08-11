/*
 *   keyboard and controller static InputMethod instances
 */

namespace AdventureGame.Engine
{
    public class Inputs
    {
        // ADD leftclick, rightclick, middleclick??
        public static InputMethod keyboard = new InputMethod(
            up: KeyboardInput.W,
            down: KeyboardInput.S,
            left: KeyboardInput.A,
            right: KeyboardInput.D,
            button1: KeyboardInput.Enter,
            button2: KeyboardInput.LeftShift,
            button3: null,
            button4: null,
            button5: KeyboardInput.Q,
            button6: KeyboardInput.E,
            button7: KeyboardInput.R,
            button8: KeyboardInput.T
        );

        public static InputMethod controller = new InputMethod(
            up: ControllerInput.LeftThumbUp,
            down: ControllerInput.LeftThumbDown,
            left: ControllerInput.LeftThumbLeft,
            right: ControllerInput.LeftThumbRight,
            button1: ControllerInput.A,
            button2: ControllerInput.B,
            button3: null,
            button4: null,
            button5: ControllerInput.LeftTrigger,
            button6: ControllerInput.RightTrigger,
            button7: ControllerInput.LeftShoulder,
            button8: ControllerInput.RightShoulder
        );

    }

}
