using Microsoft.Xna.Framework;
using System;

namespace AdventureGame.Engine
{
    // todo? rename to MovementSystem
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
            IntentionComponent intention = entity.GetComponent<IntentionComponent>();
            PhysicsComponent physicsComponent = entity.GetComponent<PhysicsComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            // Set the previous position to the current position
            transformComponent.PreviousPosition = transformComponent.Position;
            physicsComponent.PreviousVelocity = physicsComponent.Velocity;

            // Sprint
            // todo - change so that methods are called ONCE (when sprint changes)
            //if (intention.AnyChanged())
            //{
            //    Console.WriteLine("Physics - intention has changed");
            //}   

            if (intention.Start("sprint"))
            {
                //Console.WriteLine("Physics - start sprint");
                physicsComponent.StartSprint();
            }
            else if (intention.Stop("sprint"))
            {
                physicsComponent.StopSprint();
            }

            // Set the direction vector and string
            Vector2 direction = Vector2.Zero;

            if (intention.Get("up"))
                direction.Y -= 1;
            if (intention.Get("down"))
                direction.Y += 1;
            if (intention.Get("left"))
                direction.X -= 1;
            if (intention.Get("right"))
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
    }
}
