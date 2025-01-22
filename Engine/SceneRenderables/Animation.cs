using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

using System.Collections.Generic;
using S = System.Diagnostics.Debug;

namespace Engine
{
    public class Animation : SceneRenderable
    {
        private List<Texture2D> _textureList;
        private int _animationDelay;
        private int _textureTimer;
        private int _textureIndex;
        private Color _tint;
        private bool _loop;
        private bool _play;
        private bool _reverse;

        public Animation(List<Texture2D> textureList, Vector2 size = default, int animationDelay = 8, Color tint = default, bool loop = true, bool play = true, bool reverse = false, Vector2 position = default, Anchor anchor = Anchor.None, Rectangle anchorParent = default, Padding padding = default, float alpha = 1.0f, bool visible = true) : base(position, anchor, anchorParent, padding, alpha, visible)
        {
            _textureList = textureList;

            if (!reverse)
                _textureIndex = 0;
            else
                _textureIndex = textureList.Count - 1;

            _animationDelay = animationDelay;
            _textureTimer = 0;

            if (size != default)
                Size = size;
            else
            {
                Size.X = textureList[0].Width;
                Size.Y = textureList[0].Height;
            }

            if (tint == default(Color))
                _tint = Color.White;
            else
                _tint = tint;

            _loop = loop;
            _play = play;
            _reverse = reverse;

            CalculateAnchors();
        }

        public void Play() { _play = true; }
        public void Pause() { _play = false; }

        public void Stop()
        {
            _play = false;
            Reset();
        }

        public void Reset()
        {
            _textureTimer = 0;
            if (!_reverse)
                _textureIndex = 0;
            else
                _textureIndex = _textureList.Count - 1;
        }

        public void Reverse(bool reverse)
        {
            _reverse = reverse;
        }

        public override void Update()
        {

            if (!_play)
                return;

            _textureTimer += 1;

            if (!_reverse)
            {
                if (_textureIndex == _textureList.Count - 1 && !_loop)
                {
                    //textureTimer = 0;
                    _play = false;
                }
                else
                {
                    if (_textureTimer >= _animationDelay)
                    {
                        _textureTimer = 0;
                        _textureIndex += 1;
                        if (_textureIndex > _textureList.Count - 1)
                            _textureIndex = 0;
                    }
                }
            }
            else
            {
                if (_textureIndex == 0 && !_loop)
                {
                    //textureTimer = 0;
                    _play = false;
                }
                else
                {
                    if (_textureTimer >= _animationDelay)
                    {
                        _textureTimer = 0;
                        _textureIndex -= 1;
                        if (_textureIndex < 0)
                            _textureIndex = _textureList.Count - 1;
                    }
                }
            }
             
        }

        public override void Draw()
        {
            if (!Visible)
                return;

            Globals.spriteBatch.Draw(
                _textureList[_textureIndex],
                new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y),
                _tint * Alpha
            );

            // Testing
            /*Globals.spriteBatch.DrawRectangle(
                new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y),
                Color.BlueViolet, 1);*/
        }
    }
}
