using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace AdventureGame.Engine
{
    public class HitboxSystem : System
    {
        public HitboxSystem()
        {
            RequiredComponent<HitboxComponent>();
            RequiredComponent<TransformComponent>();
        }

        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            HitboxComponent hitboxComponent = entity.GetComponent<HitboxComponent>();
            //HurtboxComponent hurtBoxComponent = entity.GetComponent<HurtboxComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            if (hitboxComponent == null || transformComponent == null)
            {
                return;
            }
                
            // move the hitbox based on the entity position
            Vector2 newPosition = transformComponent.position;
            int w = hitboxComponent.rectangle.Width;
            int h = hitboxComponent.rectangle.Height;
            hitboxComponent.rectangle.X = (int)newPosition.X - (int)(w / 2) + hitboxComponent.xOffset;
            hitboxComponent.rectangle.Y = (int)newPosition.Y - (int)(h / 2) + hitboxComponent.yOffset;
        }

        public override void DrawEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            if (!EngineGlobals.DEBUG)
                return;

            HitboxComponent hitboxComponent = entity.GetComponent<HitboxComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            // Testing: draw hitbox rectangle outline
            Rectangle rectangle = hitboxComponent.rectangle;
            Color color = hitboxComponent.color;
            int lineWidth = 2;
            Globals.spriteBatch.DrawRectangle(rectangle, color, lineWidth);
        }

    }
}
