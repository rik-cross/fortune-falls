using Microsoft.Xna.Framework;

namespace AdventureGame.Engine
{
    public class SpriteSystem : System
    {
        public SpriteSystem()
        {
            RequiredComponent<SpriteComponent>();
            RequiredComponent<TransformComponent>();
        }

        public override void DrawEntity(GameTime gameTime, Scene scene, Entity entity)
        {

            SpriteComponent spriteComponent = entity.GetComponent<SpriteComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();
            // ColliderComponent?

            if (spriteComponent == null || transformComponent == null)
                return;

            Globals.spriteBatch.Draw(
                spriteComponent.sprite,
                new Rectangle(
                    (int)(transformComponent.position.X - (transformComponent.size.X/2)),
                    (int)(transformComponent.position.Y - (transformComponent.size.Y/2)),
                    (int)transformComponent.size.X,
                    (int)transformComponent.size.Y
                ), Color.White);

        }

    }
}
