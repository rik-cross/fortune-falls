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
        public List<Texture2D> _textures;
        public Vector2 backgroundSize;
        public Color backgroundColor;
        public int _currentIndex;
        public int _timer;
        public int _frameDelay;
        public Color borderColor;
        public int borderSize;

        public Vector2 textureSize;
        public bool showBackground;
        public DoubleAnimation alpha = new DoubleAnimation(0, 0.02f);

        public static Action<Scene, Entity> drawMethod;
        public Action<Scene, Entity> componentSpecificDrawMethod;

        public AnimatedEmoteComponent(
            List<Texture2D> textures,
            int frameDelay = 8,
            Vector2 textureSize = default,
            bool showBackground = false,
            Color backgroundColor = default,
            Color borderColor = default,
            int borderSize = 0,
            Action<Scene, Entity> drawMethod = null,
            Action<Scene, Entity> componentSpecificDrawMethod = null
        )
        {
            //this._textures = new List<Texture2D>();
            this._textures = textures;
            this._frameDelay = frameDelay;
            this.showBackground = showBackground;

            if (textureSize == default)
                this.textureSize = new Vector2(textures[0].Width, textures[0].Height);
            else
                this.textureSize = textureSize;

            this._currentIndex = 0;
            this._timer = 0;

            this.backgroundSize = new Vector2(
                this._textures[0].Width + Theme.BorderSmall*2,
                this._textures[0].Height + Theme.BorderSmall*2
            );

            if (backgroundColor == default)
                this.backgroundColor = Color.White;
            else
                this.backgroundColor = backgroundColor;

            if (borderColor == default)
                this.borderColor = Color.Black;
            else
                this.borderColor = borderColor;
            this.borderSize = borderSize;

            AnimatedEmoteComponent.drawMethod = drawMethod;
            this.componentSpecificDrawMethod = componentSpecificDrawMethod;

        }
        public void Show()
        {
            alpha.Value = 1.0;
        }
        public void Hide()
        {
            alpha.Value = 0.0;
        }

        public override void Reset()
        {
            alpha.Value = 0.0;
        }
    }
}
