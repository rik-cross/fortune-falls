using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame.Engine
{
    public class SpriteSheet
    {

        public Texture2D texture;
        public Vector2 spriteSize;

        public SpriteSheet(Texture2D texture, Vector2 spriteSize)
        {
            this.texture = texture;
            this.spriteSize = spriteSize;
        }

        public Texture2D GetSubTexture(int x, int y)
        {
            
            Rectangle textureRect = new Rectangle((int)(x * spriteSize.X), (int)(y * spriteSize.Y), (int)spriteSize.X, (int)spriteSize.Y);
            Texture2D subTexture = new Texture2D(Globals.graphicsDevice, textureRect.Width, textureRect.Height);
            Color[] data = new Color[textureRect.Width * textureRect.Height];
            texture.GetData(0, textureRect, data, 0, data.Length);
            subTexture.SetData(data);
            return subTexture;
        }

    }

}
