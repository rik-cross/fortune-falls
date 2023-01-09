using Microsoft.Xna.Framework;
using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    // Should this be CollisionMovementResponseSystem?
    // or a separate system(s) for resolving damage, sounds etc on collide?
    class CollisionResponseSystem2 : System
    {
        // Testing
        List<Entity> SingleCollisionsList;
        List<Entity> MultipleCollisionsList;

        // Testing
        HashSet<Entity> AllCollisions;

        HashSet<Entity> OneMovingCollisions;

        HashSet<Entity> SingleCollisions;
        HashSet<Entity> MultipleCollisions;
        HashSet<Entity> MultipleMovingCollisions;

        HashSet<string> ResolvedCollisons;

        public CollisionResponseSystem2()
        {
            RequiredComponent<ColliderComponent>();
            RequiredComponent<CollisionHandlerComponent>();
            //RequiredComponent<PhysicsComponent>(); // Teleport / AddItem?
            RequiredComponent<TransformComponent>();
        }


        //    // Check if there are two valid overlaps
        //    // Try the higher one first and see if the collision is resolved
        //    // If not, try the lower one and see if the collision is resolved
        //    // Otherwise resolve both


        public void ResolveStationary(Entity entity, Entity otherEntity, char axis)
        {
            ColliderComponent colliderComponent = entity.GetComponent<ColliderComponent>();
            ColliderComponent otherColliderComponent = otherEntity.GetComponent<ColliderComponent>();
            PhysicsComponent physicsComponent = entity.GetComponent<PhysicsComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            Console.WriteLine($"\nResolve stationary {axis}-axis: {entity.Id} and {otherEntity.Id}");

            // Get the bounding boxes
            Rectangle box = colliderComponent.Box;
            Rectangle otherBox = otherColliderComponent.Box;

            // Get the entity's distance moved and direction vectors
            Vector2 distanceMoved = transformComponent.DistanceMoved();
            Vector2 direction = physicsComponent.Direction;

            // Resolve X-axis collisions
            if (axis == 'X')
            {
                int overlapX = 0;

                // Check for an in-range overlap in the X-axis
                if (direction.X > 0)
                {
                    // Check for a valid right overlap
                    overlapX = otherBox.Left - box.Right;
                    //Console.WriteLine($"Overlap (X-right) {overlapX}  {Math.Ceiling(distanceMoved.X)}");
                    if (overlapX > 0 || Math.Abs(overlapX) > Math.Ceiling(distanceMoved.X))
                        overlapX = 0;
                }
                else if (direction.X < 0)
                {
                    // Check for a valid left overlap
                    overlapX = otherBox.Right - box.Left;
                    //Console.WriteLine($"Overlap (X-left) {overlapX}  {Math.Floor(distanceMoved.X)}");
                    if (overlapX < 0 || overlapX > Math.Ceiling(Math.Abs(distanceMoved.X)))
                        overlapX = 0;
                }

                Console.WriteLine($"Valid overlap X:{overlapX}");

                // Return if there is no valid overlap to resolve
                if (overlapX == 0)
                    return;
                else
                    MoveX(entity, overlapX);
            }

            // Resolve Y-axis collisions
            else if (axis == 'Y')
            {
                int overlapY = 0;

                // Check for an in-range overlap in the Y-axis
                if (direction.Y < 0)
                {
                    // Check for a valid top overlap
                    overlapY = otherBox.Bottom - box.Top;
                    //Console.WriteLine($"Overlap (Y-top) {overlapY}  {Math.Floor(distanceMoved.Y)}");
                    if (overlapY < 0 || overlapY > Math.Ceiling(Math.Abs(distanceMoved.Y)))
                        overlapY = 0;
                }
                else if (direction.Y > 0)
                {
                    // Check for a valid bottom overlap
                    overlapY = otherBox.Top - box.Bottom;
                    //Console.WriteLine($"Overlap (Y-bottom) {overlapY}  {Math.Ceiling(distanceMoved.Y)}");
                    if (overlapY > 0 || Math.Abs(overlapY) > Math.Ceiling(distanceMoved.Y))  // -2 < overlap < 0
                        overlapY = 0;
                }

                Console.WriteLine($"Valid overlap Y:{overlapY}");

                // Return if there is no valid overlap to resolve
                if (overlapY == 0)
                    return;
                else
                    MoveY(entity, overlapY);
            }

        }


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

            // Get the entity's distance moved, direction and velocity
            Vector2 distanceMoved = transformComponent.DistanceMoved();
            Vector2 direction = physicsComponent.Direction;
            Vector2 velocity = physicsComponent.Velocity;
            //float velocityX = physicsComponent.Velocity.X;
            //float velocityY = physicsComponent.Velocity.Y;
            float absVelocityX = Math.Abs(velocity.X); // maxOverlapX  absVelocityX
            float absVelocityY = Math.Abs(velocity.Y); // maxOverlapY  absVelocityY

            // Get the other entity's distance moved, direction and velocity
            Vector2 otherDistanceMoved = otherTransformComponent.DistanceMoved();
            Vector2 otherDirection = otherPhysicsComponent.Direction;
            Vector2 otherVelocity = otherPhysicsComponent.Velocity;
            //int otherVelocityX = otherPhysicsComponent.Velocity.X;
            //int otherVelocityY = otherPhysicsComponent.Velocity.Y;
            float absOtherVelocityX = Math.Abs(otherVelocity.X);
            float absOtherVelocityY = Math.Abs(otherVelocity.Y);

            // Calculate the total absolute X and Y velocities
            Vector2 totalVelocity = velocity - otherVelocity;
            float totalAbsVelocityX = absVelocityX + absOtherVelocityX;
            float totalAbsVelocityY = absVelocityY + absOtherVelocityY;

            // Get the entity's direction
            string directionString = physicsComponent.DirectionString;
            bool oneDirection = physicsComponent.IsMovingOneDirection();
            bool multipleDirections = physicsComponent.IsMovingMultipleDirections();

            //Console.WriteLine($"One direction {oneDirection}, multiple directions {multipleDirections}");
            Console.WriteLine($"Total velocity X:{totalVelocity.X} Y:{totalVelocity.Y}");

            // Resolve X-axis collisions
            if (axis == 'X')
            {
                int overlapX = 0;

                // Check for an in-range overlap in the X-axis
                if (direction.X > 0)
                {
                    // Check for a valid right overlap
                    overlapX = otherBox.Left - box.Right;
                    Console.WriteLine($"Overlap (X-right) {overlapX}  {Math.Ceiling(totalVelocity.X)}");
                    if (overlapX > 0 || Math.Abs(overlapX) > Math.Ceiling(totalVelocity.X))
                        overlapX = 0;
                }
                else if (direction.X < 0)
                {
                    // Check for a valid left overlap
                    overlapX = otherBox.Right - box.Left;
                    Console.WriteLine($"Overlap (X-left) {overlapX}  {Math.Floor(totalVelocity.X)}");
                    if (overlapX < 0 || overlapX > Math.Ceiling(Math.Abs(totalVelocity.X)))
                        overlapX = 0;
                }

                Console.WriteLine($"Valid overlap X:{overlapX}");

                // Check if the X overlap is the largest it can be
                bool isMaxOverlapX = Math.Abs(overlapX) == Math.Ceiling(Math.Abs(totalVelocity.X));
                // bool isMaxOverlapX = Math.Abs(overlapX) == totalAbsVelocityX;
                Console.WriteLine($"Is Max Overlap X {isMaxOverlapX}");


                //if (isMaxOverlapX && isMaxOverlapY)
                //{
                //    Console.WriteLine("Both max X and Y overlaps");
                //    Console.WriteLine("Resolve on the X axis only");

                //    if (directionString.Contains('E'))
                //    {
                //        double offsetRatio = absVelocityX / (double)totalAbsVelocityX * overlapRight;
                //        int offsetX = (int)Math.Round(offsetRatio);
                //        int otherOffsetX = overlapRight - offsetX;

                //        // Move both entities based on their offset values
                //        MoveX(entity, -offsetX);
                //        MoveX(otherEntity, otherOffsetX);

                //        Console.WriteLine("Max East overlap");
                //    }
                //    else if (directionString.Contains('W'))
                //    {
                //        double offsetRatio = absVelocityX / (double)totalAbsVelocityX * overlapLeft;
                //        int offsetX = (int)Math.Round(offsetRatio);
                //        int otherOffsetX = overlapLeft - offsetX;

                //        // Move both entities based on their offset values
                //        MoveX(entity, offsetX);
                //        MoveX(otherEntity, -otherOffsetX);

                //        Console.WriteLine("Max West overlap");
                //    }
                //}



                // Return if there is no valid overlap to resolve
                if (overlapX == 0)
                    return;
                else
                    MoveX(entity, overlapX);
            }



            // Used for checking if the X or Y overlaps are the absolute max
            // for any direction that is not the same as the other direction
            //bool isMaxOverlapX = false;
            bool isMaxOverlapY = false;

            //    // Check if the top or bottom overlap is the largest it can be
            //if (directionString.Contains('N') && overlapTop == totalAbsVelocityY
            //    || directionString.Contains('S') && overlapBottom == totalAbsVelocityY)
            //    isMaxOverlapY = true;

            //// Check if the right or left overlap is the largest it can be
            //if (directionString.Contains('E') && overlapRight == totalAbsVelocityX
            //    || directionString.Contains('W') && overlapLeft == totalAbsVelocityX)
            //    isMaxOverlapX = true;


            //// distanceMoved.Normalize();

            ///*
            //// Calculate the amount of overlap in each direction
            //int overlapTop = otherBox.Bottom - box.Top;
            //int overlapBottom = box.Bottom - otherBox.Top;
            //int overlapRight = box.Right - otherBox.Left;
            //int overlapLeft = otherBox.Right - box.Left;
            //*/


            //// Resolve both entities that are moving in a single opposite direction
            //// or are both moving in multiple opposing directions.

            //// Store valid X and Y overlaps for resolving multiple directions
            ////int overlapX = 0;
            ////int overlapY = 0;

            //// Check if the entity is moving in multiple directions
            //if (multipleDirections)
            //{
            //    // Get the largest valid Y overlap
            //    if (directionString.Contains('N') && overlapTop > 0
            //        && overlapTop <= totalAbsVelocityY)
            //        overlapY = overlapTop;
            //    else if (directionString.Contains('S') && overlapBottom > 0
            //        && overlapBottom <= totalAbsVelocityY)
            //        //&& overlapBottom > overlapY)
            //        overlapY = overlapBottom;

            //    // Get the largest valid X overlap
            //    if (directionString.Contains('E') && overlapRight > 0
            //        && overlapRight <= totalAbsVelocityX)
            //        overlapX = overlapRight;
            //    else if (directionString.Contains('W') && overlapLeft > 0
            //        && overlapLeft <= totalAbsVelocityX)
            //        //&& overlapLeft > overlapX)
            //        overlapX = overlapLeft;
            //}

            //Console.WriteLine($"Overlap X {overlapX}");
            //Console.WriteLine($"Overlap Y {overlapY}");

            //if (isMaxOverlapX && isMaxOverlapY)
            //{
            //    Console.WriteLine("Both max X and Y overlaps");
            //    Console.WriteLine("Resolve on the X axis only");

            //    if (directionString.Contains('E'))
            //    {
            //        double offsetRatio = absVelocityX / (double)totalAbsVelocityX * overlapRight;
            //        int offsetX = (int)Math.Round(offsetRatio);
            //        int otherOffsetX = overlapRight - offsetX;

            //        // Move both entities based on their offset values
            //        MoveX(entity, -offsetX);
            //        MoveX(otherEntity, otherOffsetX);

            //        Console.WriteLine("Max East overlap");
            //    }
            //    else if (directionString.Contains('W'))
            //    {
            //        double offsetRatio = absVelocityX / (double)totalAbsVelocityX * overlapLeft;
            //        int offsetX = (int)Math.Round(offsetRatio);
            //        int otherOffsetX = overlapLeft - offsetX;

            //        // Move both entities based on their offset values
            //        MoveX(entity, offsetX);
            //        MoveX(otherEntity, -otherOffsetX);

            //        Console.WriteLine("Max West overlap");
            //    }
            //}

            /*
            // Resolve Y-axis collisions
            else if (axis == 'Y')
            {
                int overlapY = 0;

                // Check for an in-range overlap in the Y-axis
                if (direction.Y < 0)
                {
                    // Check for a valid top overlap
                    overlapY = otherBox.Bottom - box.Top;
                    Console.WriteLine($"Overlap (Y-top) {overlapY}  {Math.Floor(distanceMoved.Y)}");
                    if (overlapY < 0 || overlapY > Math.Ceiling(Math.Abs(distanceMoved.Y)))
                        overlapY = 0;
                }
                else if (direction.Y > 0)
                {
                    // Check for a valid bottom overlap
                    overlapY = otherBox.Top - box.Bottom;
                    Console.WriteLine($"Overlap (Y-bottom) {overlapY}  {Math.Ceiling(distanceMoved.Y)}");
                    if (overlapY > 0 || Math.Abs(overlapY) > Math.Ceiling(distanceMoved.Y))  // -2 < overlap < 0
                        overlapY = 0;
                }

                Console.WriteLine($"Valid overlap Y:{overlapY}");

                // Return if there is no valid overlap to resolve
                if (overlapY == 0)
                    return;
                else
                    MoveY(entity, overlapY);
            }
            */

        }

        public void ResolveSameDirection(Entity entity, Entity otherEntity)
        {
            Console.WriteLine($"\nResolve same direction {entity.Id} and {otherEntity.Id}");
        }

        public void ResolvePerpendicularDirection(Entity entity, Entity otherEntity)
        {
            Console.WriteLine($"\nResolve perpendicular directions {entity.Id} and {otherEntity.Id}");
        }

        public void ResolveOtherDirection(Entity entity, Entity otherEntity)
        {
            Console.WriteLine($"\nResolve other directions {entity.Id} and {otherEntity.Id}");
        }

        // Returns if the entities should be allowed to collide or not
        public bool IsCollisionValid(Entity entity, Entity otherEntity)
        {
            ColliderComponent colliderComponent = entity.GetComponent<ColliderComponent>();
            PhysicsComponent physicsComponent = entity.GetComponent<PhysicsComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            ColliderComponent otherColliderComponent = otherEntity.GetComponent<ColliderComponent>();
            TransformComponent otherTransformComponent = otherEntity.GetComponent<TransformComponent>();
            /*
            // MOVE to after the X axis check or during multiple moving collisions
            // Check if the collision has already been resolved
            if (!colliderComponent.Box.Intersects(otherColliderComponent.Box)) // axis == 'X' && 
            {
                Console.WriteLine("Collision already resolved");
                return false;
            }
            */

            // Check if the other entity no longer has a collider or transform component
            if (otherColliderComponent == null || otherTransformComponent == null)
                return false;

            // Check if either entity is not solid
            if (!colliderComponent.IsSolid || !otherColliderComponent.IsSolid)
                return false;

            // CHANGE either make required or use transform instead
            // Check if the entity is not moving
            if (physicsComponent == null)
                return false;

            if (!(physicsComponent.HasVelocity() || transformComponent.HasMoved()))
                return false;

            return true;
        }

        // Returns if the entity is moving or not
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



        public void HandleCollisions(Entity entity, char axis)
        {
            ColliderComponent colliderComponent = entity.GetComponent<ColliderComponent>();
            CollisionHandlerComponent handlerComponent = entity.GetComponent<CollisionHandlerComponent>();
            PhysicsComponent physicsComponent = entity.GetComponent<PhysicsComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();
            
            //// CHANGE either make required or use transform instead
            //// Check if the entity is not moving
            //if (physicsComponent == null)
            //    return;

            //if (!(physicsComponent.HasVelocity() || transformComponent.HasMoved()))
            //    return;

            //// Check if the entity is not solid
            //if (!colliderComponent.IsSolid)
            //    return;

            // Respond to entities that have started colliding
            foreach (Entity otherEntity in handlerComponent.CollidedEntities)
            {
                ColliderComponent otherColliderComponent = otherEntity.GetComponent<ColliderComponent>();
                PhysicsComponent otherPhysicsComponent = otherEntity.GetComponent<PhysicsComponent>();
                TransformComponent otherTransformComponent = otherEntity.GetComponent<TransformComponent>();

                //// Check if the other entity no longer has a collider or transform component
                //if (otherColliderComponent == null || otherTransformComponent == null)
                //    continue;

                //// Check if the other entity is not solid
                //if (!otherColliderComponent.IsSolid)
                //    continue;

                //// Testing: here or below??

                //// Add the entity to the single or multiple hashsets

                //// Add the other entity to the moving hashset

                //// After the loop or method, combine the multiple and moving hashets



                // MOVE to after the X axis check or during multiple moving collisions
                // Check if the collision has already been resolved
                if (!colliderComponent.Box.Intersects(otherColliderComponent.Box)) // axis == 'X' && 
                {
                    Console.WriteLine("Collision already resolved");
                    continue;
                }

                // Check if the other entity is moving
                bool hasOtherMoved = true;
                if (otherPhysicsComponent == null)
                    hasOtherMoved = false;
                else if (!otherPhysicsComponent.HasVelocity() && !otherTransformComponent.HasMoved())
                    hasOtherMoved = false;

                // Handle a collision between a moving and a stationary entity
                if (!hasOtherMoved)
                {
                    ResolveStationary(entity, otherEntity, axis);
                }

                // Otherwise handle a collision between two moving entities
                else
                {
                    // Calculate the dot product
                    float dotProduct = Vector2.Dot(physicsComponent.Direction, otherPhysicsComponent.Direction);
                    Console.WriteLine($"Dot product {dotProduct}");

                    // Handle entities moving in opposite directions
                    if (dotProduct == -1)
                        ResolveOppositeDirections(entity, otherEntity, axis);

                    // Handle entities moving in the same direction
                    else if (dotProduct == 1)
                        ResolveSameDirection(entity, otherEntity);

                    // Handle entities moving in perpendicular directions
                    else if (dotProduct == 0 || dotProduct == -0)
                        ResolvePerpendicularDirection(entity, otherEntity);

                    // Handle all other directions
                    else
                        ResolveOtherDirection(entity, otherEntity);
                }
            }
        }


        public void HandleSingleCollisions()
        {
            // Move entities in the X-axis and resolve collisions first
            foreach (Entity entity in SingleCollisionsList)
            {
                Console.WriteLine($"\nX-axis collision resolving for entity {entity.Id}");
                MoveX(entity);
                HandleCollisions(entity, 'X');
            }

            // Then move entities in the Y-axis and resolve collisions
            foreach (Entity entity in SingleCollisionsList)
            {
                Console.WriteLine($"\nY-axis collision resolving for entity {entity.Id}");
                MoveY(entity);
                HandleCollisions(entity, 'Y');
            }
        }

        // Handle multiple collisions with 0 or 1 moving entities
        public void HandleMultipleCollisions()
        {
            // REPEAT
            // Validate the other entity

            // Check if the other entity is moving
            // If so, add to the moving set
            // END

            // Check if more than one other entity is moving for a single collision
            // If so, add to the multiple moving set
            // Remove from the moving set??

            // Process multiple collisions with <= 1 moving other entities

            // Move entities in the X-axis and resolve collisions first
            foreach (Entity entity in MultipleCollisionsList)
            {
                Console.WriteLine($"\nX-axis collision resolving for entity {entity.Id}");
                MoveX(entity);
                HandleCollisions(entity, 'X');
            }

            // Then move entities in the Y-axis and resolve collisions
            foreach (Entity entity in MultipleCollisionsList)
            {
                Console.WriteLine($"\nY-axis collision resolving for entity {entity.Id}");
                MoveY(entity);
                HandleCollisions(entity, 'Y');
            }

        }

        // More complex collision handling for multiple moving entities
        public void HandleMultipleMovingCollisions()
        {

        }


        // Resolve X collisions first and then Y collisions for all moveable entities
        public override void Update(GameTime gameTime, Scene scene)
        {
            // Get all collisions from the CollisionSystem.CollisionsStarted set?

            // OR
            //HashSet<Entity> ResolvedCollisons = new HashSet<Entity>();
            //AllCollisions = new HashSet<string>();
            //AllCollisions = new HashSet<Entity>(entityList);
            ResolvedCollisons = new HashSet<string>();
            SingleCollisions = new HashSet<Entity>();
            MultipleCollisions = new HashSet<Entity>();

            SingleCollisionsList = new List<Entity>();
            MultipleCollisionsList = new List<Entity>();

            // Get only active collisions from the CollidedEntities set. For active entities,
            // check if the entity is colliding with one or multiple entities and then
            // move the entity back to it's previous position before the collision started.
            foreach (Entity entity in entityList)
            {
                CollisionHandlerComponent handler = entity.GetComponent<CollisionHandlerComponent>();

                int count = handler.CollidedEntities.Count;
                if (count == 1)
                {
                    if (IsCollisionValid(entity, handler.CollidedEntities.Single()))
                    {
                        SingleCollisionsList.Add(entity);
                        MoveToPreviousPosition(entity);
                    }

                    //SingleCollisions.Add(entity);
                    //AllCollisions.Add(entity);
                }
                else if (count > 1)
                {
                    MultipleCollisionsList.Add(entity);
                    //MultipleCollisions.Add(entity);
                    //AllCollisions.Add(entity);

                    MoveToPreviousPosition(entity);

                    //foreach (Entity otherEntity in handler.CollidedEntities)
                    //{
                    //    // Check if the entity is moving i.e. it has a collider component
                    //}
                }
            }

            MultipleMovingCollisions = new HashSet<Entity>(MultipleCollisions);
            //MultipleMovingCollisions.IntersectWith(AllCollisions); // CHANGE to string

            Console.Write("\nSingle collisions: ");
            Console.WriteLine(string.Join(", ", SingleCollisionsList));
            Console.Write("Multiple collisions: ");
            Console.WriteLine(string.Join(", ", MultipleCollisionsList));
            //Console.Write("Multiple moving collisions set: ");
            //Console.WriteLine(string.Join(", ", MultipleMovingCollisions));

            // Handle entities with single collisions first and then multiple collisions.
            // Entities with multiple moving collisions need more advanced collision handling.
            HandleSingleCollisions();
            HandleMultipleCollisions();
            HandleMultipleMovingCollisions();
        }


        public void AddSinglePairToResolved(Entity entity, Entity otherEntity)
        {
            string hash = entity.Id.ToString() + ':' + otherEntity.Id;
            ResolvedCollisons.Add(hash);
        }

        public void AddBothPairsToResolved(Entity entity, Entity otherEntity)
        {
            string hashA = entity.Id.ToString() + ':' + otherEntity.Id;
            string hashB = otherEntity.Id.ToString() + ':' + entity.Id;

            ResolvedCollisons.Add(hashA);
            ResolvedCollisons.Add(hashB);
        }

        public bool IsPairResolved(Entity entity, Entity otherEntity)
        {
            string hashA = entity.Id.ToString() + ':' + otherEntity.Id;
            string hashB = otherEntity.Id.ToString() + ':' + entity.Id;

            if (ResolvedCollisons.Contains(hashA) || ResolvedCollisons.Contains(hashB))
                return true;
            else
                return false;
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

                // Get the direction vectors and normalise them
                // CHANGE to direction and direction to directionString
                //Vector2 directionVector = Vector2.Normalize(physicsComponent.DirectionVector);
                //Vector2 otherDirectionVector = Vector2.Normalize(otherPhysicsComponent.DirectionVector);
                //Vector2 directionVector = physicsComponent.Direction;
                //Vector2 otherDirectionVector = otherPhysicsComponent.Direction;
                float dotProduct = Vector2.Dot(physicsComponent.Direction, otherPhysicsComponent.Direction);

                Console.WriteLine($"Dot product {dotProduct}");

                // Check if the entities are moving in opposite directions
                if (dotProduct == -1)
                {
                    //ResolveOppositeDirections(entity, otherEntity);
                    //continue;
                }
                // Check if the entities are moving in the same direction
                else if (dotProduct == 1)
                {
                    ResolveSameDirection(entity, otherEntity);
                    continue;
                }
                // Check if the entities are moving in perpendicular directions
                else if (dotProduct == 0 || dotProduct == -0)
                {
                    ResolvePerpendicularDirection(entity, otherEntity);
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
                int velocityX = (int)physicsComponent.Velocity.X; // Change to float?
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
                    otherVelocityX = (int)otherPhysicsComponent.Velocity.X; // Change to float?
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
                // 

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


        // Move the X position of an entity's transform component and bounding bounding
        public void MoveToPreviousPosition(Entity e)
        {
            ColliderComponent colliderComponent = e.GetComponent<ColliderComponent>();
            TransformComponent transformComponent = e.GetComponent<TransformComponent>();

            Vector2 distance = transformComponent.position - transformComponent.previousPosition;

            transformComponent.position -= distance;
            colliderComponent.Box.X -= (int)distance.X; //(int)Math.Ceiling(distance.X)
            colliderComponent.Box.Y -= (int)distance.Y; //(int)Math.Ceiling(distance.Y)
        }


        // Move the X position based on the calculated velocity
        public void MoveX(Entity e)
        {
            ColliderComponent colliderComponent = e.GetComponent<ColliderComponent>();
            PhysicsComponent physicsComponent = e.GetComponent<PhysicsComponent>();
            TransformComponent transformComponent = e.GetComponent<TransformComponent>();

            transformComponent.position.X += physicsComponent.Velocity.X;
            colliderComponent.Box.X += (int)physicsComponent.Velocity.X;

            Console.WriteLine($"Moved X of {e.Id} by {physicsComponent.Velocity.X}");
        }

        // Move the Y position based on the calculated velocity
        public void MoveY(Entity e)
        {
            ColliderComponent colliderComponent = e.GetComponent<ColliderComponent>();
            PhysicsComponent physicsComponent = e.GetComponent<PhysicsComponent>();
            TransformComponent transformComponent = e.GetComponent<TransformComponent>();

            transformComponent.position.Y += physicsComponent.Velocity.Y;
            colliderComponent.Box.Y += (int)physicsComponent.Velocity.Y;

            Console.WriteLine($"Moved Y of {e.Id} by {physicsComponent.Velocity.Y}");
        }

        // Move the transform component and bounding bounding X position by a given amount
        public void MoveX(Entity e, int amount)
        {
            ColliderComponent colliderComponent = e.GetComponent<ColliderComponent>();
            TransformComponent transformComponent = e.GetComponent<TransformComponent>();

            transformComponent.position.X += amount;
            colliderComponent.Box.X += amount;

            Console.WriteLine($"Moved X of {e.Id} by {amount}");
        }

        // Move the transform component and bounding bounding Y position by a given amount
        public void MoveY(Entity e, int amount)
        {
            ColliderComponent colliderComponent = e.GetComponent<ColliderComponent>();
            TransformComponent transformComponent = e.GetComponent<TransformComponent>();

            transformComponent.position.Y += amount;
            colliderComponent.Box.Y += amount;

            Console.WriteLine($"Moved Y of {e.Id} by {amount}");
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
