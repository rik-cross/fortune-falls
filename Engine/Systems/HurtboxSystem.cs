using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Engine
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
            HurtboxComponent hurtboxComponent = entity.GetComponent<HurtboxComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            hurtboxComponent.Rect = new Rectangle(
                (int)transformComponent.Position.X + (int)hurtboxComponent.Offset.X,
                (int)transformComponent.Position.Y + (int)hurtboxComponent.Offset.Y,
                (int)hurtboxComponent.Size.X, (int)hurtboxComponent.Size.Y
            );

        }

        public override void DrawEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            if (!EngineGlobals.DEBUG)
                return;

            HurtboxComponent hurtboxComponent = entity.GetComponent<HurtboxComponent>();

            Color color = hurtboxComponent.BorderColor;
            int lineWidth = 2;
            //Globals.spriteBatch.DrawRectangle(hurtboxComponent.Rect, color, lineWidth);
        }

    }
}
