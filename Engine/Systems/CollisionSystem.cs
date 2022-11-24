using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace AdventureGame.Engine
{
    public class CollisionSystem : System
    {
        public HashSet<Entity> collisionStarted;
        public HashSet<Entity> collisionEnded;

        public CollisionSystem()
        {
            RequiredComponent<ColliderComponent>();
            RequiredComponent<TransformComponent>();

            collisionStarted = new HashSet<Entity>();
            collisionEnded = new HashSet<Entity>();
        }

        public void TestingOutputSets()
        {
            // Testing: output collided entities set
            Console.WriteLine("\nCollision started set: ");
            foreach (Entity entity in collisionStarted)
            {
                ColliderComponent colliderComponent = entity.GetComponent<ColliderComponent>();
                Console.Write($"Entity {entity.Id}: ");
                foreach (Entity otherEntity in colliderComponent.collidedEntities)
                {
                    Console.Write($"{otherEntity.Id}  ");
                }
                Console.WriteLine();
            }

            // Testing: output collided entities ended set
            Console.WriteLine("\nCollision ended set: ");
            foreach (Entity entity in collisionEnded)
            {
                ColliderComponent colliderComponent = entity.GetComponent<ColliderComponent>();
                Console.Write($"Entity {entity.Id}: ");
                foreach (Entity otherEntity in colliderComponent.collidedEntitiesEnded)
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
            foreach (Entity otherEntity in colliderComponent.collidedEntities)
            {
                ColliderComponent otherColliderComponent = otherEntity.GetComponent<ColliderComponent>();
                otherColliderComponent.collidedEntities.Remove(entityToRemove);

                // Check if the entity has any more collisions to handle
                if (otherColliderComponent.collidedEntities.Count == 0)
                    collisionStarted.Remove(otherEntity);
            }
            // Remove the entity from the collision started set
            collisionStarted.Remove(entityToRemove);

            // Remove the entity from all of the collided entities ended sets
            foreach (Entity otherEntity in colliderComponent.collidedEntitiesEnded)
            {
                ColliderComponent otherColliderComponent = otherEntity.GetComponent<ColliderComponent>();
                otherColliderComponent.collidedEntitiesEnded.Remove(entityToRemove);

                // Check if the entity has any more collisions to handle
                if (otherColliderComponent.collidedEntitiesEnded.Count == 0)
                    collisionEnded.Remove(otherEntity);
            }
            // Remove the entity from the collision ended set
            collisionEnded.Remove(entityToRemove);
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

            colliderComponent.rect = new Rectangle(
                (int)transformComponent.position.X + (int)colliderComponent.offset.X,
                (int)transformComponent.position.Y + (int)colliderComponent.offset.Y,
                (int)colliderComponent.size.X, (int)colliderComponent.size.Y
            );

            // Check for collider intersects
            foreach (Entity otherEntity in entityList) // scene.EntityList)
            {
                if (entityMapper.ContainsKey(otherEntity.Id) && entity != otherEntity)
                {
                    ColliderComponent otherColliderComponent = otherEntity.GetComponent<ColliderComponent>();

                    // Check if the entities have collided
                    if (colliderComponent.rect.Intersects(otherColliderComponent.rect))
                    {
                        //Console.WriteLine($"\nEntity {entity.Id} intersects with {otherEntity.Id}");

                        // Check if the entities are already colliding
                        if (!(collisionStarted.Contains(entity)
                            && colliderComponent.collidedEntities.Contains(otherEntity)))
                        {
                            //Console.WriteLine($"Start collision: {entity.Id} & {otherEntity.Id}");

                            // Add the entity and other entity to the collision started sets
                            collisionStarted.Add(entity);
                            collisionStarted.Add(otherEntity);
                            colliderComponent.collidedEntities.Add(otherEntity);
                            otherColliderComponent.collidedEntities.Add(entity);

                            // Testing
                            //TestingOutputSets();
                        }
                    }

                    // Check if the entities were colliding and now they are not
                    else if (collisionStarted.Contains(entity)
                        && colliderComponent.collidedEntities.Contains(otherEntity))
                    {
                        // Remove the collided entity from the component set
                        colliderComponent.collidedEntities.Remove(otherEntity);
                        otherColliderComponent.collidedEntities.Remove(entity);

                        // Check if the entity has any more collisions to handle
                        if (colliderComponent.collidedEntities.Count == 0)
                            collisionStarted.Remove(entity);

                        // Check if the other entity has any more collisions to handle
                        if (otherColliderComponent.collidedEntities.Count == 0)
                            collisionStarted.Remove(otherEntity);

                        // Add the entities to the collision ended sets
                        collisionEnded.Add(entity);
                        collisionEnded.Add(otherEntity);
                        colliderComponent.collidedEntitiesEnded.Add(otherEntity);
                        otherColliderComponent.collidedEntitiesEnded.Add(entity);

                        // Testing
                        //TestingOutputSets();
                    }
                    // Check if the entites are exiting a previous collision
                    else if (collisionEnded.Contains(entity)
                        && colliderComponent.collidedEntitiesEnded.Contains(otherEntity))
                    {
                        // Remove the collided entity from the component set
                        colliderComponent.collidedEntitiesEnded.Remove(otherEntity);
                        otherColliderComponent.collidedEntitiesEnded.Remove(entity);

                        // Check if the entity has any more ended collisions to handle
                        if (colliderComponent.collidedEntitiesEnded.Count == 0)
                            collisionEnded.Remove(entity);

                        // Check if the other entity has any more ended collisions to handle
                        if (otherColliderComponent.collidedEntitiesEnded.Count == 0)
                            collisionEnded.Remove(otherEntity);

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
            
            Color color = colliderComponent.color;
            int lineWidth = 1;
            Globals.spriteBatch.DrawRectangle(colliderComponent.rect, color, lineWidth);
        }

    }
}
