using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame.Engine
{
    public class Text: SceneRenderable
    {

        private String caption;
        public String Caption
        {
            get { return caption; }
            set {
                this.caption = value;
                AdjustTextSize();
            }
        }
        private SpriteFont font;

        public Text(String caption, Vector2 position = default, SpriteFont font = null, anchor a = anchor.topleft, float alpha = 1.0f) : base(position, a, alpha)
        {
            this.caption = caption;

            if (font == null)
                this.font = Globals.fontSmall;
            else
                this.font = font;
            
            this.size.X = this.font.MeasureString(this.caption).X;
            this.size.Y = this.font.MeasureString(this.caption).Y;
            CalculateAnchors();
        }

        public override void Draw()
        {
            Globals.spriteBatch.DrawString(font, caption, position, new Color(255, 255, 255) * this.alpha);
        }

        public void AdjustTextSize()
        {
            
            double oldX = this.size.X;
            double oldY = this.size.Y;
            this.size.X = font.MeasureString(this.caption).X;
            this.size.Y = font.MeasureString(this.caption).Y;
            double diffX = this.size.X - oldX;
            double diffY = this.size.Y - oldY;

            // adjust for center
            if (a == anchor.topcenter || a == anchor.middlecenter || a == anchor.bottomcenter)
                position.X -= (float)(diffX / 2);
            // adjust for right
            if (a == anchor.topright || a == anchor.middleright || a == anchor.bottomright)
                position.X -= (float)(diffX);
            // adjust for middle
            if (a == anchor.middleleft || a == anchor.middlecenter || a == anchor.middleright)
                position.Y -= (float)(diffY / 2);
            // adjust for bottom
            if (a == anchor.bottomleft || a == anchor.bottomcenter || a == anchor.bottomright)
                position.Y -= (float)(diffY);

        }

    }

}
