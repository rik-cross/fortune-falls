using System;

namespace Engine
{
    public class DoubleAnimation : AnimatedValue<double>
    {
        public DoubleAnimation(double value, double increment = 0.1) : base(value, increment) { }
        
        public override void Update()
        {
            if (_value == _target)
                return;
            else if (Math.Abs(_value - _target) < Increment)
                _value = _target;
            if (_value < _target)
                _value += Increment;
            else if (_value > _target)
                _value -= Increment;
        }

    }
}
