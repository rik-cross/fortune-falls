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
        private bool _outline;
        private Color _outlineColour;
        private int _outlineThickness;

        public Text(string caption, Vector2 position = default, SpriteFont font = null, Color colour = default, Anchor anchor = Anchor.None, Rectangle anchorParent = default, Padding padding = default, float alpha = 1.0f, bool visible = true, bool outline = false, Color outlineColour = default, int outlineThickness = 1) : base(position, anchor, anchorParent, padding, alpha, visible)
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

            _outline = outline;
            _outlineColour = outlineColour;
            _outlineThickness = outlineThickness;

            Size.X = _font.MeasureString(_caption).X;
            Size.Y = _font.MeasureString(_caption).Y;

            if (outline)
            {
                Size.X += outlineThickness;
                Size.Y += outlineThickness;
            }

            CalculateAnchors();
        }

        public override void Draw()
        {
            if (!Visible)
                return;

            // Outline
            if (_outline)
            {
                for (int i = 1; i < _outlineThickness; i++)
                {
                    Globals.spriteBatch.DrawString(_font, _caption, new Vector2(Position.X - i, Position.Y - i), _outlineColour * (float)Alpha2.Value);
                    Globals.spriteBatch.DrawString(_font, _caption, new Vector2(Position.X + i, Position.Y - i), _outlineColour * (float)Alpha2.Value);
                    Globals.spriteBatch.DrawString(_font, _caption, new Vector2(Position.X - i, Position.Y + i), _outlineColour * (float)Alpha2.Value);
                    Globals.spriteBatch.DrawString(_font, _caption, new Vector2(Position.X + i, Position.Y + i), _outlineColour * (float)Alpha2.Value);
                    Globals.spriteBatch.DrawString(_font, _caption, new Vector2(Position.X - i, Position.Y), _outlineColour * (float)Alpha2.Value);
                    Globals.spriteBatch.DrawString(_font, _caption, new Vector2(Position.X + i, Position.Y), _outlineColour * (float)Alpha2.Value);
                    Globals.spriteBatch.DrawString(_font, _caption, new Vector2(Position.X, Position.Y - i), _outlineColour * (float)Alpha2.Value);
                    Globals.spriteBatch.DrawString(_font, _caption, new Vector2(Position.X, Position.Y + i), _outlineColour * (float)Alpha2.Value);
                }
            }
            Globals.spriteBatch.DrawString(_font, _caption, Position, _colour * (float)Alpha2.Value);

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
