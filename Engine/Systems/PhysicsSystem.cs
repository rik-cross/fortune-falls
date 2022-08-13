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

            // Set the previous position to the current position
            transformComponent.previousPosition = transformComponent.position;

            string direction = "";

            // Modify whether a player is running or not
            /*if (intentionComponent.button2)
            {
                physicsComponent.SetSpeed(2.0);
            }
            else
            {
                physicsComponent.SetSpeed(0.5);
            }*/

            InputComponent inputComponent = EngineGlobals.entityManager.GetEntityByTag("player").GetComponent<InputComponent>();
            if (inputComponent != null)
            {
                InputMethod inputMethod = inputComponent.input;
                if (inputMethod != null)
                {
                    InputItem inputItem = inputMethod.button1;
                    if (inputItem != null)
                    {
                        if (EngineGlobals.inputManager.IsPressed(inputMethod.button2))
                            physicsComponent.SpeedBonus(50);
                        else if (EngineGlobals.inputManager.IsReleased(inputMethod.button2))
                            physicsComponent.SpeedBonus(0);
                    }
                }
            }

            //if (EngineGlobals.inputManager.IsDown(inputComponent.input.up))

            //if (EngineGlobals.inputManager.IsPressed(inputMethod.button1))

            // If the run button is pressed increase the players speed by 50%
            /*if (intentionComponent.button2)
                physicsComponent.SpeedBonus(50);
            else
                physicsComponent.SpeedBonus(0);
            */

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
