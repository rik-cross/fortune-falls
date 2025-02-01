using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

using Engine;

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

            // adjusted dimensions
            Vector2 entityscreenPosition = scene.GetCameraByName("main").GetScreenPosition(transformComponent.Position);
            Vector2 entityScreenSize = transformComponent.Size * scene.GetCameraByName("main").zoom;
            
            // calculate bottom-middle of component
            Vector2 entityTopMiddle = new Vector2(
                entityscreenPosition.X + (entityScreenSize.X / 2),
                entityscreenPosition.Y
            );

            // draw background
            UI.DrawRect(
                (int)(entityTopMiddle.X - (animatedEmoteComponent.backgroundSize.X / 2)),
                (int)(entityTopMiddle.Y - animatedEmoteComponent.backgroundSize.Y - animatedEmoteComponent.heightAboveEntity), // TODO - borderSize
                animatedEmoteComponent.backgroundSize.X,
                animatedEmoteComponent.backgroundSize.Y,
                (float)animatedEmoteComponent.alpha.Value,
                borderWidth: GameAssets.UIBoxBorder
            );

            // draw image
            EngineGlobals.spriteBatch.Draw(
                animatedEmoteComponent._textures[animatedEmoteComponent._currentIndex],
                new Rectangle(
                    (int)(entityTopMiddle.X - (animatedEmoteComponent.textureSize.X / 2)),
                    (int)(entityTopMiddle.Y - animatedEmoteComponent.textureSize.Y - animatedEmoteComponent.heightAboveEntity - GameAssets.EmoteBorderSize), 
                    (int)animatedEmoteComponent.textureSize.X,
                    (int)animatedEmoteComponent.textureSize.Y
                ),
                Color.White * (float)animatedEmoteComponent.alpha.Value
            );

        }

        public static void DrawEmote(Scene scene, Entity entity)
        {
            EmoteComponent animatedEmoteComponent = entity.GetComponent<EmoteComponent>();
            TransformComponent transformComponent = entity.GetComponent<TransformComponent>();

            if (scene.GetCameraByName("main") == null)
                return;

            // adjusted dimensions
            Vector2 entityscreenPosition = scene.GetCameraByName("main").GetScreenPosition(transformComponent.Position);
            Vector2 entityScreenSize = transformComponent.Size * scene.GetCameraByName("main").zoom;

            // calculate bottom-middle of component
            Vector2 entityTopMiddle = new Vector2(
                entityscreenPosition.X + (entityScreenSize.X / 2),
                entityscreenPosition.Y
            );

            // draw background
            UI.DrawRect(
                (int)(entityTopMiddle.X - (animatedEmoteComponent.backgroundSize.X / 2)),
                (int)(entityTopMiddle.Y - animatedEmoteComponent.backgroundSize.Y - animatedEmoteComponent.heightAboveEntity), // TODO - borderSize
                animatedEmoteComponent.backgroundSize.X,
                animatedEmoteComponent.backgroundSize.Y,
                (float)animatedEmoteComponent.alpha.Value,
                borderWidth: GameAssets.UIBoxBorder
            );

            // draw image
            EngineGlobals.spriteBatch.Draw(
                animatedEmoteComponent._texture,
                new Rectangle(
                    (int)(entityTopMiddle.X - (animatedEmoteComponent.textureSize.X / 2)),
                    (int)(entityTopMiddle.Y - animatedEmoteComponent.textureSize.Y - animatedEmoteComponent.heightAboveEntity - GameAssets.EmoteBorderSize),
                    (int)animatedEmoteComponent.textureSize.X,
                    (int)animatedEmoteComponent.textureSize.Y
                ),
                Color.White * (float)animatedEmoteComponent.alpha.Value
            );
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
            EngineGlobals.spriteBatch.DrawString(button.font, button.text, new Vector2(button.position.X + button.textOffset.X, button.position.Y + button.textOffset.Y + 1), button.textColour * a);
            // Draw highlight if active
            if(button.selected)
            {
                // Top left
                EngineGlobals.spriteBatch.Draw(selectTopLeft, new Rectangle((int)button.position.X - currentActiveBorder, (int)button.position.Y - currentActiveBorder, 20, 20), Color.White * a);
                // Top Right
                EngineGlobals.spriteBatch.Draw(selectTopRight, new Rectangle((int)(button.position.X + button.size.X - 20 + currentActiveBorder), (int)button.position.Y - currentActiveBorder, 20, 20), Color.White*a);
                // Bottom left
                EngineGlobals.spriteBatch.Draw(selectBottomLeft, new Rectangle((int)button.position.X - currentActiveBorder, (int)(button.position.Y + button.size.Y - 20 + currentActiveBorder), 20, 20), Color.White*a);
                // Bottom right
                EngineGlobals.spriteBatch.Draw(selectBottomRight, new Rectangle((int)(button.position.X + button.size.X - 20 + currentActiveBorder), (int)(button.position.Y + button.size.Y - 20 + currentActiveBorder), 20, 20), Color.White*a);
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
            
            // Draw background image
            UI.DrawRect(button.position.X, button.position.Y, button.size.X, button.size.Y, a);

            // Draw value
            float t = button.size.X - 8 * 2;
            float p = (float)(button.currentValue / (button.maxValue - button.minValue) * 100);
            EngineGlobals.spriteBatch.FillRectangle(new Vector2(button.position.X + 8, button.position.Y + 8), new Vector2(t / 100 * p, button.size.Y - 8 * 2), button.onColour * a);
            EngineGlobals.spriteBatch.FillRectangle(new Vector2(button.position.X + 8 + (t / 100 * p), button.position.Y + 8), new Vector2((t) - (t / 100 * p), button.size.Y - 8 * 2), button.offColour * a);

            // Draw text
            EngineGlobals.spriteBatch.DrawString(button.font, button.text, new Vector2(button.position.X + button.textOffset.X, button.position.Y + button.textOffset.Y + 1), button.textColour * a);
            // Draw arrows
            if (button.active)
            {
                if (button.currentValue > button.minValue)
                    EngineGlobals.spriteBatch.DrawString(button.font, "<", new Vector2((button.position.X + 10), button.position.Y + button.textOffset.Y), button.textColour * a);
                if (button.currentValue < button.maxValue)
                    EngineGlobals.spriteBatch.DrawString(button.font, ">", new Vector2((button.position.X + button.size.X - (button.font.MeasureString(">").X) - 10), button.position.Y + button.textOffset.Y), button.textColour * a);
            }
            // Draw highlight if active
            if (button.selected)
            {
                // Top left
                EngineGlobals.spriteBatch.Draw(selectTopLeft, new Rectangle((int)button.position.X - currentActiveBorder, (int)button.position.Y - currentActiveBorder, 20, 20), Color.White * a);
                // Top Right
                EngineGlobals.spriteBatch.Draw(selectTopRight, new Rectangle((int)(button.position.X + button.size.X - 20 + currentActiveBorder), (int)button.position.Y - currentActiveBorder, 20, 20), Color.White * a);
                // Bottom left
                EngineGlobals.spriteBatch.Draw(selectBottomLeft, new Rectangle((int)button.position.X - currentActiveBorder, (int)(button.position.Y + button.size.Y - 20 + currentActiveBorder), 20, 20), Color.White * a);
                // Bottom right
                EngineGlobals.spriteBatch.Draw(selectBottomRight, new Rectangle((int)(button.position.X + button.size.X - 20 + currentActiveBorder), (int)(button.position.Y + button.size.Y - 20 + currentActiveBorder), 20, 20), Color.White * a);
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
            //EngineGlobals.spriteBatch.DrawRectangle(button.position, button.size, button.outlineColour, button.outlineThickness);
            EngineGlobals.spriteBatch.Draw(labelLeft, new Rectangle((int)button.position.X, (int)button.position.Y, 12, (int)button.size.Y), Color.White);
            EngineGlobals.spriteBatch.Draw(labelMiddle, new Rectangle((int)button.position.X + 12, (int)button.position.Y, (int)button.size.X - (2 * 12), (int)button.size.Y), Color.White);
            EngineGlobals.spriteBatch.Draw(labelRight, new Rectangle((int)(button.position.X + button.size.X - 12), (int)button.position.Y, 12, (int)button.size.Y), Color.White);

            // Draw text
            button.text = "Keyboard";
            if (button.currentValue == 1)
                button.text = "Controller";
            button.Init();
            EngineGlobals.spriteBatch.DrawString(button.font, button.text, new Vector2(button.position.X + button.textOffset.X, button.position.Y + button.textOffset.Y + 1), button.textColour);

            // Draw arrows
            if (button.currentValue > button.minValue)
                EngineGlobals.spriteBatch.DrawString(button.font, "<", new Vector2((button.position.X + 10), button.position.Y + button.textOffset.Y), button.textColour);
            if (EngineGlobals.inputManager.IsControllerConnected() && button.currentValue < button.maxValue)
                EngineGlobals.spriteBatch.DrawString(button.font, ">", new Vector2((button.position.X + button.size.X - (button.font.MeasureString(">").X) - 10), button.position.Y + button.textOffset.Y), button.textColour);

            // Draw highlight if active
            if (button.selected)
            {
                // Top left
                EngineGlobals.spriteBatch.Draw(selectTopLeft, new Rectangle((int)button.position.X - currentActiveBorder, (int)button.position.Y - currentActiveBorder, 20, 20), Color.White);
                // Top Right
                EngineGlobals.spriteBatch.Draw(selectTopRight, new Rectangle((int)(button.position.X + button.size.X - 20 + currentActiveBorder), (int)button.position.Y - currentActiveBorder, 20, 20), Color.White);
                // Bottom left
                EngineGlobals.spriteBatch.Draw(selectBottomLeft, new Rectangle((int)button.position.X - currentActiveBorder, (int)(button.position.Y + button.size.Y - 20 + currentActiveBorder), 20, 20), Color.White);
                // Bottom right
                EngineGlobals.spriteBatch.Draw(selectBottomRight, new Rectangle((int)(button.position.X + button.size.X - 20 + currentActiveBorder), (int)(button.position.Y + button.size.Y - 20 + currentActiveBorder), 20, 20), Color.White);
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
            if (Globals.IsControllerSelected)
            {
                Globals.IsControllerSelected = false;
                button.text = "Controller";
            }
            else
            {
                if (EngineGlobals.inputManager.IsControllerConnected())
                {
                    Globals.IsControllerConnected = true;
                    Globals.IsControllerSelected = true;
                    button.text = "Keyboard";
                }

            }
            button.Init();
        }

    }
}
