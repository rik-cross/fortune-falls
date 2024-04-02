using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

using Microsoft.Xna.Framework;

using S = System.Diagnostics.Debug;

namespace AdventureGame.Engine
{
    public class SoundManager
    {
        public bool _mute = false;
        public float prevVolume;
        public float prevSFXVol;
        public bool Mute
        {
            get
            {
                return _mute;
            }
            set
            {
                if (value == true)
                {
                    prevVolume = Volume;
                    prevSFXVol = SFXVolume;
                    Volume = 0;
                    SFXVolume = 0;
                    _mute = value;
                }
                else
                {
                    _targetVolume = prevVolume;
                    SFXVolume = prevSFXVol;
                    _mute = value;
                }
            }
        }
        public float _targetVolume;
        public float Volume {
            get
            {
                return MediaPlayer.Volume;
            }
            set
            {
                _targetVolume = value;
                //if (value > 0)
                //    Mute = false;
            }
        }
        public float SFXVolume;

        private float volumeIncrement = 0.015f;
        private Song _currentSong = null;
        private Song _nextSong = null;
        public SoundManager()
        {
            MediaPlayer.Volume = 1.0f;
            _targetVolume = 1.0f;
            MediaPlayer.IsRepeating = true;
            SFXVolume = 1.0f;

            prevVolume = _targetVolume;
            prevSFXVol = SFXVolume;
        }
        public void Update(GameTime gameTime)
        {

            //S.WriteLine(Mute);

            if (_nextSong != null)
            {
                MediaPlayer.Volume -= volumeIncrement;
                if (MediaPlayer.Volume <= 0.0f)
                {
                    PlaySong(_nextSong);
                }
            }
            else {
                if (_targetVolume > MediaPlayer.Volume)
                    MediaPlayer.Volume += volumeIncrement;
                else if (_targetVolume < MediaPlayer.Volume)
                    MediaPlayer.Volume -= volumeIncrement;
                if (Math.Abs(MediaPlayer.Volume - _targetVolume) < volumeIncrement)
                    MediaPlayer.Volume = _targetVolume;
            }
        }
        public void PlaySong(Song song)
        {
            if (song == _currentSong)
            {
                _nextSong = null;
                return;
            }

            MediaPlayer.Play(song);
            _currentSong = song;
            _nextSong = null;
        }
        public void PlaySongFade(Song song)
        {
            if (song == _currentSong)
            {
                _nextSong = null;
                return;
            }
            _nextSong = song;
        }
        public void PlaySoundEffect(SoundEffect soundEffect)
        { 
            soundEffect.Play(SFXVolume, 0, 0);
        }
    }
}
