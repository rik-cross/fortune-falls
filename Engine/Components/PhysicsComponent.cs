using Microsoft.Xna.Framework;

namespace AdventureGame.Engine
{
    class PhysicsComponent : Component
    {
        public int baseSpeed;
        public int speedModifier;
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

        public PhysicsComponent(int baseSpeed, int speedModifier,
            string direction = "")
        {
            this.baseSpeed = baseSpeed;
            this.speedModifier = speedModifier;
            speed = baseSpeed * speedModifier;
            this.direction = direction;
        }
    }

}
