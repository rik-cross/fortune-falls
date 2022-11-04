using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame.Engine
{
    public class Text: SceneRenderable
    {

        private string _caption;
        public string Caption
        {
            get { return _caption; }
            set {
                _caption = value;
                AdjustSize();
            }
        }
        private SpriteFont font;
        public Color colour;

        public Text(string caption, Vector2 position = default, SpriteFont font = null, Color colour = default, Anchor anchor = Anchor.none, Rectangle anchorParent = default, Padding padding = default, float alpha = 1.0f, bool visible = true) : base(position, anchor, anchorParent, padding, alpha, visible)
        {
            _caption = caption;

            if (font == null)
                this.font = Theme.secondaryFont;
            else
                this.font = font;

            if (colour == default)
                this.colour = Color.White;
            else
                this.colour = colour;
            
            size.X = this.font.MeasureString(_caption).X;
            size.Y = this.font.MeasureString(_caption).Y;
            CalculateAnchors();
        }

        public override void Draw()
        {
            if (!visible)
                return;

            Globals.spriteBatch.DrawString(font, _caption, position, colour * alpha);
            /*
            Globals.spriteBatch.DrawRectangle(
                new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y),
                Color.BlueViolet, 1);*/
        }

        public void AdjustSize()
        {
            
            double oldX = size.X;
            double oldY = size.Y;
            size.X = font.MeasureString(_caption).X;
            size.Y = font.MeasureString(_caption).Y;
            double diffX = size.X - oldX;
            double diffY = size.Y - oldY;
            /*
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
            */
        }

    }

}
