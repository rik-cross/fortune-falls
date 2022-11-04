using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace AdventureGame.Engine
{
    class Image2 : SceneRenderable2
    {
        public Color tint;
        private Texture2D texture;
        public Texture2D Texture { get => texture; set => texture = value; }

        public Image2(Texture2D texture, Vector2 size = default, Color tint = default, Vector2 position = default, Anchor anchor = Anchor.topleft, Rectangle anchorParent = default, Padding padding = default, float alpha = 1.0f, bool visible = true) : base(position, anchor, anchorParent, padding, alpha, visible)
        {
            this.texture = texture;

            if (size != default)
                this.size = size;
            else
            {
                this.size.X = texture.Width;
                this.size.Y = texture.Height;
            }

            if (tint == default)
                this.tint = Color.White;
            else
                this.tint = tint;

            CalculateAnchors();
        }

        public override void Draw()
        {
            if (!visible)
                return;

            Globals.spriteBatch.Draw(
                texture,
                new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y),
                tint * alpha
            );
            /*
            Globals.spriteBatch.DrawRectangle(
                new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y),
                Color.BlueViolet, 1);*/
        }
    }
}
