using Microsoft.Xna.Framework;
using System;

namespace AdventureGame.Engine
{
    // Should this be CollisionMovementResponseSystem?
    // or a separate system(s) for resolving damage, sounds etc on collide?
    class CollisionResponseSystem : System
    {
        //public List<>

        public CollisionResponseSystem()
        {
            RequiredComponent<PhysicsComponent>();
            RequiredComponent<ColliderComponent>();
            RequiredComponent<TransformComponent>();
            // ColliderResponseComponent? instead of the ColliderComponent?
        }

        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            PhysicsComponent physicsComponent = entity.GetComponent<PhysicsComponent>();
            ColliderComponent colliderComponent = entity.GetComponent<ColliderComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            // Respond to entities that have started colliding
            foreach (Entity collidedEntity in colliderComponent.collidedEntities)
            {
                ColliderComponent otherColliderComponent = collidedEntity.GetComponent<ColliderComponent>();

                // Return if either of the entities are not solid
                if (!colliderComponent.isSolid || !otherColliderComponent.isSolid)
                    return;

                // CHANGE to dictionary of directions <string, Boolean> ??
                string direction = physicsComponent.direction;
                Console.WriteLine($"Physics component direction: {direction}");

                // Return if the entity is not moving in any direction
                if (String.IsNullOrEmpty(direction))
                    return;

                PhysicsComponent otherPhysicsComponent = collidedEntity.GetComponent<PhysicsComponent>();
                TransformComponent otherTransformComponent = collidedEntity.GetComponent<TransformComponent>();

                //string direction = colliderComponent.collidingDirection;
                //Console.WriteLine($"Colliding direction: {direction}");
                int velocityX = physicsComponent.velocityX;
                int velocityY = physicsComponent.velocityY;

                // Absolute values of the entity
                int absVelocityX = Math.Abs(velocityX);
                int absVelocityY = Math.Abs(velocityY);

                //string colliderDirection = ""; // DELETE??

                // Declare the attributes of the other entity
                string otherDirection = "";
                int otherVelocityX = 0;
                int otherVelocityY = 0;
                int absOtherVelocityX = 0;
                int absOtherVelocityY = 0;
                //int velocityDifference = 0;

                // If the other component can move, the new position of the entities
                // should be based on the velocity and direction of both entities
                if (otherPhysicsComponent != null)
                {
                    otherDirection = otherPhysicsComponent.direction;
                    otherVelocityX = otherPhysicsComponent.velocityX;
                    otherVelocityY = otherPhysicsComponent.velocityY;
                    absOtherVelocityX = Math.Abs(otherVelocityX);
                    absOtherVelocityY = Math.Abs(otherVelocityY);

                }

                // Get both the bounding boxes
                Rectangle boundingBox = colliderComponent.boundingBox;
                Rectangle otherBoundingBox = otherColliderComponent.boundingBox;

                // Calculate the amount of overlap in each direction
                int overlapTop = otherBoundingBox.Bottom - boundingBox.Top;
                int overlapBottom = boundingBox.Bottom - otherBoundingBox.Top;
                int overlapRight = boundingBox.Right - otherBoundingBox.Left;
                int overlapLeft = otherBoundingBox.Right - boundingBox.Left;

                // The default max overlap values are the entity's velocities
                int maxOverlapX = absVelocityX; //velocityX;
                int maxOverlapY = absVelocityY; //velocityY;

                bool perpendicularDirection = false;
                // Check if the entities are moving in the same direction
                bool sameDirection = false;
                if (direction == otherDirection
                    || direction.Length == 1 && otherDirection.Contains(direction))
                    sameDirection = true;
                else if (velocityX != 0 && otherVelocityY != 0
                    || velocityY != 0 && otherVelocityX !=0)
                    //(direction == "N" && (otherDirection == "E" || otherDirection == "W"))
                    perpendicularDirection = true;


                // Resolve the entity position if the other entity is stationary
                // or both entities are moving in the same direction
                if (String.IsNullOrEmpty(otherDirection) || sameDirection
                    || perpendicularDirection)
                {
                    if (sameDirection)
                    {
                        // Check if the entity is faster than the other entity 
                        if (absVelocityX > absOtherVelocityX)
                        {
                            // Max overlap is the difference between X velocities
                            maxOverlapX = absVelocityX - absOtherVelocityX;
                            Console.WriteLine($"Same X direction");
                        }
                        else if (absVelocityY > absOtherVelocityY)
                        {
                            // Max overlap is the difference between Y velocities
                            maxOverlapY = absVelocityY - absOtherVelocityY;
                            Console.WriteLine($"Same Y direction");
                        }
                    }

                    if (perpendicularDirection)
                        Console.WriteLine("Perpendicular directions");

                    // Check if the entity is moving north
                    if (direction.Contains('N'))
                        if (overlapTop > 0 && overlapTop <= maxOverlapY)
                        {
                            transformComponent.position.Y += overlapTop;
                            Console.WriteLine($"Overlap (Y-up) {overlapTop}");
                        }

                    // Check if the entity is moving south
                    if (direction.Contains('S'))
                        if (overlapBottom > 0 && overlapBottom <= maxOverlapY)
                        {
                            transformComponent.position.Y -= overlapBottom;
                            Console.WriteLine($"Overlap (Y-down) {overlapBottom}");
                        }

                    // Check if the entity is moving east
                    if (direction.Contains('E'))
                        if (overlapRight > 0 && overlapRight <= maxOverlapX)
                        {
                            transformComponent.position.X -= overlapRight;
                            Console.WriteLine($"Overlap (X-right) {overlapRight}");
                        }

                    // Check if the entity is moving west
                    if (direction.Contains('W'))
                        if (overlapLeft > 0 && overlapLeft <= maxOverlapX)
                        {
                            transformComponent.position.X += overlapLeft;
                            Console.WriteLine($"Overlap (X-left) {overlapLeft}");
                        }
                }
                // Check if both entites are moving in the opposite direction
                // and resolve both entities as one opposing pair
                else if (direction == "N" && otherDirection == "S"
                    || direction == "E" && otherDirection == "W")
                {
                    Console.WriteLine("Opposite single directions");
                    Console.WriteLine($"Other physics component direction: {otherDirection}");
                    int totalVelocityX = absVelocityX + absOtherVelocityX;
                    int totalVelocityY = absVelocityY + absOtherVelocityY;

                    // Check if the top overlap is the largest it can be
                    if (direction == "N" && overlapTop == totalVelocityY)
                    {
                        // Move both entities back to their previous position
                        transformComponent.position.Y += -velocityY;
                        otherTransformComponent.position.Y += -otherVelocityY;

                        Console.WriteLine("Total Y velocity overlap");
                        Console.WriteLine($"Overlap top {overlapTop}");
                    }
                    else if (direction == "N")
                    {
                        // Calculate the amount to offset each entity based
                        // on the total Y velocity and the amount of overlap
                        double offsetRatio = absVelocityY / (double)totalVelocityY * overlapTop;
                        int offsetY = (int)Math.Round(offsetRatio);
                        int otherOffsetY = overlapTop - offsetY;

                        transformComponent.position.Y += offsetY;
                        otherTransformComponent.position.Y -= otherOffsetY;

                        Console.WriteLine($"Overlap top {overlapTop}");
                        Console.WriteLine($"Offset Y {offsetY}");
                        Console.WriteLine($"Other offset Y {otherOffsetY}");
                    }

                    // Check if the right overlap is the largest it can be
                    if (direction == "E" && overlapRight == totalVelocityX)
                    {
                        // Move both entities back to their previous position
                        transformComponent.position.X += -velocityX;
                        otherTransformComponent.position.X += -otherVelocityX;

                        Console.WriteLine("Total X velocity overlap");
                        Console.WriteLine($"Overlap right {overlapRight}");
                    }
                    else if (direction == "E")
                    {
                        // Calculate the amount to offset each entity based
                        // on the total X velocity and the amount of overlap
                        double offsetRatio = absVelocityX / (double)totalVelocityX * overlapRight;
                        int offsetX = (int)Math.Round(offsetRatio);
                        int otherOffsetX = overlapRight - offsetX;

                        transformComponent.position.X -= offsetX;
                        otherTransformComponent.position.X += otherOffsetX;

                        Console.WriteLine($"Overlap right {overlapRight}");
                        Console.WriteLine($"Offset X {offsetX}");
                        Console.WriteLine($"Other offset X {otherOffsetX}");
                    }
                }
                // Check if the entities are moving in a perpendicular dirction
                else if (perpendicularDirection)
                {
                    //Console.WriteLine("Perpendicular directions");

                }
            }

            // Respond to entities that have stopped colliding
            foreach (Entity collidedEntity in colliderComponent.collidedEntitiesEnded)
            {
                Console.WriteLine($"Collision response system: Entity {entity.id} stopped colliding with Entity {collidedEntity.id}");
            }
        }
        //transformComponent.position.X = newPosition;
        //transformComponent.position.X += physicsComponent.velocity;


        /*
        public void ResolveCollision(int overlap, int maxOverlap)
        {
            if (overlap > 0 && overlap <= maxOverlap)
            {
                transformComponent.position.Y += overlapTop;
                Console.WriteLine($"Overlap (Y-up) {overlapTop}");
            }
        }
        */
    }
}
