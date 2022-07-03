using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace AdventureGame.Engine
{
    public class HurtboxSystem : System
    {
        public HurtboxSystem()
        {
            RequiredComponent<HurtboxComponent>();
            RequiredComponent<TransformComponent>();
        }

        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            HurtboxComponent hurtBoxComponent = entity.GetComponent<HurtboxComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

        }

        public override void DrawEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            if (!EngineGlobals.DEBUG)
                return;

            HurtboxComponent hurtboxComponent = entity.GetComponent<HurtboxComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            // Testing: draw hitbox rectangle outline
            Rectangle rectangle = new Rectangle(
                (int)transformComponent.position.X + hurtboxComponent.xOffset,
                (int)transformComponent.position.Y + hurtboxComponent.yOffset,
                32, 32
            );

            Color color = hurtboxComponent.color;
            int lineWidth = 2;
            //Globals.spriteBatch.DrawRectangle(rectangle, color, lineWidth);
        }

    }
}
