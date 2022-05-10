﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

            if (hurtBoxComponent == null || transformComponent == null)
                return;

            // move the hurtbox based on the entity position
            Vector2 newPosition = transformComponent.position;
            int w = hurtBoxComponent.rectangle.Width;
            int h = hurtBoxComponent.rectangle.Height;
            hurtBoxComponent.rectangle.X = (int)newPosition.X - (int)(w / 2) + hurtBoxComponent.xOffset;
            hurtBoxComponent.rectangle.Y = (int)newPosition.Y - (int)(h / 2) + hurtBoxComponent.yOffset;
        }

        public override void DrawEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            HurtboxComponent hurtboxComponent = entity.GetComponent<HurtboxComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            if (hurtboxComponent == null || transformComponent == null)
                return;

            // TESTING draw hitbox rectangle outline
            Rectangle rectangle = hurtboxComponent.rectangle;
            Color color = hurtboxComponent.color;
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
