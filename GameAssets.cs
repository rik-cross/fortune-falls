using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace AdventureGame
{
    public static class GameAssets
    {

        public static int UIBoxBorder = 8;
        public static int EmoteHeightAboveEntity = 12;
        public static int EmoteBorderSize = 12;

        //private static string contentLocation = "../../../Content/";

        // spritesheets
        //public static Engine.SpriteSheet emote_spritesheet = new Engine.SpriteSheet("../../../Content/Emojis/emotes.png");

        // general images
        public static Texture2D marker = Engine.Utils.LoadTexture("UI/pointer.png");
        public static Texture2D texture_controller_movement = Engine.Utils.LoadTexture("Emojis/controller_movement.png");
        public static Texture2D texture_keyboard_movement = Engine.Utils.LoadTexture("Emojis/keyboard_movement.png");
        public static Texture2D enter = Engine.Utils.LoadTexture("Emojis/enter.png");
        public static Texture2D button_a = Engine.Utils.LoadTexture("Emojis/button_a.png");
        public static Texture2D right_trigger = Engine.Utils.LoadTexture("Emojis/right_trigger.png");
        public static Texture2D key_axe = Engine.Utils.LoadTexture("Emojis/key.png");
        // this is the texture for keyboard sprint
        public static Texture2D shift = Engine.Utils.LoadTexture("Emojis/shift.png");
        // this is the texture for controller sprint
        public static Texture2D button_b = Engine.Utils.LoadTexture("Emojis/button_b.png");

        public static Texture2D texture_axe_broke = Engine.Utils.LoadTexture("Emojis/axeBroke.png");

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
        //public static Engine.Image emote_pickaxe = new Engine.Image(Engine.Utils.LoadTexture("Emojis/emote_pickaxe.png"));

        // npc headshots

        // blacksmith
        public static Texture2D blacksmith_headshot = Engine.Utils.LoadTexture("Characters/NPC/headshot.png");

        // sound effects

        public static SoundEffect sound_notification = Engine.Utils.LoadSoundEffect("Sounds/notification.wav");

        // components

        public static Engine.EmoteComponent speakEmote;
        

        public static Engine.AnimatedEmoteComponent controllerMovementEmote = new Engine.AnimatedEmoteComponent(
            list_texture_controller_movement,
            frameDelay: 20,
            borderSize: EmoteBorderSize,
            heightAboveEntity: EmoteHeightAboveEntity,
            drawMethod: UICustomisations.DrawAnimatedEmote,
            textureSize: new Vector2(13*3, 13*3)
        );

        public static Engine.AnimatedEmoteComponent keyboardMovementEmote = new Engine.AnimatedEmoteComponent(
            list_texture_keyboard_movement,
            frameDelay: 20,
            borderSize: EmoteBorderSize,
            heightAboveEntity: EmoteHeightAboveEntity,
            drawMethod: UICustomisations.DrawAnimatedEmote,
            textureSize: new Vector2(26*4, 13*4)
        );

        public static Engine.EmoteComponent controllerSprintEmote = new Engine.EmoteComponent(
            button_b,
            borderSize: EmoteBorderSize,
            heightAboveEntity: EmoteHeightAboveEntity,
            drawMethod: UICustomisations.DrawEmote,
            textureSize: new Vector2(13 * 3, 13 * 3)
        );

        public static Engine.EmoteComponent keyboardSprintEmote = new Engine.EmoteComponent(
            shift,
            borderSize: EmoteBorderSize,
            heightAboveEntity: EmoteHeightAboveEntity,
            drawMethod: UICustomisations.DrawEmote,
            textureSize: new Vector2(21 * 3, 9 * 3)
        );

        //public static Engine.EmoteComponent sprintEmote = keyboardSprintEmote;
        
        public static Engine.EmoteComponent controllerInteractEmote = new Engine.EmoteComponent(
            button_a,
            borderSize: EmoteBorderSize,
            heightAboveEntity: EmoteHeightAboveEntity,
            drawMethod: UICustomisations.DrawEmote,
            textureSize: new Vector2(13 * 3, 13 * 3)
        );

        public static Engine.EmoteComponent keyboardInteractEmote = new Engine.EmoteComponent(
            enter,
            borderSize: EmoteBorderSize,
            heightAboveEntity: EmoteHeightAboveEntity,
            drawMethod: UICustomisations.DrawEmote,
            textureSize: new Vector2(13 * 3, 13 * 3)
        );

        public static Engine.EmoteComponent controllerWeaponEmote = new Engine.EmoteComponent(
            right_trigger,
            borderSize: EmoteBorderSize,
            heightAboveEntity: EmoteHeightAboveEntity,
            drawMethod: UICustomisations.DrawEmote,
            textureSize: new Vector2(13*3, 13*3)
        );

        public static Engine.EmoteComponent keyboardWeaponEmote = new Engine.EmoteComponent(
            key_axe,
            borderSize: EmoteBorderSize,
            heightAboveEntity: EmoteHeightAboveEntity,
            drawMethod: UICustomisations.DrawEmote,
            textureSize: new Vector2(13 * 3, 13 * 3)
        );

        public static Engine.EmoteComponent AxeBrokeEmote = new Engine.EmoteComponent(
            texture_axe_broke,
            borderSize: EmoteBorderSize,
            heightAboveEntity: EmoteHeightAboveEntity,
            drawMethod: UICustomisations.DrawEmote,
            textureSize: new Vector2(13 * 3, 13 * 3)
        );



    }
}
