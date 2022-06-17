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

                    Console.WriteLine($"Other physics component direction: {otherDirection}");
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

                bool oneDirection = direction.Length == 1;
                //bool twoDirections = direction.Length == 2;
                bool perpendicularDirection = false;

                // Check if the entities are moving in the same direction
                bool sameDirection = false;
                if (direction == otherDirection
                    || oneDirection && otherDirection.Contains(direction))
                    sameDirection = true;
                else if (velocityX != 0 && otherVelocityY != 0
                    || velocityY != 0 && otherVelocityX !=0)
                    //(direction == "N" && (otherDirection == "E" || otherDirection == "W"))
                    perpendicularDirection = true;


                // Resolve the entity position if the other entity is stationary
                // or both entities are moving in the same direction
                if (String.IsNullOrEmpty(otherDirection) || sameDirection
                    || (perpendicularDirection && oneDirection))
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

                // Resolve entities that are moving in a single opposite direction
                // or are both moving in multiple opposing directions.
                // Note that only one of the colliding entities is considered
                // and that the positions of both entities are resolved together.
                else if (direction == "N" && otherDirection == "S"
                    || direction == "E" && otherDirection == "W"
                    || direction == "NE" && otherDirection == "SW"
                    || direction == "NW" && otherDirection == "SE")
                {
                    Console.WriteLine("Opposite directions");

                    // Calculate the total absolute X and Y velocities
                    int totalVelocityX = absVelocityX + absOtherVelocityX;
                    int totalVelocityY = absVelocityY + absOtherVelocityY;

                    // Store valid X and Y overlaps for resolving multiple directions
                    int overlapX = 0;
                    int overlapY = 0;

                    // Check if the entities are moving in multiple directions
                    if (!oneDirection)
                    {
                        // Get the largest valid Y overlap
                        if (direction.Contains('N') && overlapTop > 0
                            && overlapTop <= totalVelocityY)
                            overlapY = overlapTop;

                        // Get the largest valid X overlap
                        if (direction.Contains('E') && overlapRight > 0
                            && overlapRight <= totalVelocityX)
                            overlapX = overlapRight;
                        else if (direction.Contains('W') && overlapLeft > 0
                            && overlapLeft <= totalVelocityX && overlapLeft > overlapX)
                            overlapX = overlapLeft;
                    }

                    // Resolve entities moving in a single Y direction
                    // or entities moving in multiple directions with larger Y overlaps
                    if (oneDirection || overlapY > overlapX)
                    {
                        // Check if the top overlap is the largest it can be
                        if (direction.Contains('N') && overlapTop == totalVelocityY)
                        {
                            // Move both entities back to their previous position
                            transformComponent.position.Y += -velocityY;
                            otherTransformComponent.position.Y += -otherVelocityY;
                        }
                        else if (direction.Contains('N'))
                        {
                            // Calculate the amount to offset each entity based on
                            // the total Y velocity and the amount of top overlap
                            double offsetRatio = absVelocityY / (double)totalVelocityY * overlapTop;
                            int offsetY = (int)Math.Round(offsetRatio);
                            int otherOffsetY = overlapTop - offsetY;

                            transformComponent.position.Y += offsetY;
                            otherTransformComponent.position.Y -= otherOffsetY;
                        }
                    }

                    // Resolve entities moving in a single X direction
                    // or entities moving in multiple directions with larger X overlaps
                    // or equal X and Y overlaps above 0
                    if (oneDirection || overlapX > overlapY
                        || overlapX == overlapY && overlapX > 0)
                    {
                        // Check if the right or left overlap is the largest it can be
                        if (direction.Contains('E') && overlapRight == totalVelocityX
                            || direction.Contains('W') && overlapLeft == totalVelocityX)
                        {
                            // Move both entities back to their previous position
                            transformComponent.position.X += -velocityX;
                            otherTransformComponent.position.X += -otherVelocityX;
                        }
                        else if (direction.Contains('E'))
                        {
                            // Calculate the amount to offset each entity based on
                            // the total X velocity and the amount of right overlap
                            double offsetRatio = absVelocityX / (double)totalVelocityX * overlapRight;
                            int offsetX = (int)Math.Round(offsetRatio);
                            int otherOffsetX = overlapRight - offsetX;

                            transformComponent.position.X -= offsetX;
                            otherTransformComponent.position.X += otherOffsetX;
                        }
                        else if (direction.Contains('W'))
                        {
                            // Calculate the amount to offset each entity based on
                            // the total X velocity and the amount of left overlap
                            double offsetRatio = absVelocityX / (double)totalVelocityX * overlapLeft;
                            int offsetX = (int)Math.Round(offsetRatio);
                            int otherOffsetX = overlapLeft - offsetX;

                            transformComponent.position.X += offsetX;
                            otherTransformComponent.position.X -= otherOffsetX;
                        }
                    }
                }
                // Resolve entities that are moving in perpendicular directions
                // and where the entity is moving in multiple directions
                else if (perpendicularDirection && !oneDirection)
                {
                    Console.WriteLine("Perpendicular directions - multiple directions");

                    // Calculate the total absolute X and Y velocities
                    int totalVelocityX = absVelocityX + absOtherVelocityX;
                    int totalVelocityY = absVelocityY + absOtherVelocityY;

                    // Store valid X and Y overlaps
                    int overlapX = 0;
                    int overlapY = 0;

                    // Get the largest valid Y overlap
                    if (direction.Contains('N') && overlapTop > 0
                        && overlapTop <= totalVelocityY)
                        overlapY = overlapTop;
                    else if (direction.Contains('S') && overlapBottom > 0
                        && overlapBottom <= totalVelocityY && overlapBottom > overlapY)
                        overlapY = overlapBottom;

                    // Get the largest valid X overlap
                    if (direction.Contains('E') && overlapRight > 0
                        && overlapRight <= totalVelocityX)
                        overlapX = overlapRight;
                    else if (direction.Contains('W') && overlapLeft > 0
                        && overlapLeft <= totalVelocityX && overlapLeft > overlapX)
                        overlapX = overlapLeft;

                    Console.WriteLine($"Overlap X {overlapX}");
                    Console.WriteLine($"Overlap Y {overlapY}");


                    // Resolve perpendicular directions when the other entity
                    // is only moving in one direction
                    if (otherDirection.Length == 1)
                    {
                        Console.WriteLine("One direction - other entity");

                        // Resolve entities with larger Y overlaps
                        if (overlapY > overlapX)
                        {
                            // Move the entity's Y position back by the overlap amount
                            if (direction.Contains('N'))
                                transformComponent.position.Y += overlapY;
                            else if (direction.Contains('S'))
                                transformComponent.position.Y -= overlapY;

                            Console.WriteLine("Overlap Y greater");
                        }

                        // Resolve entities with larger X overlaps
                        // or equal X and Y overlaps above 0
                        else if (overlapX > overlapY
                            || overlapX == overlapY && overlapX > 0)
                        {
                            // Move the entity's X position back by the overlap amount
                            if (direction.Contains('E'))
                                transformComponent.position.X -= overlapX;
                            else if (direction.Contains('W'))
                                transformComponent.position.X += overlapX;

                            Console.WriteLine("Overlap X greater");
                        }
                    }
                    // Resolve perpendicular directions when both entities
                    // are moving in multiple directions
                    else if (otherDirection.Length > 1)
                    {
                        Console.WriteLine("Multiple directions - other entity");


                    }
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
