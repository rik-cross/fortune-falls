using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using S = System.Diagnostics.Debug;

namespace AdventureGame.Engine
{
    public class Text: SceneRenderable
    {

        private String text;
        public String Textt
        {
            get { return text; }
            set {
                this.text = value;
                AdjustTextSize();
            }
        }
        private SpriteFont font = Globals.font;

        public Text(String text, Vector2 position, SpriteFont font, anchor a = anchor.topleft) : base(position, a)
        {
            this.text = text;
            this.font = font;
            this.size.X = font.MeasureString(this.text).X;
            this.size.Y = font.MeasureString(this.text).Y;
            CalculateAnchors();
        }

        public override void Draw()
        {
            Globals.spriteBatch.DrawString(font, text, position, new Color(255, 255, 255));
        }

        public void AdjustTextSize()
        {
            
            double oldX = this.size.X;
            double oldY = this.size.Y;
            this.size.X = font.MeasureString(this.text).X;
            this.size.Y = font.MeasureString(this.text).Y;
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
