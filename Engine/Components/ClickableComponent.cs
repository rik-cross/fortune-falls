using System;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Input;

namespace AdventureGame.Engine
{
    public class ClickableComponent : Component
    {
        public bool IsClicked { get; set; }
        public bool IsActive { get; set; }

        public ClickableComponent(bool isActive = true, bool isClicked = false)
        {
            IsActive = isActive;
            IsClicked = isClicked;
        }

    }
}
