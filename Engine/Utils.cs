using S = System;
using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame.Engine
{
    public static class Utils
    {
        public static string ProjectPath = ProjectSourcePath.Value;
        public static string ContentLocation = "Content/";

        public static Texture2D LoadTexture(string uri)
        {
            string FullFilePath = ProjectPath + ContentLocation + uri;
            S.IO.FileStream imageFile = new S.IO.FileStream(FullFilePath, S.IO.FileMode.Open, S.IO.FileAccess.Read);
            Texture2D Image = Texture2D.FromStream(Globals.graphicsDevice, imageFile);
            imageFile.Close();
            imageFile = null;
            return Image;
        }
    }
}
