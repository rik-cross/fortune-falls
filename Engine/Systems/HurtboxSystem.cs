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

            // move the hurtbox based on the entity position
            Vector2 newPosition = transformComponent.position;
            int w = hurtBoxComponent.rectangle.Width;
            int h = hurtBoxComponent.rectangle.Height;
            hurtBoxComponent.rectangle.X = (int)newPosition.X - (int)(w / 2) + hurtBoxComponent.xOffset;
            hurtBoxComponent.rectangle.Y = (int)newPosition.Y - (int)(h / 2) + hurtBoxComponent.yOffset;
        }

        public override void DrawEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            if (!EngineGlobals.DEBUG)
                return;

            HurtboxComponent hurtboxComponent = entity.GetComponent<HurtboxComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            // Testing: draw hitbox rectangle outline
            Rectangle rectangle = hurtboxComponent.rectangle;
            Color color = hurtboxComponent.color;
            int lineWidth = 2;
            //Globals.spriteBatch.DrawRectangle(rectangle, color, lineWidth);
        }

    }
}
