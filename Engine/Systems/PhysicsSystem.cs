using Microsoft.Xna.Framework;

namespace AdventureGame.Engine
{
    class PhysicsSystem : System
    {
        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {

            IntentionComponent intentionComponent = entity.GetComponent<IntentionComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            if (intentionComponent == null || transformComponent == null)
                return;

            if (intentionComponent.up)
                transformComponent.position.Y -= 1;

            if (intentionComponent.down)
                transformComponent.position.Y += 1;

            if (intentionComponent.left)
                transformComponent.position.X -= 1;

            if (intentionComponent.right)
                transformComponent.position.X += 1;

        }
    }
}
