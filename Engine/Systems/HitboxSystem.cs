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
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            hitboxComponent.rect = new Rectangle(
                (int)transformComponent.position.X + (int)hitboxComponent.offset.X,
                (int)transformComponent.position.Y + (int)hitboxComponent.offset.Y,
                (int)hitboxComponent.size.X, (int)hitboxComponent.size.Y
            );

        }

        public override void DrawEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            if (!EngineGlobals.DEBUG)
                return;

            HitboxComponent hitboxComponent = entity.GetComponent<HitboxComponent>();

            Color color = hitboxComponent.color;
            int lineWidth = 2;
            //Globals.spriteBatch.DrawRectangle(hitboxComponent.rect, color, lineWidth);
        }

    }
}
