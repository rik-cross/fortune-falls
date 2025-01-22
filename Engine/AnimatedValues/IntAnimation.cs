using System;
//using S = System.Diagnostics.Debug;

namespace Engine
{
    public class IntAnimation : AnimatedValue<int>
    {
        public IntAnimation(int value, int increment = 1) : base(value, increment) { }
        
        public override void Update()
        {
            //S.WriteLine(Value + " ==> " + _target);
            if (_value == _target)
                return;
            else if (Math.Abs(Value - _target) < Increment)
                _value = _target;
            if (Value < _target)
                _value += Increment;
            else if (Value > _target)
                _value -= Increment;
        }

    }
}
