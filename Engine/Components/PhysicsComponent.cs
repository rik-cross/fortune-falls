using Microsoft.Xna.Framework;
using System;

namespace AdventureGame.Engine
{
    class PhysicsComponent : Component
    {
        public int baseSpeed;
        public double speedModifier;
        public int speedBonus = 0;
        public int speedReduction = 0;

        //public int SpeedBonus { get; set; }

        public int speed;

        public int velocityX;
        public int velocityY;

        public string direction;

        public PhysicsComponent(int baseSpeed, string direction = "")
        {
            this.baseSpeed = baseSpeed;
            speed = baseSpeed;
            this.direction = direction;
        }

        public PhysicsComponent(int baseSpeed, double speedModifier,
            string direction = "")
        {
            this.baseSpeed = baseSpeed;
            this.speedModifier = speedModifier;
            this.direction = direction;

            CalculateSpeed();
        }

        public void SetModifier(double modifier)
        {
            speedModifier *= modifier;

            if (speedModifier < 0.0)
                speedModifier = 0.0;

            CalculateSpeed();
        }

        public int GetSpeed()
        {
            return speed;
        }

        public void SetSpeed(double multiplier)
        {
            //running = true;

            speedModifier *= multiplier;

            if (speedModifier < 0.0)
                speedModifier = 0.0;

            CalculateSpeed();
        }

        public void SpeedBonus(int speedBonus)
        {
            this.speedBonus = speedBonus;
            CalculateSpeed();
        }

        public void SpeedReduction(int speedReduction)
        {
            this.speedReduction = speedReduction;
            CalculateSpeed();
        }

        public void CalculateSpeed()
        {
            //speed = (int)(baseSpeed * speedModifier);
            double speedIncrease = baseSpeed * (speedBonus / 100);
            double speedDecrease = baseSpeed * (speedReduction / 100);

            speed = (int)(baseSpeed + speedIncrease - speedDecrease);

            Console.WriteLine($"Speed {speed}");

            speed = speed < 0 ? 0 : speed;
        }
    }

}
