using Microsoft.Xna.Framework.Graphics;
using System;

namespace AdventureGame
{
    public static class GameAssets
    {
        private static string contentLocation = "../../../Content/";

        // spritesheets
        //public static Engine.SpriteSheet emote_spritesheet = new Engine.SpriteSheet("../../../Content/Emojis/emotes.png");

        // empte images
        //public static Engine.Image emote_pickaxe = new Engine.Image(Engine.Utils.LoadTexture(contentLocation + "Emojis/emote_pickaxe.png"));
        public static Engine.Image emote_pickaxe = new Engine.Image(Engine.Utils.LoadTexture("Emojis/emote_pickaxe.png"));
    }
}
