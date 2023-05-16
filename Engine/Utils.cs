using S = System;
using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame.Engine
{
    public static class Utils
    {
        public static Texture2D LoadTexture(string uri)
        {
            S.IO.FileStream imageFile = new S.IO.FileStream(uri, S.IO.FileMode.Open, S.IO.FileAccess.Read);
            Texture2D Image = Texture2D.FromStream(Globals.graphicsDevice, imageFile);
            imageFile.Close();
            imageFile = null;
            return Image;
        }
    }
}
