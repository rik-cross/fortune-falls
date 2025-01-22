using Engine;
using Microsoft.Xna.Framework.Audio;

namespace AdventureGame
{
    public static class SoundEffects
    {
        public static SoundEffect dialogueTickSound = Utils.LoadSoundEffect("Sounds/blip.wav");
        public static SoundEffect itemPickupSound = Utils.LoadSoundEffect("Sounds/pickup_item.wav");
        public static SoundEffect footstepSound = Utils.LoadSoundEffect("Sounds/footstep.wav");
    }
}