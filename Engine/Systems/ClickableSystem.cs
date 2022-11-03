using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;

namespace AdventureGame.Engine
{
    public class ClickableSystem : System
    {
        //Texture2D cursorTexture;
        //Vector2 cursorPosition;
        // bool IsCursorVisible; // here or Globals??

        public ClickableSystem()
        {
            RequiredComponent<ClickableComponent>();
            //RequiredComponent<InputComponent>();
            RequiredComponent<TransformComponent>();

            //cursorTexture = Globals.content.Load<Texture2D>("cursor");
            //Inputs input = EngineGlobals.entityManager.GetLocalPlayer().GetComponent<InputComponent>().input;
            // Engine.Inputs.controller

            //InputComponent inputComponent = EngineGlobals.entityManager.GetLocalPlayer().GetComponent<InputComponent>();
            //InputMethod input = EngineGlobals.entityManager.GetLocalPlayer().GetComponent<InputComponent>().input;
        }

        public override void Update(GameTime gameTime, Scene scene)
        {
            //InputComponent input = GetComponent<InputComponent>().input = Engine.Inputs.controller;

            // CHECK whether mouse OR controller is set up??

            //MouseState mouseState = Mouse.GetState();
            //cursorPosition = new Vector2(mouseState.X, mouseState.Y);
        }

        public override void UpdateEntity(GameTime gameTime, Scene scene, Entity entity)
        {
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();
            
            // if (mouse != null)

            // if (controller != null)

            Rectangle clickableArea = transformComponent.GetRectangle();

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
            }
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
