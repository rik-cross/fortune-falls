using Microsoft.Xna.Framework;
using System;

namespace AdventureGame.Engine
{
    class PhysicsSystem : System
    {
        //public List<>

        public PhysicsSystem()
        {
            RequiredComponent<IntentionComponent>();
            RequiredComponent<PhysicsComponent>();
            RequiredComponent<TransformComponent>();
        }

        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            IntentionComponent intentionComponent = entity.GetComponent<IntentionComponent>();
            PhysicsComponent physicsComponent = entity.GetComponent<PhysicsComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            // should all the collider code be in ColliderSystem?
            ColliderComponent colliderComponent = entity.GetComponent<ColliderComponent>();

            if (intentionComponent.up)
                transformComponent.position.Y -= physicsComponent.velocity;

            if (intentionComponent.down)
                transformComponent.position.Y += physicsComponent.velocity;

            if (intentionComponent.left)
                transformComponent.position.X -= physicsComponent.velocity;

            if (intentionComponent.right)
            {
                float newPosition = transformComponent.position.X + physicsComponent.velocity;

                // check for collision, hit, damage?
                if (colliderComponent != null)
                {
                    foreach (Entity collidedEntity in colliderComponent.collidedEntities)
                    {
                        ColliderComponent otherColliderComponent = collidedEntity.GetComponent<ColliderComponent>();

                        //Console.WriteLine($"Physics system: Entity {entity.id} collided with Entity {collidedEntity.id}");
                        
                        if (colliderComponent.isSolid && otherColliderComponent.isSolid)
                        {
                            float colliderHalfWidth = colliderComponent.rectangle.Width / 2;

                            newPosition = otherColliderComponent.rectangle.Left - colliderHalfWidth;
                        }
                    }

                    foreach (Entity collidedEntity in colliderComponent.collidedEntitiesEnded)
                    {
                        Console.WriteLine($"Physics system: Entity {entity.id} stopped colliding with Entity {collidedEntity.id}");
                    }
                    /*if (colliderComponent.collidedEntityId != -1)
                    {
                        // get entity based on collidedEntityID
                        // get position of the entity
                        // get position of the collided entity
                        // check if collided entity is to the right of entity
                        // if so, move entity to the left edge of collided entity
                        //return;
                    }*/
                }
                transformComponent.position.X = newPosition;
                //transformComponent.position.X += physicsComponent.velocity;
            }

        }
    }
}
