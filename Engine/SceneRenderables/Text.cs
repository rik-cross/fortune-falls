using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine
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
                    EngineGlobals.spriteBatch.DrawString(_font, _caption, new Vector2(Position.X - i, Position.Y - i), _outlineColour * (float)Alpha2.Value);
                    EngineGlobals.spriteBatch.DrawString(_font, _caption, new Vector2(Position.X + i, Position.Y - i), _outlineColour * (float)Alpha2.Value);
                    EngineGlobals.spriteBatch.DrawString(_font, _caption, new Vector2(Position.X - i, Position.Y + i), _outlineColour * (float)Alpha2.Value);
                    EngineGlobals.spriteBatch.DrawString(_font, _caption, new Vector2(Position.X + i, Position.Y + i), _outlineColour * (float)Alpha2.Value);
                    EngineGlobals.spriteBatch.DrawString(_font, _caption, new Vector2(Position.X - i, Position.Y), _outlineColour * (float)Alpha2.Value);
                    EngineGlobals.spriteBatch.DrawString(_font, _caption, new Vector2(Position.X + i, Position.Y), _outlineColour * (float)Alpha2.Value);
                    EngineGlobals.spriteBatch.DrawString(_font, _caption, new Vector2(Position.X, Position.Y - i), _outlineColour * (float)Alpha2.Value);
                    EngineGlobals.spriteBatch.DrawString(_font, _caption, new Vector2(Position.X, Position.Y + i), _outlineColour * (float)Alpha2.Value);
                }
            }
            EngineGlobals.spriteBatch.DrawString(_font, _caption, Position, _colour * (float)Alpha2.Value);

        }

        public void AdjustSize()
        {
            
            double oldX = Size.X;
            double oldY = Size.Y;
            Size.X = _font.MeasureString(_caption).X;
            Size.Y = _font.MeasureString(_caption).Y;
            double diffX = Size.X - oldX;
            double diffY = Size.Y - oldY;
        }

    }

}
