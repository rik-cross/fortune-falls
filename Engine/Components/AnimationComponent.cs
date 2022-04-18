using System;
using System.Collections.Generic;
using System.Text;
using MonoGame.Extended.Animations;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended.Content;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Serialization;


namespace AdventureGame.Engine
{
    public class AnimationComponent : Component
    {
        public AnimatedSprite animation;
        public string state;
        public AnimationComponent(AnimatedSprite animation, string state="default")
        {
            this.animation = animation;
            this.state = state;
        }
    }
}
