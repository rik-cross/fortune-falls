using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

            // move the hitbox based on the entity position
            Vector2 newPosition = transformComponent.position;
            int w = hitboxComponent.rectangle.Width;
            int h = hitboxComponent.rectangle.Height;
            hitboxComponent.rectangle.X = (int)newPosition.X - (int)(w / 2) + hitboxComponent.xOffset;
            hitboxComponent.rectangle.Y = (int)newPosition.Y - (int)(h / 2) + hitboxComponent.yOffset;
        }

        public override void DrawEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            HitboxComponent hitboxComponent = entity.GetComponent<HitboxComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            // TESTING draw hitbox rectangle outline
            Rectangle rectangle = hitboxComponent.rectangle;
            Color color = hitboxComponent.color;
            int lineWidth = 2;
            DrawRectangleOutline(rectangle, color, lineWidth);
        }

        // TESTING draw rectangle outline
        public void DrawRectangleOutline(Rectangle rectangle, Color color, int lineWidth)
        {
            Texture2D pointTexture = new Texture2D(Globals.spriteBatch.GraphicsDevice, 1, 1);
            pointTexture.SetData<Color>(new Color[] { Color.White });

            Globals.spriteBatch.Draw(pointTexture, new Rectangle(rectangle.X, rectangle.Y, lineWidth, rectangle.Height), color);
            Globals.spriteBatch.Draw(pointTexture, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, lineWidth), color);
            Globals.spriteBatch.Draw(pointTexture, new Rectangle(rectangle.X + rectangle.Width - lineWidth, rectangle.Y, lineWidth, rectangle.Height), color);
            Globals.spriteBatch.Draw(pointTexture, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height - lineWidth, rectangle.Width, lineWidth), color);
        }

    }
}
