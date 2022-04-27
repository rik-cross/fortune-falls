using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;

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

            int r = 0;
            foreach (string line in textComponent.splitText) {

                if (r > textComponent.currentRow)
                    continue;

                string t;
                if (r == textComponent.currentRow)
                    t = line.Substring(0, Math.Min(textComponent.currentCol, line.Length));
                else
                    t = line;

                Globals.spriteBatch.DrawString(
                    Globals.fontSmall,
                    t,
                    new Vector2(
                        transformComponent.position.X - transformComponent.size.X / 2 + textComponent.outerMargin,
                        transformComponent.position.Y - transformComponent.size.Y / 2 - rowHeight - (textComponent.outerMargin)
                    ), textComponent.textColour
                );
                rowHeight -= textComponent.singleRowheight;
                r += 1;
            }

            textComponent.timer += 1;
            if (textComponent.timer >= textComponent.delay)
            {
                textComponent.timer = 0;
                textComponent.currentCol += 1;
                if (textComponent.currentCol >= textComponent.splitText[textComponent.currentRow].Length)
                {
                    
                    if (textComponent.currentRow < textComponent.splitText.Count - 1)
                    {
                        textComponent.currentCol = 0;
                        textComponent.currentRow += 1;
                    }
                }
            }

        }
    }
}
