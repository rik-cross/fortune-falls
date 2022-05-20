using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace AdventureGame.Engine
{
    public class TextSystem : System
    {
        public TextSystem()
        {
            RequiredComponent<TextComponent>();
            RequiredComponent<TransformComponent>();
        }

        public override void DrawEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            TextComponent textComponent = entity.GetComponent<TextComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            int rowHeight = textComponent.totalHeight;

            // draw the background rectangle
            Globals.spriteBatch.FillRectangle(
                new Rectangle(
                    (int)(transformComponent.position.X - transformComponent.size.X / 2),
                    (int)(transformComponent.position.Y - transformComponent.size.Y / 2 - textComponent.totalHeight - (textComponent.outerMargin*2)),
                    textComponent.textWidth + textComponent.outerMargin * 2,
                    textComponent.totalHeight + textComponent.outerMargin * 2
                ),
                textComponent.backgroundColour
            );

            // draw the border rectangle
            Globals.spriteBatch.DrawRectangle(
                new Rectangle(
                    (int)(transformComponent.position.X - transformComponent.size.X / 2),
                    (int)(transformComponent.position.Y - transformComponent.size.Y / 2 - textComponent.totalHeight - (textComponent.outerMargin*2)),
                    textComponent.textWidth + textComponent.outerMargin * 2,
                    textComponent.totalHeight + textComponent.outerMargin * 2
                ),
                textComponent.textColour
            );

            //
            // draw the text
            //

            if (textComponent.type == "tick") {
                int r = 0;
                foreach (string line in textComponent.text) {

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
            }

            if (textComponent.type == "fade")
            {
                foreach (string line in textComponent.text)
                {
                    Globals.spriteBatch.DrawString(
                        Globals.fontSmall,
                        line,
                        new Vector2(
                            transformComponent.position.X - transformComponent.size.X / 2 + textComponent.outerMargin,
                            transformComponent.position.Y - transformComponent.size.Y / 2 - rowHeight - (textComponent.outerMargin)
                        ), new Color((int)textComponent.textColour.R, (int)textComponent.textColour.G, (int)textComponent.textColour.B, textComponent.timer)
                    );
                    rowHeight -= textComponent.singleRowheight;
                }
            }

            if (textComponent.type == "show")
            {
                foreach (string line in textComponent.text)
                {
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

            // update the timer (if needed)
            if (textComponent.type == "tick") {
                textComponent.timer += 1;
                if (textComponent.timer >= textComponent.delay)
                {
                    textComponent.timer = 0;
                    textComponent.currentCol += 1;
                    if (textComponent.currentCol >= textComponent.text[textComponent.currentRow].Length)
                    {

                        if (textComponent.currentRow < textComponent.text.Count - 1)
                        {
                            textComponent.currentCol = 0;
                            textComponent.currentRow += 1;
                        }
                    }
                }
            }
            if (textComponent.type == "fade" && textComponent.timer < 255)
            {
                textComponent.timer += 1;
            }
        }
    }
}
