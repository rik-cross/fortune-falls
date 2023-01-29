using System;
using System.Collections.Generic;
using System.Text;

namespace AdventureGame.Engine
{

    public abstract class AnimatedValue<T>
    {

        protected T _value;
        public T Value
        {
            get => _value;
            set
            {
                _target = value;
            }
        }
        protected T _target;
        protected T Increment { get; set; }
        public AnimatedValue(T value, T increment)
        {
            _value = value;
            _target = value;
            Increment = increment;
        }
        public abstract void Update();
        public void Set(T value)
        {
            _value = value;
            _target = value;
        }
    }

}
