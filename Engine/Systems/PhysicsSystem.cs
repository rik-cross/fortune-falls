using Microsoft.Xna.Framework;
using System;

namespace AdventureGame.Engine
{
    class PhysicsSystem : System
    {
        public PhysicsSystem()
        {
            RequiredComponent<IntentionComponent>();
            RequiredComponent<PhysicsComponent>();
            RequiredComponent<TransformComponent>();
        }

        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            IntentionComponent intentionComponent = entity.GetComponent<IntentionComponent>();
            PhysicsComponent physicsComponent = entity.GetComponent<PhysicsComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            string direction = "";

            // CHANGE up to north, right to east etc?
            if (intentionComponent.up && !intentionComponent.down)
            {
                physicsComponent.velocityY = -physicsComponent.speed;
                transformComponent.position.Y += physicsComponent.velocityY;
                direction = "N"; // Moving north
            }
            else if (intentionComponent.down && !intentionComponent.up)
            {
                physicsComponent.velocityY = physicsComponent.speed;
                transformComponent.position.Y += physicsComponent.velocityY;
                direction = "S"; // Moving south
            }
            else
            {
                physicsComponent.velocityY = 0;
            }

            if (intentionComponent.right && !intentionComponent.left)
            {
                physicsComponent.velocityX = physicsComponent.speed;
                transformComponent.position.X += physicsComponent.velocityX;
                direction += "E"; // Moving east
            }
            else if (intentionComponent.left && !intentionComponent.right)
            {
                physicsComponent.velocityX = -physicsComponent.speed;
                transformComponent.position.X += physicsComponent.velocityX;
                direction += "W"; // Moving west
            }
            else
            {
                physicsComponent.velocityX = 0;
            }

            physicsComponent.direction = direction;
        }
    }
}
