using Microsoft.Xna.Framework;

namespace AdventureGame.Engine
{
    public class ItemSystem : System
    {
        public ItemSystem()
        {
            RequiredComponent<ItemComponent>();
        }

        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            ItemComponent itemComponent = entity.GetComponent<ItemComponent>();
            ColliderComponent colliderComponent = entity.GetComponent<ColliderComponent>();
            //TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            // Check if the item is active and has not been collected
            if (itemComponent.IsActive && !itemComponent.HasBeenCollected)
            {
                // Respond to a collision between the item and another entity
                foreach (Entity otherEntity in colliderComponent.collidedEntities)
                {
                    // Check if the item can be collected by the other entity
                    if (itemComponent.CanCollect(otherEntity.Tags.Type))
                    {
                        // Test - delete or hide the item entity
                        //ColliderComponent otherColliderComponent = otherEntity.GetComponent<ColliderComponent>();
                        //otherColliderComponent.collidedEntities.Remove(entity);
                        EngineGlobals.entityManager.DeleteEntity(entity);
                    }
                }
            }
        }

    }
}
