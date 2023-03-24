using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

using AdventureGame.Engine;

using S = System.Diagnostics.Debug;

namespace AdventureGame
{
    public static class UICustomisations
    {

        public static Texture2D selectTopLeft = Globals.content.Load<Texture2D>("UI/selectbox_tl");
        public static Texture2D selectTopRight = Globals.content.Load<Texture2D>("UI/selectbox_tr");
        public static Texture2D selectBottomLeft = Globals.content.Load<Texture2D>("UI/selectbox_bl");
        public static Texture2D selectBottomRight = Globals.content.Load<Texture2D>("UI/selectbox_br");

        public static Texture2D labelLeft = Globals.content.Load<Texture2D>("UI/label_left");
        public static Texture2D labelMiddle = Globals.content.Load<Texture2D>("UI/label_middle");
        public static Texture2D labelRight = Globals.content.Load<Texture2D>("UI/label_right");

        public static int activeBorder = 10;

        public static void DrawButton(Engine.UIButton button)
        {

            //S.WriteLine(EngineGlobals.sceneManager._sceneStack[0].frame);
            int currentActiveBorder = activeBorder;
            if (EngineGlobals.sceneManager.ActiveScene != null) 
                currentActiveBorder = (int)(activeBorder + Math.Sin(EngineGlobals.sceneManager.ActiveScene.frame / 10) * 4);
            //S.WriteLine(c);

            // Draw background image
            //Globals.spriteBatch.DrawRectangle(button.position, button.size, button.outlineColour, button.outlineThickness);
            Globals.spriteBatch.Draw(labelLeft, new Rectangle((int)button.position.X, (int)button.position.Y, 12, (int)button.size.Y), Color.White);
            Globals.spriteBatch.Draw(labelMiddle, new Rectangle((int)button.position.X + 12, (int)button.position.Y, (int)button.size.X-(2*12), (int)button.size.Y), Color.White);
            Globals.spriteBatch.Draw(labelRight, new Rectangle((int)(button.position.X + button.size.X - 12), (int)button.position.Y, 12, (int)button.size.Y), Color.White);
            // Draw text
            Globals.spriteBatch.DrawString(button.font, button.text, new Vector2(button.position.X + button.textOffset.X, button.position.Y + button.textOffset.Y + 1), button.textColour);
            // Draw highlight if active
            if(button.active)
            {
                // Top left
                Globals.spriteBatch.Draw(selectTopLeft, new Rectangle((int)button.position.X - currentActiveBorder, (int)button.position.Y - currentActiveBorder, 20, 20), Color.White);
                // Top Right
                Globals.spriteBatch.Draw(selectTopRight, new Rectangle((int)(button.position.X + button.size.X - 20 + currentActiveBorder), (int)button.position.Y - currentActiveBorder, 20, 20), Color.White);
                // Bottom left
                Globals.spriteBatch.Draw(selectBottomLeft, new Rectangle((int)button.position.X - currentActiveBorder, (int)(button.position.Y + button.size.Y - 20 + currentActiveBorder), 20, 20), Color.White);
                // Bottom right
                Globals.spriteBatch.Draw(selectBottomRight, new Rectangle((int)(button.position.X + button.size.X - 20 + currentActiveBorder), (int)(button.position.Y + button.size.Y - 20 + currentActiveBorder), 20, 20), Color.White);
            }
        }

    }
}
