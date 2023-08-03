using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame.Engine
{
    public class EmoteComponent : Component
    {
        public Image emoteImage;
        public Rectangle emoteBackground;
        public bool showBackground;
        private Vector2 _emoteSize;
        public Vector2 EmoteSize {
            get {
                return _emoteSize;
            }
            set {
                _emoteSize = value;
                if (emoteImage != null)
                    emoteImage.Size = value;
                emoteBackground.Width = (int)(value.X + Theme.BorderTiny * 2);
                emoteBackground.Height = (int)(value.Y + Theme.BorderTiny * 2);
            }
        }
        public DoubleAnimation alpha = new DoubleAnimation(0, 0.02f);
        public EmoteComponent(string emoteImageURI, bool background = true)
        {
            EmoteSize = new Vector2(16, 16);

            emoteImage = new Image(
                Utils.LoadTexture(emoteImageURI),
                size: EmoteSize);

            emoteBackground = new Rectangle(
                0,
                0,
                (int)(EmoteSize.X + Theme.BorderTiny * 2),
                (int)(EmoteSize.Y + Theme.BorderTiny * 2));

            showBackground = background;

            Show();
        }

        public EmoteComponent(Image image, bool background = true)
        {
            EmoteSize = new Vector2(16, 16);

            //emoteImage = new Image(
            //    Utils.LoadTexture(emoteImageURI),
            //    size: EmoteSize);

            emoteImage = image;
            image.Size = EmoteSize;

            emoteBackground = new Rectangle(
                0,
                0,
                (int)(EmoteSize.X + Theme.BorderTiny * 2),
                (int)(EmoteSize.Y + Theme.BorderTiny * 2));

            showBackground = background;

            Show();
        }

        public void Show()
        {
            alpha.Value = 1.0;
        }
        public void Hide()
        {
            alpha.Value = 0.0;
        }

        public override void Reset()
        {
            alpha.Value = 0.0;
        }
    }
}
