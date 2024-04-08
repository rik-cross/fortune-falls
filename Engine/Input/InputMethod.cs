/*
 *   InputMethod
 *   
 *   Stores an InputItem for each physical action
 *   e.g. InputMethod.left = InputItem DPadLeft
 */

namespace AdventureGame.Engine
{

    public class InputMethod
    {

        // input is 4-directional
        // plus up to 8 buttons
        public InputItem up;
        public InputItem down;
        public InputItem left;
        public InputItem right;
        public InputItem button1;
        public InputItem button2;
        public InputItem button3;
        public InputItem button4;
        public InputItem button5;
        public InputItem button6;
        public InputItem button7;
        public InputItem button8;

        public InputMethod(
                InputItem up, InputItem down, InputItem left, InputItem right,
                InputItem button1, InputItem button2, InputItem button3, InputItem button4,
                //InputItem button5, InputItem button6, InputItem button7, InputItem button8
                InputItem button5 , InputItem button7, InputItem button8
            )
        {
            this.up = up;
            this.down = down;
            this.left = left;
            this.right = right;
            this.button1 = button1;
            this.button2 = button2;
            this.button3 = button3;
            this.button4 = button4;
            this.button5 = button5;
            //this.button6 = button6;
            this.button7 = button7;
            this.button8 = button8;
        }

    }

}
