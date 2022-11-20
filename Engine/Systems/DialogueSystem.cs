using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using MonoGame.Extended;

using S = System.Diagnostics.Debug;

namespace AdventureGame.Engine
{
    public class DialogueSystem : System
    {
        public DialogueSystem()
        {
            RequiredComponent<DialogueComponent>();
        }
        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {

            DialogueComponent dialogueComponent = entity.GetComponent<DialogueComponent>();

            if (dialogueComponent.dialoguePages.Count == 0)
                return;
            if (dialogueComponent.dialoguePages[0].entity == null)
                return;
            if (entity.GetComponent<SpriteComponent>() == null)
                return;

            SpriteComponent spriteComponent = entity.GetComponent<SpriteComponent>();
            dialogueComponent.dialoguePages[0].texture = spriteComponent.GetSprite(entity.State).GetCurrentTexture();
        }
        public override void Draw(GameTime gameTime, Scene scene)
        {
            foreach (Entity entity in scene.EntityList)
            {

                if (entity.GetComponent<DialogueComponent>() == null)
                    continue;

                DialogueComponent dialogueComponent = entity.GetComponent<DialogueComponent>();
                if (dialogueComponent.dialoguePages.Count == 0)
                    continue;

                foreach (Camera camera in scene.CameraList)
                {
                    if (camera.ownerEntity == entity)
                    {
                        // Draw background
                        Globals.spriteBatch.FillRectangle(
                            new Rectangle(
                                (int)(camera.screenPosition.X + Theme.BorderLarge),
                                (int)(camera.screenPosition.Y + camera.size.Y - Theme.BorderLarge - 200),
                                (int)(camera.size.X - (2 * Theme.BorderLarge)),
                                (int)(200)), Theme.ColorTertiary);

                        // Draw text

                        // Text only
                        if (dialogueComponent.dialoguePages[0].texture == null)
                        {
                            Globals.spriteBatch.DrawString(Theme.FontPrimary,
                                dialogueComponent.dialoguePages[0].text,
                                new Vector2(camera.screenPosition.X + (Theme.BorderLarge*2),
                                    camera.screenPosition.Y + camera.size.Y - 200),
                                Theme.ColorPrimary);
                        } else
                        // Text -- including a texture
                        {

                            // Image background
                            Globals.spriteBatch.DrawRectangle(
                            new Rectangle(
                                (int)(camera.screenPosition.X + (2*Theme.BorderLarge)),
                                (int)(camera.screenPosition.Y + camera.size.Y - 200),
                                (int)(200 - Theme.BorderLarge),
                                (int)(200 - (2*Theme.BorderLarge))), Theme.ColorPrimary, thickness:Theme.BorderLarge);

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
                            } else
                            {
                                float factor = requiredSize / t.Width;
                                newSize.X = t.Width * factor;
                                newSize.Y = t.Height * factor;
                                padding.X = (requiredSize - newSize.X) / 2;
                                padding.Y = (requiredSize - newSize.Y) / 2;
                            }

                            Globals.spriteBatch.Draw(t,
                                new Rectangle(
                                    (int)(camera.screenPosition.X + (3 * Theme.BorderLarge) + padding.X),
                                    (int)(camera.screenPosition.Y + camera.size.Y - 200 + Theme.BorderLarge + padding.Y),
                                    (int)(newSize.X + Theme.BorderLarge),
                                    (int)(newSize.Y)), Color.White);

                            Globals.spriteBatch.DrawString(Theme.FontPrimary,
                                dialogueComponent.dialoguePages[0].text,
                                new Vector2(camera.screenPosition.X + (Theme.BorderLarge * 2) + 200,
                                    camera.screenPosition.Y + camera.size.Y - 200),
                                Theme.ColorPrimary);
                        }
                    }
                }
            }
        }
    }
}
