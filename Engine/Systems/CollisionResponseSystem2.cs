using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    // MOVE to a separate file?
    struct CollisionData
    {
        public float ValueX { get; private set; }
        public float ValueY { get; private set; }
        public Entity Entity { get; private set; }
        public Entity OtherEntity { get; private set; }

        public CollisionData(float valueX, float valueY, Entity entity, Entity otherEntity)
        {
            ValueX = valueX;
            ValueY = valueY;
            Entity = entity;
            OtherEntity = otherEntity;
        }
    }

    // Should this be CollisionMovementResponseSystem?
    // or a separate system(s) for resolving damage, sounds etc on collide?
    class CollisionResponseSystem2 : System
    {
        // Sets to organise the types of collisions
        HashSet<Entity> _allCollisionsSet;
        HashSet<Entity> _simpleCollisionsSet;
        HashSet<Entity> _complexCollisionsSet;

        // RENAME
        // INITIALISE elsewhere?
        Dictionary<Entity, HashSet<Entity>> stationary = new Dictionary<Entity, HashSet<Entity>>();
        Dictionary<Entity, HashSet<Entity>> moving = new Dictionary<Entity, HashSet<Entity>>();

        List<CollisionData> opposite = new List<CollisionData>();
        List<CollisionData> same = new List<CollisionData>();
        List<CollisionData> other = new List<CollisionData>(); // Needed?
        HashSet<string> pairsChecked = new HashSet<string>();

        // Testing
        List<Entity> SingleCollisionsList;
        List<Entity> MultipleCollisionsList;

        HashSet<Entity> SingleCollisions;
        HashSet<Entity> MultipleCollisions;
        HashSet<Entity> MultipleMovingCollisions;

        HashSet<string> ResolvedCollisons;

        public CollisionResponseSystem2()
        {
            RequiredComponent<ColliderComponent>();
            RequiredComponent<CollisionHandlerComponent>();
            //RequiredComponent<PhysicsComponent>();
            RequiredComponent<TransformComponent>();
        }

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

            // Get the entity's direction and distance moved
            Vector2 direction = physicsComponent.Direction;
            Vector2 distanceMoved = transformComponent.DistanceMoved();

            // Check for an in-range overlap in the X-axis
            if (axis == 'X')
            {
                // Check for a valid right overlap
                if (direction.X > 0)
                {
                    int overlapX = box.Right - otherBox.Left;
                    //Console.WriteLine($"Overlap (X-right) {overlapX}, max {Math.Ceiling(distanceMoved.X)}");

                    if (overlapX > 0 && overlapX <= Math.Ceiling(distanceMoved.X))
                        SnapToSideOfOtherEntity(entity, otherEntity, "left");
                }
                // Check for a valid left overlap
                else if (direction.X < 0)
                {
                    int overlapX = box.Left - otherBox.Right;
                    //Console.WriteLine($"Overlap (X-left) {overlapX}, max {Math.Floor(distanceMoved.X)}");

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
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();
            TransformComponent otherTransformComponent = otherEntity.GetComponent<TransformComponent>();

            if (otherPhysicsComponent == null)
                return;

            Console.WriteLine($"\nResolve same direction {entity.Id} and {otherEntity.Id}");

            // Get the bounding boxes
            Rectangle box = colliderComponent.Box;
            Rectangle otherBox = otherColliderComponent.Box;

            // Get the entity's direction and distance moved
            Vector2 direction = physicsComponent.Direction;
            Vector2 distanceMoved = transformComponent.DistanceMoved();
            float absDistanceX = Math.Abs(distanceMoved.X);
            float absDistanceY = Math.Abs(distanceMoved.Y);

            // Get the entity's direction and distance moved
            // Vector2 distanceMoved = transformComponent.DistanceMoved();

            // Get the other entity's direction and distance moved
            Vector2 otherDistanceMoved = otherTransformComponent.DistanceMoved();
            float absOtherDistanceX = Math.Abs(otherDistanceMoved.X);
            float absOtherDistanceY = Math.Abs(otherDistanceMoved.Y);

            // CHECK complex collisions: > or >= ?
            // Check for an in-range overlap in the X-axis
            if (axis == 'X' && absDistanceX > absOtherDistanceX) // needed?
            {
                //Console.WriteLine($"DistanceMoved X:{distanceMoved.X}, other distanceMoved X:{otherDistanceMoved.X}");
                //Console.WriteLine($"Total distanceMoved: {distanceMoved.X - otherDistanceMoved.X} or {otherDistanceMoved.X - distanceMoved.X}");
                //Console.WriteLine($"T Entity left:{transformComponent.Left} Other right:{otherTransformComponent.Right}, Entity right:{transformComponent.Right} Other left:{otherTransformComponent.Left}");
                //Console.WriteLine($"B Entity left:{colliderComponent.Left} Other right:{otherColliderComponent.Right}, Entity right:{colliderComponent.Right} Other left:{otherColliderComponent.Left}");

                // Check for a valid right overlap
                if (direction.X > 0)
                {
                    int overlapX = box.Right - otherBox.Left;
                    int maxOverlapX = (int)(Math.Ceiling(distanceMoved.X) - Math.Floor(otherDistanceMoved.X));// + 1;
                    Console.WriteLine($"Overlap (X-right) {overlapX}, max {maxOverlapX}");

                    if (overlapX > 0 && overlapX <= maxOverlapX)
                        SnapToSideOfOtherEntity(entity, otherEntity, "left");
                }
                // Check for a valid left overlap
                else if (direction.X < 0)
                {
                    int overlapX = box.Left - otherBox.Right;
                    int maxOverlapX = (int)(Math.Floor(distanceMoved.X) - Math.Ceiling(otherDistanceMoved.X));// - 1;
                    Console.WriteLine($"Overlap (X-left) {overlapX}, max {maxOverlapX}");

                    if (overlapX < 0 && overlapX >= maxOverlapX)
                        SnapToSideOfOtherEntity(entity, otherEntity, "right");
                }
                //Console.WriteLine($"T Entity left:{transformComponent.Left} Other right:{otherTransformComponent.Right}, Entity right:{transformComponent.Right} Other left:{otherTransformComponent.Left}");
                //Console.WriteLine($"B Entity left:{colliderComponent.Left} Other right:{otherColliderComponent.Right}, Entity right:{colliderComponent.Right} Other left:{otherColliderComponent.Left}");
            }

            // Check for an in-range overlap in the Y-axis
            else if (axis == 'Y' && absDistanceY > absOtherDistanceY) // needed?
            {
                // Check for a valid top overlap
                if (direction.Y < 0)
                {
                    int overlapY = box.Top - otherBox.Bottom;
                    int maxOverlapY = (int)(Math.Floor(distanceMoved.Y) - Math.Ceiling(otherDistanceMoved.Y));
                    Console.WriteLine($"Overlap (Y-top) {overlapY}, max {maxOverlapY}");

                    if (overlapY < 0 && overlapY >= maxOverlapY)
                        SnapToSideOfOtherEntity(entity, otherEntity, "bottom");
                }
                // Check for a valid bottom overlap
                else if (direction.Y > 0)
                {
                    int overlapY = box.Bottom - otherBox.Top;
                    int maxOverlapY = (int)(Math.Ceiling(distanceMoved.Y) - Math.Floor(otherDistanceMoved.Y));
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
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();
            TransformComponent otherTransformComponent = otherEntity.GetComponent<TransformComponent>();

            if (otherPhysicsComponent == null)
                return;

            Console.WriteLine($"\nResolve opposite directions {entity.Id} and {otherEntity.Id}");

            // Get the bounding boxes
            Rectangle box = colliderComponent.Box;
            Rectangle otherBox = otherColliderComponent.Box;

            // Get the entity's direction and distance moved
            Vector2 direction = physicsComponent.Direction;
            Vector2 distanceMoved = transformComponent.DistanceMoved();
            float absDistanceX = Math.Abs(distanceMoved.X);
            float absDistanceY = Math.Abs(distanceMoved.Y);

            // Get the other entity's direction and distance moved
            Vector2 otherDistanceMoved = otherTransformComponent.DistanceMoved();
            float absOtherDistanceX = Math.Abs(otherDistanceMoved.X);
            float absOtherDistanceY = Math.Abs(otherDistanceMoved.Y);

            // Calculate the total absolute X and Y velocities
            float totalAbsDistanceX = (float)(Math.Ceiling(absDistanceX) + Math.Ceiling(absOtherDistanceX));
            float totalAbsDistanceY = (float)(Math.Ceiling(absDistanceY) + Math.Ceiling(absOtherDistanceY));

            //Vector2 totalDistanceMoved = distanceMoved - otherDistanceMoved;
            //Console.WriteLine($"Total distanceMoved X:{totalDistanceMoved.X}, {totalAbsDistanceX} Y:{totalDistanceMoved.Y}, {totalAbsDistanceY}");

            // Check for an in-range overlap in the X-axis
            if (axis == 'X') // && absDistanceX >= absOtherDistanceX) ??
            {
                // Check for a valid right overlap
                if (direction.X > 0)
                {
                    int overlapX = box.Right - otherBox.Left;
                    int maxOverlapUpper = (int)(Math.Ceiling(distanceMoved.X) + Math.Ceiling(absOtherDistanceX));
                    int maxOverlapLower = (int)(Math.Floor(distanceMoved.X) + Math.Floor(absOtherDistanceX));
                    Console.WriteLine($"Overlap (X-right) {overlapX}, max {maxOverlapLower} to {maxOverlapUpper}");

                    if (overlapX > 0 && overlapX <= maxOverlapUpper)
                    {
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
                            // Calculate the amount of offset based on the total distance moved
                            float offsetRatio = absDistanceX / (float)totalAbsDistanceX * overlapX;
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
                    int maxOverlapUpper = (int)(Math.Floor(distanceMoved.X) - Math.Floor(absOtherDistanceX));
                    int maxOverlapLower = (int)(Math.Ceiling(distanceMoved.X) - Math.Ceiling(absOtherDistanceX));
                    Console.WriteLine($"Overlap (X-right) {overlapX}, max {maxOverlapLower} to {maxOverlapUpper}");

                    if (overlapX < 0 && overlapX >= maxOverlapUpper)
                    {
                        if (overlapX <= maxOverlapLower && overlapX >= maxOverlapUpper)
                        {
                            // Move both entities back to their original X positions
                            MoveToPreviousX(entity);
                            MoveToPreviousX(otherEntity);
                        }
                        else
                        {
                            // CHECK should velocities be rounded / ceiling / floor before totalAbs?
                            // Calculate the amount of offset based on the total distance moved
                            float offsetRatio = absDistanceX / (float)totalAbsDistanceX * overlapX;
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
                    int maxOverlapUpper = (int)(Math.Floor(distanceMoved.Y) - Math.Floor(absOtherDistanceY));
                    int maxOverlapLower = (int)(Math.Ceiling(distanceMoved.Y) - Math.Ceiling(absOtherDistanceY));
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
                            // Calculate the amount of offset based on the total distance moved
                            float offsetRatio = absDistanceY / (float)totalAbsDistanceY * overlapY;
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
                    int maxOverlapUpper = (int)(Math.Ceiling(distanceMoved.Y) + Math.Ceiling(absOtherDistanceY));
                    int maxOverlapLower = (int)(Math.Floor(distanceMoved.Y) + Math.Floor(absOtherDistanceY));
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
                            // Calculate the amount of offset based on the total distance moved
                            float offsetRatio = absDistanceY / (float)totalAbsDistanceY * overlapY;
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
            PhysicsComponent physicsComponent = entity.GetComponent<PhysicsComponent>();
            PhysicsComponent otherPhysicsComponent = otherEntity.GetComponent<PhysicsComponent>();

            if (otherPhysicsComponent == null)
                return;

            Console.WriteLine($"\nResolve perpendicular directions {entity.Id} and {otherEntity.Id}");

            // Check if both entities are moving in multilpe directions
            if (physicsComponent.IsMovingMultipleDirections()
                && otherPhysicsComponent.IsMovingMultipleDirections())
            {
                ResolveOppositeDirections(entity, otherEntity, axis);
            }
            else
            {
                ResolveStationary(entity, otherEntity, axis);
            }

        }

        // Testing - this should never execute
        public void ResolveOtherDirection(Entity entity, Entity otherEntity)
        {
            Console.WriteLine($"\nUnhandled - Resolve other directions {entity.Id} and {otherEntity.Id}");
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

                if (!IsOtherEntityValid(otherEntity))
                    continue;

                // Check if the other entity is moving
                bool isOtherMoving = IsEntityMoving(otherEntity);

                // Re-create the bounding boxes in case collisions have already been resolved
                Rectangle box = colliderComponent.GetBoundingBox(transformComponent.Position);
                Rectangle otherBox;
                if (isOtherMoving)
                    otherBox = otherColliderComponent.GetBoundingBox(otherTransformComponent.Position);
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
                else if (dotProduct == 1 || dotProduct == 2)
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
            // Move entities back to their previous Y-axis position
            foreach (Entity entity in _simpleCollisionsSet)
            {
                MoveToPreviousY(entity);
            }

            // Resolve collisions in the X-axis first
            foreach (Entity entity in _simpleCollisionsSet)
            {
                //Console.WriteLine($"\nX-axis collision resolving for entity {entity.Id}")
                HandleCollisions(entity, 'X');
            }

            // Then move entities in the Y-axis and resolve any remaining collisions
            foreach (Entity entity in _simpleCollisionsSet)
            {
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
            // Move all (?) the entities in the X direction     CHANGE
            // Calculate the distance to move each entity
            // - Resolve all stationary other entities first
            // - Then the collisions with the biggest overlap??
            // - OR Then opposite  , taking into account stationary
            // e.g. entity A has not moved because of stationary,
            // therefore entity B can move all the way to entity A's side
            // - Then resolve same direction
            // - Perpendicular (stationary)?
            // - Multiple directions perpendicular (opposite)??

            // Update the distances of the other entity and any related collisions
            // if an entity has to be moved multiple times
            // Finally apply all the MoveX's for each entity
            // Repeat for Y

            //HashSet<Entity> stationary = new HashSet<Entity>();
            /*
            Dictionary<Entity, HashSet<Entity>> stationary = new Dictionary<Entity, HashSet<Entity>>();
            List<CollisionData> opposite = new List<CollisionData>();
            List<CollisionData> same = new List<CollisionData>();
            List<CollisionData> other = new List<CollisionData>(); // Needed?

            HashSet<string> pairsChecked = new HashSet<string>();
            */

            // CHANGE to MoveToPrevious(entity)
            // since opposite collisions need to check Distance between previous positions
            // Move entities back to their previous Y-axis position
            foreach (Entity entity in _complexCollisionsSet)
            {
                MoveToPreviousY(entity);
            }

            // Resolve collisions in the X-axis first
            foreach (Entity entity in _complexCollisionsSet)
            {
                //Console.WriteLine($"\nX-axis collision resolving for entity {entity.Id}");
                //HandleCollisions(entity, 'X');

                // Add every collided entity to related collisions
                // ?? as entity.Id + ':' + otherEntity.Id ??
                // Check each entity in the collided entities set for any other
                // BUT different collisions
                // Repeat with each other MOVING entity until all pairs have been found

                // if (_allCollisions.Contains(other/entity))
                // then assume (other)entity is moving

                // Questions:
                // Should each type of collision be computed here or later?
                // How to stop an infinite loop of re-checking the same pairs?

                CollisionHandlerComponent handler = entity.GetComponent<CollisionHandlerComponent>();
                //HashSet<Entity> pairsChecked = new HashSet<Entity>();
                //HashSet<string> pairsChecked = new HashSet<string>();
                //HashSet<Entity> relatedCollisions = new HashSet<Entity>();
                // OR use stationary, opposite, same dictionaries?

                foreach (Entity otherEntity in handler.CollidedEntities)
                {
                    // Continue if the pair have already been checked
                    if (pairsChecked.Contains(entity.Id + ":" + otherEntity.Id)
                        || pairsChecked.Contains(otherEntity.Id + ":" + entity.Id))
                    {
                        Console.WriteLine($"Already checked {entity.Id} and {otherEntity.Id}");
                        continue;
                    }

                    // Check if the other entity is stationary
                    if (!IsEntityMoving(otherEntity))
                    //if (!_allCollisionsSet.Contains(otherEntity))
                    {
                        if (!stationary.ContainsKey(entity))
                        {
                            stationary.Add(entity, new HashSet<Entity>());
                        }
                        stationary[entity].Add(otherEntity);
                    }
                    else
                    {
                        // REPEAT for each other entity that has CollidedEntities that
                        // have not been added to related

                        // Should the intersect be checked again first?
                        // If not, should both axis be added to the lists?

                        // Get the necessary components
                        ColliderComponent colliderComponent = entity.GetComponent<ColliderComponent>();
                        ColliderComponent otherColliderComponent = otherEntity.GetComponent<ColliderComponent>();
                        PhysicsComponent physicsComponent = entity.GetComponent<PhysicsComponent>();
                        PhysicsComponent otherPhysicsComponent = otherEntity.GetComponent<PhysicsComponent>();
                        TransformComponent transformComponent = entity.GetComponent<TransformComponent>();
                        TransformComponent otherTransformComponent = otherEntity.GetComponent<TransformComponent>();

                        // Calculate the dot product
                        float dotProduct = Vector2.Dot(physicsComponent.Direction, otherPhysicsComponent.Direction);
                        Console.WriteLine($"Dot product {dotProduct}");

                        // Handle entities moving in opposite directions
                        if (dotProduct == -1 || dotProduct == -2)
                        {
                            //ResolveOppositeDirections(entity, otherEntity, axis);

                            // MOVE to another method, axis is param
                            char axis = 'X';
                            float distance;
                            if (axis == 'X')
                                distance = colliderComponent.Box.X - otherColliderComponent.Box.X;
                            else
                                distance = colliderComponent.Box.Y - otherColliderComponent.Box.Y;

                            //opposite.Add(new CollisionData(distance, entity, otherEntity));
                        }

                        // Handle entities moving in the same direction
                        else if (dotProduct == 1 || dotProduct == 2)
                        {
                            //ResolveSameDirection(entity, otherEntity, axis);

                            // MOVE to another method, axis is param
                            char axis = 'X';
                            //float velocity;
                            //float otherVelocity;
                            float distanceMoved;
                            float otherDistanceMoved;
                            if (axis == 'X')
                            {
                                distanceMoved = transformComponent.DistanceMoved().X;
                                otherDistanceMoved = otherTransformComponent.DistanceMoved().X;
                            }
                            else
                            {
                                distanceMoved = transformComponent.DistanceMoved().Y;
                                otherDistanceMoved = otherTransformComponent.DistanceMoved().Y;
                            }

                            // The resolve method will check both for valid overlaps
                            //same.Add(new CollisionData(distanceMoved, entity, otherEntity));
                            //same.Add(new CollisionData(otherDistanceMoved, otherEntity, entity));
                        }

                        // Hanle entities moving in perpendicular directions
                        else if (dotProduct == 0 || dotProduct == -0)
                        {
                            //ResolvePerpendicularDirection(entity, otherEntity, axis);

                            // Check if both entities are moving in multilpe directions
                            if (physicsComponent.IsMovingMultipleDirections()
                                && otherPhysicsComponent.IsMovingMultipleDirections())
                            {
                                //ResolveOppositeDirections(entity, otherEntity, axis);

                                // Add to opposite list
                            }
                            else
                            {
                                //ResolveStationary(entity, otherEntity, axis);

                                // Add to stationary dictionary
                            }
                        }

                        // Handle all other directions
                        else
                        {
                            ResolveOtherDirection(entity, otherEntity);
                        }
                    }

                    // Add the collision to the pairs checked set
                    pairsChecked.Add(entity.Id + ":" + otherEntity.Id);

                    // DELETE?
                    /*
                    // If opposite or same, check if the collision has already been added 
                    if ((opposite.ContainsKey(entity) && opposite[entity].Contains(otherEntity))
                        || (opposite.ContainsKey(otherEntity) && opposite[otherEntity].Contains(entity)))
                    {
                        continue;
                    }*/
                }
            }


            // For stationary collisions, resolve them in whatever order they are in

            // For opposite direction collisions, resolve the entities with the
            // smallest valid distances between them first
            // (do we need to ensure the entities will actually collide on this axis?)

            // For same direction collisions, resolve the entities with the
            // smallest velocity first
            // (do we need to ensure the entities will actually collide on this axis?)


            // Then move entities in the Y-axis and resolve any remaining collisions
            foreach (Entity entity in _complexCollisionsSet)
            {
                MoveY(entity);
            }

            foreach (Entity entity in _complexCollisionsSet)
            {
                //Console.WriteLine($"\nY-axis collision resolving for entity {entity.Id}");
                HandleCollisions(entity, 'Y');
            }
        }



        public void HandleComplexTest()
        {

            // Find all stationary collisions (in current axis?)
            //   Add to stationary
            //   OR add to moving

            foreach (Entity entity in _complexCollisionsSet)
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

                    if (!IsOtherEntityValid(otherEntity))
                        continue;

                    // CHANGE to IsMovingX ??
                    // Check if the other entity is moving
                    bool isOtherMoving = IsEntityMoving(otherEntity);

                    // Re-create the bounding boxes in case collisions have already been resolved
                    Rectangle box = colliderComponent.GetBoundingBox(transformComponent.Position);
                    Rectangle otherBox;
                    if (isOtherMoving)
                        otherBox = otherColliderComponent.GetBoundingBox(otherTransformComponent.Position);
                    else
                        otherBox = otherColliderComponent.Box;

                    // Check if the collision has already been resolved
                    if (!box.Intersects(otherBox)) // axis == 'X' && 
                    {
                        Console.WriteLine("Collision already resolved");
                        continue;
                    }

                    // SEPARATE METHOD?

                    // Handle a collision between a moving and a stationary entity
                    if (!isOtherMoving)
                    {
                        if (!stationary.ContainsKey(entity))
                        {
                            stationary.Add(entity, new HashSet<Entity>());
                        }
                        stationary[entity].Add(otherEntity);
                    }
                    else
                    {
                        if (!moving.ContainsKey(entity))
                        {
                            moving.Add(entity, new HashSet<Entity>());
                        }
                        moving[entity].Add(otherEntity);
                    }
                }
            }

            // Resolve all stationary collisions
            foreach (KeyValuePair<Entity, HashSet<Entity>> kvp in stationary)
            {
                Entity entity = kvp.Key;
                Console.WriteLine($"Key:{entity} Value count:{kvp.Value.Count}");

                foreach (Entity otherEntity in stationary[entity])
                {
                    ResolveStationary(entity, otherEntity, 'X'); // axis??
                }
            }

            // Check each moving collision (pair?)
            // If not moving in current axis or not perpendicular and both multiple directions
            //    Resolve as stationary
            // Otherwise
            //    Add to opposite or same

            // Resolve all moving collisions
            foreach (KeyValuePair<Entity, HashSet<Entity>> kvp in moving)
            {
                Entity entity = kvp.Key;
                Console.WriteLine($"Key:{entity} Value count:{kvp.Value.Count}");

                foreach (Entity otherEntity in moving[entity])
                {
                    if (!IsEntityMovingX(entity)) // CHANGE to IsEntityMoving(entity, opt axis)
                        //|| !IsEntityMovingX(otherEntity)
                        //|| IsPerpendicularAndStationary(entity))
                    {
                        ResolveStationary(entity, otherEntity, 'X'); // axis??
                    }
                    else
                    {
                        //    Add to opposite or same
                    }
                }
            }

            // Order opposites list and resolve

            // Order same list and resolve
        }



        // Use char axis??
        public void UpdateOppositeCollisions(char axis)
        {
            // CHECK should collisions that are technically stationary be resolved as such
            // i.e. not moving in the current axis (already resolved?)

            for (int i = 0; i < opposite.Count; i++)
            {
                Entity entity = opposite[i].Entity;
                Entity otherEntity = opposite[i].OtherEntity;

                ColliderComponent colliderComponent = entity.GetComponent<ColliderComponent>();
                ColliderComponent otherColliderComponent = otherEntity.GetComponent<ColliderComponent>();

                // Check if the other entity is moving in the X-axis
                if (axis == 'X' && !IsEntityMovingX(otherEntity))
                {
                    // Handle it as a collision between a moving and a stationary entity
                    ResolveStationary(entity, otherEntity, axis);
                }

                float distanceX = Math.Abs(colliderComponent.Box.X - otherColliderComponent.Box.X);
                float distanceY = Math.Abs(colliderComponent.Box.Y - otherColliderComponent.Box.Y);

                opposite[i] = new CollisionData(distanceX, distanceY, entity, otherEntity);
            }
        }

        // Use char axis??
        public void UpdateSameCollisions(char axis)
        {
            // CHECK should collisions that are technically stationary be resolved as such
            // i.e. not moving in the current axis (already resolved?)

            for (int i = 0; i < same.Count; i++)
            {
                Entity entity = same[i].Entity;
                Entity otherEntity = same[i].OtherEntity;

                TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

                float distanceMovedX = Math.Abs(transformComponent.DistanceMoved().X);
                float distanceMovedY = Math.Abs(transformComponent.DistanceMoved().Y);

                same[i] = new CollisionData(distanceMovedX, distanceMovedY, entity, otherEntity);
            }
        }



        public void OrganiseComplexCollisions(Entity entity, char axis)
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

                if (!IsOtherEntityValid(otherEntity))
                    continue;







                // Check if the other entity is moving
                bool isOtherMoving = IsEntityMoving(otherEntity);

                // Re-create the bounding boxes in case collisions have already been resolved
                Rectangle box = colliderComponent.GetBoundingBox(transformComponent.Position);
                Rectangle otherBox;
                if (isOtherMoving)
                    otherBox = otherColliderComponent.GetBoundingBox(otherTransformComponent.Position);
                else
                    otherBox = otherColliderComponent.Box;

                // Check if the collision has already been resolved
                if (!box.Intersects(otherBox)) // axis == 'X' && 
                {
                    Console.WriteLine("Collision already resolved");
                    continue;
                }

                // SEPARATE METHOD?

                // Handle a collision between a moving and a stationary entity
                if (!isOtherMoving)
                {
                    if (!stationary.ContainsKey(entity))
                    {
                        stationary.Add(entity, new HashSet<Entity>());
                    }
                    stationary[entity].Add(otherEntity);
                }
                else
                {
                    // Calculate the dot product
                    float dotProduct = Vector2.Dot(physicsComponent.Direction, otherPhysicsComponent.Direction);
                    Console.WriteLine($"Dot product {dotProduct}");

                    // Handle entities moving in opposite directions
                    if (dotProduct == -1 || dotProduct == -2)
                    {
                        //ResolveOppositeDirections(entity, otherEntity, axis);

                        //float distance;
                        float distanceX = Math.Abs(colliderComponent.Box.X - otherColliderComponent.Box.X);
                        float distanceY = Math.Abs(colliderComponent.Box.Y - otherColliderComponent.Box.Y);

                        opposite.Add(new CollisionData(distanceX, distanceY, entity, otherEntity));
                    }

                    // Handle entities moving in the same direction
                    else if (dotProduct == 1 || dotProduct == 2)
                    {
                        //ResolveSameDirection(entity, otherEntity, axis);

                        //float distanceMoved;
                        //float otherDistanceMoved;
                        float distanceMovedX = Math.Abs(transformComponent.DistanceMoved().X);
                        float distanceMovedY = Math.Abs(transformComponent.DistanceMoved().Y);

                        same.Add(new CollisionData(distanceMovedX, distanceMovedY, entity, otherEntity));
                    }

                    // Hanle entities moving in perpendicular directions
                    else if (dotProduct == 0 || dotProduct == -0)
                    {
                        //ResolvePerpendicularDirection(entity, otherEntity, axis);

                        // Check if both entities are moving in multilpe directions
                        if (physicsComponent.IsMovingMultipleDirections()
                            && otherPhysicsComponent.IsMovingMultipleDirections())
                        {
                            //ResolveOppositeDirections(entity, otherEntity, axis);

                            // Add to opposite list
                            float distanceX = Math.Abs(colliderComponent.Box.X - otherColliderComponent.Box.X);
                            float distanceY = Math.Abs(colliderComponent.Box.Y - otherColliderComponent.Box.Y);

                            opposite.Add(new CollisionData(distanceX, distanceY, entity, otherEntity));
                        }
                        else
                        {
                            //ResolveStationary(entity, otherEntity, axis);

                            // Add to stationary dictionary
                            if (!stationary.ContainsKey(entity))
                            {
                                stationary.Add(entity, new HashSet<Entity>());
                            }
                            stationary[entity].Add(otherEntity);
                        }
                    }

                    // Handle all other directions
                    else
                    {
                        //ResolveOtherDirection(entity, otherEntity);

                        // Testing - should always be empty
                        other.Add(new CollisionData(0, 0, entity, otherEntity));
                    }
                }
            }
        }







        // Resolve X collisions first and then Y collisions for all moveable entities
        public override void Update(GameTime gameTime, Scene scene)
        {
            // Testing
            if (EntityList.Count > 0)
            {
                Console.Write("Collisions to resolve!! ");
                foreach (Entity e in EntityList)
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
            foreach (Entity entity in EntityList)
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

        // Returns true if the entity should be resolving collisions with other entities
        public bool IsEntityValid(Entity entity)
        {
            ColliderComponent colliderComponent = entity.GetComponent<ColliderComponent>();
            PhysicsComponent physicsComponent = entity.GetComponent<PhysicsComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            // Check if the entity is not solid
            if (!colliderComponent.IsSolid)
                return false;

            // TO DO remove physics
            // Check if the entity is not moving
            if (physicsComponent == null)
                return false; // CHANGE either make required or use transform instead

            if (!(physicsComponent.HasVelocity() || transformComponent.HasMoved()))
                return false;

            return true;
        }

        // Returns true if the other entity has a collider and is solid
        public bool IsOtherEntityValid(Entity otherEntity)
        {
            ColliderComponent otherColliderComponent = otherEntity.GetComponent<ColliderComponent>();
            TransformComponent otherTransformComponent = otherEntity.GetComponent<TransformComponent>();

            // Check if the other entity no longer has a collider or transform component
            if (otherColliderComponent == null || otherTransformComponent == null)
                return false;

            // Check if the other entity is not solid
            if (!otherColliderComponent.IsSolid)
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

        // Returns true if the entity is moving in the X-axis or false if it is stationary
        public bool IsEntityMovingX(Entity entity)
        {
            PhysicsComponent physicsComponent = entity.GetComponent<PhysicsComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            if (physicsComponent == null)
                return false;
            else if (!physicsComponent.HasVelocityX() && !transformComponent.HasMovedX())
                return false;
            else
                return true;
        }

        // Returns true if the entity is moving in the Y-axis or false if it is stationary
        public bool IsEntityMovingY(Entity entity)
        {
            PhysicsComponent physicsComponent = entity.GetComponent<PhysicsComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            if (physicsComponent == null)
                return false;
            else if (!physicsComponent.HasVelocityY() && !transformComponent.HasMovedY())
                return false;
            else
                return true;
        }

        // Move the entity back to it's previous X position and redraw the bounding box
        public void MoveToPreviousX(Entity e)
        {
            ColliderComponent colliderComponent = e.GetComponent<ColliderComponent>();
            TransformComponent transformComponent = e.GetComponent<TransformComponent>();
            
            transformComponent.ToPreviousX();
            colliderComponent.GetBoundingBox(transformComponent.Position);
        }

        // Move the entity back to it's previous Y position and redraw the bounding box
        public void MoveToPreviousY(Entity e)
        {
            ColliderComponent colliderComponent = e.GetComponent<ColliderComponent>();
            TransformComponent transformComponent = e.GetComponent<TransformComponent>();

            transformComponent.ToPreviousY();
            colliderComponent.GetBoundingBox(transformComponent.Position);
        }

        // Move the X position based on the calculated velocity
        public void MoveX(Entity e)
        {
            ColliderComponent colliderComponent = e.GetComponent<ColliderComponent>();
            PhysicsComponent physicsComponent = e.GetComponent<PhysicsComponent>();
            TransformComponent transformComponent = e.GetComponent<TransformComponent>();

            if (physicsComponent == null) // Testing
                return;

            transformComponent.Position.X += physicsComponent.Velocity.X;
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

            transformComponent.Position.Y += physicsComponent.Velocity.Y;
            colliderComponent.Box.Y += (int)physicsComponent.Velocity.Y;
        }

        // Move the transform component and bounding bounding X position by a given amount
        public void MoveX(Entity e, int amount)
        {
            ColliderComponent colliderComponent = e.GetComponent<ColliderComponent>();
            TransformComponent transformComponent = e.GetComponent<TransformComponent>();

            transformComponent.Position.X += amount;
            colliderComponent.Box.X += amount;
        }

        // Move the transform component and bounding bounding Y position by a given amount
        public void MoveY(Entity e, int amount)
        {
            ColliderComponent colliderComponent = e.GetComponent<ColliderComponent>();
            TransformComponent transformComponent = e.GetComponent<TransformComponent>();

            transformComponent.Position.Y += amount;
            colliderComponent.Box.Y += amount;
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
            colliderComponent.GetBoundingBox(transformComponent.Position);
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
