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

            string direction = ""; // Can be N, NE, E, SE etc.

            // Set the previous position to the current position
            transformComponent.previousPosition = transformComponent.position;

            // Action anything that is player only
            if (entity.IsPlayerType())
            {
                // If the run button is pressed increase the players speed by 50%
                if (EngineGlobals.inputManager.IsPressed(Globals.button2Input)) // OR IntentionComponent?
                {
                    // Increase the speed multipler by x2
                    physicsComponent.MultiplySpeed(2);

                    // TO DO: change this so that a modified delay is applied to the
                    // sprite component instead of being recalculated here
                    // Halve the animation delay of the sprite
                    SpriteComponent spriteComponent = entity.GetComponent<SpriteComponent>();
                    if (spriteComponent != null)
                        spriteComponent.ModifyAnimationDelay(0.5f);

                    Console.WriteLine("Button 2 pressed");
                    Console.WriteLine($"Speed is {physicsComponent.Speed}");
                }

                // FIX this is not called if released during scene transition / pause
                // Should a check by made on prev / current input state and updated?
                if (EngineGlobals.inputManager.IsReleased(Globals.button2Input))
                {
                    // Decrease the speed multipler by x0.5
                    physicsComponent.MultiplySpeed(0.5);

                    // Double the animation delay of the sprite
                    SpriteComponent spriteComponent = entity.GetComponent<SpriteComponent>();
                    if (spriteComponent != null)
                        spriteComponent.ModifyAnimationDelay(2.0f);

                    Console.WriteLine("Button 2 released");
                    Console.WriteLine($"Speed is now {physicsComponent.Speed}");
                }
            }

            // CHANGE up to north, right to east etc?
            if (intentionComponent.up && !intentionComponent.down)
            {
                physicsComponent.VelocityY = -physicsComponent.Speed;
                transformComponent.position.Y += physicsComponent.VelocityY;
                direction = "N"; // Moving north
            }
            else if (intentionComponent.down && !intentionComponent.up)
            {
                physicsComponent.VelocityY = physicsComponent.Speed;
                transformComponent.position.Y += physicsComponent.VelocityY;
                direction = "S"; // Moving south
            }
            else
            {
                physicsComponent.VelocityY = 0;
            }

            if (intentionComponent.right && !intentionComponent.left)
            {
                physicsComponent.VelocityX = physicsComponent.Speed;
                transformComponent.position.X += physicsComponent.VelocityX;
                direction += "E"; // Moving east
            }
            else if (intentionComponent.left && !intentionComponent.right)
            {
                physicsComponent.VelocityX = -physicsComponent.Speed;
                transformComponent.position.X += physicsComponent.VelocityX;
                direction += "W"; // Moving west
            }
            else
            {
                physicsComponent.VelocityX = 0;
            }

            physicsComponent.Direction = direction;
        }
    }
}
