using Microsoft.Xna.Framework;

namespace AdventureGame.Engine
{
    class PhysicsSystem : System
    {
        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {

            IntentionComponent intentionComponent = entity.GetComponent<IntentionComponent>();
            PhysicsComponent physicsComponent = entity.GetComponent<PhysicsComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            if (intentionComponent == null || physicsComponent == null || transformComponent == null)
                return;

            ColliderComponent colliderComponent = entity.GetComponent<ColliderComponent>();

            if (intentionComponent.up)
                transformComponent.position.Y -= physicsComponent.velocity;

            if (intentionComponent.down)
                transformComponent.position.Y += physicsComponent.velocity;

            if (intentionComponent.left)
                transformComponent.position.X -= physicsComponent.velocity;

            if (intentionComponent.right)
            {
                // check for collision, hit, damage?
                if (colliderComponent != null)
                {
                    if (colliderComponent.collidedEntityId != -1)
                    {
                        // get entity based on collidedEntityID
                        // get position of the entity
                        // get position of the collided entity
                        // check if collided entity is to the right of entity
                        // if so, move entity to the left edge of collided entity
                        //return;
                    }
                }
                transformComponent.position.X += physicsComponent.velocity;
            }

        }
    }
}
