using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended.Content;
using MonoGame.Extended.ViewportAdapters;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Serialization;

namespace AdventureGame
{

    public class Actor
    {

        public Vector2 _position;
        public Vector2 _size;
        public AnimatedSprite _animation;

        public Actor() {}
        public void Init() { }
        public void Update(GameTime gameTime) { }
        public void Draw(GameTime gameTime) { }


    }
}
