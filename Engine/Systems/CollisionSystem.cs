using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

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
            Console.WriteLine($"Entity {entity.Id} collider added to scene");
            // Initialise the bounding box
            colliderComponent.Box = new Rectangle(
                (int)transformComponent.position.X + (int)colliderComponent.Offset.X,
                (int)transformComponent.position.Y + (int)colliderComponent.Offset.Y,
                (int)colliderComponent.Size.X, (int)colliderComponent.Size.Y
            );
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

        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            // Only check moving entities
            if (!transformComponent.HasMoved() && !_collisionEnded.Contains(entity))
                return;

            ColliderComponent colliderComponent = entity.GetComponent<ColliderComponent>();
            CollisionHandlerComponent handlerComponent = entity.GetComponent<CollisionHandlerComponent>();
            //PhysicsComponent physicsComponent = entity.GetComponent<PhysicsComponent>();

            // Update the bounding box
            colliderComponent.Box = new Rectangle(
                (int)transformComponent.position.X + (int)colliderComponent.Offset.X,
                (int)transformComponent.position.Y + (int)colliderComponent.Offset.Y,
                (int)colliderComponent.Size.X, (int)colliderComponent.Size.Y
            );

            // Testing
            /*
            if (physicsComponent != null)
            {
                Rectangle box = colliderComponent.Box;
                colliderComponent.Sweep = new Rectangle(
                    (int)Math.Ceiling(box.X + physicsComponent.Velocity.X * 2),
                    (int)Math.Ceiling(box.Y + physicsComponent.Velocity.Y * 2),
                    (int)Math.Ceiling(box.Width + physicsComponent.Velocity.X * 2),
                    (int)Math.Ceiling(box.Height + physicsComponent.Velocity.Y * 2)
                );

                // Clear box if !transformComponent.HasMoved()
            }*/

            // CHANGE to use a grid or quadtree?
            // Check for collider intersects
            foreach (Entity otherEntity in entityList) // scene.EntityList)
            {
                //if (entityMapper.ContainsKey(otherEntity.Id) && entity != otherEntity)
                if (entity != otherEntity)
                {
                    ColliderComponent otherColliderComponent = otherEntity.GetComponent<ColliderComponent>();

                    // Check if the entities have collided
                    if (colliderComponent.Box.Intersects(otherColliderComponent.Box))
                    {
                        //Console.WriteLine($"\nEntity {entity.Id} intersects with {otherEntity.Id}");

                        // Check if the entity isn't handling any collisions currently
                        if (handlerComponent == null)
                        {
                            entity.AddComponent(new CollisionHandlerComponent(), true);
                            handlerComponent = entity.GetComponent<CollisionHandlerComponent>();
                        }

                        // Check if the entities are already colliding
                        if (!(_collisionStarted.Contains(entity)
                            && handlerComponent.CollidedEntities.Contains(otherEntity)))
                        {
                            Console.WriteLine($"\nStart collision: {entity.Id} & {otherEntity.Id}");

                            // Add the entity to the collision started sets
                            _collisionStarted.Add(entity);
                            handlerComponent.CollidedEntities.Add(otherEntity);

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
                        Console.WriteLine($"End collision: {entity.Id} & {otherEntity.Id}");

                        // Remove the collided entity from the component set
                        handlerComponent.CollidedEntities.Remove(otherEntity);

                        // Check if the entity has any more collisions to handle
                        if (handlerComponent.CollidedEntities.Count == 0)
                            _collisionStarted.Remove(entity);

                        // Add the entities to the collision ended sets
                        _collisionEnded.Add(entity);
                        handlerComponent.CollidedEntitiesEnded.Add(otherEntity);

                        TestingOutputSets(); // Testing
                    }

                    // Check if the entites are exiting a previous collision
                    else if (_collisionEnded.Contains(entity) && handlerComponent != null
                        && handlerComponent.CollidedEntitiesEnded.Contains(otherEntity))
                    {
                        Console.WriteLine($"Exiting collision: {entity.Id} & {otherEntity.Id}");

                        // Remove the collided entity from the component set
                        handlerComponent.CollidedEntitiesEnded.Remove(otherEntity);

                        // Check if the entity has any more ended collisions to handle
                        if (handlerComponent.CollidedEntitiesEnded.Count == 0)
                        {
                            _collisionEnded.Remove(entity);

                            if (handlerComponent.CollidedEntities.Count == 0)
                                entity.RemoveComponent<CollisionHandlerComponent>();
                        }

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
            Globals.spriteBatch.DrawRectangle(colliderComponent.Sweep, Color.Black, lineWidth);
        }

        public void TestingOutputSets()
        {
            // Testing: output collided entities set
            Console.WriteLine("\nCollision started set: ");
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
            Console.WriteLine("\nCollision ended set: ");
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
