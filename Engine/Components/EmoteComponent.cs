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
        public Vector2 emoteSize = new Vector2(16, 16);
        public DoubleAnimation alpha = new DoubleAnimation(0, 0.02f);
        public EmoteComponent(string emoteImageURI)
        {
            emoteImage = new Image(
                Globals.content.Load<Texture2D>(emoteImageURI),
                size: emoteSize);

            emoteBackground = new Rectangle(
                0,
                0,
                (int)(emoteSize.X + Theme.BorderTiny * 2),
                (int)(emoteSize.Y + Theme.BorderTiny * 2));

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
