using Microsoft.Xna.Framework;
using System;

namespace AdventureGame.Engine
{
    class PhysicsComponent : Component
    {
        public string Direction { get; set; }
        public int Speed { get; set; }
        public int VelocityX { get; set; }
        public int VelocityY { get; set; }
        //public int SpeedBonus { get; set; }

        private int baseSpeed;
        private double speedMultiplier;
        private readonly int maxSpeed;

        public PhysicsComponent(int baseSpeed = 1, double speedMultiplier = 1.0,
            string direction = "")
        {
            this.baseSpeed = baseSpeed;
            this.speedMultiplier = speedMultiplier;
            maxSpeed = 10;
            Direction = direction;

            CalculateSpeed();
        }

        public void MultiplySpeed(double multiplier)
        {
            speedMultiplier *= multiplier;

            if (speedMultiplier < 0.0)
                speedMultiplier = 0.0;

            CalculateSpeed();
        }

        public void ResetSpeed()
        {
            speedMultiplier = 1.0;
            CalculateSpeed();
        }

        public void SetBaseSpeed(int baseSpeed)
        {
            if (baseSpeed > maxSpeed)
                this.baseSpeed = maxSpeed;
            else if (baseSpeed < 0)
                this.baseSpeed = 0;
            else
                this.baseSpeed = baseSpeed;
        }

        public void CalculateSpeed()
        {
            Speed = (int)(baseSpeed * speedMultiplier);

            if (Speed > maxSpeed)
                Speed = maxSpeed;
            else if (Speed < 0)
                Speed = 0;

            //Speed = Speed < 0 ? 0 : Speed;
            //Console.WriteLine($"Speed {Speed}");
        }
    }

}
