using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace AdventureGame.Engine
{
    public class Image : SceneRenderable
    {
        public Texture2D _texture;
        public Color _tint;

        public Image(Texture2D texture, Vector2 size = default, Color tint = default, Vector2 position = default, Anchor anchor = Anchor.None, Rectangle anchorParent = default, Padding padding = default, float alpha = 1.0f, bool visible = true) : base(position, anchor, anchorParent, padding, alpha, visible)
        {
            _texture = texture;

            if (size != default)
                Size = size;
            else
            {
                Size.X = texture.Width;
                Size.Y = texture.Height;
            }

            if (tint == default)
                _tint = Color.White;
            else
                _tint = tint;

            CalculateAnchors();
        }

        public override void Draw()
        {
            if (!Visible)
                return;

            Globals.spriteBatch.Draw(
                _texture,
                new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y),
                _tint * Alpha
            );

            // Testing
            /*Globals.spriteBatch.DrawRectangle(
                new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y),
                Color.BlueViolet, 1);*/
        }
    }
}
