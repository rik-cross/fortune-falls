﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine
{
    public class ThumbnailComponent : Component
    {
        public Texture2D ThumbnailImage { get; set; }
        public Vector2 ThumbnailSize { get; set; }

        public ThumbnailComponent(string thumbnailImageURI)
        {
            ThumbnailImage = Utils.LoadTexture(thumbnailImageURI);
            ThumbnailSize = new Vector2(ThumbnailImage.Width, ThumbnailImage.Height);
        }

        public ThumbnailComponent(Texture2D texture)
        {
            ThumbnailImage = texture;
            ThumbnailSize = new Vector2(ThumbnailImage.Width, ThumbnailImage.Height);
        }

        /*
        public Image ThumbnailImage { get; set; }

        private Vector2 _thumbnailSize;
        public Vector2 ThumbnailSize {
            get {
                return _thumbnailSize;
            }
            set {
                _thumbnailSize = value;
                if (ThumbnailImage != null)
                    ThumbnailImage.Size = value;
            }
        }

        public ThumbnailComponent(string thumbnailImageURI)
        {
            ThumbnailImage = new Image(Utils.LoadTexture(thumbnailImageURI));
            ThumbnailSize = ThumbnailImage.Size;
        }
        */
    }
}
