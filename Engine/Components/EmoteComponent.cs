using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Graphics;
using S = System.Diagnostics.Debug;

namespace AdventureGame.Engine
{
    public class EmoteComponent : Component
    {
        public Texture2D _texture;
        public Vector2 backgroundSize;
        public Color backgroundColor;
        public Color borderColor;
        public int borderSize;
        public int heightAboveEntity;

        public Vector2 textureSize;
        public bool showBackground;
        public DoubleAnimation alpha = new DoubleAnimation(0, 0.02f);

        public static Action<Scene, Entity> drawMethod;
        public Action<Scene, Entity> componentSpecificDrawMethod;

        public EmoteComponent(
            Texture2D texture,
            Vector2 textureSize = default,
            bool showBackground = false,
            Color backgroundColor = default,
            Color borderColor = default,
            int borderSize = 0,
            int heightAboveEntity = 0,
            Action<Scene, Entity> drawMethod = null,
            Action<Scene, Entity> componentSpecificDrawMethod = null
        )
        {
            this._texture = texture;
            this.showBackground = showBackground;
            this.borderSize = borderSize;
            this.heightAboveEntity = heightAboveEntity;
            if (textureSize == default)
                this.textureSize = new Vector2(texture.Width, texture.Height);
            else
                this.textureSize = textureSize;

            this.backgroundSize = new Vector2(
                this.textureSize.X + this.borderSize * 2,
                this.textureSize.Y + this.borderSize * 2
            );

            if (backgroundColor == default)
                this.backgroundColor = Color.White;
            else
                this.backgroundColor = backgroundColor;

            if (borderColor == default)
                this.borderColor = Color.Black;
            else
                this.borderColor = borderColor;

            EmoteComponent.drawMethod = drawMethod;
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
