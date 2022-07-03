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
            HurtboxComponent hurtboxComponent = entity.GetComponent<HurtboxComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            hurtboxComponent.rect = new Rectangle(
                (int)transformComponent.position.X + hurtboxComponent.xOffset,
                (int)transformComponent.position.Y + hurtboxComponent.yOffset,
                hurtboxComponent.width, hurtboxComponent.height
            );

        }

        public override void DrawEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            if (!EngineGlobals.DEBUG)
                return;

            HurtboxComponent hurtboxComponent = entity.GetComponent<HurtboxComponent>();

            Color color = hurtboxComponent.color;
            int lineWidth = 2;
            //Globals.spriteBatch.DrawRectangle(hurtboxComponent.rect, color, lineWidth);
        }

    }
}
