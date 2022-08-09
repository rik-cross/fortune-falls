using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame.Engine
{
    class Image : SceneRenderable
    {

        private Texture2D texture;
        public Color tint;

        public Image(Texture2D texture, Vector2 size = default, Color tint = default, Vector2 position = default, Anchor anchor = Anchor.topleft, float alpha = 1.0f, bool visible = true) : base(position, anchor, alpha, visible)
        {
            this.texture = texture;

            if (size != default)
                this.size = size;
            else
            {
                this.size.X = texture.Width;
                this.size.Y = texture.Height;
            }

            if (tint == default(Color))
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
                this.texture,
                new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y),
                tint * alpha
            );
        }
    }
}
