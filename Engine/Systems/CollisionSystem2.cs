using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace AdventureGame.Engine
{
    public class CollisionSystem2 : System
    {
        public HashSet<Entity> collisionStarted;
        public HashSet<Entity> collisionEnded;
        public Dictionary<Entity, HashSet<Entity>> collisionStartedDict;
        public Dictionary<Entity, HashSet<Entity>> collisionEndedDict;

        public CollisionSystem2()
        {
            RequiredComponent<ColliderComponent>();
            RequiredComponent<TransformComponent>();

            collisionStarted = new HashSet<Entity>();
            collisionEnded = new HashSet<Entity>();
            collisionStartedDict = new Dictionary<Entity, HashSet<Entity>>();
            collisionEndedDict = new Dictionary<Entity, HashSet<Entity>>();
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

            // check for collider intersects
            foreach (Entity otherEntity in entityList) // scene.EntityList)
            {
                if (entityMapper.ContainsKey(otherEntity.Id) && entity != otherEntity)
                {
                    ColliderComponent otherColliderComponent = otherEntity.GetComponent<ColliderComponent>();
                    TransformComponent otherTransformComponent = otherEntity.GetComponent<TransformComponent>();

                    // Check if the entities have collided
                    if (colliderComponent.rect.Intersects(otherColliderComponent.rect))
                    {
                        AddEntity(entity, otherEntity, collisionStartedDict);

                        // Testing: change component outline colour
                        colliderComponent.color = Color.Orange;
                        otherColliderComponent.color = Color.Orange;
                    }
                    // Check if the entities were colliding and now they are not
                    else if (collisionStartedDict.TryGetValue(
                        entity, out HashSet<Entity> collisionStartedSet))
                    {
                        if (collisionStartedSet.Contains(otherEntity))
                        {
                            RemoveEntity(entity, otherEntity, collisionStartedDict);
                            AddEntity(entity, otherEntity, collisionEndedDict);
                        }
                    }
                    // Check if the entites are exiting a previous collision
                    else if (collisionEndedDict.TryGetValue(
                        entity, out HashSet<Entity> collisionEndedSet))
                    {
                        if (collisionEndedSet.Contains(otherEntity))
                        {
                            RemoveEntity(entity, otherEntity, collisionEndedDict);

                            // Testing: change component outline colour
                            colliderComponent.color = Color.Yellow;
                            otherColliderComponent.color = Color.Yellow;
                        }
                    }
                }
            }
        }

        // DELETE?
        public void HandleCollisionStarted(Entity entity, Entity otherEntity)
        {
            AddEntity(entity, otherEntity, collisionStartedDict);
        }

        // Add an entity to the hashset of a collision dictionary
        public void AddEntity(Entity entity, Entity entityToAdd,
            Dictionary<Entity, HashSet<Entity>> dictionary)
        {
            // Check if the entity does not already exist in the dictionary
            if (!dictionary.ContainsKey(entity))
            {
                Console.WriteLine($"CREATE new dict key {entity.Id} and {entityToAdd.Id}");
                HashSet<Entity> collisionSet = new HashSet<Entity>();
                collisionSet.Add(entityToAdd);
                dictionary.Add(entity, collisionSet);
            }
            // Check if the other entity is not already in the collision set
            else if (!dictionary[entity].Contains(entityToAdd))
            {
                Console.WriteLine($"ADD to dict hashset {entity.Id} and {entityToAdd.Id}");
                dictionary[entity].Add(entityToAdd);
            }
        }

        // Remove an entity from the hashset of a collision dictionary
        public void RemoveEntity(Entity entity, Entity entityToRemove,
            Dictionary<Entity, HashSet<Entity>> dictionary)
        {
            Console.WriteLine($"TRY to remove entity {entityToRemove.Id} from {entity.Id}");
            // Try to remove the other entity from the entity's collision set
            if (dictionary.TryGetValue(entity, out HashSet<Entity> collisionSet))
            {
                Console.WriteLine($"Collision set count: {collisionSet.Count}");
                dictionary[entity].Remove(entityToRemove);
                Console.WriteLine($"New collision set count: {collisionSet.Count}");

                if (collisionSet.Count == 0)
                {
                    Console.WriteLine($"Remove from collision dictionary: {entity.Id}");
                    dictionary.Remove(entity);
                }
            }
            else
            {
                Console.WriteLine($"No such key for entity: {entity.Id}");
            }
        }

        public void RemoveEntityFromAll(Entity entityToRemove,
            Dictionary<Entity, HashSet<Entity>> dictionary)
        {
            // Try to remove the entity from the collision dictionary
            if (dictionary.TryGetValue(entityToRemove, out HashSet<Entity> collisionSet))
            {
                // Remove each collided entity from the collision set
                foreach (Entity collidedEntity in collisionSet)
                {
                    RemoveEntity(collidedEntity, entityToRemove, collisionStartedDict);
                }
            }
        }

        /* use on Update() instead of UpdateEntity()
        public override void Update(GameTime gameTime, Scene scene)
        public void HandleCollisions()
        {

            foreach (Entity collidingEntity in collisionStarted)
            {
                ColliderComponent colliderComponent = collidingEntity.GetComponent<ColliderComponent>();
                TransformComponent transformComponent = collidingEntity.GetComponent<TransformComponent>();

                if (colliderComponent != null || transformComponent != null)
                {
                    foreach (Entity otherEntity in colliderComponent.collidedEntities)
                    {
                        ColliderComponent otherColliderComponent = otherEntity.GetComponent<ColliderComponent>();
                        TransformComponent otherTransformComponent = otherEntity.GetComponent<TransformComponent>();

                        if (otherColliderComponent != null && otherTransformComponent != null)
                        {
                            // Check if the entities have stopped collided
                            if (!colliderComponent.rect.Intersects(otherColliderComponent.rect))
                            {
                                Console.WriteLine($"Collision ended between {collidingEntity.Id} and {otherEntity.Id}");
                                RemoveEntity(collidingEntity, otherEntity, collisionStartedDict);
                            }
                            else
                            {
                                Console.WriteLine($"Still colliding between {collidingEntity.Id} and {otherEntity.Id}");
                            }
                        }
                    }
                }
            }
        }
        */

        public override void OnEntityDestroy (GameTime gameTime, Scene scene, Entity entity)
        {
            Console.WriteLine($"REMOVE {entity.Id} from all collision dicts");

            RemoveEntityFromAll(entity, collisionStartedDict);
            RemoveEntityFromAll(entity, collisionEndedDict);

            // Testing: output collided entities started dict
            Console.WriteLine("\nCollision started dict: ");
            foreach (KeyValuePair<Entity, HashSet<Entity>> kvp in collisionStartedDict)
            {
                Console.WriteLine($"Entity {kvp.Key.Id}");
                foreach (Entity e in kvp.Value)
                {
                    Console.WriteLine($"Other entity {e.Id}  ");
                }
            }

            // Testing: output collided entities ended dict
            Console.WriteLine("\nCollision ended dict: ");
            foreach (KeyValuePair<Entity, HashSet<Entity>> kvp in collisionEndedDict)
            {
                Console.WriteLine($"Entity {kvp.Key.Id}");
                foreach (Entity e in kvp.Value)
                {
                    Console.WriteLine($"Other entity {e.Id}  ");
                }
            }
        }

        /*
        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            ColliderComponent colliderComponent = entity.GetComponent<ColliderComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            colliderComponent.rect = new Rectangle(
                (int)transformComponent.position.X + (int)colliderComponent.offset.X,
                (int)transformComponent.position.Y + (int)colliderComponent.offset.Y,
                (int)colliderComponent.size.X, (int)colliderComponent.size.Y
            );

            // check for collider intersects
            foreach (Entity otherEntity in entityList) // scene.EntityList)
            {
                if (entityMapper.ContainsKey(otherEntity.Id) && entity != otherEntity)
                {
                    ColliderComponent otherColliderComponent = otherEntity.GetComponent<ColliderComponent>();
                    TransformComponent otherTransformComponent = otherEntity.GetComponent<TransformComponent>();

                    if (otherColliderComponent != null && otherTransformComponent != null)
                    {
                        // Check if the entities have collided
                        if (colliderComponent.rect.Intersects(otherColliderComponent.rect))
                        {
                            //Console.WriteLine($"\nEntity {entity.Id} intersects with {otherEntity.Id}");
                            // Check if the entities are already colliding
                            if (!(collisionStarted.Contains(entity)
                                && colliderComponent.collidedEntities.Contains(otherEntity)))
                            {
                                //Console.WriteLine($"Start collision: {entity.Id} & {otherEntity.Id}");
                                // Add the entity to the collision started set
                                collisionStarted.Add(entity);

                                // Add the collided entity to the component set
                                colliderComponent.collidedEntities.Add(otherEntity);

                                // Testing: change component outline colour
                                colliderComponent.color = Color.Orange;
                                otherColliderComponent.color = Color.Orange;

                                // Testing: output collided entities set
                                
                                Console.WriteLine("\nCollision started set: ");
                                Console.WriteLine($"Entity {entity.Id}");
                                foreach (Entity e in colliderComponent.collidedEntities)
                                    Console.WriteLine($"Other entity {e.Id}  ");
                                
                            }
                        }
                        // HERE remove from Started / Ended sets on Entity delete
                        // CHECK whether the entities are still intersecting?

                        // OR is it because the components are gone, therefore the system
                        // does not update anymore??
                        // could check for all entities in the scene again for non-active entities
                        // or have a way to clear sets etc OnDestroy
                        
                        // Check if the entities were colliding and now they are not
                        else if (collisionStarted.Contains(entity)
                            && colliderComponent.collidedEntities.Contains(otherEntity))
                        {
                            // Remove the collided entity from the component set
                            colliderComponent.collidedEntities.Remove(otherEntity);

                            // Check if the entity has any more collisions to handle
                            if (colliderComponent.collidedEntities.Count == 0)
                                // Remove the entity from the collision started set
                                collisionStarted.Remove(entity);

                            // Add the entities to the collision ended sets
                            collisionEnded.Add(entity);
                            colliderComponent.collidedEntitiesEnded.Add(otherEntity);

                            // Remove the other entity from the resolved collisions set
                            //colliderComponent.resolvedCollisions.Remove(otherE);

                            // Testing: output collided entities ended set
                            
                            Console.WriteLine("\nCollision ended set: ");
                            Console.WriteLine($"Entity {entity.Id}");
                            foreach (Entity e in colliderComponent.collidedEntitiesEnded)
                                Console.WriteLine($"Other entity {e.Id}  ");
                            Console.WriteLine();
                            
                        }
                        // Check if the entites are exiting a previous collision
                        else if (collisionEnded.Contains(entity)
                            && colliderComponent.collidedEntitiesEnded.Contains(otherEntity))
                        {
                            // Remove the collided entity from the component set
                            colliderComponent.collidedEntitiesEnded.Remove(otherEntity);

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
        */

        public void OnCollisionEnter(Entity e, Entity otherEntity)
        {

        }

        public override void DrawEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            if (!EngineGlobals.DEBUG)
                return;
            /*
            foreach (Entity ent in collisionStarted)
                Console.WriteLine($"Collision started {ent.Id}");

            foreach (Entity ent in collisionEnded)
                Console.WriteLine($"Collision ended {ent.Id}");
            */
            //Console.WriteLine($"Entity {entity.Id}");
            //Console.WriteLine($"{entity.Id}: {entity.Signature}");
            //Console.WriteLine($"{entity.Id}: {entity.Signature} {gameTime.ElapsedGameTime.TotalSeconds}");
            ColliderComponent colliderComponent = entity.GetComponent<ColliderComponent>();
            
            Color color = colliderComponent.color;
            int lineWidth = 1;
            Globals.spriteBatch.DrawRectangle(colliderComponent.rect, color, lineWidth);
        }

    }
}
