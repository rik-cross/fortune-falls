using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Animations;


namespace AdventureGame.Engine
{
    public class AnimationComponent : Component
    {
        public AnimatedSprite animation;
        public string state;
    }
}
