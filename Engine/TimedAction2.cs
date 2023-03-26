using Microsoft.Xna.Framework;
using System;

namespace AdventureGame.Engine
{
    public class ActionOverTime
    {
        private Action _action;
        private Entity _entity;
        private float _secondsLeft;
        public bool IsFinished { get; private set; }

        public ActionOverTime(Entity entity, float seconds, Action action)
        {
            _entity = entity;
            _secondsLeft = seconds;
            _action = action;
            IsFinished = false;
        }

        public void Update(GameTime gameTime)
        {
            if (IsFinished)
                return; // Delete self after broadcasting IsFinished = true;? 

            if (_secondsLeft > 0)
                _secondsLeft -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_secondsLeft <= 0)
            {
                _action();
                IsFinished = true;
            }
        }

    }
}
