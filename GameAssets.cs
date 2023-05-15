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
        //public static Texture2D emotes_texture = LoadTexture("../../../Content/Emojis/emotes.png");
        //public static Engine.SpriteSheet emote_spritesheet = new Engine.SpriteSheet("../../../Content/Emojis/emotes.png");

        public static Texture2D pd = LoadTexture("../../../Content/Emojis/emote_pickaxe.png");//Globals.content.Load<Texture2D>("Emojis/emote_pickaxe");
        public static Engine.Image emote_pickaxe = new Engine.Image(pd);

        
        public static Texture2D LoadTexture(string uri)
        {
            System.IO.FileStream imageFile = new System.IO.FileStream(uri, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            Texture2D Image = Texture2D.FromStream(Globals.graphicsDevice, imageFile);
            imageFile.Close();
            imageFile = null;
            return Image;
        }
    }
 
}
