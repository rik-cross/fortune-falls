using Microsoft.Xna.Framework;
using System;

namespace AdventureGame.Engine
{
    class PhysicsComponent : Component
    {
        public Vector2 Velocity { get; set; }
        public Vector2 PreviousVelocity { get; set; }
        public Vector2 Direction { get; set; }
        public string DirectionString { get; set; }
        public float Speed { get; private set; }
        public bool CanSprint { get; private set; }
        public bool IsSprint { get; private set; }

        private float _baseSpeed;
        private float _maxSpeed;
        private float _minSpeed;
        private float _sprintMultiplier;
        private float _speedBuff;
        private float _slowDebuff;

        public PhysicsComponent(
            float baseSpeed = 50,
            float sprintMultiplier = 2.0f,
            float maxSpeed = 100,
            float minSpeed = 0,
            bool canSprint = true,
            bool isSprint = false,
            float speedBuff = 1,
            float slowDebuff = 1)
        {
            _baseSpeed = baseSpeed;
            _sprintMultiplier = sprintMultiplier;
            _maxSpeed = maxSpeed;
            _minSpeed = minSpeed;

            CanSprint = canSprint;
            if (CanSprint)
                IsSprint = isSprint;

            Velocity = Vector2.Zero;
            PreviousVelocity = Vector2.Zero;
            Direction = Vector2.Zero;
            DirectionString = "";

            CalculateSpeed();
        }

        public void CalculateSpeed()
        {
            //Speed = _baseSpeed * _speedMultiplier;
            Speed = _baseSpeed * _speedBuff * _slowDebuff;

            if (IsSprint)
                Speed *= _sprintMultiplier;

            if (Speed > _maxSpeed)
                Speed = _maxSpeed;
            else if (Speed < _minSpeed)
                Speed = _minSpeed;

            //Speed = Speed < 0 ? 0 : Speed;
            //Console.WriteLine($"Speed {Speed}");
        }

        public void StartSprint()
        {
            if (CanSprint)
            {
                IsSprint = true;
                CalculateSpeed();
            }
        }

        public void StopSprint()
        {
            IsSprint = false;
            CalculateSpeed();
        }

        public void EnableSprint()
        {
            CanSprint = true;
            CalculateSpeed();
        }

        public void DisableSprint()
        {
            CanSprint = false;
            IsSprint = false;
            CalculateSpeed();
        }

        // Multipler > 1
        // duration? (default forever)
        public void ApplySpeedBuff(float multiplier,
            bool replaceExisting = false, bool keepHighest = false)
        {
            if (replaceExisting || _speedBuff == 1)
            {
                if (keepHighest && multiplier > _speedBuff)
                {
                    if (multiplier > 1)
                    {
                        _speedBuff = multiplier;
                        CalculateSpeed();
                    }
                }
            }
        }

        // Multiplier >= 0 and < 1
        // duration? (default forever)
        public void ApplySlowDebuff(float multiplier,
            bool replaceExisting = false, bool keepLowest = false)
        {
            if (replaceExisting || _slowDebuff == 1)
            {
                if (keepLowest && multiplier < _slowDebuff)
                {
                    if (multiplier < 1 && multiplier >= 0)
                    {
                        _slowDebuff = multiplier;
                        CalculateSpeed();
                    }
                }
            }
        }

        public void ClearSpeedBuff()
        {
            _speedBuff = 1;
            CalculateSpeed();
        }

        public void ClearSlowDebuff()
        {
            _slowDebuff = 1;
            CalculateSpeed();
        }

        public void ResetSpeed()
        {
            IsSprint = false;
            _speedBuff = 1;
            _slowDebuff = 1;
            CalculateSpeed();
        }

        public void SetBaseSpeed(int baseSpeed)
        {
            if (baseSpeed > _maxSpeed)
                _baseSpeed = _maxSpeed;
            else if (baseSpeed < _minSpeed)
                _baseSpeed = _minSpeed;
            else
                _baseSpeed = baseSpeed;
        }

        public bool HasVelocity()
        {
            return Velocity != Vector2.Zero;
        }

        public bool HasVelocityX()
        {
            return Velocity.X != Vector2.Zero.X;
        }

        public bool HasVelocityY()
        {
            return Velocity.Y != Vector2.Zero.Y;
        }

        public bool WasMoving()
        {
            return Velocity != PreviousVelocity && Velocity == Vector2.Zero;
        }

        public bool IsMovingOneDirection()
        {
            return (Velocity.X != 0 && Velocity.Y == 0) || (Velocity.Y != 0 && Velocity.X == 0);
        }

        public bool IsMovingMultipleDirections()
        {
            return Velocity.X != 0 && Velocity.Y != 0;
        }
    }

}
