using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace AdventureGame.Engine
{
    public class Weapon
    {
        public string name;
        public Texture2D image;
        public SoundEffect hitSound;
        public SoundEffect missSound;
        public Weapon(string name, Texture2D image = null, SoundEffect hitSound = null, SoundEffect missSound = null)
        {
            this.name = name;
            this.image = image;
            this.hitSound = hitSound;
            this.missSound = missSound;
        }
        
    }
    public static class Weapons
    {
        public static Texture2D swordThumbnail = Utils.LoadTexture("Weapons/sword.png");
        public static Texture2D hammerThumbnail = Utils.LoadTexture("Weapons/hammer.png");
        public static Texture2D axeThumbnail = Utils.LoadTexture("Weapons/axe.png");

        public static SoundEffect axeSwipeSoundEffect = Utils.LoadSoundEffect("Sounds/swipe.wav");

        public static Weapon sword = new Weapon("sword", swordThumbnail);
        public static Weapon hammer = new Weapon("hammer", hammerThumbnail);
        public static Weapon axe = new Weapon("axe", axeThumbnail, missSound: axeSwipeSoundEffect);
    }
    

}
