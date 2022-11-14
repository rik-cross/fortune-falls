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
        private SpriteFont _font;
        private Color _colour;

        public Text(string caption, Vector2 position = default, SpriteFont font = null, Color colour = default, Anchor anchor = Anchor.None, Rectangle anchorParent = default, Padding padding = default, float alpha = 1.0f, bool visible = true) : base(position, anchor, anchorParent, padding, alpha, visible)
        {
            _caption = caption;

            if (font == null)
                _font = Theme.FontSecondary;
            else
                _font = font;

            if (colour == default)
                _colour = Color.White;
            else
                _colour = colour;
            
            Size.X = _font.MeasureString(_caption).X;
            Size.Y = _font.MeasureString(_caption).Y;
            CalculateAnchors();
        }

        public override void Draw()
        {
            if (!Visible)
                return;

            Globals.spriteBatch.DrawString(_font, _caption, Position, _colour * Alpha);
            /*
            Globals.spriteBatch.DrawRectangle(
                new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y),
                Color.BlueViolet, 1);*/
        }

        public void AdjustSize()
        {
            
            double oldX = Size.X;
            double oldY = Size.Y;
            Size.X = _font.MeasureString(_caption).X;
            Size.Y = _font.MeasureString(_caption).Y;
            double diffX = Size.X - oldX;
            double diffY = Size.Y - oldY;
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
