using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    // Should this be CollisionMovementResponseSystem?
    // or a separate system(s) for resolving damage, sounds etc on collide?
    class CollisionResponseSystem3 : System
    {
        public Dictionary<Entity, Dictionary<Entity, int>> _resolvedOverlaps;

        public CollisionResponseSystem3()
        {
            RequiredComponent<ColliderComponent>();
            RequiredComponent<CollisionHandlerComponent>();
            //RequiredComponent<PhysicsComponent>();
            RequiredComponent<TransformComponent>();
            // ColliderResponseComponent? instead of the ColliderComponent?

            _resolvedOverlaps = new Dictionary<Entity, Dictionary<Entity, int>>();
        }


        //    // Check if there are two valid overlaps
        //    // Try the higher one first and see if the collision is resolved
        //    // If not, try the lower one and see if the collision is resolved
        //    // Otherwise resolve both

        //    // Testing
        //    if (overlapTop != 0 && overlapLeft != 0)
        //    {
        //        Rectangle collisionCheck = new Rectangle(box.X, box.Y, box.Width, box.Height);

        //        if (overlapTop > overlapLeft)
        //        {
        //            collisionCheck.Y += overlapTop;

        //            if (collisionCheck.Intersects(otherBox))
        //            {
        //                collisionCheck.Y -= overlapTop;
        //                collisionCheck.X += overlapLeft;
        //            }

        //            if (collisionCheck.Intersects(otherBox))
        //            {
        //                collisionCheck.Y += overlapTop;
        //            }

        //            colliderComponent.Box = collisionCheck;
        //        }
        //        else
        //        {
        //            collisionCheck.X += overlapLeft;

        //            if (collisionCheck.Intersects(otherBox))
        //            {
        //                collisionCheck.X -= overlapLeft;
        //                collisionCheck.Y += overlapTop;
        //            }

        //            if (collisionCheck.Intersects(otherBox))
        //            {
        //                collisionCheck.X += overlapLeft;
        //            }

        //            colliderComponent.Box = collisionCheck;
        //        }
        //        return;
        //    }


        //    // CHECK
        //    // If max overlap, re-calculate distance with 1 / -1 instead?



        //    // Or check equals
        //    if (overlapTop > 0 && overlapTop == overlapLeft)
        //    {
        //        MoveY(entity, overlapTop);
        //        Console.WriteLine($"Overlap (Y-up) {overlapTop}");

        //        MoveX(entity, overlapLeft);
        //        Console.WriteLine($"Overlap (X-left) {overlapLeft}");

        //        return;
        //    }


        //    // Resolve Y if the overlap is higher otherwise resolve X
        //    if (overlapTop > overlapLeft || overlapTop > overlapRight
        //        || overlapBottom > overlapLeft || overlapBottom > overlapRight)
        //    {
        //        if (overlapTop > overlapBottom)
        //        {
        //            MoveY(entity, overlapTop);
        //            Console.WriteLine($"Overlap (Y-up) {overlapTop}");
        //        }
        //        else
        //        {
        //            MoveY(entity, -overlapBottom);
        //            Console.WriteLine($"Overlap (Y-down) {overlapBottom}");
        //        }
        //    }
        //    else
        //    {
        //        if (overlapLeft > overlapRight)
        //        {
        //            MoveX(entity, overlapLeft);
        //            Console.WriteLine($"Overlap (X-left) {overlapLeft}");
        //        }
        //        else
        //        {
        //            MoveX(entity, -overlapRight);
        //            Console.WriteLine($"Overlap (X-right) {overlapRight}");
        //        }
        //    }

        //    // Repeat above for unresolved other axis??


        //    // Check for other collisions? Use previous positions to handle?



        //    // TESTING

        //    // Check for an in-range overlap in the Y-axis
        //    if (direction.Y < 0)
        //    {
        //        // Check for a valid top overlap
        //        overlapY = otherBox.Bottom - box.Top;
        //        Console.WriteLine($"Overlap (Y-top) {overlapY}  {Math.Floor(distanceMoved.Y)}");
        //        if (overlapY < 0 || overlapY > Math.Ceiling(Math.Abs(distanceMoved.Y)))
        //            overlapY = 0;
        //    }
        //    else if (direction.Y > 0)
        //    {
        //        // Check for a valid bottom overlap
        //        overlapY = otherBox.Top - box.Bottom;
        //        Console.WriteLine($"Overlap (Y-bottom) {overlapY}  {Math.Ceiling(distanceMoved.Y)}");
        //        if (overlapY > 0 || Math.Abs(overlapY) > Math.Ceiling(distanceMoved.Y))  // -2 < overlap < 0
        //            overlapY = 0;
        //    }

        //    // Check for an in-range overlap in the X-axis
        //    if (direction.X > 0)
        //    {
        //        // Check for a valid right overlap
        //        overlapX = otherBox.Left - box.Right;
        //        Console.WriteLine($"Overlap (X-right) {overlapX}  {Math.Ceiling(distanceMoved.X)}");
        //        if (overlapX > 0 || Math.Abs(overlapX) > Math.Ceiling(distanceMoved.X))
        //            overlapX = 0;
        //    }
        //    else if (direction.X < 0)
        //    {
        //        // Check for a valid left overlap
        //        overlapX = otherBox.Right - box.Left;
        //        Console.WriteLine($"Overlap (X-left) {overlapX}  {Math.Floor(distanceMoved.X)}");
        //        if (overlapX < 0 || overlapX > Math.Ceiling(Math.Abs(distanceMoved.X)))
        //            overlapX = 0;
        //    }

        //    Console.WriteLine($"Valid overlap X:{overlapX} Y:{overlapY}");

        //    // Return if there is no valid overlap to resolve
        //    if (overlapX == 0 && overlapY == 0)
        //        return;

        //    // Check if there are two valid overlaps
        //    // Try the higher one first and see if the collision is resolved
        //    // If not, try the lower one and see if the collision is resolved
        //    // Otherwise resolve both

        //    // Testing
        //    if (overlapX != 0 && overlapY != 0)
        //    {
        //        Rectangle collisionCheck = new Rectangle(box.X, box.Y, box.Width, box.Height);

        //        if (Math.Abs(overlapY) > Math.Abs(overlapX))
        //        {
        //            Console.WriteLine($"Resolve X and Y - check Y first. X:{colliderComponent.Box.X - collisionCheck.X} Y:{colliderComponent.Box.Y - collisionCheck.Y}");
        //            collisionCheck.Y += overlapY;

        //            if (collisionCheck.Intersects(otherBox))
        //            {
        //                collisionCheck.Y -= overlapY;
        //                collisionCheck.X += overlapX;
        //            }

        //            if (collisionCheck.Intersects(otherBox))
        //            {
        //                collisionCheck.Y += overlapY;
        //            }

        //            // CHANGE to MoveY()
        //            Console.WriteLine($"Resolve X and Y - check Y first. X:{colliderComponent.Box.X - collisionCheck.X} Y:{colliderComponent.Box.Y - collisionCheck.Y}");
        //            colliderComponent.Box = collisionCheck;
        //        }
        //        else
        //        {
        //            Console.WriteLine($"Resolve X and Y - check X first");
        //            Console.WriteLine($"Other box Right:{otherBox.Right} Bottom:{otherBox.Bottom}");
        //            Console.WriteLine($"Collision check Left:{collisionCheck.Left} Top:{collisionCheck.Top}");
        //            if (collisionCheck.Intersects(otherBox))
        //            {
        //                Console.WriteLine("Intersects");
        //            }

        //            collisionCheck.X += overlapX;

        //            if (collisionCheck.Intersects(otherBox))
        //            {
        //                Console.WriteLine("Still intersects with X, try Y");
        //                collisionCheck.X -= overlapX;
        //                collisionCheck.Y += overlapY;
        //            }

        //            if (collisionCheck.Intersects(otherBox))
        //            {
        //                Console.WriteLine("Intersects with X");
        //                collisionCheck.X += overlapX;
        //            }

        //            // CHANGE to MoveX()
        //            Console.WriteLine($"Resolve X and Y - check X first. X:{colliderComponent.Box.X - collisionCheck.X} Y:{colliderComponent.Box.Y - collisionCheck.Y}");
        //            colliderComponent.Box = collisionCheck;
        //        }
        //        return;
        //    }

        //    // NOT WORKING above - incorrectly showing no intersect no matter if X or Y is moved first


        //    // CHANGE to resolve if X != 0, if Y != 0







        //public void ResolveStationary(Entity entity, Entity otherEntity, char axis)
        //{
        //    // Get the necessary components
        //    ColliderComponent colliderComponent = entity.GetComponent<ColliderComponent>();
        //    ColliderComponent otherColliderComponent = otherEntity.GetComponent<ColliderComponent>();
        //    PhysicsComponent physicsComponent = entity.GetComponent<PhysicsComponent>();
        //    TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

        //    Console.WriteLine($"\nResolve stationary {axis}-axis: {entity.Id} and {otherEntity.Id}");

        //    // Get the bounding boxes
        //    Rectangle box = colliderComponent.Box;
        //    Rectangle otherBox = otherColliderComponent.Box;

        //    // Get the entity's distance moved and direction
        //    Vector2 distanceMoved = transformComponent.DistanceMoved();
        //    Vector2 direction = physicsComponent.Direction;

        //    // Resolve X-axis collisions
        //    if (axis == 'X')
        //    {
        //        int overlapX = 0;
        //        int absOverlapX = 0;

        //        // Check for an in-range overlap in the X-axis
        //        if (direction.X > 0)
        //        {
        //            // Check for a valid right overlap
        //            overlapX = otherBox.Left - box.Right;
        //            Console.WriteLine($"Overlap (X-right) {overlapX}  {Math.Ceiling(distanceMoved.X)}");
        //            if (overlapX > 0 || Math.Abs(overlapX) > Math.Ceiling(distanceMoved.X))
        //                overlapX = 0;
        //        }
        //        else if (direction.X < 0)
        //        {
        //            // Check for a valid left overlap
        //            overlapX = otherBox.Right - box.Left;
        //            Console.WriteLine($"Overlap (X-left) {overlapX}  {Math.Floor(distanceMoved.X)}");
        //            if (overlapX < 0 || overlapX > Math.Ceiling(Math.Abs(distanceMoved.X)))
        //                overlapX = 0;
        //        }

        //        Console.WriteLine($"Valid overlap X:{overlapX}");

        //        // Return if there is no valid overlap to resolve
        //        if (overlapX == 0)
        //            return;
        //        //else
        //        //    MoveX(entity, overlapX);
        //        else
        //        {
        //            //_resolvedOverlaps.Add(entity, overlapX);

        //            if (!_resolvedOverlaps.ContainsKey(entity))
        //                _resolvedOverlaps.Add(entity, new Dictionary<Entity, int>());
        //            _resolvedOverlaps[entity][otherEntity] = overlapX;

        //            MoveX(entity, overlapX);
        //        }

        //        /*
        //        // Testing
        //        Rectangle collisionCheck = new Rectangle(box.X, box.Y, box.Width, box.Height);

        //        Console.WriteLine($"Resolve X and Y - check X first");
        //        Console.WriteLine($"Other box Right:{otherBox.Right} Bottom:{otherBox.Bottom}");
        //        Console.WriteLine($"Collision check Left:{collisionCheck.Left} Top:{collisionCheck.Top}");
        //        if (collisionCheck.Intersects(otherBox))
        //        {
        //            Console.WriteLine("Intersects");
        //        }

        //        collisionCheck.X += overlapX;

        //        if (!collisionCheck.Intersects(otherBox))
        //        {
        //            Console.WriteLine("Doesn't intersects on X resolve");
        //            MoveX(entity, overlapX);
        //        }
        //        */

        //        // CHANGE above so that it also checks other colliding entities
        //        // and reverts or moves to closest edge when an X collision is encountered

        //        // OR
        //        // store any resolved collisions on the X-axis
        //        // when checking Y, see if a collision for that entity has been resolved
        //        // if so, un-do the overlapX move and check Y-axis first
        //        // if it still intersects, re-do overlapX
        //        // final check to see if collision has been resolved?
        //    }


        //    // Resolve Y-axis collisions
        //    else if (axis == 'Y')
        //    {
        //        int overlapY = 0;
        //        int absOverlapY = 0;
        //        int movedX = 0;


        //        if (_resolvedOverlaps.ContainsKey(entity)
        //            && _resolvedOverlaps[entity].ContainsKey(otherEntity))
        //        {
        //            movedX = _resolvedOverlaps[entity][otherEntity];
        //        }

        //        if (_resolvedOverlaps.ContainsKey(entity)
        //            && _resolvedOverlaps[entity].ContainsKey(otherEntity))
        //        {
        //            // Un-do the X-axis move
        //            MoveX(entity, -_resolvedOverlaps[entity][otherEntity]);
        //            Console.WriteLine($"Un-do X resolve by {-_resolvedOverlaps[entity][otherEntity]}");

        //        }

        //        // Check for an in-range overlap in the Y-axis
        //        if (direction.Y < 0)
        //        {
        //            // Check for a valid top overlap
        //            overlapY = otherBox.Bottom - box.Top;
        //            Console.WriteLine($"Overlap (Y-top) {overlapY}  {Math.Floor(distanceMoved.Y)}");
        //            if (overlapY < 0 || overlapY > Math.Ceiling(Math.Abs(distanceMoved.Y)))
        //                overlapY = 0;
        //        }
        //        else if (direction.Y > 0)
        //        {
        //            // Check for a valid bottom overlap
        //            overlapY = otherBox.Top - box.Bottom;
        //            Console.WriteLine($"Overlap (Y-bottom) {overlapY}  {Math.Ceiling(distanceMoved.Y)}");
        //            if (overlapY > 0 || Math.Abs(overlapY) > Math.Ceiling(distanceMoved.Y))  // -2 < overlap < 0
        //                overlapY = 0;
        //        }

        //        Console.WriteLine($"Valid overlap Y:{overlapY}");

        //        // Return if there is no valid overlap to resolve
        //        /*if (overlapY == 0)
        //            return;
        //        else
        //            MoveY(entity, overlapY);*/
        //        /*else
        //        {
        //            if (_resolvedOverlaps.ContainsKey(entity)
        //                && _resolvedOverlaps[entity].ContainsKey(otherEntity))
        //            {
        //                // Un-do the X-axis move
        //                //transformComponent.position.X -= _resolvedOverlaps[entity];
        //                //colliderComponent.Box.X -= _resolvedOverlaps[entity];
        //                transformComponent.position.X -= _resolvedOverlaps[entity][otherEntity];
        //                colliderComponent.Box.X -= _resolvedOverlaps[entity][otherEntity];
        //                Console.WriteLine($"Un-do X resolve by {_resolvedOverlaps[entity][otherEntity]}");

        //                MoveY(entity, overlapY);

        //                if (box.Intersects(otherBox))
        //                {
        //                    Console.WriteLine($"Re-do X resolve by {_resolvedOverlaps[entity][otherEntity]}");
        //                    MoveX(entity, _resolvedOverlaps[entity][otherEntity]);
        //                }
        //            }
        //            else
        //                MoveY(entity, overlapY);
        //        }*/

        //        if (overlapY != 0)
        //            MoveY(entity, overlapY);
        //        else if (movedX != 0)
        //            MoveX(entity, movedX); // Shouldn't be needed!

        //        if (_resolvedOverlaps.ContainsKey(entity)
        //            && _resolvedOverlaps[entity].ContainsKey(otherEntity))
        //        {
        //            Console.WriteLine("Check re-do X");
        //            if (box.Intersects(otherBox))
        //            {
        //                Console.WriteLine($"Re-do X resolve by {_resolvedOverlaps[entity][otherEntity]}");
        //                MoveX(entity, _resolvedOverlaps[entity][otherEntity]);
        //            }

        //            _resolvedOverlaps[entity].Remove(otherEntity);
        //        }
        //    }

        //}






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

            // Get the entity's distance moved and direction
            Vector2 distanceMoved = transformComponent.DistanceMoved();
            Vector2 direction = physicsComponent.Direction;

            // Resolve X-axis collisions
            if (axis == 'X')
            {
                int overlapX = 0;
                int absOverlapX = 0;

                // Check for an in-range overlap in the X-axis
                if (direction.X > 0)
                {
                    // Check for a valid right overlap
                    overlapX = otherBox.Left - box.Right;
                    Console.WriteLine($"Overlap (X-right) {overlapX}  {Math.Ceiling(distanceMoved.X)}");
                    if (overlapX > 0 || Math.Abs(overlapX) > Math.Ceiling(distanceMoved.X))
                        overlapX = 0;
                }
                else if (direction.X < 0)
                {
                    // Check for a valid left overlap
                    overlapX = otherBox.Right - box.Left;
                    Console.WriteLine($"Overlap (X-left) {overlapX}  {Math.Floor(distanceMoved.X)}");
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
                int absOverlapY = 0;

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

        }

        public void ResolveOppositeDirections(Entity entity, Entity otherEntity)
        {
            // Get the necessary components
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

            // Get the entity's direction and velocities
            string direction = physicsComponent.DirectionString;
            //int velocityX = physicsComponent.VelocityX;
            //int velocityY = physicsComponent.VelocityY;
            float velocityX = physicsComponent.Direction.X;
            float velocityY = physicsComponent.Direction.Y;
            float absVelocityX = Math.Abs(velocityX); // maxOverlapX  absVelocityX
            float absVelocityY = Math.Abs(velocityY); // maxOverlapY  absVelocityY

            // Declare the attributes of the other entity
            string otherDirection = otherPhysicsComponent.DirectionString;
            //int otherVelocityX = otherPhysicsComponent.VelocityX;
            //int otherVelocityY = otherPhysicsComponent.VelocityY;
            float otherVelocityX = otherPhysicsComponent.Direction.X;
            float otherVelocityY = otherPhysicsComponent.Direction.Y;
            float absOtherVelocityX = Math.Abs(otherVelocityX);
            float absOtherVelocityY = Math.Abs(otherVelocityY);

            // Calculate the total absolute X and Y velocities
            float totalAbsVelocityX = absVelocityX + absOtherVelocityX;
            float totalAbsVelocityY = absVelocityY + absOtherVelocityY;

            // Calculate the distance 
            Vector2 distance = transformComponent.position - transformComponent.previousPosition;
            //distance.Normalize();

            // Calculate the amount of overlap in each direction
            int overlapTop = otherBox.Bottom - box.Top;
            int overlapBottom = box.Bottom - otherBox.Top;
            int overlapRight = box.Right - otherBox.Left;
            int overlapLeft = otherBox.Right - box.Left;
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


        public void HandleCollisions(Entity entity, char axis)
        {
            ColliderComponent colliderComponent = entity.GetComponent<ColliderComponent>();
            CollisionHandlerComponent handlerComponent = entity.GetComponent<CollisionHandlerComponent>();
            PhysicsComponent physicsComponent = entity.GetComponent<PhysicsComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            // Check if the entity is not moving
            if (physicsComponent == null)
                return; // CHANGE either make required or use transform instead

            if (!(physicsComponent.HasVelocity() || transformComponent.HasMoved()))
                return;

            // Check if the entity is not solid
            if (!colliderComponent.IsSolid)
                return;

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

                // Check if the other entity is not solid
                if (!otherColliderComponent.IsSolid)
                    continue;

                // Check if the other entity is moving
                PhysicsComponent otherPhysicsComponent = otherEntity.GetComponent<PhysicsComponent>();

                bool hasOtherMoved = true;
                if (otherPhysicsComponent == null)
                    hasOtherMoved = false;
                else if (!otherPhysicsComponent.HasVelocity() && !otherTransformComponent.HasMoved())
                    hasOtherMoved = false;

                // Re-create the bounding boxes in case collisions have already been resolved
                Rectangle box = colliderComponent.CreateBoundingBox(transformComponent.position);
                Rectangle otherBox;
                if (hasOtherMoved)
                    otherBox = otherColliderComponent.CreateBoundingBox(otherTransformComponent.position);
                else
                    otherBox = otherColliderComponent.Box;

                // Check if the collision has already been resolved
                if (!box.Intersects(otherBox)) // axis == 'X' && 
                {
                    Console.WriteLine("\nCollision already resolved");
                    continue;
                }

                // Handle a collision between a moving and a stationary entity
                if (!hasOtherMoved)
                {
                    ResolveStationary(entity, otherEntity, axis);
                    continue;
                }

                // Calculate the dot product
                float dotProduct = Vector2.Dot(physicsComponent.Direction, otherPhysicsComponent.Direction);
                Console.WriteLine($"Dot product {dotProduct}");

                // Handle entities moving in opposite directions
                if (dotProduct == -1)
                {
                    ResolveOppositeDirections(entity, otherEntity);
                }

                // Handle entities moving in the same direction
                else if (dotProduct == 1)
                {
                    ResolveSameDirection(entity, otherEntity);
                }

                // Hanle entities moving in perpendicular directions
                else if (dotProduct == 0 || dotProduct == -0)
                {
                    ResolvePerpendicularDirection(entity, otherEntity);
                }

                // Handle all other directions
                else
                {
                    ResolveOtherDirection(entity, otherEntity);
                }
            }
        }


        public override void Update(GameTime gameTime, Scene scene)
        {
            // TESTING

            // Return all entities to their previous positions

            // Move the position + velocity.X
            // Check and resolve collisions

            // Move the position + velocity.Y
            // Check and resolve collisions

            // OR

            // Try returning to previous X position
            // Check intersect
            // If so, resolve other axis instead and return to previous?
            // ...


            // Resolve X-axis collisions first of all moving entities
            foreach (Entity entity in entityList)
            {
                Console.WriteLine($"\nX-axis collision resolving for entity {entity.Id}");
                HandleCollisions(entity, 'X');
            }

            // Then resolve Y-axis collisions of all moving entities
            foreach (Entity entity in entityList)
            {
                Console.WriteLine($"\nY-axis collision resolving for entity {entity.Id}");
                HandleCollisions(entity, 'Y');
            }
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
                Rectangle box = colliderComponent.CreateBoundingBox(transformComponent.position);
                Rectangle otherBox;
                if (hasOtherMoved)
                    otherBox = otherColliderComponent.CreateBoundingBox(otherTransformComponent.position);
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
                    ResolveOppositeDirections(entity, otherEntity);
                    continue;
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
                    otherDirection = otherPhysicsComponent.DirectionString;
                    otherVelocityX = otherPhysicsComponent.VelocityX;
                    otherVelocityY = otherPhysicsComponent.VelocityY;
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
        public void MoveX(Entity e, int amount)
        {
            TransformComponent transformComponent = e.GetComponent<TransformComponent>();
            ColliderComponent colliderComponent = e.GetComponent<ColliderComponent>();

            transformComponent.position.X += amount;
            colliderComponent.Box.X += amount;
        }

        // Move the Y position of an entity's transform component and bounding bounding
        public void MoveY(Entity e, int amount)
        {
            TransformComponent transformComponent = e.GetComponent<TransformComponent>();
            ColliderComponent colliderComponent = e.GetComponent<ColliderComponent>();

            transformComponent.position.Y += amount;
            colliderComponent.Box.Y += amount;
        }

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
