using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;

namespace Engine
{
    public class ClickableSystem : System
    {
        public ClickableSystem()
        {
            RequiredComponent<ClickableComponent>();
            RequiredComponent<TransformComponent>();
        }

        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();
            Rectangle clickableArea = transformComponent.GetRectangle();
            /*
            // Check if the mouse position is inside the rectangle
            if (EngineGlobals.inputManager.IsPressed(Globals.primaryCursorInput))
            {
                if (clickableArea.Contains(EngineGlobals.inputManager.CursorPosition))
                {
                    Console.WriteLine("Inside area");
                }
                else
                {
                    Console.WriteLine("Outside area");
                }
            }*/
        }

        public override void Draw(GameTime gameTime, Scene scene)
        {
            // Draw the cursor image here??
            // Issue: currently draws the cursor for both screens and below InventoryScene
            /*
            if (EngineGlobals.inputManager.IsCursorVisible)
            {
                Engine.Image2 cursor = new Engine.Image2(
                    texture: EngineGlobals.inputManager.CursorTexture,
                    position: EngineGlobals.inputManager.CursorPosition
                );
                cursor.Draw();
            }*/
        }

    }
}
