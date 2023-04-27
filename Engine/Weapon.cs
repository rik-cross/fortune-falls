using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame.Engine
{
    public class Weapon
    {
        public string name;
        public Texture2D image;
        public List<string> canHurt;
        public Weapon(string name, Texture2D image, List<string> canHurt)
        {
            this.name = name;
            this.image = image;
            this.canHurt = canHurt;
        }
        
    }

    public static class Weapons
    {
        public static Texture2D swordThumbnail = Globals.content.Load<Texture2D>("Weapons/sword");
        public static Texture2D hammerThumbnail = Globals.content.Load<Texture2D>("Weapons/hammer");
        public static Texture2D axeThumbnail = Globals.content.Load<Texture2D>("Weapons/axe");

        public static Weapon sword = new Weapon("sword", swordThumbnail, new List<string> {"tree"});
        public static Weapon hammer = new Weapon("hammer", hammerThumbnail, new List<string> { "tree" });
        public static Weapon axe = new Weapon("axe", axeThumbnail, new List<string> { "tree" });
    }
    

}
