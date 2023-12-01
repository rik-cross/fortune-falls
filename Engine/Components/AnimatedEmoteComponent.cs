using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Graphics;

namespace AdventureGame.Engine
{
    public class AnimatedEmoteComponent : Component
    {
        private List<Texture2D> _textures;
        private int _currentIndex;
        private int _timer;
        private int _frameDelay;

        public Vector2 textureSize;
        public bool showBackground;
        public DoubleAnimation alpha = new DoubleAnimation(0, 0.02f);
        public AnimatedEmoteComponent(List<Texture2D> textures, int frameDelay=8, bool showBackground=false)
        {
            //this._textures = new List<Texture2D>();
            this._textures = textures;
            this._frameDelay = frameDelay;
            this.showBackground = showBackground;
        }
    }
}
