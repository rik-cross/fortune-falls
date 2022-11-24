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
            foreach (Entity otherEntity in colliderComponent.CollidedEntities)
            {
                ColliderComponent otherColliderComponent = otherEntity.GetComponent<ColliderComponent>();

                // Check if the other entity no longer has a collider component
                if (otherColliderComponent == null)
                {
                    colliderComponent.CollidedEntities.Remove(otherEntity);
                    return;
                }

                // Check if either of the entities are not solid
                if (!colliderComponent.IsSolid || !otherColliderComponent.IsSolid)
                    return;

                /*
                if (!colliderComponent.boundingBox.Intersects(otherColliderComponent.boundingBox))
                {
                    Console.WriteLine("Collision already resolved");
                    return;
                }
                */


                // Get the entity's direction
                string direction = physicsComponent.Direction;
                //Console.WriteLine($"\nPhysics component direction: {direction}");

                // Check if the entity is not trying to move in any direction
                if (String.IsNullOrEmpty(direction))
                    return;

                // Get the other entity's components
                PhysicsComponent otherPhysicsComponent = otherEntity.GetComponent<PhysicsComponent>();
                TransformComponent otherTransformComponent = otherEntity.GetComponent<TransformComponent>();

                //Console.WriteLine($"Entity {entity.id} has position {transformComponent.position} and previous position {transformComponent.previousPosition}");
                //Console.WriteLine($"Other entity {otherEntity.id} has position {otherTransformComponent.position} and previous position {otherTransformComponent.previousPosition}");

                // MOVE TO after isSolid
                // Return if the entity is not moving
                if (transformComponent.position == transformComponent.previousPosition)
                    return;

                bool hasOtherMovedX = otherTransformComponent.position.X != otherTransformComponent.previousPosition.X;
                bool hasOtherMovedY = otherTransformComponent.position.Y != otherTransformComponent.previousPosition.Y;
                bool hasOtherMoved = hasOtherMovedX || hasOtherMovedY;

                // Get the entity's velocities
                int velocityX = physicsComponent.VelocityX;
                int velocityY = physicsComponent.VelocityY;

                // Absolute values of the entity
                int absVelocityX = Math.Abs(velocityX);
                int absVelocityY = Math.Abs(velocityY);

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
                    otherDirection = otherPhysicsComponent.Direction;
                    otherVelocityX = otherPhysicsComponent.VelocityX;
                    otherVelocityY = otherPhysicsComponent.VelocityY;
                    absOtherVelocityX = Math.Abs(otherVelocityX);
                    absOtherVelocityY = Math.Abs(otherVelocityY);

                    //Console.WriteLine($"Other physics component direction: {otherDirection}");
                }

                // Get both the bounding boxes
                //Rectangle boundingBox = colliderComponent.boundingBox;
                //Rectangle otherBoundingBox = otherColliderComponent.boundingBox;

                Rectangle boundingBox = new Rectangle(
                    (int)transformComponent.position.X + (int)colliderComponent.Offset.X,
                    (int)transformComponent.position.Y + (int)colliderComponent.Offset.Y,
                    (int)colliderComponent.Size.X, (int)colliderComponent.Size.Y
                );

                Rectangle otherBoundingBox = new Rectangle(
                    (int)otherTransformComponent.position.X + (int)otherColliderComponent.Offset.X,
                    (int)otherTransformComponent.position.Y + (int)otherColliderComponent.Offset.Y,
                    (int)otherColliderComponent.Size.X, (int)otherColliderComponent.Size.Y
                );

                // Calculate the amount of overlap in each direction
                int overlapTop = otherBoundingBox.Bottom - boundingBox.Top;
                int overlapBottom = boundingBox.Bottom - otherBoundingBox.Top;
                int overlapRight = boundingBox.Right - otherBoundingBox.Left;
                int overlapLeft = otherBoundingBox.Right - boundingBox.Left;

                //Console.WriteLine($"Overlap top {overlapTop}");
                //Console.WriteLine($"Overlap bottom {overlapBottom}");
                //Console.WriteLine($"Overlap left {overlapLeft}");
                //Console.WriteLine($"Overlap right {overlapRight}");

                //Console.WriteLine($"Entity {entity.Id} has velocity Y {velocityY}");
                //Console.WriteLine($"Other entity {otherEntity.Id} has velocity Y {otherVelocityY}");
                //Console.WriteLine($"{hasOtherMoved} hasOtherMoved");

                // Calculate the total absolute X and Y velocities
                int totalAbsVelocityX = absVelocityX + absOtherVelocityX;
                int totalAbsVelocityY = absVelocityY + absOtherVelocityY;

                bool oneDirection = direction.Length == 1;
                bool multipleDirections = direction.Length > 1;

                // Check if the entities are moving in the same or perpendicular directions
                bool sameDirection = false;
                bool perpendicularDirection = false;

                if (direction == otherDirection
                    || oneDirection && otherDirection.Contains(direction))
                    sameDirection = true;
                else if (velocityX != 0 && otherVelocityY != 0
                    || velocityY != 0 && otherVelocityX !=0)
                    //(direction == "N" && (otherDirection == "E" || otherDirection == "W"))
                    perpendicularDirection = true;


                // Used for checking if the X or Y overlaps are the absolute max
                // for any direction that is not the same as the other direction
                bool isMaxOverlapX = false;
                bool isMaxOverlapY = false;

                // Check the max overlaps if both directions are not the same
                if (!sameDirection)
                {
                    // Check if the top or bottom overlap is the largest it can be
                    if (direction.Contains('N') && overlapTop == totalAbsVelocityY
                        || direction.Contains('S') && overlapBottom == totalAbsVelocityY)
                        isMaxOverlapY = true;

                    // Check if the right or left overlap is the largest it can be
                    if (direction.Contains('E') && overlapRight == totalAbsVelocityX
                        || direction.Contains('W') && overlapLeft == totalAbsVelocityX)
                        isMaxOverlapX = true;
                }/*
                // Otherwise calculate the new max overlaps and 
                else
                {
                    // Max overlap is the absolute difference between X velocities
                    int maxOverlapX = Math.Abs(absVelocityX - absOtherVelocityX);

                    // Max overlap is the absolute difference between Y velocities
                    int maxOverlapY = Math.Abs(absVelocityY - absOtherVelocityY);


                    // Check if the top or bottom overlap is the largest it can be
                    if (direction.Contains('N') && overlapTop == maxOverlapY
                        || direction.Contains('S') && overlapBottom == maxOverlapY)
                        isMaxOverlapY = true;

                    // Check if the right or left overlap is the largest it can be
                    if (direction.Contains('E') && overlapRight == maxOverlapX
                        || direction.Contains('W') && overlapLeft == maxOverlapX)
                        isMaxOverlapX = true;
                }*/

                // CHECK work out oppositeDirection using velocities



                // TEST
                /*if (!hasOtherMoved) //!isOtherMovingX && !isOtherMovingY)
                {
                    // The default max overlap values are the entity's velocities
                    int maxOverlapX = absVelocityX; //velocityX;
                    int maxOverlapY = absVelocityY; //velocityY;

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


                // CHECK uncomment 3rd condition

                // Resolve the entity position if the other entity is stationary
                // or both entities are moving in the same direction
                else*/ if (String.IsNullOrEmpty(otherDirection) || sameDirection
                    || (perpendicularDirection && oneDirection) || !hasOtherMoved)
                {
                    // The default max overlap values are the entity's velocities
                    int maxOverlapX = absVelocityX; //velocityX;
                    int maxOverlapY = absVelocityY; //velocityY;

                    // Calculate the new max overlaps
                    if (sameDirection && hasOtherMoved)
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

                    // TESTING
                    // AND NOT sameDirection???
                    if (isMaxOverlapX && isMaxOverlapY && !sameDirection && hasOtherMoved)
                    {
                        Console.WriteLine("First if - Both max X and Y overlaps");
                        Console.WriteLine("Resolve on the X axis only");

                        // Check if the entity is moving east
                        if (direction.Contains('E'))
                        {
                            transformComponent.position.X -= overlapRight;
                            Console.WriteLine("Max East overlap");
                            Console.WriteLine($"Overlap (X-right) {overlapRight}");
                        }

                        // Check if the entity is moving west
                        if (direction.Contains('W'))
                        {
                            transformComponent.position.X += overlapLeft;
                            Console.WriteLine("Max West overlap");
                            Console.WriteLine($"Overlap (X-left) {overlapLeft}");
                        }
                    }
                    // Resolve all valid directions
                    else
                    {
                        // Check if the entity is moving north
                        if (direction.Contains('N'))
                            if (overlapTop > 0 && overlapTop <= maxOverlapY)
                            {
                                transformComponent.position.Y += overlapTop;
                                //Console.WriteLine($"Overlap (Y-up) {overlapTop}");
                            }

                        // Check if the entity is moving south
                        if (direction.Contains('S'))
                            if (overlapBottom > 0 && overlapBottom <= maxOverlapY)
                            {
                                transformComponent.position.Y -= overlapBottom;
                                //Console.WriteLine($"Overlap (Y-down) {overlapBottom}");
                            }

                        // Check if the entity is moving east
                        if (direction.Contains('E'))
                            if (overlapRight > 0 && overlapRight <= maxOverlapX)
                            {
                                transformComponent.position.X -= overlapRight;
                                //Console.WriteLine($"Overlap (X-right) {overlapRight}");
                            }

                        // Check if the entity is moving west
                        if (direction.Contains('W'))
                            if (overlapLeft > 0 && overlapLeft <= maxOverlapX)
                            {
                                transformComponent.position.X += overlapLeft;
                                //Console.WriteLine($"Overlap (X-left) {overlapLeft}");
                            }
                    }
                }

                // Resolve both entities that are moving in a single opposite direction
                // or are both moving in multiple opposing directions.
                else if (direction == "N" && otherDirection == "S"
                    || direction == "E" && otherDirection == "W"
                    || direction == "S" && otherDirection == "N"
                    || direction == "W" && otherDirection == "E"
                    || direction == "NE" && otherDirection == "SW"
                    || direction == "NW" && otherDirection == "SE"
                    || direction == "SE" && otherDirection == "NW"
                    || direction == "SW" && otherDirection == "NE")
                    //|| (perpendicularDirection && !oneDirection))
                {
                    Console.WriteLine("Opposite directions");

                    // Store valid X and Y overlaps for resolving multiple directions
                    int overlapX = 0;
                    int overlapY = 0;

                    // Check if the entity is moving in multiple directions
                    if (multipleDirections)
                    {
                        // Get the largest valid Y overlap
                        if (direction.Contains('N') && overlapTop > 0
                            && overlapTop <= totalAbsVelocityY)
                            overlapY = overlapTop;
                        else if (direction.Contains('S') && overlapBottom > 0
                            && overlapBottom <= totalAbsVelocityY)
                            //&& overlapBottom > overlapY)
                            overlapY = overlapBottom;

                        // Get the largest valid X overlap
                        if (direction.Contains('E') && overlapRight > 0
                            && overlapRight <= totalAbsVelocityX)
                            overlapX = overlapRight;
                        else if (direction.Contains('W') && overlapLeft > 0
                            && overlapLeft <= totalAbsVelocityX)
                            //&& overlapLeft > overlapX)
                            overlapX = overlapLeft;
                    }

                    Console.WriteLine($"Overlap X {overlapX}");
                    Console.WriteLine($"Overlap Y {overlapY}");

                    if (isMaxOverlapX && isMaxOverlapY)
                    {
                        Console.WriteLine("Both max X and Y overlaps");
                        Console.WriteLine("Resolve on the X axis only");

                        if (direction.Contains('E'))
                        {
                            double offsetRatio = absVelocityX / (double)totalAbsVelocityX * overlapRight;
                            int offsetX = (int)Math.Round(offsetRatio);
                            int otherOffsetX = overlapRight - offsetX;

                            // Move both entities based on their offset values
                            MovePositions(entity, 'X', -offsetX);
                            MovePositions(otherEntity, 'X', otherOffsetX);

                            Console.WriteLine("Max East overlap");
                        }
                        else if (direction.Contains('W'))
                        {
                            double offsetRatio = absVelocityX / (double)totalAbsVelocityX * overlapLeft;
                            int offsetX = (int)Math.Round(offsetRatio);
                            int otherOffsetX = overlapLeft - offsetX;

                            // Move both entities based on their offset values
                            MovePositions(entity, 'X', offsetX);
                            MovePositions(otherEntity, 'X', -otherOffsetX);

                            Console.WriteLine("Max West overlap");
                        }
                    }

                    // Resolve entities moving in a single Y direction
                    // or entities moving in multiple directions with larger Y overlaps
                    if (oneDirection || overlapY > overlapX)
                    {
                        // Return if the collision has already been resolved if
                        // both X and Y overlaps used to be at the maximum values
                        if (!colliderComponent.BoundingBox.Intersects(otherColliderComponent.BoundingBox))
                        {
                            Console.WriteLine("Collision already resolved");
                            return;
                        }

                        // Check if the top or bottom overlap is the largest it can be
                        if (direction.Contains('N') && overlapTop == totalAbsVelocityY
                            || direction.Contains('S') && overlapBottom == totalAbsVelocityY)
                        {
                            // Move both entities back to their previous Y position
                            MovePositions(entity, 'Y', -velocityY);
                            MovePositions(otherEntity, 'Y', -otherVelocityY);
                        }
                        else if (direction.Contains('N'))
                        {
                            // Calculate the amount to offset each entity based on
                            // the total Y velocity and the amount of top overlap
                            double offsetRatio = absVelocityY / (double)totalAbsVelocityY * overlapTop;
                            int offsetY = (int)Math.Round(offsetRatio);
                            int otherOffsetY = overlapTop - offsetY;

                            // Move both entities based on their offset values
                            MovePositions(entity, 'Y', offsetY);
                            MovePositions(otherEntity, 'Y', -otherOffsetY);
                        }
                        else if (direction.Contains('S'))
                        {
                            // Calculate the amount to offset each entity based on
                            // the total Y velocity and the amount of bottom overlap
                            double offsetRatio = absVelocityY / (double)totalAbsVelocityY * overlapBottom;
                            int offsetY = (int)Math.Round(offsetRatio);
                            int otherOffsetY = overlapBottom - offsetY;

                            // Move both entities based on their offset values
                            MovePositions(entity, 'Y', -offsetY);
                            MovePositions(otherEntity, 'Y', otherOffsetY);
                        }
                    }

                    // Resolve entities moving in a single X direction
                    // or entities moving in multiple directions with larger X overlaps
                    // or equal X and Y overlaps above 0
                    if (oneDirection || overlapX > overlapY
                        || overlapX == overlapY && overlapX > 0)
                    {
                        // Check if the right or left overlap is the largest it can be
                        if (direction.Contains('E') && overlapRight == totalAbsVelocityX
                            || direction.Contains('W') && overlapLeft == totalAbsVelocityX)
                        {
                            // Move both entities back to their previous X position
                            MovePositions(entity, 'X', -velocityX);
                            MovePositions(otherEntity, 'X', -otherVelocityX);
                        }
                        else if (direction.Contains('E'))
                        {
                            // Calculate the amount to offset each entity based on
                            // the total X velocity and the amount of right overlap
                            double offsetRatio = absVelocityX / (double)totalAbsVelocityX * overlapRight;
                            int offsetX = (int)Math.Round(offsetRatio);
                            int otherOffsetX = overlapRight - offsetX;

                            // Move both entities based on their offset values
                            MovePositions(entity, 'X', -offsetX);
                            MovePositions(otherEntity, 'X', otherOffsetX);
                        }
                        else if (direction.Contains('W'))
                        {
                            // Calculate the amount to offset each entity based on
                            // the total X velocity and the amount of left overlap
                            double offsetRatio = absVelocityX / (double)totalAbsVelocityX * overlapLeft;
                            int offsetX = (int)Math.Round(offsetRatio);
                            int otherOffsetX = overlapLeft - offsetX;

                            // Move both entities based on their offset values
                            MovePositions(entity, 'X', offsetX);
                            MovePositions(otherEntity, 'X', -otherOffsetX);
                        }
                    }
                }
                // Resolve entities that are moving in perpendicular directions
                // and where the entity is moving in multiple directions
                else if (perpendicularDirection && !oneDirection)
                        //&& direction.Contains('N') || direction.Contains('E'))
                {
                    Console.WriteLine("\nPerpendicular directions");

                    // Store valid X and Y overlaps
                    int overlapX = 0;
                    int overlapY = 0;

                    // Get the largest valid Y overlap
                    if (direction.Contains('N') && overlapTop > 0
                        && overlapTop <= totalAbsVelocityY)
                        overlapY = overlapTop;
                    else if (direction.Contains('S') && overlapBottom > 0
                        && overlapBottom <= totalAbsVelocityY
                        && overlapBottom > overlapY)
                        overlapY = overlapBottom;

                    // Get the largest valid X overlap
                    if (direction.Contains('E') && overlapRight > 0
                        && overlapRight <= totalAbsVelocityX)
                        overlapX = overlapRight;
                    else if (direction.Contains('W') && overlapLeft > 0
                        && overlapLeft <= totalAbsVelocityX
                        && overlapLeft > overlapX)
                        overlapX = overlapLeft;

                    Console.WriteLine($"Overlap X {overlapX}");
                    Console.WriteLine($"Overlap Y {overlapY}");

                    // Resolve perpendicular directions when the other entity
                    // is only moving in one direction
                    if (otherDirection.Length == 1 || otherDirection.Length == 2) // TEST
                    {
                        if (otherDirection.Length == 1)
                            Console.WriteLine("One direction - other entity");
                        else if (otherDirection.Length == 2)
                            Console.WriteLine("Two directions - other entity");

                        if (overlapX > overlapY
                            || overlapX == overlapY && overlapX > 0)
                        {
                            if (isMaxOverlapX && isMaxOverlapY)
                            {
                                Console.WriteLine("Both max X and Y overlaps");
                                //entity.RemoveComponent<PhysicsComponent>();
                                //otherEntity.RemoveComponent<PhysicsComponent>();
                            }

                            if (direction.Contains('E'))
                            {
                                // Calculate the amount to offset each entity based on
                                // the total X velocity and the amount of right overlap
                                double offsetRatio = absVelocityX / (double)totalAbsVelocityX * overlapRight;
                                int offsetX = (int)Math.Round(offsetRatio);
                                int otherOffsetX = overlapRight - offsetX;

                                // Move both entities based on their offset values
                                MovePositions(entity, 'X', -offsetX);
                                MovePositions(otherEntity, 'X', otherOffsetX);

                                Console.WriteLine("East overlap");
                            }
                            else if (direction.Contains('W'))
                            {
                                // Calculate the amount to offset each entity based on
                                // the total X velocity and the amount of left overlap
                                double offsetRatio = absVelocityX / (double)totalAbsVelocityX * overlapLeft;
                                int offsetX = (int)Math.Round(offsetRatio);
                                int otherOffsetX = overlapLeft - offsetX;

                                // Move both entities based on their offset values
                                MovePositions(entity, 'X', offsetX);
                                MovePositions(otherEntity, 'X', -otherOffsetX);

                                Console.WriteLine("West overlap");
                            }
                        }

                        // Resolve entities with larger Y overlaps
                        else if (overlapY > overlapX)
                        {
                            // Return if the collision has already been resolved
                            // in the X axis if both X and Y overlaps used to be
                            // equal and the maximum overlap amount
                            if (isMaxOverlapY &&
                                !colliderComponent.BoundingBox.Intersects(otherColliderComponent.BoundingBox))
                            {
                                Console.WriteLine("Collision already resolved");
                                return;
                            }

                            if (direction.Contains('N'))
                            {
                                // Calculate the amount to offset each entity based on
                                // the total Y velocity and the amount of top overlap
                                double offsetRatio = absVelocityY / (double)totalAbsVelocityY * overlapTop;
                                int offsetY = (int)Math.Round(offsetRatio);
                                int otherOffsetY = overlapTop - offsetY;

                                // Move both entities based on their offset values
                                MovePositions(entity, 'Y', offsetY);
                                MovePositions(otherEntity, 'Y', -otherOffsetY);

                                Console.WriteLine("North overlap");
                            }
                            else if (direction.Contains('S'))
                            {
                                // Calculate the amount to offset each entity based on
                                // the total Y velocity and the amount of bottom overlap
                                double offsetRatio = absVelocityY / (double)totalAbsVelocityY * overlapBottom;
                                int offsetY = (int)Math.Round(offsetRatio);
                                int otherOffsetY = overlapBottom - offsetY;

                                // Move both entities based on their offset values
                                MovePositions(entity, 'Y', -offsetY);
                                MovePositions(otherEntity, 'Y', +otherOffsetY);

                                Console.WriteLine("South overlap");
                            }
                        }

                        //entity.RemoveComponent<PhysicsComponent>();
                        //otherEntity.RemoveComponent<PhysicsComponent>();
                    }
                    // Resolve perpendicular directions when both entities
                    // are moving in multiple directions
                    else if (otherDirection.Length > 1)
                    {
                        Console.WriteLine("Multiple directions - other entity");


                    }
                    /*
                    // Collision were not resolved - it may have already been
                    // resolved by the other entity in the same game tick
                    else
                        return;

                    // Testing: output resolved collisions set
                    Console.WriteLine("\nResolved collisions set (before add): ");
                    Console.WriteLine($"Entity {entity.id}");
                    foreach (Entity e in colliderComponent.resolvedCollisions)
                        Console.WriteLine($"Other entity {e.id}  ");
                    Console.WriteLine();

                    colliderComponent.resolvedCollisions.Add(otherEntity);

                    // Testing: output resolved collisions set
                    Console.WriteLine("\nResolved collisions set (after add): ");
                    Console.WriteLine($"Entity {entity.id}");
                    foreach (Entity e in colliderComponent.resolvedCollisions)
                        Console.WriteLine($"Other entity {e.id}  ");
                    Console.WriteLine();
                    */
                }
                
                //Console.WriteLine("END OF RESOLUTION");

                //Console.WriteLine($"Entity {entity.id} has position {transformComponent.position} and previous position {transformComponent.previousPosition}");
                //Console.WriteLine($"Other entity {otherEntity.id} has position {otherTransformComponent.position} and previous position {otherTransformComponent.previousPosition}");
                

            }

            // Respond to entities that have stopped colliding
            foreach (Entity otherEntity in colliderComponent.CollidedEntitiesEnded)
            {
                ColliderComponent otherColliderComponent = otherEntity.GetComponent<ColliderComponent>();

                // Check if the other entity no longer has a collider component
                if (otherColliderComponent == null)
                {
                    colliderComponent.CollidedEntitiesEnded.Remove(otherEntity);
                    return;
                }

                //Console.WriteLine($"Collision response system: Entity {entity.id} stopped colliding with Entity {otherEntity.id}");
            }
        }
        //transformComponent.position.X = newPosition;
        //transformComponent.position.X += physicsComponent.velocity;

        // Move an entity's transform component position and bounding bounding
        // by a given amount
        public void MovePositions(Entity e, char direction, int amount)
        {
            TransformComponent transformComponent = e.GetComponent<TransformComponent>();
            ColliderComponent colliderComponent = e.GetComponent<ColliderComponent>();

            // Check if the direction is in the X or Y axis
            if (direction.Equals('X'))
            {
                transformComponent.position.X += amount;
                colliderComponent.BoundingBox.X += amount;
            }
            else if (direction.Equals('Y'))
            {
                transformComponent.position.Y += amount;
                colliderComponent.BoundingBox.Y += amount;
            }
        }


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
