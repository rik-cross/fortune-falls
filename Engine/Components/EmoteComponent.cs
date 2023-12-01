using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using S = System.Diagnostics.Debug;

namespace AdventureGame.Engine
{
    public class EmoteComponent : Component
    {
        public Image emoteImage;
        public Rectangle emoteBackground;
        public Vector2 backgroundSize;
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

        public static Action<Scene, Entity> drawMethod;
        public Action<Scene, Entity> componentSpecificDrawMethod;

        public EmoteComponent(
            string emoteImageURI,
            bool background = true,
            Vector2 emoteSize = default,
            Action<Scene, Entity> drawMethod = null,
            Action<Scene, Entity> componentSpecificDrawMethod = null
        )
        {

            Texture2D t = Utils.LoadTexture(emoteImageURI);

            if (emoteSize == default)
                EmoteSize = new Vector2(t.Width, t.Height);
            else
                EmoteSize = emoteSize;

            emoteImage = new Image(
                t,
                size: EmoteSize);

            emoteBackground = new Rectangle(
                0,
                0,
                (int)(EmoteSize.X + Theme.BorderTiny * 2),
                (int)(EmoteSize.Y + Theme.BorderTiny * 2));

            showBackground = background;

            this.backgroundSize = new Vector2(
                this.EmoteSize.X + Theme.BorderSmall * 2,
                this.EmoteSize.Y + Theme.BorderSmall * 2
            );

            EmoteComponent.drawMethod = drawMethod;
            this.componentSpecificDrawMethod = componentSpecificDrawMethod;

            Show();
        }

        public EmoteComponent(
            Image image,
            bool background = true,
            Vector2 emoteSize = default,
            Action<Scene, Entity> drawMethod = null,
            Action<Scene, Entity> componentSpecificDrawMethod = null
        )
        {

            if (emoteSize == default)
                EmoteSize = new Vector2(image.Width, image.Height);
            else
                EmoteSize = emoteSize;

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

            this.backgroundSize = new Vector2(
                this.EmoteSize.X + Theme.BorderSmall * 2,
                this.EmoteSize.Y + Theme.BorderSmall * 2
            );

            EmoteComponent.drawMethod = drawMethod;
            this.componentSpecificDrawMethod = componentSpecificDrawMethod;

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
