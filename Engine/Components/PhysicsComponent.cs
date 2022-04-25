using Microsoft.Xna.Framework;

namespace AdventureGame.Engine
{
    class PhysicsComponent : Component
    {
        public int velocity;

        public PhysicsComponent(int velocity)
        {
            this.velocity = velocity;
        }
    }

}
