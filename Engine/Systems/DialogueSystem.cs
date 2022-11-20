using Microsoft.Xna.Framework;
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
                        Globals.spriteBatch.FillRectangle(
                            new Rectangle(
                                (int)(camera.screenPosition.X + Theme.BorderLarge),
                                (int)(camera.screenPosition.Y + camera.size.Y - Theme.BorderLarge - 200),
                                (int)(camera.size.X - (2 * Theme.BorderLarge)),
                                (int)(200)), Theme.ColorTertiary);

                        Globals.spriteBatch.DrawString(Theme.FontPrimary,
                            dialogueComponent.dialoguePages[0],
                            new Vector2(camera.screenPosition.X + Theme.BorderLarge,
                                camera.screenPosition.Y + camera.size.Y - Theme.BorderLarge - 200),
                            Theme.ColorPrimary);
                    }
                }
            }
        }
    }
}
