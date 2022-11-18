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
        public EmoteComponent(string emoteImageURI)
        {
            EmoteSize = new Vector2(8, 8);

            emoteImage = new Image(
                Globals.content.Load<Texture2D>(emoteImageURI),
                size: EmoteSize);

            emoteBackground = new Rectangle(
                0,
                0,
                (int)(EmoteSize.X + Theme.BorderTiny * 2),
                (int)(EmoteSize.Y + Theme.BorderTiny * 2));

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
    }
}
