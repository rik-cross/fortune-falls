using System;
using Microsoft.Xna.Framework;

namespace Engine
{
    public class Vector2Animation : AnimatedValue<Vector2>
    {
        public Vector2Animation(Vector2 value, Vector2 increment = default) : base(value, increment)
        {
            if (increment == default)
                this.Increment = new Vector2(1.0f, 1.0f);
        }
        
        public override void Update()
        {

            // X
            if (_value.X == _target.X)
                ;
            else if (Math.Abs(_value.X - _target.X) < Increment.X)
                _value.X = _target.X;
            if (_value.X < _target.X)
                _value.X += Increment.X;
            else if (_value.X > _target.X)
                _value.X -= Increment.X;

            // Y
            if (Value.Y == _target.Y)
                ;
            else if (Math.Abs(_value.Y - _target.Y) < Increment.Y)
                _value.Y = _target.Y;
            if (_value.Y < _target.Y)
                _value.Y += Increment.Y;
            else if (_value.Y > _target.Y)
                _value.Y -= Increment.Y;

        }

    }
}
