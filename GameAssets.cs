using Microsoft.Xna.Framework.Graphics;

namespace AdventureGame
{
    public static class GameAssets
    {
        //public static Texture2D emotes_texture = LoadTexture("../../../Content/Emojis/emotes.png");
        //public static Engine.SpriteSheet emote_spritesheet = new Engine.SpriteSheet("../../../Content/Emojis/emotes.png");

        public static Texture2D pd = Engine.Utils.LoadTexture("../../../Content/Emojis/emote_pickaxe.png");//Globals.content.Load<Texture2D>("Emojis/emote_pickaxe");
        public static Engine.Image emote_pickaxe = new Engine.Image(pd);
    }
}
