using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame.Engine
{
    public class Sprite
    {
        public List<Texture2D> TextureList { get; private set; }
        public Vector2 Size { get; private set; }
        public Vector2 Offset { get; private set; }
        public bool FlipH { get; private set; }
        public bool FlipV { get; private set; }
        public Color SpriteHue { get; set; }
        public int CurrentFrame { get; set; } // todo: should all frame info be in AnimatedSprite?
        public int MaxFrame { get; set; } // todo: delete
        public int StartFrame { get; set; }
        public int EndFrame { get; set; }

        public Sprite(Texture2D texture, Vector2 size = default, Vector2 offset = default,
            bool flipH = false, bool flipV = false, Color spriteHue = default)
        {

            if (spriteHue == default)
                spriteHue = Color.White;
            SpriteHue = spriteHue;

            TextureList = new List<Texture2D>() { texture };

            if (size != default)
                Size = size;
            else
                Size = new Vector2(texture.Width, texture.Height);

            if (offset == default)
                Offset = new Vector2(0, 0);
            else
                Offset = offset;

            FlipH = flipH;
            FlipV = flipV;
            CurrentFrame = 0;
            MaxFrame = TextureList.Count - 1;
        }

        public Sprite(List<Texture2D> textureList, Vector2 offset = default,
            bool flipH = false, bool flipV = false, Color spriteHue = default)
        {

            if (spriteHue == default)
                spriteHue = Color.White;
            SpriteHue = spriteHue;

            TextureList = textureList;
            Size = new Vector2(textureList[0].Width, textureList[0].Height);

            if (offset == default)
                Offset = new Vector2(0, 0);
            else
                Offset = offset;

            FlipH = flipH;
            FlipV = flipV;
            CurrentFrame = 0;
            MaxFrame = TextureList.Count - 1;
        }

        public Texture2D GetCurrentTexture()
        {
            return TextureList[CurrentFrame];
        }

        public Texture2D GetTexture(int frame)
        {
            if (frame >= 0 && frame < TextureList.Count)
                return TextureList[frame];
            else
                return null;
        }
    }
}