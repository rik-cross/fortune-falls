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
        private float _targetVolume;
        public float Volume {
            get
            {
                return MediaPlayer.Volume;
            }
            set
            {
                _targetVolume = value;
            }
        }
        private float volumeIncrement = 0.03f;
        private Song _currentSong = null;
        private Song _nextSong = null;
        public SoundManager()
        {
            MediaPlayer.Volume = 1.0f;
            _targetVolume = 1.0f;
            MediaPlayer.IsRepeating = true;
        }
        public void Update(GameTime gameTime)
        {
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
            soundEffect.Play(Volume, 0, 0);
        }
    }
}
