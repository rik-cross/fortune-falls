using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

using System;
using System.Collections.Generic;
using S = System.Diagnostics.Debug;

namespace AdventureGame.Engine
{
    public class DialogueSystem : System
    {
        public DialogueSystem()
        {
            RequiredComponent<DialogueComponent>();
        }

        public void DialogueInputController(Entity e)
        {
            Engine.PlayerControlComponent controlComponent = e.GetComponent<Engine.PlayerControlComponent>();
            Engine.DialogueComponent dc = e.GetComponent<Engine.DialogueComponent>();
            if(controlComponent != null && dc != null)
            {
                if(EngineGlobals.inputManager.IsPressed(controlComponent.Get("skip"))
                    && dc.dialoguePages.Count > 0)
                {
                    // if dialogue not yet finished then skip to the end
                    if(dc.dialoguePages[0].index < dc.dialoguePages[0].text.Length)
                    {
                        dc.dialoguePages[0].index = dc.dialoguePages[0].text.Length;
                    }
                    // if dialogue at end then remove the page
                    else
                    {
                        dc.RemovePage();
                    }
                }
            }
        }

        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            DialogueComponent dialogueComponent = entity.GetComponent<DialogueComponent>();

            // ensure that the top inputController is for dialogue
            Engine.InputComponent ic = entity.GetComponent<InputComponent>();
            if (ic != null && dialogueComponent.dialoguePages.Count > 0 && ic.TopControllerLabel != "dialogue")
            {
                if (entity.GetComponent<IntentionComponent>() != null)
                    entity.GetComponent<IntentionComponent>().ResetAll();
                if (entity.State.Split("_").Length == 2 && (entity.State.Split("_")[0] == "walk" || entity.State.Split("_")[0] == "run"))
                    entity.State = "idle_" + entity.State.Split("_")[1];
                ic.PushController(DialogueInputController);
                ic.TopControllerLabel = "dialogue";
            }

            dialogueComponent.alpha.Update();

            if (dialogueComponent.dialoguePages.Count == 0)
                return;

            dialogueComponent.dialoguePages[0].Update();

            if (dialogueComponent.dialoguePages[0].markForRemoval && dialogueComponent.dialoguePages[0].textAlpha.Value == 0)
            {
                // To do: Trigger any actions at the end of a page
                //if (dialogueComponent.dialoguePages[0].onDialogueComplete != null)
                //    triggerComponent.onCollide(entity, e, distance);

                if (dialogueComponent.dialoguePages[0].onDialogueComplete != null)
                {
                    dialogueComponent.dialoguePages[0].onDialogueComplete(entity);
                }

                if (dialogueComponent.dialoguePages.Count > 1 && dialogueComponent.dialoguePages[0].texture == dialogueComponent.dialoguePages[1].texture)
                {
                    //dialogueComponent.dialoguePages[0].imageAlpha.Set(1);
                    dialogueComponent.dialoguePages[1].imageAlpha.Set(1);
                }
                    
                // remove the current dialogue page
                dialogueComponent.dialoguePages.RemoveAt(0);

                // load the next dialogue page
                if (dialogueComponent.dialoguePages.Count > 0)
                {
                    // execute script if there is one
                    if (dialogueComponent.dialoguePages[0].script != null)
                        dialogueComponent.dialoguePages[0].script();
                    // show dialogue
                    // I'm not sure this is needed. They always start at 1.
                    //dialogueComponent.dialoguePages[0].imageAlpha.Value = 1;
                    //dialogueComponent.dialoguePages[0].textAlpha.Value = 1;
                }
            }

            if (dialogueComponent.dialoguePages.Count == 0)
            {
                if (ic.TopControllerLabel == "dialogue" && ic.InputControllerStack.Count > 0)
                {
                    ic.PopController();
                }
            }

            if (dialogueComponent.dialoguePages.Count == 0)
                return;
            if (dialogueComponent.dialoguePages[0].entity == null)
                return;
            if (entity.GetComponent<SpriteComponent>() == null)
                return;

            // below for dialogue linked to an entity only

            SpriteComponent spriteComponent = entity.GetComponent<SpriteComponent>();
            dialogueComponent.dialoguePages[0].texture = spriteComponent.GetSprite(entity.State).GetCurrentTexture();
        }

