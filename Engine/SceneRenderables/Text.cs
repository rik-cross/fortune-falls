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
                AdjustSize();
            }
        }
        private SpriteFont font;
        public Color colour;

        public Text(String caption, Vector2 position = default, SpriteFont font = null, Color colour = default, Anchor anchor = Anchor.topleft, float alpha = 1.0f, bool visible = true) : base(position, anchor, alpha, visible)
        {
            this.caption = caption;

            if (font == null)
                this.font = Globals.fontSmall;
            else
                this.font = font;

            if (colour == default(Color))
                this.colour = Color.White;
            else
                this.colour = colour;
            
            this.size.X = this.font.MeasureString(this.caption).X;
            this.size.Y = this.font.MeasureString(this.caption).Y;
            CalculateAnchors();
        }

        public override void Draw()
        {
            if (!visible)
                return;

            Globals.spriteBatch.DrawString(font, caption, position, colour * alpha);
        }

        public void AdjustSize()
        {
            
            double oldX = size.X;
            double oldY = size.Y;
            this.size.X = font.MeasureString(this.caption).X;
            this.size.Y = font.MeasureString(this.caption).Y;
            double diffX = size.X - oldX;
            double diffY = size.Y - oldY;

            // adjust for center
            if (anchor == Anchor.topcenter || anchor == Anchor.middlecenter || anchor == Anchor.bottomcenter)
                position.X -= (float)(diffX / 2);
            // adjust for right
            if (anchor == Anchor.topright || anchor == Anchor.middleright || anchor == Anchor.bottomright)
                position.X -= (float)(diffX);
            // adjust for middle
            if (anchor == Anchor.middleleft || anchor == Anchor.middlecenter || anchor == Anchor.middleright)
                position.Y -= (float)(diffY / 2);
            // adjust for bottom
            if (anchor == Anchor.bottomleft || anchor == Anchor.bottomcenter || anchor == Anchor.bottomright)
                position.Y -= (float)(diffY);

        }

    }

}
