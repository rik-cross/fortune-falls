using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Input;

namespace AdventureGame.Engine
{
    public class InputItem
    {

        public Keys? key;
        public Buttons? button;

        public InputItem(Keys? key = null, Buttons? button = null)
        {

            this.key = key;
            this.button = button;

        }

    }

}
