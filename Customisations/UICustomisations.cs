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

        public static Texture2D selectTopLeft = Utils.LoadTexture("UI/selectbox_tl.png");
        public static Texture2D selectTopRight = Utils.LoadTexture("UI/selectbox_tr.png");
        public static Texture2D selectBottomLeft = Utils.LoadTexture("UI/selectbox_bl.png");
        public static Texture2D selectBottomRight = Utils.LoadTexture("UI/selectbox_br.png");

        public static Texture2D labelLeft = Utils.LoadTexture("UI/label_left.png");
        public static Texture2D labelMiddle = Utils.LoadTexture("UI/label_middle.png");
        public static Texture2D labelRight = Utils.LoadTexture("UI/label_right.png");

        public static SoundEffect soundEffect = Utils.LoadSoundEffect("Sounds/pickupCoin.wav");

        public static Texture2D full = Utils.LoadTexture("UI/full.png");
        public static Texture2D window = Utils.LoadTexture("UI/window.png");

        public static int activeBorder = 10;

        public static void DrawAnimatedEmote(Scene scene, Entity entity)
        {
            AnimatedEmoteComponent animatedEmoteComponent = entity.GetComponent<AnimatedEmoteComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            // calculate bottom-middle of component
            Vector2 playerTopMiddle = new Vector2(
                transformComponent.Position.X + (transformComponent.Size.X / 2),
                transformComponent.Position.Y
            );

            // draw border
            UI.DrawRect(
                (int)(playerTopMiddle.X - (animatedEmoteComponent.backgroundSize.X / 2)),
                (int)(playerTopMiddle.Y - animatedEmoteComponent.backgroundSize.Y - Theme.BorderSmall),
                animatedEmoteComponent.backgroundSize.X,
                animatedEmoteComponent.backgroundSize.Y,
                (float)animatedEmoteComponent.alpha.Value,
                borderWidth: 2
            );

            // draw image
            Globals.spriteBatch.Draw(
                animatedEmoteComponent._textures[animatedEmoteComponent._currentIndex],
                new Rectangle(
                    (int)(playerTopMiddle.X - (animatedEmoteComponent.textureSize.X / 2)),
                    (int)(playerTopMiddle.Y - animatedEmoteComponent.textureSize.Y - Theme.BorderSmall * 2),
                    (int)animatedEmoteComponent.textureSize.X,
                    (int)animatedEmoteComponent.textureSize.Y
                ),
                Color.White * (float)animatedEmoteComponent.alpha.Value
            );

        }

        public static void DrawEmote(Scene scene, Entity entity)
        {

            EmoteComponent emoteComponent = entity.GetComponent<EmoteComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            // calculate bottom-middle of component
            Vector2 playerTopMiddle = new Vector2(
                transformComponent.Position.X + (transformComponent.Size.X / 2),
                transformComponent.Position.Y
            );

            // draw border
            UI.DrawRect( 100, 100, 100, 100, 1, 2
                //(int)(playerTopMiddle.X - (emoteComponent.emoteBackground.X / 2)),
                //(int)(playerTopMiddle.Y - emoteComponent.emoteBackground.Y - Theme.BorderSmall),
                //emoteComponent.emoteBackground.X,
                //emoteComponent.emoteBackground.Y,
                //(float)emoteComponent.alpha.Value,
                //borderWidth: 2
            );

            // draw image
            /*Globals.spriteBatch.Draw(
                emoteComponent._textures[emoteComponent._currentIndex],
                new Rectangle(
                    (int)(playerTopMiddle.X - (emoteComponent.textureSize.X / 2)),
                    (int)(playerTopMiddle.Y - emoteComponent.textureSize.Y - Theme.BorderSmall * 2),
                    (int)emoteComponent.textureSize.X,
                    (int)emoteComponent.textureSize.Y
                ),
                Color.White * (float)emoteComponent.alpha.Value
            );*/

        }

        public static void DrawButton(Engine.UIButton button)
        {

            float a = 1.0f;
            if (!button.active)
                a = 0.5f;

            //S.WriteLine(EngineGlobals.sceneManager._sceneStack[0].frame);
            int currentActiveBorder = activeBorder;
            if (EngineGlobals.sceneManager.ActiveScene != null) 
                currentActiveBorder = (int)(activeBorder + Math.Sin(EngineGlobals.sceneManager.ActiveScene.frame / 10) * 4);
            
            // Draw background image
            UI.DrawRect(button.position.X, button.position.Y, button.size.X, button.size.Y, a);
            
            // Draw text
            Globals.spriteBatch.DrawString(button.font, button.text, new Vector2(button.position.X + button.textOffset.X, button.position.Y + button.textOffset.Y + 1), button.textColour * a);
            // Draw highlight if active
            if(button.selected)
            {
                // Top left
                Globals.spriteBatch.Draw(selectTopLeft, new Rectangle((int)button.position.X - currentActiveBorder, (int)button.position.Y - currentActiveBorder, 20, 20), Color.White * a);
                // Top Right
                Globals.spriteBatch.Draw(selectTopRight, new Rectangle((int)(button.position.X + button.size.X - 20 + currentActiveBorder), (int)button.position.Y - currentActiveBorder, 20, 20), Color.White*a);
                // Bottom left
                Globals.spriteBatch.Draw(selectBottomLeft, new Rectangle((int)button.position.X - currentActiveBorder, (int)(button.position.Y + button.size.Y - 20 + currentActiveBorder), 20, 20), Color.White*a);
                // Bottom right
                Globals.spriteBatch.Draw(selectBottomRight, new Rectangle((int)(button.position.X + button.size.X - 20 + currentActiveBorder), (int)(button.position.Y + button.size.Y - 20 + currentActiveBorder), 20, 20), Color.White*a);
            }
        }

        public static void DrawSlider(Engine.UISlider button)
        {

            float a = 1.0f;
            if (!button.active)
                a = 0.5f;

            //S.WriteLine(EngineGlobals.sceneManager._sceneStack[0].frame);
            int currentActiveBorder = activeBorder;
            if (EngineGlobals.sceneManager.ActiveScene != null)
                currentActiveBorder = (int)(activeBorder + Math.Sin(EngineGlobals.sceneManager.ActiveScene.frame / 10) * 4);
            //S.WriteLine(c);

            // Draw background image
            //Globals.spriteBatch.DrawRectangle(button.position, button.size, button.outlineColour, button.outlineThickness);
            Globals.spriteBatch.Draw(labelLeft, new Rectangle((int)button.position.X, (int)button.position.Y, 12, (int)button.size.Y), Color.White*a);
            Globals.spriteBatch.Draw(labelMiddle, new Rectangle((int)button.position.X + 12, (int)button.position.Y, (int)button.size.X - (2 * 12), (int)button.size.Y), Color.White * a);
            Globals.spriteBatch.Draw(labelRight, new Rectangle((int)(button.position.X + button.size.X - 12), (int)button.position.Y, 12, (int)button.size.Y), Color.White * a);
            
            // Draw value
            float t = button.size.X - 6 * 2;
            float p = (float)(button.currentValue / (button.maxValue - button.minValue) * 100);
            Globals.spriteBatch.FillRectangle(new Vector2(button.position.X + 6, button.position.Y + 6), new Vector2(t / 100 * p, button.size.Y - 6 * 2), button.onColour * a);
            Globals.spriteBatch.FillRectangle(new Vector2(button.position.X + 6 + (t / 100 * p), button.position.Y + 6), new Vector2((t) - (t / 100 * p), button.size.Y - 6 * 2), button.offColour * a);

            // Draw text
            Globals.spriteBatch.DrawString(button.font, button.text, new Vector2(button.position.X + button.textOffset.X, button.position.Y + button.textOffset.Y + 1), button.textColour * a);
            // Draw arrows
            if (button.active)
            {
                if (button.currentValue > button.minValue)
                    Globals.spriteBatch.DrawString(button.font, "<", new Vector2((button.position.X + 10), button.position.Y + button.textOffset.Y), button.textColour * a);
                if (button.currentValue < button.maxValue)
                    Globals.spriteBatch.DrawString(button.font, ">", new Vector2((button.position.X + button.size.X - (button.font.MeasureString(">").X) - 10), button.position.Y + button.textOffset.Y), button.textColour * a);
            }
            // Draw highlight if active
            if (button.selected)
            {
                // Top left
                Globals.spriteBatch.Draw(selectTopLeft, new Rectangle((int)button.position.X - currentActiveBorder, (int)button.position.Y - currentActiveBorder, 20, 20), Color.White * a);
                // Top Right
                Globals.spriteBatch.Draw(selectTopRight, new Rectangle((int)(button.position.X + button.size.X - 20 + currentActiveBorder), (int)button.position.Y - currentActiveBorder, 20, 20), Color.White * a);
                // Bottom left
                Globals.spriteBatch.Draw(selectBottomLeft, new Rectangle((int)button.position.X - currentActiveBorder, (int)(button.position.Y + button.size.Y - 20 + currentActiveBorder), 20, 20), Color.White * a);
                // Bottom right
                Globals.spriteBatch.Draw(selectBottomRight, new Rectangle((int)(button.position.X + button.size.X - 20 + currentActiveBorder), (int)(button.position.Y + button.size.Y - 20 + currentActiveBorder), 20, 20), Color.White * a);
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
            button.text = "Keyboard";
            if (button.currentValue == 1)
                button.text = "Controller";
            button.Init();
            Globals.spriteBatch.DrawString(button.font, button.text, new Vector2(button.position.X + button.textOffset.X, button.position.Y + button.textOffset.Y + 1), button.textColour);

            // Draw arrows
            if (button.currentValue > button.minValue)
                Globals.spriteBatch.DrawString(button.font, "<", new Vector2((button.position.X + 10), button.position.Y + button.textOffset.Y), button.textColour);
            if (EngineGlobals.inputManager.IsControllerConnected() && button.currentValue < button.maxValue)
                Globals.spriteBatch.DrawString(button.font, ">", new Vector2((button.position.X + button.size.X - (button.font.MeasureString(">").X) - 10), button.position.Y + button.textOffset.Y), button.textColour);

            // Draw highlight if active
            if (button.selected)
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

        public static void SetControls(UIButton button)
        {
            if (EngineGlobals.entityManager.GetLocalPlayer().GetComponent<InputComponent>().input == Engine.Inputs.controller)
            {
                EngineGlobals.entityManager.GetLocalPlayer().GetComponent<InputComponent>().input = Engine.Inputs.keyboard;
                button.text = "Keyboard";
            }
            else
            {
                if (EngineGlobals.inputManager.IsControllerConnected())
                {
                    EngineGlobals.entityManager.GetLocalPlayer().GetComponent<InputComponent>().input = Engine.Inputs.controller;
                    button.text = "Controller";
                }

            }
            button.Init();
            //if ( && value == 1)
            //{
            //    EngineGlobals.entityManager.GetLocalPlayer().GetComponent<InputComponent>().input = Engine.Inputs.controller;
            //}
            //else
            //{
            //    button.currentValue = 0;
            //    
            //}
        }

    }
}
