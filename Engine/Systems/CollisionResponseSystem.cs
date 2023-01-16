using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    // Should this be CollisionMovementResponseSystem?
    // or a separate system(s) for resolving damage, sounds etc on collide?
    class CollisionResponseSystem : System
    {
        // Sets to organise the types of collisions
        HashSet<Entity> _allCollisionsSet;
        HashSet<Entity> _simpleCollisionsSet;
        HashSet<Entity> _complexCollisionsSet;

        // Testing
        List<Entity> SingleCollisionsList;
        List<Entity> MultipleCollisionsList;

        HashSet<Entity> SingleCollisions;
        HashSet<Entity> MultipleCollisions;
        HashSet<Entity> MultipleMovingCollisions;

        HashSet<string> ResolvedCollisons;

        public CollisionResponseSystem()
        {
            RequiredComponent<ColliderComponent>();
            RequiredComponent<CollisionHandlerComponent>();
            //RequiredComponent<PhysicsComponent>();
            RequiredComponent<TransformComponent>();
        }


        //    // Check if there are two valid overlaps
        //    // Try the higher one first and see if the collision is resolved
        //    // If not, try the lower one and see if the collision is resolved
        //    // Otherwise resolve both





        //public void ResolveSameDirection(Entity entity, Entity otherEntity, char axis)
        //{
        //    ColliderComponent colliderComponent = entity.GetComponent<ColliderComponent>();
        //    ColliderComponent otherColliderComponent = otherEntity.GetComponent<ColliderComponent>();
        //    PhysicsComponent physicsComponent = entity.GetComponent<PhysicsComponent>();
        //    PhysicsComponent otherPhysicsComponent = otherEntity.GetComponent<PhysicsComponent>();
        //    TransformComponent transformComponent = entity.GetComponent<TransformComponent>();
        //    TransformComponent otherTransformComponent = otherEntity.GetComponent<TransformComponent>();

        //    if (otherPhysicsComponent == null)
        //        return;

        //    Console.WriteLine($"\nResolve same direction {entity.Id} and {otherEntity.Id}");

        //    // Get the bounding boxes
        //    Rectangle box = colliderComponent.Box;
        //    Rectangle otherBox = otherColliderComponent.Box;

        //    // Get the entity's distance moved, direction and velocity
        //    Vector2 distanceMoved = transformComponent.DistanceMoved();
        //    Vector2 direction = physicsComponent.Direction;
        //    Vector2 velocity = physicsComponent.Velocity;
        //    //float velocityX = physicsComponent.Velocity.X;
        //    //float velocityY = physicsComponent.Velocity.Y;
        //    float absVelocityX = Math.Abs(velocity.X); // maxOverlapX  absVelocityX
        //    float absVelocityY = Math.Abs(velocity.Y); // maxOverlapY  absVelocityY

        //    // Get the other entity's distance moved, direction and velocity
        //    Vector2 otherDistanceMoved = otherTransformComponent.DistanceMoved();
        //    Vector2 otherDirection = otherPhysicsComponent.Direction;
        //    Vector2 otherVelocity = otherPhysicsComponent.Velocity;
        //    //int otherVelocityX = otherPhysicsComponent.Velocity.X;
        //    //int otherVelocityY = otherPhysicsComponent.Velocity.Y;
        //    float absOtherVelocityX = Math.Abs(otherVelocity.X);
        //    float absOtherVelocityY = Math.Abs(otherVelocity.Y);

        //    // Calculate the total absolute X and Y velocities
        //    Vector2 totalVelocity = velocity - otherVelocity;
        //    float totalAbsVelocityX = absVelocityX + absOtherVelocityX;
        //    float totalAbsVelocityY = absVelocityY + absOtherVelocityY;

        //    // Get the entity's direction
        //    string directionString = physicsComponent.DirectionString;
        //    bool oneDirection = physicsComponent.IsMovingOneDirection();
        //    bool multipleDirections = physicsComponent.IsMovingMultipleDirections();

        //    //Console.WriteLine($"One direction {oneDirection}, multiple directions {multipleDirections}");
        //    //Console.WriteLine($"Velocity {physicsComponent.Velocity}, other velocity {otherPhysicsComponent.Velocity}");
        //    //Console.WriteLine($"Total velocity X:{totalVelocity.X} Y:{totalVelocity.Y}");

        //    // Only resolve collisions if the entity is faster than the other entity.
        //    // Snap the faster entity just behind the slower entity

        //    // CHECK complex collisions: > or >= ?
        //    // Check for an in-range overlap in the X-axis
        //    if (axis == 'X' && absVelocityX > absOtherVelocityX)
        //    {
        //        //Console.WriteLine($"Entity {entity.Id} is faster in X");
        //        //Console.WriteLine($"Max overlap: {maxOverlapX}");
        //        //Console.WriteLine($"Entity left:{colliderComponent.Left}, right:{colliderComponent.Right}. OtherEntity left:{otherColliderComponent.Left}, right:{otherColliderComponent.Right}");
        //        //Console.WriteLine($"Distance moved e:{transformComponent.DistanceMoved()} otherE{otherTransformComponent.DistanceMoved()}");

        //        // Check for a valid right overlap
        //        if (direction.X > 0)
        //        {
        //            int overlapX = box.Right - otherBox.Left;
        //            int maxOverlapX = (int)(Math.Ceiling(velocity.X) - Math.Floor(otherVelocity.X));
        //            Console.WriteLine($"Overlap (X-right) {overlapX}, max {maxOverlapX}");

        //            if (overlapX > 0 && overlapX <= maxOverlapX)
        //                SnapToSideOfOtherEntity(entity, otherEntity, "left");
        //        }
        //        // Check for a valid left overlap
        //        else if (direction.X < 0)
        //        {
        //            int overlapX = box.Left - otherBox.Right;
        //            int maxOverlapX = (int)(Math.Floor(velocity.X) - Math.Ceiling(otherVelocity.X));
        //            Console.WriteLine($"Overlap (X-left) {overlapX}, max {maxOverlapX}");

        //            if (overlapX < 0 && overlapX >= maxOverlapX)
        //                SnapToSideOfOtherEntity(entity, otherEntity, "right");
        //        }
        //    }

        //    // Check for an in-range overlap in the Y-axis
        //    else if (axis == 'Y')
        //    {
        //        // Check for a valid top overlap
        //        if (direction.Y < 0)
        //        {
        //            int overlapY = box.Top - otherBox.Bottom;
        //            int maxOverlapY = (int)(Math.Floor(velocity.Y) - Math.Ceiling(otherVelocity.Y));
        //            Console.WriteLine($"Overlap (Y-top) {overlapY}, max {maxOverlapY}");

        //            if (overlapY < 0 && overlapY >= maxOverlapY)
        //                SnapToSideOfOtherEntity(entity, otherEntity, "bottom");
        //        }
        //        // Check for a valid bottom overlap
        //        else if (direction.Y > 0)
        //        {
        //            int overlapY = box.Bottom - otherBox.Top;
        //            int maxOverlapY = (int)(Math.Ceiling(velocity.Y) - Math.Floor(otherVelocity.Y));
        //            Console.WriteLine($"Overlap (Y-bottom) {overlapY}, max {maxOverlapY}");

        //            if (overlapY > 0 && overlapY <= maxOverlapY)
        //                SnapToSideOfOtherEntity(entity, otherEntity, "top");
        //        }
        //    }

        //}



        // Resolves two entities colliding where the other entity is not moving
        public void ResolveStationary(Entity entity, Entity otherEntity, char axis)
        {
            // Get the necessary components
            ColliderComponent colliderComponent = entity.GetComponent<ColliderComponent>();
            ColliderComponent otherColliderComponent = otherEntity.GetComponent<ColliderComponent>();
            PhysicsComponent physicsComponent = entity.GetComponent<PhysicsComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            Console.WriteLine($"\nResolve stationary {axis}-axis: {entity.Id} and {otherEntity.Id}");

            // Get the bounding boxes
            Rectangle box = colliderComponent.Box;
            Rectangle otherBox = otherColliderComponent.Box;

            // CHANGE distanceMoved to velocity??
            // Get the entity's distance moved and direction
            Vector2 distanceMoved = transformComponent.DistanceMoved();
            Vector2 direction = physicsComponent.Direction;

            // Check for an in-range overlap in the X-axis
            if (axis == 'X')
            {
                // Check for a valid right overlap
                if (direction.X > 0)
                {
                    int overlapX = box.Right - otherBox.Left;
                    Console.WriteLine($"Overlap (X-right) {overlapX}, max {Math.Ceiling(distanceMoved.X)}");

                    if (overlapX > 0 && overlapX <= Math.Ceiling(distanceMoved.X))
                        SnapToSideOfOtherEntity(entity, otherEntity, "left");
                }
                // Check for a valid left overlap
                else if (direction.X < 0)
                {
                    int overlapX = box.Left - otherBox.Right;
                    Console.WriteLine($"Overlap (X-left) {overlapX}, max {Math.Floor(distanceMoved.X)}");

                    if (overlapX < 0 && overlapX >= Math.Floor(distanceMoved.X))
                        SnapToSideOfOtherEntity(entity, otherEntity, "right");
                }
            }

            // Check for an in-range overlap in the Y-axis
            else if (axis == 'Y')
            {
                // Check for a valid top overlap
                if (direction.Y < 0)
                {
                    int overlapY = box.Top - otherBox.Bottom; // -10
                    //Console.WriteLine($"Overlap (Y-top) {overlapY}, max {Math.Floor(distanceMoved.Y)}");

                    if (overlapY < 0 && overlapY >= Math.Floor(distanceMoved.Y))
                        SnapToSideOfOtherEntity(entity, otherEntity, "bottom");
                }
                // Check for a valid bottom overlap
                else if (direction.Y > 0)
                {
                    int overlapY = box.Bottom - otherBox.Top; // 10
                    //Console.WriteLine($"Overlap (Y-bottom) {overlapY}, max {Math.Ceiling(distanceMoved.Y)}");

                    if (overlapY > 0 && overlapY <= Math.Ceiling(distanceMoved.Y))
                        SnapToSideOfOtherEntity(entity, otherEntity, "top");
                }
            }

        }


        // Resolves two entities colliding that are moving in the same X or Y direction.
        // Only resolve collisions if the entity is faster than the other entity.
        // Snap the faster entity just behind the slower entity
        public void ResolveSameDirection(Entity entity, Entity otherEntity, char axis)
        {
            ColliderComponent colliderComponent = entity.GetComponent<ColliderComponent>();
            ColliderComponent otherColliderComponent = otherEntity.GetComponent<ColliderComponent>();
            PhysicsComponent physicsComponent = entity.GetComponent<PhysicsComponent>();
            PhysicsComponent otherPhysicsComponent = otherEntity.GetComponent<PhysicsComponent>();

            if (otherPhysicsComponent == null)
                return;

            Console.WriteLine($"\nResolve same direction {entity.Id} and {otherEntity.Id}");

            // Get the bounding boxes
            Rectangle box = colliderComponent.Box;
            Rectangle otherBox = otherColliderComponent.Box;

            // Get the entity's distance moved, direction and velocity
            Vector2 direction = physicsComponent.Direction;
            Vector2 velocity = physicsComponent.Velocity;
            float absVelocityX = Math.Abs(velocity.X);
            float absVelocityY = Math.Abs(velocity.Y);

            // Get the other entity's distance moved, direction and velocity
            Vector2 otherVelocity = otherPhysicsComponent.Velocity;
            float absOtherVelocityX = Math.Abs(otherVelocity.X);
            float absOtherVelocityY = Math.Abs(otherVelocity.Y);

            // CHECK complex collisions: > or >= ?
            // Check for an in-range overlap in the X-axis
            if (axis == 'X' && absVelocityX > absOtherVelocityX) // needed?
            {
                //Console.WriteLine($"Velocity X:{velocity.X}, other velocity X:{otherVelocity.X}");
                //Console.WriteLine($"Total velocity: {velocity.X - otherVelocity.X} or {otherVelocity.X - velocity.X}");
                //Console.WriteLine($"T Entity left:{transformComponent.Left} Other right:{otherTransformComponent.Right}, Entity right:{transformComponent.Right} Other left:{otherTransformComponent.Left}");
                //Console.WriteLine($"B Entity left:{colliderComponent.Left} Other right:{otherColliderComponent.Right}, Entity right:{colliderComponent.Right} Other left:{otherColliderComponent.Left}");

                // Check for a valid right overlap
                if (direction.X > 0)
                {
                    int overlapX = box.Right - otherBox.Left;
                    int maxOverlapX = (int)(Math.Ceiling(velocity.X) - Math.Floor(otherVelocity.X));// + 1;
                    Console.WriteLine($"Overlap (X-right) {overlapX}, max {maxOverlapX}");

                    if (overlapX > 0 && overlapX <= maxOverlapX)
                        SnapToSideOfOtherEntity(entity, otherEntity, "left");
                }
                // Check for a valid left overlap
                else if (direction.X < 0)
                {
                    int overlapX = box.Left - otherBox.Right;
                    int maxOverlapX = (int)(Math.Floor(velocity.X) - Math.Ceiling(otherVelocity.X));// - 1;
                    Console.WriteLine($"Overlap (X-left) {overlapX}, max {maxOverlapX}");

                    if (overlapX < 0 && overlapX >= maxOverlapX)
                        SnapToSideOfOtherEntity(entity, otherEntity, "right");
                }
                //Console.WriteLine($"T Entity left:{transformComponent.Left} Other right:{otherTransformComponent.Right}, Entity right:{transformComponent.Right} Other left:{otherTransformComponent.Left}");
                //Console.WriteLine($"B Entity left:{colliderComponent.Left} Other right:{otherColliderComponent.Right}, Entity right:{colliderComponent.Right} Other left:{otherColliderComponent.Left}");
            }

            // Check for an in-range overlap in the Y-axis
            else if (axis == 'Y' && absVelocityY > absOtherVelocityY) // needed?
            {
                // Check for a valid top overlap
                if (direction.Y < 0)
                {
                    int overlapY = box.Top - otherBox.Bottom;
                    int maxOverlapY = (int)(Math.Floor(velocity.Y) - Math.Ceiling(otherVelocity.Y));
                    Console.WriteLine($"Overlap (Y-top) {overlapY}, max {maxOverlapY}");

                    if (overlapY < 0 && overlapY >= maxOverlapY)
                        SnapToSideOfOtherEntity(entity, otherEntity, "bottom");
                }
                // Check for a valid bottom overlap
                else if (direction.Y > 0)
                {
                    int overlapY = box.Bottom - otherBox.Top;
                    int maxOverlapY = (int)(Math.Ceiling(velocity.Y) - Math.Floor(otherVelocity.Y));
                    Console.WriteLine($"Overlap (Y-bottom) {overlapY}, max {maxOverlapY}");

                    if (overlapY > 0 && overlapY <= maxOverlapY)
                        SnapToSideOfOtherEntity(entity, otherEntity, "top");
                }
            }

        }

        // Resolves two entities colliding that are moving in the opposite X or Y direction
        public void ResolveOppositeDirections(Entity entity, Entity otherEntity, char axis)
        {
            ColliderComponent colliderComponent = entity.GetComponent<ColliderComponent>();
            ColliderComponent otherColliderComponent = otherEntity.GetComponent<ColliderComponent>();
            PhysicsComponent physicsComponent = entity.GetComponent<PhysicsComponent>();
            PhysicsComponent otherPhysicsComponent = otherEntity.GetComponent<PhysicsComponent>();

            if (otherPhysicsComponent == null)
                return;

            Console.WriteLine($"\nResolve opposite directions {entity.Id} and {otherEntity.Id}");

            // Get the bounding boxes
            Rectangle box = colliderComponent.Box;
            Rectangle otherBox = otherColliderComponent.Box;

            // Get the entity's direction and velocity
            Vector2 direction = physicsComponent.Direction;
            Vector2 velocity = physicsComponent.Velocity;
            float absVelocityX = Math.Abs(velocity.X);
            float absVelocityY = Math.Abs(velocity.Y);

            // Get the other entity's direction and velocity
            Vector2 otherVelocity = otherPhysicsComponent.Velocity;
            float absOtherVelocityX = Math.Abs(otherVelocity.X);
            float absOtherVelocityY = Math.Abs(otherVelocity.Y);

            // Calculate the total absolute X and Y velocities
            float totalAbsVelocityX = (float)(Math.Ceiling(absVelocityX) + Math.Ceiling(absOtherVelocityX));
            float totalAbsVelocityY = (float)(Math.Ceiling(absVelocityY) + Math.Ceiling(absOtherVelocityY));

            //Vector2 totalVelocity = velocity - otherVelocity;
            //Console.WriteLine($"Total velocity X:{totalVelocity.X}, {totalAbsVelocityX} Y:{totalVelocity.Y}, {totalAbsVelocityY}");

            // Check for an in-range overlap in the X-axis
            if (axis == 'X') // && absVelocityX >= absOtherVelocityX) ??
            {
                // Check for a valid right overlap
                if (direction.X > 0)
                {
                    int overlapX = box.Right - otherBox.Left;
                    int maxOverlapUpper = (int)(Math.Ceiling(velocity.X) + Math.Ceiling(absOtherVelocityX));
                    int maxOverlapLower = (int)(Math.Floor(velocity.X) + Math.Floor(absOtherVelocityX));
                    Console.WriteLine($"Overlap (X-right) {overlapX}, max {maxOverlapLower} to {maxOverlapUpper}");

                    if (overlapX > 0 && overlapX <= maxOverlapUpper)
                    {
                        //if (overlapX == maxOverlapX)
                        //if (overlapX == maxOverlapUpper)
                        if (overlapX >= maxOverlapLower && overlapX <= maxOverlapUpper)
                        {
                            // Move both entities back to their original X positions
                            MoveToPreviousX(entity);
                            MoveToPreviousX(otherEntity);
                        }
                        else
                        {
                            // CHECK should velocities be rounded / ceiling / floor before totalAbs?
                            // CHECK float offsetX??
                            // Calculate the amount of offset based on the velocity of each entity
                            float offsetRatio = absVelocityX / (float)totalAbsVelocityX * overlapX;
                            int offsetX = (int)Math.Round(offsetRatio);
                            //int otherOffsetX = overlapX - offsetX;
                            //Console.WriteLine($"Offset:{offsetX}  Other offset:{otherOffsetX}");

                            // Move entity based on it's offset and snap other entity to right side
                            MoveX(entity, -offsetX);
                            //MoveX(otherEntity, -otherOffsetX);
                            SnapToSideOfOtherEntity(otherEntity, entity, "right");
                        }
                    }
                }

                // Check for a valid left overlap
                else if (direction.X < 0)
                {
                    int overlapX = box.Left - otherBox.Right;
                    int maxOverlapUpper = (int)(Math.Floor(velocity.X) - Math.Floor(absOtherVelocityX));
                    int maxOverlapLower = (int)(Math.Ceiling(velocity.X) - Math.Ceiling(absOtherVelocityX));
                    Console.WriteLine($"Overlap (X-right) {overlapX}, max {maxOverlapLower} to {maxOverlapUpper}");

                    if (overlapX < 0 && overlapX >= maxOverlapUpper)
                    {
                        //if (overlapX == maxOverlapX)
                        //if (overlapX == maxOverlapUpper)
                        if (overlapX <= maxOverlapLower && overlapX >= maxOverlapUpper)
                        {
                            // Move both entities back to their original X positions
                            MoveToPreviousX(entity);
                            MoveToPreviousX(otherEntity);
                        }
                        else
                        {
                            // CHECK should velocities be rounded / ceiling / floor before totalAbs?
                            // Calculate the amount of offset based on the velocity of each entity
                            float offsetRatio = absVelocityX / (float)totalAbsVelocityX * overlapX;
                            int offsetX = (int)Math.Round(offsetRatio); // round for - ??
                            //int otherOffsetX = overlapX - offsetX;
                            //Console.WriteLine($"Offset:{offsetX}  Other offset:{otherOffsetX}");

                            // Move entity based on it's offset and snap other entity to left side
                            MoveX(entity, -offsetX);
                            //MoveX(otherEntity, -otherOffsetX);
                            SnapToSideOfOtherEntity(otherEntity, entity, "left");
                        }
                    }
                }
            }

            else if (axis == 'Y')
            {
                // Check for a valid top overlap
                if (direction.Y < 0)
                {
                    int overlapY = box.Top - otherBox.Bottom;
                    int maxOverlapUpper = (int)(Math.Floor(velocity.Y) - Math.Floor(absOtherVelocityY));
                    int maxOverlapLower = (int)(Math.Ceiling(velocity.Y) - Math.Ceiling(absOtherVelocityY));
                    Console.WriteLine($"Overlap (Y-top) {overlapY}, max {maxOverlapLower} to {maxOverlapUpper}");

                    if (overlapY < 0 && overlapY >= maxOverlapUpper)
                    {
                        if (overlapY <= maxOverlapLower && overlapY >= maxOverlapUpper)
                        {
                            // Move both entities back to their original Y positions
                            MoveToPreviousY(entity);
                            MoveToPreviousY(otherEntity);
                        }
                        else
                        {
                            // Calculate the amount of offset based on the velocity of each entity
                            float offsetRatio = absVelocityY / (float)totalAbsVelocityY * overlapY;
                            int offsetY = (int)Math.Round(offsetRatio); // round for - ??
                            //int otherOffsetY = overlapY - offsetY;
                            //Console.WriteLine($"Offset:{offsetY}  Other offset:{otherOffsetY}");

                            // Move entity based on it's offset and snap other entity to top side
                            MoveY(entity, -offsetY);
                            SnapToSideOfOtherEntity(otherEntity, entity, "top");
                        }
                    }
                }

                // Check for a valid bottom overlap
                else if (direction.Y > 0)
                {
                    int overlapY = box.Bottom - otherBox.Top;
                    int maxOverlapUpper = (int)(Math.Ceiling(velocity.Y) + Math.Ceiling(absOtherVelocityY));
                    int maxOverlapLower = (int)(Math.Floor(velocity.Y) + Math.Floor(absOtherVelocityY));
                    Console.WriteLine($"Overlap (Y-bottom) {overlapY}, max {maxOverlapLower} to {maxOverlapUpper}");

                    if (overlapY > 0 && overlapY <= maxOverlapUpper)
                    {
                        if (overlapY >= maxOverlapLower && overlapY <= maxOverlapUpper)
                        {
                            // Move both entities back to their original Y positions
                            MoveToPreviousY(entity);
                            MoveToPreviousY(otherEntity);
                        }
                        else
                        {
                            // Calculate the amount of offset based on the velocity of each entity
                            float offsetRatio = absVelocityY / (float)totalAbsVelocityY * overlapY;
                            int offsetY = (int)Math.Round(offsetRatio);
                            //int otherOffsetY = overlapY - offsetY;
                            //Console.WriteLine($"Offset:{offsetY}  Other offset:{otherOffsetY}");

                            // Move entity based on it's offset and snap other entity to bottom side
                            MoveY(entity, -offsetY);
                            //MoveX(otherEntity, -otherOffsetX);
                            SnapToSideOfOtherEntity(otherEntity, entity, "bottom");
                        }
                    }
                }
            }

        }



        // Resolves two entities colliding that are moving in a perpendicular X or Y direction
        public void ResolvePerpendicularDirection(Entity entity, Entity otherEntity, char axis)
        {
            Console.WriteLine($"\nResolve perpendicular directions {entity.Id} and {otherEntity.Id}");

            // TESTING
            ResolveStationary(entity, otherEntity, axis);
        }

        public void ResolveOtherDirection(Entity entity, Entity otherEntity)
        {
            Console.WriteLine($"\nResolve other directions {entity.Id} and {otherEntity.Id}");
        }




        // Move the transform component and bounding bounding X position by a given amount
        public void SnapToSideOfOtherEntity(Entity entity, Entity otherEntity, string side)
        {
            ColliderComponent colliderComponent = entity.GetComponent<ColliderComponent>();
            ColliderComponent otherColliderComponent = otherEntity.GetComponent<ColliderComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();
            TransformComponent otherTransformComponent = otherEntity.GetComponent<TransformComponent>();

            int positionToBoxDifference;
            int otherPositionToBoxDifference;
            // OR float??

            if (side == "left")
            {
                // Snap right side of entity to left side of other entity
                positionToBoxDifference = (int)transformComponent.Right - colliderComponent.Right;
                otherPositionToBoxDifference = otherColliderComponent.Left - (int)otherTransformComponent.Left;

                transformComponent.Right = otherTransformComponent.Left + otherPositionToBoxDifference + positionToBoxDifference;
            }
            else if (side == "right")
            {
                // Snap left side of entity to right side of other entity
                positionToBoxDifference = colliderComponent.Left - (int)transformComponent.Left;
                otherPositionToBoxDifference = (int)otherTransformComponent.Right - otherColliderComponent.Right;

                transformComponent.Left = otherTransformComponent.Right - otherPositionToBoxDifference - positionToBoxDifference;
            }
            else if (side == "top")
            {
                // Snap bottom side of entity to top side of other entity
                positionToBoxDifference = (int)transformComponent.Bottom - colliderComponent.Bottom;
                otherPositionToBoxDifference = otherColliderComponent.Top - (int)otherTransformComponent.Top;

                transformComponent.Bottom = otherTransformComponent.Top + otherPositionToBoxDifference + positionToBoxDifference;
            }
            else if (side == "bottom")
            {
                // Snap top side of entity to bottom side of other entity
                positionToBoxDifference = colliderComponent.Top - (int)transformComponent.Top;
                otherPositionToBoxDifference = (int)otherTransformComponent.Bottom - otherColliderComponent.Bottom;

                transformComponent.Top = otherTransformComponent.Bottom - otherPositionToBoxDifference - positionToBoxDifference;
            }

            //Console.WriteLine($"Snap position {transformComponent.position}, prev position {transformComponent.previousPosition}");
            colliderComponent.GetBoundingBox(transformComponent.position);
        }





        public void HandleCollisions(Entity entity, char axis)
        {
            ColliderComponent colliderComponent = entity.GetComponent<ColliderComponent>();
            CollisionHandlerComponent handlerComponent = entity.GetComponent<CollisionHandlerComponent>();
            PhysicsComponent physicsComponent = entity.GetComponent<PhysicsComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            // Respond to entities that have started colliding
            foreach (Entity otherEntity in handlerComponent.CollidedEntities)
            {
                ColliderComponent otherColliderComponent = otherEntity.GetComponent<ColliderComponent>();
                PhysicsComponent otherPhysicsComponent = otherEntity.GetComponent<PhysicsComponent>();
                TransformComponent otherTransformComponent = otherEntity.GetComponent<TransformComponent>();

                // Check if the other entity no longer has a collider or transform component
                if (otherColliderComponent == null || otherTransformComponent == null)
                {
                    //handlerComponent.CollidedEntities.Remove(otherEntity);
                    continue;
                }

                // Check if the other entity is not solid
                if (!otherColliderComponent.IsSolid)
                    continue;

                // Check if the other entity is moving
                bool isOtherMoving = IsEntityMoving(otherEntity);

                // Re-create the bounding boxes in case collisions have already been resolved
                Rectangle box = colliderComponent.GetBoundingBox(transformComponent.position);
                Rectangle otherBox;
                if (isOtherMoving)
                    otherBox = otherColliderComponent.GetBoundingBox(otherTransformComponent.position);
                else
                    otherBox = otherColliderComponent.Box;

                // Check if the collision has already been resolved
                if (!box.Intersects(otherBox)) // axis == 'X' && 
                {
                    Console.WriteLine("Collision already resolved");
                    continue;
                }

                // Handle a collision between a moving and a stationary entity
                if (!isOtherMoving)
                {
                    ResolveStationary(entity, otherEntity, axis);
                    continue;
                }

                // Calculate the dot product
                float dotProduct = Vector2.Dot(physicsComponent.Direction, otherPhysicsComponent.Direction);
                Console.WriteLine($"Dot product {dotProduct}");

                // Handle entities moving in opposite directions
                if (dotProduct == -1 || dotProduct == -2)
                {
                    ResolveOppositeDirections(entity, otherEntity, axis);
                }

                // Handle entities moving in the same direction
                else if (dotProduct == 1)
                {
                    ResolveSameDirection(entity, otherEntity, axis);
                }

                // Hanle entities moving in perpendicular directions
                else if (dotProduct == 0 || dotProduct == -0)
                {
                    ResolvePerpendicularDirection(entity, otherEntity, axis);
                }

                // Handle all other directions
                else
                {
                    ResolveOtherDirection(entity, otherEntity);
                }
            }
        }

        // Handle simple collisions between a moving and multiple stationary entities
        // or no more than two moving entities
        public void HandleSimpleCollisions()
        {
            // Move entities in the X-axis and resolve collisions first
            foreach (Entity entity in _simpleCollisionsSet)
            {
                //Console.WriteLine($"\nX-axis collision resolving for entity {entity.Id}");
                //MoveX(entity);
                //MoveEntityToPreviousPosition(entity);
                MoveToPreviousY(entity);
            }
            foreach (Entity entity in _simpleCollisionsSet)
            {
                HandleCollisions(entity, 'X');
            }

            // Then move entities in the Y-axis and resolve collisions
            foreach (Entity entity in _simpleCollisionsSet)
            {
                //Console.WriteLine($"\nY-axis collision resolving for entity {entity.Id}");
                MoveY(entity);
            }
            foreach (Entity entity in _simpleCollisionsSet)
            {
                //Console.WriteLine($"\nY-axis collision resolving for entity {entity.Id}");
                HandleCollisions(entity, 'Y');
            }
        }

        // Handle more complex collisions with multiple entities where at least
        // two entities are moving
        public void HandleComplexCollisions()
        {
            // TO DO
            // Move all (?) the entities in the X direction
            // Calculate the distance to move each entity
            // - Resolve all stationary other entities first
            // - Then the collisions with the biggest overlap??
            // Update the distances of the other entity and any related collisions
            // if an entity has to be moved multiple times
            // Finally apply all the MoveX's for each entity
            // Repeat for Y

            // Move entities in the X-axis and resolve collisions first
            foreach (Entity entity in _complexCollisionsSet)
            {
                //Console.WriteLine($"\nX-axis collision resolving for entity {entity.Id}");
                //MoveX(entity);
                //MoveEntityToPreviousPosition(entity);
                MoveToPreviousY(entity);
            }
            foreach (Entity entity in _complexCollisionsSet)
            {
                HandleCollisions(entity, 'X');
            }

            // Then move entities in the Y-axis and resolve collisions
            foreach (Entity entity in _complexCollisionsSet)
            {
                //Console.WriteLine($"\nY-axis collision resolving for entity {entity.Id}");
                MoveY(entity);
            }
            foreach (Entity entity in _complexCollisionsSet)
            {
                //Console.WriteLine($"\nY-axis collision resolving for entity {entity.Id}");
                HandleCollisions(entity, 'Y');
            }
        }

        // Resolve X collisions first and then Y collisions for all moveable entities
        public override void Update(GameTime gameTime, Scene scene)
        {
            // Testing
            if (entityList.Count > 0)
            {
                Console.Write("Collisions to resolve!! ");
                foreach (Entity e in entityList)
                    Console.Write(e.Id + ", ");
                Console.WriteLine();
            }

            // All collisions set stores the active collisions that are valid
            // e.g. entities that are moving.
            _allCollisionsSet = new HashSet<Entity>();

            // More complex collision handling is required when an entity is colliding
            // with multiple entities and at least one of the other entities is moving.
            // This set also stores all of the other entities the entity is colliding with.
            _complexCollisionsSet = new HashSet<Entity>();

            // Simple collisions are the entities in all collisions but not in complex.
            _simpleCollisionsSet = new HashSet<Entity>();

            // Loop through each moving entity in the system list
            foreach (Entity entity in entityList)
            {
                if (!IsEntityValid(entity))
                    continue;

                CollisionHandlerComponent handler = entity.GetComponent<CollisionHandlerComponent>();
                int count = handler.CollidedEntities.Count;

                // Single collisions
                if (count == 1)
                    _allCollisionsSet.Add(entity);

                // Multiple collisions
                else if (count > 1)
                {
                    _allCollisionsSet.Add(entity);

                    bool complex = false;
                    foreach (Entity otherEntity in handler.CollidedEntities)
                    {
                        // Check if the entity is moving i.e. it has a collider component
                        if (IsEntityMoving(otherEntity))
                            complex = true;
                    }

                    // Add all of the collided entities to the complex collisions set
                    //if (complex) TESTING!
                    //{
                        _complexCollisionsSet.Add(entity);
                        _complexCollisionsSet.UnionWith(handler.CollidedEntities);
                    //}
                }
            }

            // TESTING! DANGER causes stationary to moving bug!!
            //AllCollisions = new HashSet<Entity>(entityList);

            // The complex collisions set may contain entities that do not need to be
            // resolved e.g. non-moving entities. So remove them from the set.
            _complexCollisionsSet.IntersectWith(_allCollisionsSet);

            // Remove the complex collisions in a new set
            _simpleCollisionsSet = new HashSet<Entity>(_allCollisionsSet);
            _simpleCollisionsSet.ExceptWith(_complexCollisionsSet);

            // Handle simple collisions first then complex collisions
            HandleSimpleCollisions();
            HandleComplexCollisions();

            //TestingOutputSets(); // Testing
        }

        // Returns true if the entity should be resolving collisions with other entities
        public bool IsEntityValid(Entity entity)
        {
            ColliderComponent colliderComponent = entity.GetComponent<ColliderComponent>();
            PhysicsComponent physicsComponent = entity.GetComponent<PhysicsComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            // Check if the entity is not solid
            if (!colliderComponent.IsSolid)
                return false;

            // Check if the entity is not moving
            if (physicsComponent == null)
                return false; // CHANGE either make required or use transform instead

            if (!(physicsComponent.HasVelocity() || transformComponent.HasMoved()))
                return false;

            return true;
        }

        // Returns true if the entity is moving or false if it is stationary
        public bool IsEntityMoving(Entity entity)
        {
            PhysicsComponent physicsComponent = entity.GetComponent<PhysicsComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            if (physicsComponent == null)
                return false;
            else if (!physicsComponent.HasVelocity() && !transformComponent.HasMoved())
                return false;
            else
                return true;
        }

        public void TestingOutputSets()
        {
            /*
            Console.Write("\nAll collisions: ");
            Console.WriteLine(string.Join(", ", AllCollisions));
            Console.Write("Simple collisions: ");
            Console.WriteLine(string.Join(", ", SimpleCollisionsSet));
            Console.Write("Complex collisions: ");
            Console.WriteLine(string.Join(", ", ComplexCollisionsSet));
            */

            Console.Write("\nAll collisions: ");
            foreach (Entity e in _allCollisionsSet)
            {
                Console.Write(e.Id + ", ");
            }
            Console.WriteLine();
            Console.Write("Simple collisions: ");
            foreach (Entity e in _simpleCollisionsSet)
            {
                Console.Write(e.Id + ", ");
            }
            Console.WriteLine();
            Console.Write("Complex collisions: ");
            foreach (Entity e in _complexCollisionsSet)
            {
                Console.Write(e.Id + ", ");
            }
            Console.WriteLine();
        }

        //public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        public void TestingOriginal(GameTime gameTime, Scene scene, Entity entity)
        {
            ColliderComponent colliderComponent = entity.GetComponent<ColliderComponent>();
            CollisionHandlerComponent handlerComponent = entity.GetComponent<CollisionHandlerComponent>();
            PhysicsComponent physicsComponent = entity.GetComponent<PhysicsComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            // Respond to entities that have started colliding
            foreach (Entity otherEntity in handlerComponent.CollidedEntities)
            {
                ColliderComponent otherColliderComponent = otherEntity.GetComponent<ColliderComponent>();
                TransformComponent otherTransformComponent = otherEntity.GetComponent<TransformComponent>();

                // Check if the other entity no longer has a collider or transform component
                if (otherColliderComponent == null || otherTransformComponent == null)
                {
                    handlerComponent.CollidedEntities.Remove(otherEntity);
                    continue;
                }

                // Check if either of the entities are not solid
                if (!colliderComponent.IsSolid || !otherColliderComponent.IsSolid)
                    continue;

                // Check if the entity is not moving
                if (!(physicsComponent.HasVelocity() || transformComponent.HasMoved()))
                    continue;

                // Check if the other entity is moving
                PhysicsComponent otherPhysicsComponent = otherEntity.GetComponent<PhysicsComponent>();

                bool hasOtherMoved = true;
                if (otherPhysicsComponent == null)
                    hasOtherMoved = false;
                else if (!otherPhysicsComponent.HasVelocity() && !otherTransformComponent.HasMoved())
                    hasOtherMoved = false;

                // Re-create the bounding boxes in case collisions have already been resolved
                Rectangle box = colliderComponent.GetBoundingBox(transformComponent.position);
                Rectangle otherBox;
                if (hasOtherMoved)
                    otherBox = otherColliderComponent.GetBoundingBox(otherTransformComponent.position);
                else
                    otherBox = otherColliderComponent.Box;

                // Check if the collision has already been resolved
                if (!box.Intersects(otherBox))
                {
                    Console.WriteLine("\nCollision already resolved");
                    continue;
                }

                // Resolve a collision between a moving and a stationary entity
                if (!hasOtherMoved)
                {
                    ResolveStationary(entity, otherEntity, 'X');
                    continue; // Testing
                }

                float dotProduct = Vector2.Dot(physicsComponent.Direction, otherPhysicsComponent.Direction);

                Console.WriteLine($"Dot product {dotProduct}");

                // Check if the entities are moving in opposite directions
                if (dotProduct == -1)
                {
                    ResolveOppositeDirections(entity, otherEntity, 'X');
                    continue;
                }
                // Check if the entities are moving in the same direction
                else if (dotProduct == 1)
                {
                    ResolveSameDirection(entity, otherEntity, 'X');
                    continue;
                }
                // Check if the entities are moving in perpendicular directions
                else if (dotProduct == 0 || dotProduct == -0)
                {
                    ResolvePerpendicularDirection(entity, otherEntity, 'X');
                    continue;
                }
                else
                {
                    ResolveOtherDirection(entity, otherEntity);
                    continue;
                }

                //Console.WriteLine($"Entity {entity.id} has position {transformComponent.position} and previous position {transformComponent.previousPosition}");
                //Console.WriteLine($"Other entity {otherEntity.id} has position {otherTransformComponent.position} and previous position {otherTransformComponent.previousPosition}");

                // Get the entity's velocities
                int velocityX = (int)physicsComponent.Velocity.X;
                int velocityY = (int)physicsComponent.Velocity.Y;

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
                    otherDirection = otherPhysicsComponent.DirectionString;
                    otherVelocityX = (int)otherPhysicsComponent.Velocity.X;
                    otherVelocityY = (int)otherPhysicsComponent.Velocity.Y;
                    absOtherVelocityX = Math.Abs(otherVelocityX);
                    absOtherVelocityY = Math.Abs(otherVelocityY);

                    //Console.WriteLine($"Other physics component direction: {otherDirection}");
                }

                // Calculate the amount of overlap in each direction
                int overlapTop = otherBox.Bottom - box.Top;
                int overlapBottom = box.Bottom - otherBox.Top;
                int overlapRight = box.Right - otherBox.Left;
                int overlapLeft = otherBox.Right - box.Left;

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

                // Get the entity's direction
                string direction = physicsComponent.DirectionString;
                bool oneDirection = direction.Length == 1;
                bool multipleDirections = direction.Length > 1;

                // Check if the entities are moving in the same or perpendicular directions
                bool sameDirection = false;
                bool perpendicularDirection = false;

                if (direction == otherDirection
                    || oneDirection && otherDirection.Contains(direction))
                    sameDirection = true;
                else if (velocityX != 0 && otherVelocityY != 0
                    || velocityY != 0 && otherVelocityX != 0)
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
                else*/
                if (String.IsNullOrEmpty(otherDirection) || sameDirection
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

                //else if (Vector2.Dot(physicsComponent.VelocityX, otherPhysicsComponent.VelocityX))
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
                            MoveX(entity, -offsetX);
                            MoveX(otherEntity, otherOffsetX);

                            Console.WriteLine("Max East overlap");
                        }
                        else if (direction.Contains('W'))
                        {
                            double offsetRatio = absVelocityX / (double)totalAbsVelocityX * overlapLeft;
                            int offsetX = (int)Math.Round(offsetRatio);
                            int otherOffsetX = overlapLeft - offsetX;

                            // Move both entities based on their offset values
                            MoveX(entity, offsetX);
                            MoveX(otherEntity, -otherOffsetX);

                            Console.WriteLine("Max West overlap");
                        }
                    }

                    // Resolve entities moving in a single Y direction
                    // or entities moving in multiple directions with larger Y overlaps
                    if (oneDirection || overlapY > overlapX)
                    {
                        // Continue if the collision has already been resolved if
                        // both X and Y overlaps used to be at the maximum values
                        if (!colliderComponent.Box.Intersects(otherColliderComponent.Box))
                        {
                            Console.WriteLine("Collision already resolved");
                            continue;
                        }

                        // Check if the top or bottom overlap is the largest it can be
                        if (direction.Contains('N') && overlapTop == totalAbsVelocityY
                            || direction.Contains('S') && overlapBottom == totalAbsVelocityY)
                        {
                            // Move both entities back to their previous Y position
                            MoveY(entity, -velocityY);
                            MoveY(otherEntity, -otherVelocityY);
                        }
                        else if (direction.Contains('N'))
                        {
                            // Calculate the amount to offset each entity based on
                            // the total Y velocity and the amount of top overlap
                            double offsetRatio = absVelocityY / (double)totalAbsVelocityY * overlapTop;
                            int offsetY = (int)Math.Round(offsetRatio);
                            int otherOffsetY = overlapTop - offsetY;

                            // Move both entities based on their offset values
                            MoveY(entity, offsetY);
                            MoveY(otherEntity, -otherOffsetY);
                        }
                        else if (direction.Contains('S'))
                        {
                            // Calculate the amount to offset each entity based on
                            // the total Y velocity and the amount of bottom overlap
                            double offsetRatio = absVelocityY / (double)totalAbsVelocityY * overlapBottom;
                            int offsetY = (int)Math.Round(offsetRatio);
                            int otherOffsetY = overlapBottom - offsetY;

                            // Move both entities based on their offset values
                            MoveY(entity, -offsetY);
                            MoveY(otherEntity, otherOffsetY);
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
                            MoveX(entity, -velocityX);
                            MoveX(otherEntity, -otherVelocityX);
                        }
                        else if (direction.Contains('E'))
                        {
                            // Calculate the amount to offset each entity based on
                            // the total X velocity and the amount of right overlap
                            double offsetRatio = absVelocityX / (double)totalAbsVelocityX * overlapRight;
                            int offsetX = (int)Math.Round(offsetRatio);
                            int otherOffsetX = overlapRight - offsetX;

                            // Move both entities based on their offset values
                            MoveX(entity, -offsetX);
                            MoveX(otherEntity, otherOffsetX);
                        }
                        else if (direction.Contains('W'))
                        {
                            // Calculate the amount to offset each entity based on
                            // the total X velocity and the amount of left overlap
                            double offsetRatio = absVelocityX / (double)totalAbsVelocityX * overlapLeft;
                            int offsetX = (int)Math.Round(offsetRatio);
                            int otherOffsetX = overlapLeft - offsetX;

                            // Move both entities based on their offset values
                            MoveX(entity, offsetX);
                            MoveX(otherEntity, -otherOffsetX);
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
                                MoveX(entity, -offsetX);
                                MoveX(otherEntity, otherOffsetX);

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
                                MoveX(entity, offsetX);
                                MoveX(otherEntity, -otherOffsetX);

                                Console.WriteLine("West overlap");
                            }
                        }

                        // Resolve entities with larger Y overlaps
                        else if (overlapY > overlapX)
                        {
                            // Continue if the collision has already been resolved
                            // in the X axis if both X and Y overlaps used to be
                            // equal and the maximum overlap amount
                            if (isMaxOverlapY &&
                                !colliderComponent.Box.Intersects(otherColliderComponent.Box))
                            {
                                Console.WriteLine("Collision already resolved");
                                continue;
                            }

                            if (direction.Contains('N'))
                            {
                                // Calculate the amount to offset each entity based on
                                // the total Y velocity and the amount of top overlap
                                double offsetRatio = absVelocityY / (double)totalAbsVelocityY * overlapTop;
                                int offsetY = (int)Math.Round(offsetRatio);
                                int otherOffsetY = overlapTop - offsetY;

                                // Move both entities based on their offset values
                                MoveY(entity, offsetY);
                                MoveY(otherEntity, -otherOffsetY);

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
                                MoveY(entity, -offsetY);
                                MoveY(otherEntity, +otherOffsetY);

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
                        continue;

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
            foreach (Entity otherEntity in handlerComponent.CollidedEntitiesEnded)
            {
                ColliderComponent otherColliderComponent = otherEntity.GetComponent<ColliderComponent>();

                // Check if the other entity no longer has a collider component
                if (otherColliderComponent == null)
                {
                    handlerComponent.CollidedEntitiesEnded.Remove(otherEntity);
                    continue;
                }

                //Console.WriteLine($"Collision response system: Entity {entity.id} stopped colliding with Entity {otherEntity.id}");
            }
        }
        //transformComponent.position.X = newPosition;
        //transformComponent.position.X += physicsComponent.velocity;

        // TO DELETE
        // Move the X position of an entity's transform component and bounding bounding
        public void MoveEntityToPreviousPosition(Entity e)
        {
            ColliderComponent colliderComponent = e.GetComponent<ColliderComponent>();
            TransformComponent transformComponent = e.GetComponent<TransformComponent>();
            
            Vector2 distance = transformComponent.position - transformComponent.previousPosition;

            transformComponent.position -= distance;
            colliderComponent.Box.X -= (int)distance.X; //(int)Math.Ceiling(distance.X)
            colliderComponent.Box.Y -= (int)distance.Y; //(int)Math.Ceiling(distance.Y)
            
        }

        // Move the entity back to it's previous X position and redraw the bounding box
        public void MoveToPreviousX(Entity e)
        {
            ColliderComponent colliderComponent = e.GetComponent<ColliderComponent>();
            TransformComponent transformComponent = e.GetComponent<TransformComponent>();
            
            transformComponent.position.X = transformComponent.previousPosition.X;
            colliderComponent.GetBoundingBox(transformComponent.position);
        }

        // Move the entity back to it's previous Y position and redraw the bounding box
        public void MoveToPreviousY(Entity e)
        {
            ColliderComponent colliderComponent = e.GetComponent<ColliderComponent>();
            TransformComponent transformComponent = e.GetComponent<TransformComponent>();

            transformComponent.position.Y = transformComponent.previousPosition.Y;
            colliderComponent.GetBoundingBox(transformComponent.position);
        }



        // Move the X position based on the calculated velocity
        public void MoveX(Entity e)
        {
            ColliderComponent colliderComponent = e.GetComponent<ColliderComponent>();
            PhysicsComponent physicsComponent = e.GetComponent<PhysicsComponent>();
            TransformComponent transformComponent = e.GetComponent<TransformComponent>();

            if (physicsComponent == null) // Testing
                return;

            transformComponent.position.X += physicsComponent.Velocity.X;
            colliderComponent.Box.X += (int)physicsComponent.Velocity.X;
        }

        // Move the Y position based on the calculated velocity
        public void MoveY(Entity e)
        {
            ColliderComponent colliderComponent = e.GetComponent<ColliderComponent>();
            PhysicsComponent physicsComponent = e.GetComponent<PhysicsComponent>();
            TransformComponent transformComponent = e.GetComponent<TransformComponent>();

            if (physicsComponent == null) // Testing
                return;

            transformComponent.position.Y += physicsComponent.Velocity.Y;
            colliderComponent.Box.Y += (int)physicsComponent.Velocity.Y;
        }

        // Move the transform component and bounding bounding X position by a given amount
        public void MoveX(Entity e, int amount)
        {
            ColliderComponent colliderComponent = e.GetComponent<ColliderComponent>();
            TransformComponent transformComponent = e.GetComponent<TransformComponent>();

            transformComponent.position.X += amount;
            colliderComponent.Box.X += amount;
        }

        // Move the transform component and bounding bounding Y position by a given amount
        public void MoveY(Entity e, int amount)
        {
            ColliderComponent colliderComponent = e.GetComponent<ColliderComponent>();
            TransformComponent transformComponent = e.GetComponent<TransformComponent>();

            transformComponent.position.Y += amount;
            colliderComponent.Box.Y += amount;
        }
        /*
        // Move the X position of an entity's transform component and bounding bounding
        public void MoveX(Entity e, float amount)
        {
            TransformComponent transformComponent = e.GetComponent<TransformComponent>();
            ColliderComponent colliderComponent = e.GetComponent<ColliderComponent>();

            transformComponent.position.X += amount;
            colliderComponent.Box.X += (int)amount; // (int)Math.Ceiling(amount);
        }

        // Move the Y position of an entity's transform component and bounding bounding
        public void MoveY(Entity e, float amount)
        {
            TransformComponent transformComponent = e.GetComponent<TransformComponent>();
            ColliderComponent colliderComponent = e.GetComponent<ColliderComponent>();

            transformComponent.position.Y += amount;
            colliderComponent.Box.Y += (int)amount; // (int)Math.Ceiling(amount);
        }
        */

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
