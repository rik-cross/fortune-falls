using Microsoft.Xna.Framework;
using MonoGame.Extended;

using System;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    public class CollisionSystem : System
    {
        private HashSet<Entity> _collisionStarted;
        private HashSet<Entity> _collisionEnded;

        public CollisionSystem()
        {
            RequiredComponent<ColliderComponent>();
            RequiredComponent<TransformComponent>();

            _collisionStarted = new HashSet<Entity>();
            _collisionEnded = new HashSet<Entity>();
        }

        public override void OnEntityAddedToScene(Entity entity)
        {
            ColliderComponent colliderComponent = entity.GetComponent<ColliderComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            // Initialise the bounding box
            colliderComponent.GetBoundingBox(transformComponent.position);
        }

        public override void OnEntityDestroyed(GameTime gameTime, Scene scene, Entity entity)
        {
            //Console.WriteLine($"Remove {entity.Id} from all collision sets");
            RemoveEntityFromAllSets(entity);
        }

        public void RemoveEntityFromAllSets(Entity entityToRemove)
        {
            Console.WriteLine("Remove entity from all collision sets");
            TestingOutputSets();

            HashSet<Entity> allCollisions = new HashSet<Entity>(_collisionStarted);
            allCollisions.UnionWith(_collisionEnded);

            // Check if the entity to remove belongs in any collision sets
            foreach (Entity entity in allCollisions)
            {
                CollisionHandlerComponent handlerComponent = entity.GetComponent<CollisionHandlerComponent>();

                if (handlerComponent == null)
                    continue;

                // Remove the entity from the collision started sets
                if (handlerComponent.CollidedEntities.Contains(entityToRemove))
                {
                    handlerComponent.CollidedEntities.Remove(entityToRemove);
                    if (handlerComponent.CollidedEntities.Count == 0)
                        _collisionStarted.Remove(entity);
                }

                // Remove the entity from the collision ended sets
                if (handlerComponent.CollidedEntitiesEnded.Contains(entityToRemove))
                {
                    handlerComponent.CollidedEntitiesEnded.Remove(entityToRemove);
                    if (handlerComponent.CollidedEntitiesEnded.Count == 0)
                        _collisionEnded.Remove(entity);
                }

                // Check if the entity has any more collisions to handle
                if (handlerComponent.CollidedEntities.Count == 0
                    && handlerComponent.CollidedEntitiesEnded.Count == 0)
                    entity.RemoveComponent<CollisionHandlerComponent>();
            }

            // Remove the collision handler if the entity has one
            CollisionHandlerComponent entityToRemoveHandler = entityToRemove.GetComponent<CollisionHandlerComponent>();
            if (entityToRemoveHandler != null)
                entityToRemove.RemoveComponent<CollisionHandlerComponent>();

            TestingOutputSets();
        }

        public void AddCollisionHandler(Entity entity)
        {
            CollisionHandlerComponent handlerComponent = entity.GetComponent<CollisionHandlerComponent>();

            // Check if the entity isn't handling any collisions currently
            if (handlerComponent == null)
                entity.AddComponent(new CollisionHandlerComponent(), true);
        }

        public void AddToCollisionStarted(Entity entity, Entity otherEntity)
        {
            CollisionHandlerComponent handlerComponent = entity.GetComponent<CollisionHandlerComponent>();

            // Add a collision handler if the entity isn't handling any collisions currently
            if (handlerComponent == null)
            {
                AddCollisionHandler(entity);
                handlerComponent = entity.GetComponent<CollisionHandlerComponent>();
            }

            // Add the entity to the collision started sets
            _collisionStarted.Add(entity);
            handlerComponent.CollidedEntities.Add(otherEntity);
        }

        public void RemoveFromCollisionStarted(Entity entity, Entity otherEntity)
        {
            CollisionHandlerComponent handlerComponent = entity.GetComponent<CollisionHandlerComponent>();
            if (handlerComponent == null)
                return;

            // Remove the collided entity from the component set
            handlerComponent.CollidedEntities.Remove(otherEntity);

            // Check if the entity has any more collisions to handle
            if (handlerComponent.CollidedEntities.Count == 0)
                _collisionStarted.Remove(entity);
        }

        public void AddToCollisionEnded(Entity entity, Entity otherEntity)
        {
            CollisionHandlerComponent handlerComponent = entity.GetComponent<CollisionHandlerComponent>();
            if (handlerComponent == null)
                return;

            // Add the entities to the collision ended sets
            _collisionEnded.Add(entity);
            handlerComponent.CollidedEntitiesEnded.Add(otherEntity);
        }

        public void RemoveFromCollisionEnded(Entity entity, Entity otherEntity)
        {
            CollisionHandlerComponent handlerComponent = entity.GetComponent<CollisionHandlerComponent>();
            if (handlerComponent == null)
                return;

            // Remove the collided entity from the component set
            handlerComponent.CollidedEntitiesEnded.Remove(otherEntity);

            // Check if the entity has any more ended collisions to handle
            if (handlerComponent.CollidedEntitiesEnded.Count == 0)
            {
                _collisionEnded.Remove(entity);

                if (handlerComponent.CollidedEntities.Count == 0)
                    entity.RemoveComponent<CollisionHandlerComponent>();
            }
        }

        public override void Update(GameTime gameTime, Scene scene)
        {
            foreach (Entity e in entityList)
            {
                TransformComponent transformComponent = e.GetComponent<TransformComponent>();

                // Update the bounding box of all moving entities
                if (transformComponent.HasMoved())
                {
                    ColliderComponent colliderComponent = e.GetComponent<ColliderComponent>();
                    colliderComponent.GetBoundingBox(transformComponent.position);
                }

                // Broad-phase here too??
            }
        }

        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            ColliderComponent colliderComponent = entity.GetComponent<ColliderComponent>();
            CollisionHandlerComponent handlerComponent = entity.GetComponent<CollisionHandlerComponent>();
            PhysicsComponent physicsComponent = entity.GetComponent<PhysicsComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            // Testing: Broad-phasing
            if (physicsComponent != null && physicsComponent.WasMoving())
                colliderComponent.Broadphase = Rectangle.Empty;

            // Only check moving entities
            if (!transformComponent.HasMoved())// && !_collisionEnded.Contains(entity))
                return;

            // Update the bounding box
            //colliderComponent.GetBoundingBox(transformComponent.position);

            // Testing: Broad-phasing
            Rectangle broadphaseBox;
            if (physicsComponent != null)
                broadphaseBox = colliderComponent.GetBroadphaseBox(physicsComponent.Velocity);

            // CHANGE to use broad-phasing, a grid or a quadtree?
            // Check for collider intersects
            foreach (Entity otherEntity in entityList) // scene.EntityList)
            {
                //if (entityMapper.ContainsKey(otherEntity.Id) && entity != otherEntity)
                if (entity != otherEntity)
                {
                    ColliderComponent otherColliderComponent = otherEntity.GetComponent<ColliderComponent>();
                    CollisionHandlerComponent otherHandlerComponent = otherEntity.GetComponent<CollisionHandlerComponent>();
                    //PhysicsComponent otherPhysicsComponent = otherEntity.GetComponent<PhysicsComponent>(); Broadphase?
                    TransformComponent otherTransformComponent = otherEntity.GetComponent<TransformComponent>();

                    // Check if the entities have collided
                    if (colliderComponent.Box.Intersects(otherColliderComponent.Box))
                    {
                        Console.WriteLine($"\nEntity {entity.Id} intersects with {otherEntity.Id}");

                        // Check if the entity isn't handling any collisions currently
                        AddCollisionHandler(entity);
                        handlerComponent = entity.GetComponent<CollisionHandlerComponent>();
                        /*if (handlerComponent == null)
                        {
                            entity.AddComponent(new CollisionHandlerComponent(), true);
                            handlerComponent = entity.GetComponent<CollisionHandlerComponent>();
                        }*/

                        // Check if the entities are already colliding
                        if (!(_collisionStarted.Contains(entity)
                            && handlerComponent.CollidedEntities.Contains(otherEntity)))
                        {
                            Console.WriteLine($"Start collision: {entity.Id} & {otherEntity.Id}");

                            // Add the entity to the collision started sets
                            AddToCollisionStarted(entity, otherEntity);

                            // Add the other entity if it is also moving
                            //if (otherTransformComponent.HasMoved())
                            //    AddToCollisionStarted(otherEntity, entity);

                            // Check if this collision ended last frame
                            if (_collisionEnded.Contains(entity)
                                && handlerComponent.CollidedEntitiesEnded.Contains(otherEntity))
                            {
                                RemoveFromCollisionEnded(entity, otherEntity);
                                //RemoveFromCollisionEnded(otherEntity, entity);
                            }

                            // Testing: change component outline colour
                            colliderComponent.color = Color.Orange;
                            otherColliderComponent.color = Color.Orange;

                            TestingOutputSets(); // Testing
                        }
                    }

                    // Check if the entities were colliding and now they are not
                    else if (_collisionStarted.Contains(entity) && handlerComponent != null
                        && handlerComponent.CollidedEntities.Contains(otherEntity))
                    {
                        Console.WriteLine($"\nEnd collision: {entity.Id} & {otherEntity.Id}");

                        RemoveFromCollisionStarted(entity, otherEntity);
                        AddToCollisionEnded(entity, otherEntity);

                        // Check if the other entity was handling collisions
                        //if (otherHandlerComponent != null)
                        //{
                        //    RemoveFromCollisionStarted(otherEntity, entity);
                        //    AddToCollisionEnded(otherEntity, entity);
                        //}

                        TestingOutputSets(); // Testing
                    }

                    // TO DO - should it also check collisionStarted / collidedEntities??
                    // Check if the entites are exiting a previous collision
                    else if (_collisionEnded.Contains(entity) && handlerComponent != null
                        && handlerComponent.CollidedEntitiesEnded.Contains(otherEntity))
                    {
                        Console.WriteLine($"\nExiting collision: {entity.Id} & {otherEntity.Id}");

                        RemoveFromCollisionEnded(entity, otherEntity);

                        // Check if the other entity was handling collisions
                        //if (otherHandlerComponent != null)
                        //    RemoveFromCollisionEnded(otherEntity, entity);

                        // Testing: change component outline colour
                        colliderComponent.color = Color.Yellow;
                        otherColliderComponent.color = Color.Yellow;

                        TestingOutputSets(); // Testing
                    }
                }
            }
        }

        public override void DrawEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            if (!EngineGlobals.DEBUG)
                return;

            ColliderComponent colliderComponent = entity.GetComponent<ColliderComponent>();

            int lineWidth = 1;
            Color color;

            if (colliderComponent.IsSolid)
                color = colliderComponent.color;
            else
                color = Color.LightGray;

            Globals.spriteBatch.DrawRectangle(colliderComponent.Box, color, lineWidth);
            Globals.spriteBatch.DrawRectangle(colliderComponent.Broadphase, Color.Black, lineWidth);
        }

        public void TestingOutputSets()
        {
            // Testing: output collided entities set
            Console.WriteLine("Collision started set: ");
            foreach (Entity entity in _collisionStarted)
            {
                CollisionHandlerComponent handlerComponent = entity.GetComponent<CollisionHandlerComponent>();
                if (handlerComponent == null)
                    continue;

                Console.Write($"Entity {entity.Id}: ");
                foreach (Entity otherEntity in handlerComponent.CollidedEntities)
                {
                    Console.Write($"{otherEntity.Id}  ");
                }
                Console.WriteLine();
            }

            // Testing: output collided entities ended set
            Console.WriteLine("Collision ended set: ");
            foreach (Entity entity in _collisionEnded)
            {
                CollisionHandlerComponent handlerComponent = entity.GetComponent<CollisionHandlerComponent>();
                if (handlerComponent == null)
                    continue;

                Console.Write($"Entity {entity.Id}: ");
                foreach (Entity otherEntity in handlerComponent.CollidedEntitiesEnded)
                {
                    Console.Write($"{otherEntity.Id}  ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

    }
}
