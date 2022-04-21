using Microsoft.Xna.Framework;
using MonoGame.Extended.Sprites;

namespace AdventureGame.Engine
{
    public class AnimationSystem : System
    {
        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {

            IntentionComponent intentionComponent = entity.GetComponent<IntentionComponent>();
            AnimationComponent animationComponent = entity.GetComponent<AnimationComponent>();

            if (intentionComponent == null || animationComponent == null)
                return;

            string animationState = "idle";

            if (intentionComponent.up)
                animationState = "walkNorth";

            if (intentionComponent.down)
                animationState = "walkSouth";

            if (intentionComponent.left)
                animationState = "walkWest";

            if (intentionComponent.right)
                animationState = "walkEast";

            animationComponent.animation.Play(animationState);
            animationComponent.animation.Update(gameTime);

        }

        public override void DrawEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            AnimationComponent animationComponent = entity.GetComponent<AnimationComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            if (animationComponent == null || transformComponent == null)
                return;

            Globals.spriteBatch.Draw(animationComponent.animation, transformComponent.position);
        }
    }
}