        public override void Draw(GameTime gameTime, Scene scene)
        {
            foreach (Entity entity in EntityList)
            {
                DialogueComponent dialogueComponent = entity.GetComponent<DialogueComponent>();

                foreach (Camera camera in scene.CameraList)
                {
                    if (camera.ownerEntity == entity)
                    {
                        //
                        // Draw background
                        //

                        UI.DrawRect(
                            camera.screenPosition.X + Theme.BorderLarge,
                            camera.screenPosition.Y + camera.size.Y - Theme.BorderLarge - 200,
                            camera.size.X - (2 * Theme.BorderLarge),
                            200,
                            (float)dialogueComponent.alpha.Value
                        );

                        // Don't draw anything else if there aren't any pages of dialogue
                        if (dialogueComponent.dialoguePages.Count == 0)
                            continue;

                        //
                        // Draw text
                        //

                        // Offset the text to appear to the right of an image if there is one
                        int xOffset = 0;
                        if (dialogueComponent.dialoguePages[0].texture != null)
                            xOffset += 200;

                        // Split the text into multiple lines no wider than the containing box

                        List<string> splitText = new List<string>();
                        string words = dialogueComponent.dialoguePages[0].text;
                        string currentRow = "";

                        // Calculate space available for text
                        int availableWidth = (int)(camera.size.X - (4 * Theme.BorderLarge));
                        // Less space available if there's an image
                        if (dialogueComponent.dialoguePages[0].texture != null)
                            availableWidth -= (200 + (Theme.BorderLarge * 2));

                        foreach (string word in words.Split())
                        {
                            if (Theme.FontPrimary.MeasureString(currentRow).X + Theme.FontPrimary.MeasureString(word).X < availableWidth)
                            {
                                if (currentRow == "")
                                    currentRow += word;
                                else
                                    currentRow += " " + word;

                            }
                            else
                            {
                                splitText.Add(currentRow);
                                currentRow = word;
                            }
                        }

                        if (currentRow != "")
                            splitText.Add(currentRow);

                        // Draw

                        int y = (int)(camera.screenPosition.Y + camera.size.Y - 200);

                        int ind = dialogueComponent.dialoguePages[0].index;
                        int acc = 0;

                        foreach (string line in splitText)
                        {

                            int s = 0;
                            int e = Math.Clamp(ind - acc, 0, line.Length);

                            Globals.spriteBatch.DrawString(Theme.FontPrimary,
                                line.Substring(s, e),
                                new Vector2(camera.screenPosition.X + (Theme.BorderLarge * 2) + xOffset,
                                    y),
                                Color.White * (float)dialogueComponent.dialoguePages[0].textAlpha.Value);

                            y += (int)(Theme.FontPrimary.MeasureString(currentRow).Y / 4 * 3);
                            acc += line.Length;
                        }

                        //
                        // Draw an image if one exists
                        //

                        if (dialogueComponent.dialoguePages[0].texture != null) {
                                                        
                            // Calculate image size

                            Texture2D t = dialogueComponent.dialoguePages[0].texture;
                            float requiredSize = (200 - (4 * Theme.BorderLarge));
                            Vector2 newSize = new Vector2();
                            Vector2 padding = new Vector2();

                            if (t.Height > t.Width)
                            {
                                float factor = requiredSize / t.Height;
                                newSize.X = t.Width * factor;
                                newSize.Y = t.Height * factor;
                                padding.X = (requiredSize - newSize.X) / 2;
                                padding.Y = (requiredSize - newSize.Y) / 2;
                            }
                            else
                            {
                                float factor = requiredSize / t.Width;
                                newSize.X = t.Width * factor;
                                newSize.Y = t.Height * factor;
                                padding.X = (requiredSize - newSize.X) / 2;
                                padding.Y = (requiredSize - newSize.Y) / 2;
                            }

                            // Draw the image

                            Globals.spriteBatch.Draw(t,
                                new Rectangle(
                                    (int)(camera.screenPosition.X + (3 * Theme.BorderLarge) + padding.X),
                                    (int)(camera.screenPosition.Y + camera.size.Y - 200 + Theme.BorderLarge + padding.Y),
                                    (int)(newSize.X + Theme.BorderLarge),
                                    (int)(newSize.Y)), Color.White * (float)dialogueComponent.dialoguePages[0].imageAlpha.Value);

                            // draw button image

                            Texture2D button;
                            if (EngineGlobals.inputManager.IsControllerConnected())
                                button = GameAssets.button_a;
                            else
                                button = GameAssets.enter;

                            Globals.spriteBatch.Draw(button,
                                new Rectangle(
                                    (int)(camera.screenPosition.X + camera.size.X - (2 * Theme.BorderLarge) - Theme.BorderSmall - button.Width*3),
                                    (int)(camera.screenPosition.Y + camera.size.Y - (2 * Theme.BorderLarge) - Theme.BorderSmall - button.Height*3),
                                    button.Width * 3,
                                    button.Height * 3
                                ),
                                Color.White * (float)dialogueComponent.alpha.Value);
                            
                        }

                    }
                }
            }
        }
    }
}
