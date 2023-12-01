using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace AdventureGame
{
    public static class GameAssets
    {
        //private static string contentLocation = "../../../Content/";

        // spritesheets
        //public static Engine.SpriteSheet emote_spritesheet = new Engine.SpriteSheet("../../../Content/Emojis/emotes.png");

        // general images
        public static Texture2D marker = Engine.Utils.LoadTexture("UI/pointer.png");
        public static Texture2D texture_controller_movement = Engine.Utils.LoadTexture("Emojis/controller_movement.png");
        public static Texture2D texture_keyboard_movement = Engine.Utils.LoadTexture("Emojis/keyboard_movement.png");
        public static Texture2D right_trigger = Engine.Utils.LoadTexture("Emojis/right_trigger.png");
        public static Texture2D key_axe = Engine.Utils.LoadTexture("Emojis/key.png");


        // image lists
        public static List<Texture2D> list_texture_controller_movement = Engine.Utilities.SplitTexture(
            texture_controller_movement,
            new Microsoft.Xna.Framework.Vector2(13, 13)
        )[0];
        public static List<Texture2D> list_texture_keyboard_movement = Engine.Utilities.SplitTexture(
            texture_keyboard_movement,
            new Microsoft.Xna.Framework.Vector2(26, 13)
        )[0];


        // emote images
        //public static Engine.Image emote_pickaxe = new Engine.Image(Engine.Utils.LoadTexture(contentLocation + "Emojis/emote_pickaxe.png"));
        public static Engine.Image emote_pickaxe = new Engine.Image(Engine.Utils.LoadTexture("Emojis/emote_pickaxe.png"));

        // npc headshots

        // blacksmith
        public static Texture2D blacksmith_headshot = Engine.Utils.LoadTexture("Characters/NPC/headshot.png");

        // sound effects

        public static SoundEffect sound_notification = Engine.Utils.LoadSoundEffect("Sounds/notification.wav");

        // components
        
        public static Engine.AnimatedEmoteComponent controllerMovementEmote = new Engine.AnimatedEmoteComponent(
            list_texture_controller_movement, frameDelay: 20, drawMethod: UICustomisations.DrawAnimatedEmote
        );

        public static Engine.AnimatedEmoteComponent keyboardMovementEmote = new Engine.AnimatedEmoteComponent(
            list_texture_keyboard_movement, frameDelay: 20, drawMethod: UICustomisations.DrawAnimatedEmote
        );

        public static Engine.EmoteComponent controllerWeaponEmote = new Engine.EmoteComponent(
            new Engine.Image(right_trigger), drawMethod: UICustomisations.DrawEmote//, emoteSize: new Vector2(26, 26)
        );

        public static Engine.EmoteComponent keyboardWeaponEmote = new Engine.EmoteComponent(
            new Engine.Image(key_axe), drawMethod: UICustomisations.DrawEmote//, emoteSize: new Vector2(26, 26)
        );

    }
}
