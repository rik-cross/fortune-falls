using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace AdventureGame.Engine
{
    public class TextSystem : System
    {
        public override void DrawEntity(GameTime gameTime, Scene scene, Entity entity)
        {

            TextComponent textComponent = entity.GetComponent<TextComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            if (textComponent == null || transformComponent == null)
                return;

            int rowHeight = textComponent.totalHeight;

            Globals.spriteBatch.FillRectangle(
                new Rectangle(
                    (int)(transformComponent.position.X - transformComponent.size.X / 2),
                    (int)(transformComponent.position.Y - transformComponent.size.Y / 2 - textComponent.totalHeight - (textComponent.outerMargin*2)),
                    textComponent.textWidth + textComponent.outerMargin * 2,
                    textComponent.totalHeight + textComponent.outerMargin * 2
                ),
                textComponent.backgroundColour
            );

            Globals.spriteBatch.DrawRectangle(
                new Rectangle(
                    (int)(transformComponent.position.X - transformComponent.size.X / 2),
                    (int)(transformComponent.position.Y - transformComponent.size.Y / 2 - textComponent.totalHeight - (textComponent.outerMargin*2)),
                    textComponent.textWidth + textComponent.outerMargin * 2,
                    textComponent.totalHeight + textComponent.outerMargin * 2
                ),
                textComponent.textColour
            );

            foreach (string line in textComponent.splitText) {
                Globals.spriteBatch.DrawString(
                    Globals.fontSmall,
                    line,
                    new Vector2(
                        transformComponent.position.X - transformComponent.size.X / 2 + textComponent.outerMargin,
                        transformComponent.position.Y - transformComponent.size.Y / 2 - rowHeight - (textComponent.outerMargin)
                    ), textComponent.textColour
                );
                rowHeight -= textComponent.singleRowheight;
            }

        }
    }
}
