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
        public double speedModifier;
        public int speedBonus = 0;
        public int speedReduction = 0;

        public PhysicsComponent(int baseSpeed = 1, string direction = "")
        {
            this.baseSpeed = baseSpeed;
            Speed = baseSpeed;
            Direction = direction;
        }

        public PhysicsComponent(int baseSpeed, double speedModifier,
            string direction = "")
        {
            this.baseSpeed = baseSpeed;
            this.speedModifier = speedModifier;
            Direction = direction;

            CalculateSpeed();
        }

        public void SetModifier(double modifier)
        {
            speedModifier *= modifier;

            if (speedModifier < 0.0)
                speedModifier = 0.0;

            CalculateSpeed();
        }

        public void SetSpeed(double multiplier)
        {
            //running = true;

            speedModifier *= multiplier;

            if (speedModifier < 0.0)
                speedModifier = 0.0;

            CalculateSpeed();
        }

        public void ModifySpeedBonus(int speedBonus)
        {
            //this.speedBonus += speedBonus;
            this.speedBonus = speedBonus;
            CalculateSpeed();
        }

        public void ReduceSpeed(int speedReduction)
        {
            this.speedReduction = speedReduction;
            CalculateSpeed();
        }

        public void CalculateSpeed()
        {
            //speed = (int)(baseSpeed * speedModifier);
            double speedIncrease = baseSpeed * (speedBonus / 100);
            double speedDecrease = baseSpeed * (speedReduction / 100);

            Speed = (int)(baseSpeed + speedIncrease - speedDecrease);

            //Console.WriteLine($"Speed {Speed}");

            Speed = Speed < 0 ? 0 : Speed;
        }
    }

}
