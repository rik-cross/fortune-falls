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

        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            // Return if the entity does not have the required components
            /*if (!entity.CheckComponents(entity.signature, systemSignature))
                return;
            */

            ColliderComponent colliderComponent = entity.GetComponent<ColliderComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            // track entity here or elsewhere?
            // CHECK why can't components be passed as parameters? Eg TrackEntity(ColliderComponent colliderComponent, TransformComponent transformComponent)
            Vector2 newPosition = transformComponent.position;
            int w = colliderComponent.boundingBox.Width;
            int h = colliderComponent.boundingBox.Height;
            colliderComponent.boundingBox.X = (int)newPosition.X - (int)(w / 2) + colliderComponent.xOffset;
            colliderComponent.boundingBox.Y = (int)newPosition.Y - (int)(h / 2) + colliderComponent.yOffset;

            // check for collider intersects
            foreach (Entity otherE in scene.entityList)
            {
                if (entityList.Contains(otherE) && entity != otherE)
                {
                    ColliderComponent otherColliderComponent = otherE.GetComponent<ColliderComponent>();
                    TransformComponent otherTransformComponent = otherE.GetComponent<TransformComponent>();

                    if (otherColliderComponent != null && otherTransformComponent != null)
                    {
                        // Check if the entities have collided
                        if (colliderComponent.boundingBox.Intersects(otherColliderComponent.boundingBox))
                        {
                            // Check if the entities are already colliding
                            if (!collisionStarted.Contains(entity) &&
                                    !colliderComponent.collidedEntities.Contains(otherE))
                            {
                                // Add the entity to the collision started set
                                collisionStarted.Add(entity);

                                // Add the collided entity to the component set
                                colliderComponent.collidedEntities.Add(otherE);

                                // Testing: change component outline colour
                                colliderComponent.color = Color.Orange;
                                otherColliderComponent.color = Color.Orange;

                                // Testing: output collided entities set
                                Console.WriteLine("\nCollision started set: ");
                                Console.WriteLine($"Entity {entity.id}");
                                foreach (Entity e in colliderComponent.collidedEntities)
                                    Console.WriteLine($"Other entity {e.id}  ");

                                // Testing: delete non-player entity on collide
                                //if (otherE.id != 0)
                                    //EngineGlobals.entityManager.DeleteEntity(otherE);
                            }
                        }
                        // Check if the entities were colliding and now they are not
                        else if (collisionStarted.Contains(entity) &&
                                    colliderComponent.collidedEntities.Contains(otherE))
                        {
                            // Remove the collided entity from the component set
                            colliderComponent.collidedEntities.Remove(otherE);

                            // Check if the entity has any more collisions to handle
                            if (colliderComponent.collidedEntities.Count == 0)
                                // Remove the entity from the collision started set
                                collisionStarted.Remove(entity);

                            // Add the entities to the collision ended sets
                            collisionEnded.Add(entity);
                            colliderComponent.collidedEntitiesEnded.Add(otherE);

                            // Testing: output collided entities ended set
                            Console.WriteLine("\nCollision ended set: ");
                            Console.WriteLine($"Entity {entity.id}");
                            foreach (Entity e in colliderComponent.collidedEntitiesEnded)
                                Console.WriteLine($"Other entity {e.id}  ");
                        }
                        // Check if the entites are exiting a previous collision
                        else if (collisionEnded.Contains(entity) &&
                            colliderComponent.collidedEntitiesEnded.Contains(otherE))
                        {
                            // Remove the collided entity from the component set
                            colliderComponent.collidedEntitiesEnded.Remove(otherE);

                            // Check if the entity has any more ended collisions to handle
                            if (colliderComponent.collidedEntitiesEnded.Count == 0)
                                // Remove the entity from the collision ended set
                                collisionEnded.Remove(entity);

                            // colliderComponent.collidingUp = false;
                            // OR
                            //colliderComponent.collidingDirection = "";

                            // Testing: change component outline colour
                            colliderComponent.color = Color.Yellow;
                            otherColliderComponent.color = Color.Yellow;
                        }
                    }
                }
            }
        }

        public HashSet<Entity> GetCollidedEntities(Entity entity)
        {
            ColliderComponent colliderComponent = entity.GetComponent<ColliderComponent>();
            return colliderComponent.collidedEntities;
        }

        public HashSet<Entity> GetCollidedEntitiesEnded(Entity entity)
        {
            ColliderComponent colliderComponent = entity.GetComponent<ColliderComponent>();
            return colliderComponent.collidedEntitiesEnded;
        }

        public void OnCollisionEnter(Entity e, Entity otherEntity)
        {

        }

        public override void DrawEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            if (!EngineGlobals.DEBUG)
                return;

            ColliderComponent colliderComponent = entity.GetComponent<ColliderComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            // Testing: draw collider bounding box outline
            Rectangle rectangle = colliderComponent.boundingBox;
            Color color = colliderComponent.color;
            int lineWidth = 1;
            Globals.spriteBatch.DrawRectangle(rectangle, color, lineWidth);
        }

    }
}
