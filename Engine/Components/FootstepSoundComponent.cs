using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Graphics;
using S = System.Diagnostics.Debug;
using Microsoft.Xna.Framework.Audio;

namespace Engine
{
    public class FootstepSoundComponent : Component
    {
        public List<int> frames;
        public SoundEffect soundEffect;
        public FootstepSoundComponent(List<int> frames, SoundEffect soundEffect)
        {
            this.frames = frames;
            this.soundEffect = soundEffect;
        }
    }
}
