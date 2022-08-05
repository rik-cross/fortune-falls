using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame.Engine
{
    class Image : SceneRenderable
    {

        public Texture2D texture;

        public Image(Texture2D texture, Vector2 size = default, Vector2 position = default, anchor a = anchor.topleft, float alpha = 1.0f) : base(position, a, alpha)
        {
            this.texture = texture;

            if (size != default)
                this.size = size;
            else
            {
                this.size.X = texture.Width;
                this.size.Y = texture.Height;
            }

            CalculateAnchors();
        }

        public override void Draw()
        {
            Globals.spriteBatch.Draw(
                this.texture,
                new Rectangle((int)this.position.X, (int)this.position.Y, (int)this.size.X, (int)this.size.Y),
                Color.White * this.alpha
            );
        }
    }
}
