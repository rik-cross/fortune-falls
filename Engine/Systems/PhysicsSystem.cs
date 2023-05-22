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
            transformComponent.PreviousPosition = transformComponent.Position;
            physicsComponent.PreviousVelocity = physicsComponent.Velocity;

            // Change:
            // Use PreviousSpeed in PhysicsComponent and apply modifier (0.0-X)
            // if different, then set previous to current speed

            // Process anything that is player-only physics
            if (entity.IsPlayerType())
            {
                // If the run button is pressed increase the players speed by 50%
                if (EngineGlobals.inputManager.IsPressed(Globals.button2Input)) // OR IntentionComponent?
                    IncreaseSpeed(entity, 2.2f); //3.5f);//1.5f);

                // FIX this is not called if released during scene transition
                if (EngineGlobals.inputManager.IsReleased(Globals.button2Input))
                    DecreaseSpeed(entity, 2.2f); //3.5f);//1.5f);
            }

            // Set the direction vector and string
            Vector2 direction = Vector2.Zero;

            if (intentionComponent.up)
                direction.Y -= 1;
            if (intentionComponent.down)
                direction.Y += 1;
            if (intentionComponent.left)
                direction.X -= 1;
            if (intentionComponent.right)
                direction.X += 1;

            string directionString = GetDirectionString(direction);

            // Update the physics component
            physicsComponent.Direction = direction;
            physicsComponent.DirectionString = directionString;

            // Calculate the velocity
            if (direction != Vector2.Zero)
            {
                direction.Normalize();

                float speed = physicsComponent.Speed; // units/second
                float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                Vector2 velocity = direction * speed * deltaTime;

                transformComponent.Position += velocity;
                physicsComponent.Velocity = velocity;
            }
            else
            {
                physicsComponent.Velocity = Vector2.Zero;
            }
        }

        // Gets the cardinal and ordinal direction based on the direction vector
        public string GetDirectionString(Vector2 direction)
        {
            string directionString = ""; // Can be N, NE, E, SE etc.

            if (direction.Y == -1)
                directionString = "N";
            else if (direction.Y == 1)
                directionString = "S";

            if (direction.X == 1)
                directionString += "E";
            else if (direction.X == -1)
                directionString += "W";

            return directionString;
        }

        // Increase the movement and animation speed of an entity
        public void IncreaseSpeed(Entity entity, float speedModifier)
        {
            PhysicsComponent physicsComponent = entity.GetComponent<PhysicsComponent>();
            SpriteComponent spriteComponent = entity.GetComponent<SpriteComponent>();

            physicsComponent.ApplySpeedModifier(speedModifier);
            Console.WriteLine($"Speed is {physicsComponent.Speed}");

            //if (spriteComponent != null)
            //    spriteComponent.ModifyAnimationDelay(1 / speedModifier);
        }

        // Decrease the movement and animation speed of an entity
        public void DecreaseSpeed(Entity entity, float speedModifier)
        {
            PhysicsComponent physicsComponent = entity.GetComponent<PhysicsComponent>();
            SpriteComponent spriteComponent = entity.GetComponent<SpriteComponent>();

            physicsComponent.ApplySpeedModifier(1 / speedModifier);
            Console.WriteLine($"Speed is {physicsComponent.Speed}");

            //if (spriteComponent != null)
            //    spriteComponent.ModifyAnimationDelay(speedModifier);
        }
    }
}
