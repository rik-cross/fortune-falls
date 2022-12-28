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

        public void TestingOutputSets()
        {
            // Testing: output collided entities set
            Console.WriteLine("\nCollision started set: ");
            foreach (Entity entity in _collisionStarted)
            {
                ColliderComponent colliderComponent = entity.GetComponent<ColliderComponent>();
                Console.Write($"Entity {entity.Id}: ");
                foreach (Entity otherEntity in colliderComponent.CollidedEntities)
                {
                    Console.Write($"{otherEntity.Id}  ");
                }
                Console.WriteLine();
            }

            // Testing: output collided entities ended set
            Console.WriteLine("\nCollision ended set: ");
            foreach (Entity entity in _collisionEnded)
            {
                ColliderComponent colliderComponent = entity.GetComponent<ColliderComponent>();
                Console.Write($"Entity {entity.Id}: ");
                foreach (Entity otherEntity in colliderComponent.CollidedEntitiesEnded)
                {
                    Console.Write($"{otherEntity.Id}  ");
                }
                Console.WriteLine();
            }
        }

        public void RemoveEntityFromAll(Entity entityToRemove)
        {
            ColliderComponent colliderComponent = entityToRemove.GetComponent<ColliderComponent>();

            // Remove the entity from all of the collided entities sets
            foreach (Entity otherEntity in colliderComponent.CollidedEntities)
            {
                ColliderComponent otherColliderComponent = otherEntity.GetComponent<ColliderComponent>();
                otherColliderComponent.CollidedEntities.Remove(entityToRemove);

                // Check if the entity has any more collisions to handle
                if (otherColliderComponent.CollidedEntities.Count == 0)
                    _collisionStarted.Remove(otherEntity);
            }
            // Remove the entity from the collision started set
            _collisionStarted.Remove(entityToRemove);

            // Remove the entity from all of the collided entities ended sets
            foreach (Entity otherEntity in colliderComponent.CollidedEntitiesEnded)
            {
                ColliderComponent otherColliderComponent = otherEntity.GetComponent<ColliderComponent>();
                otherColliderComponent.CollidedEntitiesEnded.Remove(entityToRemove);

                // Check if the entity has any more collisions to handle
                if (otherColliderComponent.CollidedEntitiesEnded.Count == 0)
                    _collisionEnded.Remove(otherEntity);
            }
            // Remove the entity from the collision ended set
            _collisionEnded.Remove(entityToRemove);
        }

        public override void OnEntityDestroy(GameTime gameTime, Scene scene, Entity entity)
        {
            //Console.WriteLine($"Remove {entity.Id} from all collision sets");
            RemoveEntityFromAll(entity);

            // Testing
            //TestingOutputSets();
        }

        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            ColliderComponent colliderComponent = entity.GetComponent<ColliderComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            colliderComponent.Rect = new Rectangle(
                (int)transformComponent.position.X + (int)colliderComponent.Offset.X,
                (int)transformComponent.position.Y + (int)colliderComponent.Offset.Y,
                (int)colliderComponent.Size.X, (int)colliderComponent.Size.Y
            );

            // Check for collider intersects
            foreach (Entity otherEntity in entityList) // scene.EntityList)
            {
                if (entityMapper.ContainsKey(otherEntity.Id) && entity != otherEntity)
                {
                    ColliderComponent otherColliderComponent = otherEntity.GetComponent<ColliderComponent>();

                    // Check if the entities have collided
                    if (colliderComponent.Rect.Intersects(otherColliderComponent.Rect))
                    {
                        //Console.WriteLine($"\nEntity {entity.Id} intersects with {otherEntity.Id}");

                        // Check if the entities are already colliding
                        if (!(_collisionStarted.Contains(entity)
                            && colliderComponent.CollidedEntities.Contains(otherEntity)))
                        {
                            //Console.WriteLine($"Start collision: {entity.Id} & {otherEntity.Id}");

                            // Add the entity and other entity to the collision started sets
                            _collisionStarted.Add(entity);
                            _collisionStarted.Add(otherEntity);
                            colliderComponent.CollidedEntities.Add(otherEntity);
                            otherColliderComponent.CollidedEntities.Add(entity);

                            // Testing: change component outline colour
                            colliderComponent.color = Color.Orange;
                            otherColliderComponent.color = Color.Orange;

                            // Testing
                            //TestingOutputSets();
                        }
                    }

                    // Check if the entities were colliding and now they are not
                    else if (_collisionStarted.Contains(entity)
                        && colliderComponent.CollidedEntities.Contains(otherEntity))
                    {
                        // Remove the collided entity from the component set
                        colliderComponent.CollidedEntities.Remove(otherEntity);
                        otherColliderComponent.CollidedEntities.Remove(entity);

                        // Check if the entity has any more collisions to handle
                        if (colliderComponent.CollidedEntities.Count == 0)
                            _collisionStarted.Remove(entity);

                        // Check if the other entity has any more collisions to handle
                        if (otherColliderComponent.CollidedEntities.Count == 0)
                            _collisionStarted.Remove(otherEntity);

                        // Add the entities to the collision ended sets
                        _collisionEnded.Add(entity);
                        _collisionEnded.Add(otherEntity);
                        colliderComponent.CollidedEntitiesEnded.Add(otherEntity);
                        otherColliderComponent.CollidedEntitiesEnded.Add(entity);

                        // Testing
                        //TestingOutputSets();
                    }
                    // Check if the entites are exiting a previous collision
                    else if (_collisionEnded.Contains(entity)
                        && colliderComponent.CollidedEntitiesEnded.Contains(otherEntity))
                    {
                        // Remove the collided entity from the component set
                        colliderComponent.CollidedEntitiesEnded.Remove(otherEntity);
                        otherColliderComponent.CollidedEntitiesEnded.Remove(entity);

                        // Check if the entity has any more ended collisions to handle
                        if (colliderComponent.CollidedEntitiesEnded.Count == 0)
                            _collisionEnded.Remove(entity);

                        // Check if the other entity has any more ended collisions to handle
                        if (otherColliderComponent.CollidedEntitiesEnded.Count == 0)
                            _collisionEnded.Remove(otherEntity);

                        // Testing: change component outline colour
                        colliderComponent.color = Color.Yellow;
                        otherColliderComponent.color = Color.Yellow;
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

            Globals.spriteBatch.DrawRectangle(colliderComponent.Rect, color, lineWidth);
        }

    }
}
