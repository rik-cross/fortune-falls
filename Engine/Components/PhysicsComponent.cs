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

        public int baseSpeed;
        public double speedMultiplier;

        public PhysicsComponent(int baseSpeed = 1, double speedMultiplier = 1.0,
            string direction = "")
        {
            this.baseSpeed = baseSpeed;
            this.speedMultiplier = speedMultiplier;
            Direction = direction;

            CalculateSpeed();
        }

        public void MultiplySpeed(double multiplier)
        {
            //running = true;
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

        public void CalculateSpeed()
        {
            Speed = (int)(baseSpeed * speedMultiplier);
            Speed = Speed < 0 ? 0 : Speed;
            //Console.WriteLine($"Speed {Speed}");
        }
    }

}
