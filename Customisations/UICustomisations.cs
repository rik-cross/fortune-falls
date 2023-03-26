using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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

        public static SoundEffect soundEffect = Globals.content.Load<SoundEffect>("Sounds/pickupCoin");

        public static Texture2D full = Globals.content.Load<Texture2D>("full");
        public static Texture2D window = Globals.content.Load<Texture2D>("window");

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

        public static void DrawSlider(Engine.UISlider button)
        {

            //S.WriteLine(EngineGlobals.sceneManager._sceneStack[0].frame);
            int currentActiveBorder = activeBorder;
            if (EngineGlobals.sceneManager.ActiveScene != null)
                currentActiveBorder = (int)(activeBorder + Math.Sin(EngineGlobals.sceneManager.ActiveScene.frame / 10) * 4);
            //S.WriteLine(c);

            // Draw background image
            //Globals.spriteBatch.DrawRectangle(button.position, button.size, button.outlineColour, button.outlineThickness);
            Globals.spriteBatch.Draw(labelLeft, new Rectangle((int)button.position.X, (int)button.position.Y, 12, (int)button.size.Y), Color.White);
            Globals.spriteBatch.Draw(labelMiddle, new Rectangle((int)button.position.X + 12, (int)button.position.Y, (int)button.size.X - (2 * 12), (int)button.size.Y), Color.White);
            Globals.spriteBatch.Draw(labelRight, new Rectangle((int)(button.position.X + button.size.X - 12), (int)button.position.Y, 12, (int)button.size.Y), Color.White);
            
            // Draw value
            float t = button.size.X - 6 * 2;
            float p = (float)(button.currentValue / (button.maxValue - button.minValue) * 100);
            Globals.spriteBatch.FillRectangle(new Vector2(button.position.X + 6, button.position.Y + 6), new Vector2(t / 100 * p, button.size.Y - 6 * 2), button.onColour);
            Globals.spriteBatch.FillRectangle(new Vector2(button.position.X + 6 + (t / 100 * p), button.position.Y + 6), new Vector2((t) - (t / 100 * p), button.size.Y - 6 * 2), button.offColour);

            // Draw text
            Globals.spriteBatch.DrawString(button.font, button.text, new Vector2(button.position.X + button.textOffset.X, button.position.Y + button.textOffset.Y + 1), button.textColour);
            // Draw arrows
            if (button.currentValue > button.minValue)
                Globals.spriteBatch.DrawString(button.font, "<", new Vector2((button.position.X + 10), button.position.Y + button.textOffset.Y), button.textColour);
            if (button.currentValue < button.maxValue)
                Globals.spriteBatch.DrawString(button.font, ">", new Vector2((button.position.X + button.size.X - (button.font.MeasureString(">").X) - 10), button.position.Y + button.textOffset.Y), button.textColour);

            // Draw highlight if active
            if (button.active)
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

        public static void DrawControlSlider(Engine.UISlider button)
        {

            //S.WriteLine(EngineGlobals.sceneManager._sceneStack[0].frame);
            int currentActiveBorder = activeBorder;
            if (EngineGlobals.sceneManager.ActiveScene != null)
                currentActiveBorder = (int)(activeBorder + Math.Sin(EngineGlobals.sceneManager.ActiveScene.frame / 10) * 4);
            //S.WriteLine(c);

            // Draw background image
            //Globals.spriteBatch.DrawRectangle(button.position, button.size, button.outlineColour, button.outlineThickness);
            Globals.spriteBatch.Draw(labelLeft, new Rectangle((int)button.position.X, (int)button.position.Y, 12, (int)button.size.Y), Color.White);
            Globals.spriteBatch.Draw(labelMiddle, new Rectangle((int)button.position.X + 12, (int)button.position.Y, (int)button.size.X - (2 * 12), (int)button.size.Y), Color.White);
            Globals.spriteBatch.Draw(labelRight, new Rectangle((int)(button.position.X + button.size.X - 12), (int)button.position.Y, 12, (int)button.size.Y), Color.White);

            // Draw text
            button.text = "Keys";
            if (button.currentValue == 1)
                button.text = "Con";
            button.Init();
            Globals.spriteBatch.DrawString(button.font, button.text, new Vector2(button.position.X + button.textOffset.X, button.position.Y + button.textOffset.Y + 1), button.textColour);

            // Draw arrows
            if (button.currentValue > button.minValue)
                Globals.spriteBatch.DrawString(button.font, "<", new Vector2((button.position.X + 10), button.position.Y + button.textOffset.Y), button.textColour);
            if (EngineGlobals.inputManager.IsControllerConnected() && button.currentValue < button.maxValue)
                Globals.spriteBatch.DrawString(button.font, ">", new Vector2((button.position.X + button.size.X - (button.font.MeasureString(">").X) - 10), button.position.Y + button.textOffset.Y), button.textColour);

            // Draw highlight if active
            if (button.active)
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

        public static void SetMusicVolume(UISlider button, double newVolume)
        {
            EngineGlobals.soundManager.Volume = (float)newVolume;
        }

        public static void SetSFXVolume(UISlider button, double newVolume)
        {
            bool changed = EngineGlobals.soundManager.SFXVolume != newVolume;
            EngineGlobals.soundManager.SFXVolume = (float)newVolume;
            if (changed)
                EngineGlobals.soundManager.PlaySoundEffect(soundEffect);
        }

        public static void SetControls(UISlider button, double value)
        {
            if (EngineGlobals.inputManager.IsControllerConnected() && value == 1)
            {
                EngineGlobals.entityManager.GetLocalPlayer().GetComponent<InputComponent>().input = Engine.Inputs.controller;
            }
            else
            {
                button.currentValue = 0;
                EngineGlobals.entityManager.GetLocalPlayer().GetComponent<InputComponent>().input = Engine.Inputs.keyboard;
            }
         }

    }
}
