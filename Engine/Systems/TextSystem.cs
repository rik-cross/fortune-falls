using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using S = System.Diagnostics.Debug;
using System.Reflection;

using System.Collections;
using System.Collections.Generic;

namespace AdventureGame.Engine
{
    public class TextSystem : System
    {
        public TextSystem()
        {
            RequiredComponent<TextComponent>();
            RequiredComponent<TransformComponent>();
            aboveMap = true;
        }

        public override void DrawEntity(GameTime gameTime, Scene scene, Entity entity)
        {

            TextComponent textComponent = entity.GetComponent<TextComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            int rowHeight = textComponent.totalHeight;

            // draw the background rectangle
            
            float a;
            if (textComponent.type == "show")
                a = 1.0f;
            else
                a = textComponent.frame * 0.02f;

            Globals.spriteBatch.FillRectangle(
                new Rectangle(
                    (int)( (transformComponent.position.X + (transformComponent.size.X/2)) - ((textComponent.textWidth + textComponent.outerMargin * 2)/2) ),
                    (int)(transformComponent.position.Y - transformComponent.size.Y / 2 - textComponent.totalHeight - (textComponent.outerMargin*2)) + 10,
                    textComponent.textWidth + textComponent.outerMargin * 2,
                    textComponent.totalHeight + textComponent.outerMargin * 2
                ),
                textComponent.backgroundColour * a
            );

            // draw the border rectangle

            float aa;
            if (textComponent.type == "show")
                aa = 1.0f;
            else
                aa = textComponent.frame * 0.02f;

            Globals.spriteBatch.DrawRectangle(
                new Rectangle(
                    (int)((transformComponent.position.X + (transformComponent.size.X/2)) - ((textComponent.textWidth + textComponent.outerMargin * 2) / 2)),
                    (int)(transformComponent.position.Y - transformComponent.size.Y / 2 - textComponent.totalHeight - (textComponent.outerMargin*2)) + 10,
                    textComponent.textWidth + textComponent.outerMargin * 2,
                    textComponent.totalHeight + textComponent.outerMargin * 2
                ),
                textComponent.textColour * aa
            );

            //
            // draw the text
            //

            float aaa;
            if (textComponent.type == "show")
                aaa = 1.0f;
            else
                aaa = textComponent.frame * 0.02f;

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
                        Theme.secondaryFont,
                        t,
                        new Vector2(
                            ((transformComponent.position.X + (transformComponent.size.X/2)) - ((textComponent.textWidth + textComponent.outerMargin * 2) / 2)) + textComponent.outerMargin,
                            transformComponent.position.Y - transformComponent.size.Y / 2 - rowHeight - (textComponent.outerMargin) + 10
                        ), textComponent.textColour * aaa
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
                        Theme.secondaryFont,
                        line,
                        new Vector2(
                            transformComponent.position.X - transformComponent.size.X / 2 + textComponent.outerMargin,
                            transformComponent.position.Y - transformComponent.size.Y / 2 - rowHeight - (textComponent.outerMargin)
                        ), textComponent.textColour * (textComponent.frame * 0.02f)
                    );
                    rowHeight -= textComponent.singleRowheight;
                }
            }

            if (textComponent.type == "show")
            {
                foreach (string line in textComponent.text)
                {
                    Globals.spriteBatch.DrawString(
                        Theme.secondaryFont,
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
            if (textComponent.type == "fade" && textComponent.frame < 255)
            {
                textComponent.timer += 1;
            }

            if (!textComponent.finished)
                textComponent.frame += 1;
            else
            {
                if (!textComponent.requiresPress) {
                    if (textComponent.outTimer < textComponent.outTimerLimit)
                    {
                        textComponent.outTimer += 1;
                    }
                    else
                    {
                        textComponent.frame -= 1;
                        if (textComponent.frame <= 0)
                            entity.RemoveComponent<TextComponent>();
                    }
                }
                else
                {
                    InputComponent inputComponent = entity.GetComponent<InputComponent>();
                    if (inputComponent != null)
                    {

                        Type w = inputComponent.input.GetType();
                        FieldInfo x = w.GetField(textComponent.input);
                        var y = x.GetValue(inputComponent.input);
                        InputItem z = (InputItem)y;

                        if (EngineGlobals.inputManager.IsPressed(z))
                        {
                            textComponent.requiresPress = false;
                            textComponent.outTimerLimit = 0;
                        }

                    }
                    
                }
            }
                

            // switch to fade out if appropriate

            if (!textComponent.finished) {
                if (textComponent.type == "fade" && textComponent.frame > (1 / 0.02) )
                {
                    textComponent.finished = true;
                    textComponent.frame = 50;
                }
                if (textComponent.type == "tick" &&
                    textComponent.currentCol == textComponent.text[textComponent.currentRow].Length + 1 && 
                    textComponent.currentRow == textComponent.text.Count - 1)
                {
                    textComponent.finished = true;
                    textComponent.frame = 50;
                }
            }

        }
    }
}
