using Microsoft.Xna.Framework;
using System;

namespace AdventureGame.Engine
{
    class PhysicsComponent : Component
    {
        public Vector2 Direction { get; set; }
        public string DirectionString { get; set; }
        public float Speed { get; set; }

        private float _baseSpeed;
        private float _speedMultiplier;
        private float _maxSpeed;

        // TO delete
        //public int Speed { get; set; }
        public int VelocityX { get; set; }
        public int VelocityY { get; set; }
        //private int _baseSpeed;

        public PhysicsComponent(float baseSpeed = 100, float speedMultiplier = 1.0f,
            string directionString = "")
        {
            _baseSpeed = baseSpeed;
            _speedMultiplier = speedMultiplier;
            _maxSpeed = 400;

            Direction = Vector2.Zero;
            DirectionString = directionString;

            CalculateSpeed();
        }

        public bool HasVelocity()
        {
            //return !(VelocityX == 0 && VelocityY == 0);
            return Direction != Vector2.Zero;
        }

        public void ApplySpeedModifier(float multiplier)
        {
            _speedMultiplier *= multiplier;

            if (_speedMultiplier < 0.0f)
                _speedMultiplier = 0.0f;

            CalculateSpeed();
        }

        public void ResetSpeed()
        {
            _speedMultiplier = 1.0f;
            CalculateSpeed();
        }

        public void SetBaseSpeed(int baseSpeed)
        {
            if (baseSpeed > _maxSpeed)
                _baseSpeed = _maxSpeed;
            else if (baseSpeed < 0)
                _baseSpeed = 0;
            else
                _baseSpeed = baseSpeed;
        }

        public void CalculateSpeed()
        {
            Speed = _baseSpeed * _speedMultiplier;

            if (Speed > _maxSpeed)
                Speed = _maxSpeed;
            else if (Speed < 0)
                Speed = 0;

            //Speed = Speed < 0 ? 0 : Speed;
            //Console.WriteLine($"Speed {Speed}");
        }
    }

}
