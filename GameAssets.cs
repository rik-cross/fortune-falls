using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

using S = System.Diagnostics.Debug;

namespace AdventureGame
{
    public static class GameAssets
    {
        //public static Engine.SpriteSheet em = new Engine.SpriteSheet("Emojis/emotes");
        //public static Engine.Image emote_pickaxe = new Engine.Image(em.GetSubTexture(4, 4));
        //em = new Engine.SpriteSheet("Emojis/emotes");
        
        //public static Texture2D p = LoadTexture("./Content/Emojis/emote_pickaxe");//Globals.content.Load<Texture2D>("Emojis/emote_pickaxe");
        //public static Engine.Image emote_pickaxe = new Engine.Image(p);
        public static Texture2D LoadTexture(string uri)
        {
            S.WriteLine(System.IO.Directory.GetCurrentDirectory());
            System.IO.FileStream imageFile = new System.IO.FileStream(uri, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            Texture2D Image = Texture2D.FromStream(Globals.graphicsDevice, imageFile);
            imageFile.Close();
            imageFile = null;
            return Image;
        }
    }
 
}
